using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using Business.Core.Storage;
using Foundation.Logger;

namespace Business.Core.Sorter;

public class Sorter
{
    private readonly long chunkSizeInBytesLimit;
    private readonly ILogger _logger;
    private readonly IStreamFactory _streamFactory;
    private readonly IComparer<string?> _dataComparer;

    public Sorter(ILogger logger, IStreamFactory streamFactory, IComparer<string?> dataComparer, long chunkSizeInBytesLimit)
    {
        this._logger = logger;
        this._streamFactory = streamFactory;
        this._dataComparer = dataComparer;
        this.chunkSizeInBytesLimit = chunkSizeInBytesLimit;
    }

    public string Sort(StreamReader reader)
    {
        List<string> chunkIDs = SplitIntoChunksAndSort(reader);
        var mainChunkID = MergeChunks(chunkIDs);

        return mainChunkID;
    }

    private List<string> SplitIntoChunksAndSort(StreamReader reader)
    {
        List<string> chunkIDs = new List<string>();

        long totalBytesRead = 0;
        int maxTheads = Environment.ProcessorCount;
        List<Task> workers = new List<Task>(maxTheads);

        long bytesRead = 0;
        List<string> chunk = new List<string>();

        while (!reader.EndOfStream)
        {
            if (workers.Count == maxTheads)
            {
                this._logger.Info($"Reached max threads: {maxTheads}. Waiting for workers to finish...");
                Task.WaitAll(workers.ToArray());
                workers.Clear();
            }

            string text = reader.ReadLine() ?? string.Empty;
            bytesRead += Encoding.UTF8.GetByteCount(text);
            totalBytesRead += bytesRead + Environment.NewLine.Length;
            chunk.Add(text);

            // chunk is ready to be processed
            if (bytesRead >= this.chunkSizeInBytesLimit)
            {
                this._logger.Info($"[Before Task] Bytes read: {bytesRead} ({(double)bytesRead / 1024 / 1024:0.00} MB)");
                this._logger.Info($"[Before Task] Memory used: {(double)MemoryUsed() / 1024 / 1024:0.00} MB");

                Task task = CreateSortAndWriteIntoChunkTask(chunk, chunkIDs);
                workers.Add(task);

                chunk = new List<string>();
                bytesRead = 0;
            }
        }

        // last chunk
        if (chunk.Count > 0)
        {
            Task task = CreateSortAndWriteIntoChunkTask(chunk, chunkIDs);
            workers.Add(task);
        }

        Task.WaitAll(workers.ToArray());

        return chunkIDs;
    }

    private string MergeChunks(List<string> chunkIDs)
    {
        List<string> mergedChunkIDs = new List<string>();

        while (chunkIDs.Count > 1)
        {
            List<Task> workers = new List<Task>();
            for (int i = 0; i < chunkIDs.Count; i += 2)
            {
                if (i + 1 < chunkIDs.Count)
                {
                    string chunkID1 = chunkIDs[i];
                    string chunkID2 = chunkIDs[i + 1];

                    Task task = CreateMergeChunksTask(chunkID1, chunkID2, mergedChunkIDs);
                    workers.Add(task);
                }
                else
                {
                    mergedChunkIDs.Add(chunkIDs[i]);
                }
            }

            Task.WaitAll(workers.ToArray());
            chunkIDs = mergedChunkIDs;
            mergedChunkIDs = new List<string>();
        }

        return chunkIDs[0];
    }

    private Task CreateSortAndWriteIntoChunkTask(List<string> chunk, List<string> chunkIDs)
    {
        var task = new Task(() =>
        {
            this._logger.Info($"[{Thread.CurrentThread.ManagedThreadId}] Task started working.");
            this._logger.Info($"[{Thread.CurrentThread.ManagedThreadId}] Sorting {chunk.Count} elements ...");
            chunk.Sort(this._dataComparer);
            Guid newGuid = Guid.NewGuid();
            using (StreamWriter writer = this._streamFactory.CreateStreamWriter(newGuid.ToString()))
            {
                foreach (string item in chunk)
                {
                    writer.WriteLine(item);
                }
            }

            lock (chunkIDs)
            {
                chunkIDs.Add(newGuid.ToString());
            }

            Process currentProcess = Process.GetCurrentProcess();
            long memoryUsed = currentProcess.WorkingSet64;

            this._logger.Info($"[{Thread.CurrentThread.ManagedThreadId}] Memory used: {(double)memoryUsed / 1024 / 1024:0.00} MB");
            this._logger.Info($"[{Thread.CurrentThread.ManagedThreadId}] Task finished working.");
        });
        task.Start();

        return task;
    }

    private Task CreateMergeChunksTask(string chunkID1, string chunkID2, List<string> mergedChunkIDs)
    {
        var task = new Task(() =>
        {
            this._logger.Info($"[{Thread.CurrentThread.ManagedThreadId}] Merge Task started working.");

            // merge chunks and write to a writer stream
            this._logger.Info($"[{Thread.CurrentThread.ManagedThreadId}] Merging chunks {chunkID1} and {chunkID2} ...");
            string mergedChunkID = MergeChunks(chunkID1, chunkID2);
            lock (mergedChunkIDs)
            {
                mergedChunkIDs.Add(mergedChunkID);
            }

            this._logger.Info($"[{Thread.CurrentThread.ManagedThreadId}] Memory used: {(double)MemoryUsed() / 1024 / 1024:0.00} MB");
            this._logger.Info($"[{Thread.CurrentThread.ManagedThreadId}] Task finished working.");
        });
        task.Start();

        return task;
    }

    private string MergeChunks(string chunkID1, string chunkID2)
    {
        string mergedChunkID = Guid.NewGuid().ToString();

        using (StreamReader reader1 = this._streamFactory.CreateStreamReader(chunkID1))
        {
            using (StreamReader reader2 = this._streamFactory.CreateStreamReader(chunkID2))
            {
                using (StreamWriter writer = this._streamFactory.CreateStreamWriter(mergedChunkID))
                {
                    string? line1 = reader1.ReadLine();
                    string? line2 = reader2.ReadLine();

                    while (line1 != null && line2 != null)
                    {
                        int compareResult = this._dataComparer.Compare(line1, line2);
                        if (compareResult < 0)
                        {
                            writer.WriteLine(line1);
                            line1 = reader1.ReadLine();
                        }
                        else
                        {
                            writer.WriteLine(line2);
                            line2 = reader2.ReadLine();
                        }
                    }

                    while (line1 != null)
                    {
                        writer.WriteLine(line1);
                        line1 = reader1.ReadLine();
                    }

                    while (line2 != null)
                    {
                        writer.WriteLine(line2);
                        line2 = reader2.ReadLine();
                    }
                }
            }
        }

        // Clean up/remove temp files
        this._streamFactory.DeleteStreamSource(chunkID1);
        this._streamFactory.DeleteStreamSource(chunkID2);

        return mergedChunkID;
    }

    private long MemoryUsed()
    {
        Process currentProcess = Process.GetCurrentProcess();
        return currentProcess.WorkingSet64;
    }
}

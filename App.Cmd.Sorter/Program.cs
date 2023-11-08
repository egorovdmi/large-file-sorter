using System.Diagnostics;
using Foundation.Logger;
using Business.Core.Storage;
using Business.Core.Sorter;
using Business.Data.Comparers;

class Program
{
    static void Main(string[] args)
    {
        var (cfg, isHelpRequested) = Config.Load(args);
        if (isHelpRequested)
        {
            return;
        }

        cfg.Info();

        var logger = new Logger();

        // BufferSize * Max parallel tasks (when use low memory limit, i.e. 256MB is important to have enough memory for write buffers)
        var memoryReservedByWriteBuffers = cfg.MaxParallelTasks * cfg.BufferSize;

        // Effective memory usage ratio when sorting
        var memoryEffectiveRatio = 1 / (double)cfg.MaxParallelTasks;

        // Calculate chunk size in bytes that can be used for sorting
        var chunkSizeInBytes = (long)((cfg.MemoryLimit - memoryReservedByWriteBuffers) / cfg.MaxParallelTasks * memoryEffectiveRatio);

        // Dependencies
        var tempFilesStreamFactory = new TempFilesStreamFactory(cfg.BufferSize, cfg.TempDir);

        // Sorter
        Stopwatch stopwatch = Stopwatch.StartNew();
        var sorter = new Sorter(logger, tempFilesStreamFactory, new NubmerAndStringComparer(), chunkSizeInBytes);
        using (var reader = new StreamReader(cfg.InputFilePath))
        {
            string streamID = sorter.Sort(reader);
            string sortedFilePath = tempFilesStreamFactory.GetStreamSourcePath(streamID);

            // Replace output file if exists
            string outputFilePath = Path.GetFullPath(cfg.OutputFilePath);
            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }
            File.Move(sortedFilePath, outputFilePath);

            Console.WriteLine($"Sorted file path: {outputFilePath}");
        }
        stopwatch.Stop();

        Process currentProcess = Process.GetCurrentProcess();
        long memoryUsed = currentProcess.WorkingSet64;

        logger.Info($"Time spent: {stopwatch.Elapsed}");
        logger.Info($"Amount of physical memory, in bytes, allocated: {(double)memoryUsed / 1024 / 1024 / 1024:0.00} GB");
    }
}
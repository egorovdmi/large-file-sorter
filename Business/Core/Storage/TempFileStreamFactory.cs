using System.IO;
using System.Text;

namespace Business.Core.Storage;

public class TempFilesStreamFactory : IStreamFactory
{
    private readonly int bufferSize;
    private readonly string workingDirectory;

    public TempFilesStreamFactory(int bufferSize, string workingDirectory)
    {
        if (!Directory.Exists(workingDirectory))
        {
            Directory.CreateDirectory(workingDirectory);
        }

        this.bufferSize = bufferSize;
        this.workingDirectory = workingDirectory;
    }

    public StreamReader CreateStreamReader(string streamID)
    {
        return new StreamReader($"{this.workingDirectory}/{streamID}.tmp");
    }

    public StreamWriter CreateStreamWriter(string streamID)
    {
        return new StreamWriter($"{this.workingDirectory}/{streamID}.tmp", false, Encoding.UTF8, this.bufferSize);
    }

    public string GetStreamSourcePath(string streamID)
    {
        return $"{this.workingDirectory}/{streamID}.tmp";
    }
}
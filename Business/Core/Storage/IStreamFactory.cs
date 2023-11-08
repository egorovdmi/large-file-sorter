namespace Business.Core.Storage;

public interface IStreamFactory
{
    StreamWriter CreateStreamWriter(string streamID);
    StreamReader CreateStreamReader(string streamID);
    string GetStreamSourcePath(string streamID);
    void DeleteStreamSource(string streamID);
}

namespace Foundation.Logging;

public interface ILogger
{
    void WriteLine(string message);
}

public class Logger : ILogger
{
    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }
}

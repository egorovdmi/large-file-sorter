namespace Foundation.Logger;

public interface ILogger
{
    void Info(string message);
}

public class Logger : ILogger
{
    public void Info(string message)
    {
        Console.WriteLine(message);
    }
}

using System;

class Program
{
    static void Main(string[] args)
    {
        var cfg = Config.Load(args);
        if (args.Contains("-h"))
        {
            return;
        }

        cfg.Info();

        Thread.Sleep(10000);
        Console.WriteLine("Hello World!");
    }
}
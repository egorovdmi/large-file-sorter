using System;
using Foundation.Logger;

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

        var logger = new Logger();
        logger.WriteLine("Hello World!");
    }
}
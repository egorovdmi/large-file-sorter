using System.Diagnostics;
using Foundation.Logger;

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

        Stopwatch stopwatch = Stopwatch.StartNew();

        // TODO: Implement the sorter

        stopwatch.Stop();

        Process currentProcess = Process.GetCurrentProcess();
        long memoryUsed = currentProcess.WorkingSet64;

        logger.Info($"Time spent: {stopwatch.Elapsed}");
        logger.Info($"Amount of physical memory, in bytes, allocated: {(double)memoryUsed / 1024 / 1024 / 1024:0.00} GB");
    }
}
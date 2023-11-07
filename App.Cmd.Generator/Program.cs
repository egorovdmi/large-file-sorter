using System.Diagnostics;
using Business.Core.Generator;
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
        var generator = new TestFileGenerator(logger);

        generator.Generate(cfg.FileSizeLimit, cfg.OutputFilePath);

        Process currentProcess = Process.GetCurrentProcess();
        long memoryUsed = currentProcess.WorkingSet64;

        logger.Info($"Memory used: {(double)memoryUsed / 1024 / 1024 / 1024:0.00} GB");
    }
}
using System;
using System.CommandLine;
using System.Linq;
using System.Collections.Generic;
using Foundation.Parsers.MemorySizeParser;

public class Config
{
    private const string DefaultOutputFilePath = "../Data/testfile.txt";
    private const string DefaultMemoryLimit = "16GB";
    private const string DefaultTempDir = "../Data/Temp";

    public readonly string OutputFilePath;
    public readonly long MemoryLimit;
    public readonly string TempDir;

    public static (Config config, bool isHelpRequested) Load(string[] args)
    {
        var cmd = new RootCommand();

        var outputFilePathOption = new Option<string>("--file-path", () => Environment.GetEnvironmentVariable("FILE_PATH") ?? DefaultOutputFilePath, $"File path (default: '{DefaultOutputFilePath}')");
        var memoryLimitOption = new Option<long>("--memory-limit", () => MemorySizeParser.Parse(Environment.GetEnvironmentVariable("MEMORY_LIMIT") ?? DefaultMemoryLimit), $"Memory limit in bytes (default: {DefaultMemoryLimit})");
        var tempDirOption = new Option<string>("--temp-dir", () => Environment.GetEnvironmentVariable("TEMP_DIR") ?? DefaultTempDir, $"Temp directory path (default: '{DefaultTempDir}')");

        cmd.AddOption(outputFilePathOption);
        cmd.AddOption(memoryLimitOption);
        cmd.AddOption(tempDirOption);

        string outputFilePath = "";
        long memoryLimit = 0;
        string tempDir = "";

        cmd.SetHandler((value1, value2, value3) =>
        {
            outputFilePath = value1;
            memoryLimit = value2;
            tempDir = value3;
        }, outputFilePathOption, memoryLimitOption, tempDirOption);

        cmd.Invoke(args);

        return (new Config(outputFilePath, memoryLimit, tempDir), args.Contains("-h"));
    }

    public Config(string outputFilePath, long memoryLimit, string tempDir)
    {
        OutputFilePath = outputFilePath;
        MemoryLimit = memoryLimit;
        TempDir = tempDir;
    }

    public void Info()
    {
        Console.WriteLine("Current Configuration:");
        Console.WriteLine($"File Path: {OutputFilePath}");
        Console.WriteLine($"Memory Limit: {MemoryLimit} bytes ({MemoryLimit / 1024 / 1024 / 1024} GB)");
        Console.WriteLine($"Temp Directory: {TempDir}");
    }
}

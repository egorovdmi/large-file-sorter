using System;
using System.CommandLine;
using System.Linq;
using System.Collections.Generic;
using Foundation.Parsers.MemorySizeParser;

public class Config
{
    private const string DefaultInputFilePath = "../Data/testfile.txt";
    private const string DefaultOutputFilePath = "../Data/testfile_sorted.txt";
    private const string DefaultMemoryLimit = "16GB";
    private const string DefaultTempDir = "../Data/Temp";
    public readonly int MaxParallelTasks = Environment.ProcessorCount;
    public readonly int BufferSize = 8 * 1024 * 1024; // 8 MB buffer as we use SSD drive and write big files

    public readonly string InputFilePath;
    public readonly string OutputFilePath;
    public readonly long MemoryLimit;
    public readonly string TempDir;

    public static (Config config, bool isHelpRequested) Load(string[] args)
    {
        var cmd = new RootCommand();

        var inputFilePathOption = new Option<string>("--input-file-path", () => Environment.GetEnvironmentVariable("INPUT_FILE_PATH") ?? DefaultInputFilePath, $"File path");
        var outputFilePathOption = new Option<string>("--output-file-path", () => Environment.GetEnvironmentVariable("OUTPUT_FILE_PATH") ?? DefaultOutputFilePath, $"Output file path");
        var memoryLimitOption = new Option<string>("--memory-limit", () => Environment.GetEnvironmentVariable("MEMORY_LIMIT") ?? DefaultMemoryLimit, $"Memory limit in bytes");
        var tempDirOption = new Option<string>("--temp-dir", () => Environment.GetEnvironmentVariable("TEMP_DIR") ?? DefaultTempDir, $"Temp directory path");

        cmd.AddOption(inputFilePathOption);
        cmd.AddOption(outputFilePathOption);
        cmd.AddOption(memoryLimitOption);
        cmd.AddOption(tempDirOption);

        string outputFilePath = "";
        string inputFilePath = "";
        long memoryLimit = 0;
        string tempDir = "";

        cmd.SetHandler((value1, value2, value3, value4) =>
        {
            inputFilePath = value1;
            outputFilePath = value2;
            memoryLimit = MemorySizeParser.Parse(value3);
            tempDir = value4;
        }, inputFilePathOption, outputFilePathOption, memoryLimitOption, tempDirOption);

        cmd.Invoke(args);

        return (new Config(inputFilePath, outputFilePath, memoryLimit, tempDir), args.Contains("-h"));
    }

    public Config(string inputFilePath, string outputFilePath, long memoryLimit, string tempDir)
    {
        InputFilePath = inputFilePath;
        OutputFilePath = outputFilePath;
        MemoryLimit = memoryLimit;
        TempDir = tempDir;
    }

    public void Info()
    {
        Console.WriteLine("Current Configuration:");
        Console.WriteLine($"Input File Path: {InputFilePath}");
        Console.WriteLine($"Output File Path: {OutputFilePath}");
        Console.WriteLine($"Memory Limit: {MemoryLimit} bytes ({MemoryLimit / 1024 / 1024 / 1024} GB)");
        Console.WriteLine($"Temp Directory: {TempDir}");
    }
}

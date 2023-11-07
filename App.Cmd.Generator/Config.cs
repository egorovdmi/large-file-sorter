using System.CommandLine;
using Foundation.Parsers.MemorySizeParser;

public class Config
{
    private const string DefaultOutputFilePath = "../Data/testfile.txt";
    private const string DefaultFileSizeLimit = "10GB";
    public readonly string OutputFilePath;
    public readonly long FileSizeLimit;

    public static (Config config, bool isHelpRequested) Load(string[] args)
    {
        var cmd = new RootCommand();
        var options = new List<Option>();

        var outputFilePathOption = new Option<string>("--output-file-path", () =>
            Environment.GetEnvironmentVariable("OUTPUT_FILE_PATH") ?? DefaultOutputFilePath, $"Output file path (default: '{DefaultOutputFilePath}')");
        cmd.AddOption(outputFilePathOption);
        options.Add(outputFilePathOption);

        var fileSizeLimitOption = new Option<long>("--file-size-limit", () =>
            MemorySizeParser.Parse(Environment.GetEnvironmentVariable("FILE_SIZE_LIMIT") ?? DefaultFileSizeLimit), $"File size limit in bytes (default: {DefaultFileSizeLimit})");
        cmd.AddOption(fileSizeLimitOption);
        options.Add(fileSizeLimitOption);

        string outputFilePath = "";
        long fileSizeLimit = 0;

        cmd.SetHandler((value1, value2) =>
        {
            outputFilePath = value1;
            fileSizeLimit = value2;
        }, outputFilePathOption, fileSizeLimitOption);

        cmd.Invoke(args);

        return (new Config(outputFilePath, fileSizeLimit), args.Contains("-h"));
    }

    public Config(string outputFilePath, long fileSizeLimit)
    {
        OutputFilePath = outputFilePath;
        FileSizeLimit = fileSizeLimit;
    }

    public void Info()
    {
        // Displaying the current configuration
        Console.WriteLine("Current Configuration:");
        Console.WriteLine($"Output File Path: {OutputFilePath}");
        Console.WriteLine($"File Size Limit: {FileSizeLimit} bytes ({FileSizeLimit / 1024 / 1024 / 1024} GB)");
    }
}

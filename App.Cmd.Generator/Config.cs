using System.CommandLine;

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
            ParseFileSize(Environment.GetEnvironmentVariable("FILE_SIZE_LIMIT") ?? DefaultFileSizeLimit), $"File size limit in bytes (default: {DefaultFileSizeLimit})");
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

    private static long ParseFileSize(string input)
    {
        if (input.EndsWith("MB", StringComparison.OrdinalIgnoreCase))
        {
            return Convert.ToInt64(input.TrimEnd("MB".ToCharArray())) * 1024 * 1024;
        }
        else if (input.EndsWith("GB", StringComparison.OrdinalIgnoreCase))
        {
            return Convert.ToInt64(input.TrimEnd("GB".ToCharArray())) * 1024 * 1024 * 1024;
        }
        else
        {
            throw new ArgumentException("Invalid file size format. Use '10MB' or '10GB'.");
        }
    }
}

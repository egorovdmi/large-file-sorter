using System.Text;
using Foundation.Logger;

namespace Business.Core.Generator;

public class TestFileGenerator
{
    private readonly ILogger _logger;

    public TestFileGenerator(ILogger logger)
    {
        _logger = logger;
    }

    public void Generate(long fileSizeLimitInBytes, string outputFilePath)
    {
        _logger.Info($"Generating a test file...");

        string[] strings = { "Apple", "Something something something", "Cherry is the best", "Banana is yellow", "is simply dummy text of the printing and typesetting industry, Lorem Ipsum has been the industry's standard dummy text ever since the 1500s" };
        Random random = new Random();

        int bufferSize = 8 * 1024 * 1024; // 8 MB buffer as we use SSD drive and write big files

        // Generating a large test file
        using (StreamWriter file = new StreamWriter(outputFilePath, false, Encoding.UTF8, bufferSize))
        {
            long totalBytesWritten = 0;
            while (totalBytesWritten < fileSizeLimitInBytes)
            {
                byte[] buffer = new byte[8];
                random.NextBytes(buffer);
                long randomNumber = Math.Abs(BitConverter.ToInt64(buffer, 0));

                string str = strings[random.Next(strings.Length)];
                string lineOfText = $"{randomNumber}. {str}";
                long byteCount = Encoding.UTF8.GetByteCount(lineOfText) + Environment.NewLine.Length;

                file.WriteLine(lineOfText);
                totalBytesWritten += byteCount;
            }

            _logger.Info($"Total bytes written: {totalBytesWritten}");
        }
    }
}

namespace Foundation.Parsers.MemorySizeParser;

public static class MemorySizeParser
{
    public static long Parse(string memorySize)
    {
        if (string.IsNullOrEmpty(memorySize))
        {
            throw new ArgumentException("Invalid memory size");
        }

        long numericPart;
        if (!long.TryParse(memorySize.Substring(0, memorySize.Length - 2), out numericPart))
        {
            throw new ArgumentException("Parsing memory size failed");
        }

        var suffix = memorySize.Substring(memorySize.Length - 2);
        switch (suffix)
        {
            case "MB":
                return numericPart * 1024 * 1024;
            case "GB":
                return numericPart * 1024 * 1024 * 1024;
            default:
                throw new ArgumentException("Invalid memory size suffix");
        }
    }
}
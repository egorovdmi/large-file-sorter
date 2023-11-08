namespace Business.Data.Comparers;

public class NubmerAndStringComparer : IComparer<string?>
{
    public int Compare(string? x, string? y)
    {
        if (x == null || y == null)
        {
            throw new ArgumentNullException();
        }

        int dotIndexX = x.IndexOf('.');
        int dotIndexY = y.IndexOf('.');

        string subStringX = x.Substring(dotIndexX + 1);
        string subStringY = y.Substring(dotIndexY + 1);

        int stringCompare = string.Compare(subStringX, subStringY, StringComparison.Ordinal);
        if (stringCompare != 0)
        {
            return stringCompare;
        }

        long longX = long.Parse(x.Substring(0, dotIndexX));
        long longY = long.Parse(y.Substring(0, dotIndexY));

        return longX.CompareTo(longY);
    }
}
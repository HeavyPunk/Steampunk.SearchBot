using System.Text;
using System.Text.RegularExpressions;

namespace SearchBot.Lib.DataComparators;

public class SimpleStringComparator : IComparer<string>
{
    private Regex regex = new("[^A-zА-я0-9]*");
    
    public int Compare(string? x, string? y)
    {
        if (x is null || y is null)
            return 0;
        var normX = NormalizeString(x);
        var normY = NormalizeString(y);
        var levenshtein = Levenshtein(normX, normY);
        return levenshtein > 10 ? levenshtein : 0;
    }

    private string NormalizeString(string input)
    {
        return regex.Replace(input, "");
    }
    
    private int Levenshtein(string a, string b) {
        if (string.IsNullOrEmpty(a))
            return !string.IsNullOrEmpty(b) ? b.Length : 0;
        

        if (string.IsNullOrEmpty(b))
            return !string.IsNullOrEmpty(a) ? a.Length : 0;
        
        int cost;
        var d = new int[a.Length + 1, b.Length + 1];
        int min1;
        int min2;
        int min3;
        for (var i = 0; i <= d.GetUpperBound(0); i += 1)
        {
            d[i, 0] = i;
        }
        for (var i = 0; i <= d.GetUpperBound(1); i += 1)
        {
            d[0, i] = i;
        }
        for (var i = 1; i <= d.GetUpperBound(0); i += 1)
        {
            for (var j = 1; j <= d.GetUpperBound(1); j += 1)
            {
                cost = Convert.ToInt32(a[i-1] != b[j - 1]);

                min1 = d[i - 1, j] + 1;
                min2 = d[i, j - 1] + 1;
                min3 = d[i - 1, j - 1] + cost;
                d[i, j] = Math.Min(Math.Min(min1, min2), min3);
            }
        }
        return d[d.GetUpperBound(0), d.GetUpperBound(1)];
    }
}
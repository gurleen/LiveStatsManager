using System.Text.RegularExpressions;

namespace Shared.Extensions;

public static partial class StringExtensions
{
    public static string GenerateSlug(this string phrase) 
    { 
        var str = phrase.RemoveAccent().ToLower(); 
        // invalid chars           
        str = MyRegex1().Replace(str, ""); 
        // convert multiple spaces into one space   
        str = MyRegex().Replace(str, " ").Trim(); 
        // cut and trim 
        str = str[..(str.Length <= 45 ? str.Length : 45)].Trim();   
        str = MyRegex2().Replace(str, "-"); // hyphens   
        return str; 
    } 

    public static string RemoveAccent(this string txt) 
    { 
        byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt); 
        return System.Text.Encoding.ASCII.GetString(bytes); 
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex MyRegex();
    [GeneratedRegex(@"[^a-z0-9\s-]")]
    private static partial Regex MyRegex1();
    [GeneratedRegex(@"\s")]
    private static partial Regex MyRegex2();

    public static int SafeParseInt(this string input, int defaultValue = 0)
    {
        if (int.TryParse(input, out var parsed)) { return parsed; }
        return defaultValue;
    }
}
namespace Shared.Utilities;

using System;

public class ColorUtils
{
    public static string DarkenColor(string hexColor, double percentage)
    {
        if (string.IsNullOrWhiteSpace(hexColor) || percentage < 0 || percentage > 100)
            throw new ArgumentException("Invalid input");

        hexColor = hexColor.TrimStart('#');

        if (hexColor.Length != 6)
            throw new ArgumentException("Hex color must be 6 characters long.");

        // parse r, g, b values
        int r = Convert.ToInt32(hexColor.Substring(0, 2), 16);
        int g = Convert.ToInt32(hexColor.Substring(2, 2), 16);
        int b = Convert.ToInt32(hexColor.Substring(4, 2), 16);

        // darken each component
        r = (int)(r * (1 - percentage / 100));
        g = (int)(g * (1 - percentage / 100));
        b = (int)(b * (1 - percentage / 100));

        // clamp values to ensure they're within 0-255
        r = Math.Max(0, Math.Min(255, r));
        g = Math.Max(0, Math.Min(255, g));
        b = Math.Max(0, Math.Min(255, b));

        // format back to hex
        return $"#{r:X2}{g:X2}{b:X2}";
    }
    
    public static string GetBestTextColor(string hexColor)
    {
        if (hexColor.StartsWith('#'))
            hexColor = hexColor[1..];

        if (hexColor.Length != 6)
            return "#000000";

        var r = Convert.ToInt32(hexColor.Substring(0, 2), 16);
        var g = Convert.ToInt32(hexColor.Substring(2, 2), 16);
        var b = Convert.ToInt32(hexColor.Substring(4, 2), 16);

        var luminance = (0.299 * r + 0.587 * g + 0.114 * b) / 255.0;
        return luminance > 0.5 ? "#000000" : "#FFFFFF";
    }
}

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
}

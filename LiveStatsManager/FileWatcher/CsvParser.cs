using System.Globalization;
using CsvHelper;

namespace LiveStatsManager.FileWatcher;

// ReSharper disable once ClassNeverInstantiated.Global
public record DataPair(string Key, string Value);

public static class CsvParser
{
    public static List<DataPair> Parse(string csvPath)
    {
        using var fileStream =
            new FileStream(csvPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var reader = new StreamReader(fileStream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<DataPair>().ToList();
    }
}
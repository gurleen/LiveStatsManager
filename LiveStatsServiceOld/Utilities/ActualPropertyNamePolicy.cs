namespace LiveStatsService.Utilities;

using System.Text.Json;

public class ActualPropertyNamePolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        return name;
    }
}
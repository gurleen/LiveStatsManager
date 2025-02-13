using System.Text.Json;

namespace LiveStatsServiceOld.Utilities;

public class ActualPropertyNamePolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        return name;
    }
}
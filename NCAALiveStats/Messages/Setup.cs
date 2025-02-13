using System.Text.Json.Serialization;
using NCAALiveStats.Messages.Helpers;

namespace NCAALiveStats.Messages;

public class Periods
{
    [JsonPropertyName("number")]
    public int Number { get; set; }
    
    [JsonPropertyName("length")]
    public int Length { get; set; }
    
    [JsonPropertyName("extraTime")]
    public bool ExtraTime { get; set; }
    
    [JsonPropertyName("extraTimeLength")]
    public int ExtraTimeLength { get; set; }
    
    [JsonPropertyName("breakPeriod")]
    public int BreakPeriod { get; set; }
    
    [JsonPropertyName("breakHalfTime")]
    public int BreakHalfTime { get; set; }
}

public enum TimeoutStyle
{
    HALF,
    PERIOD,
    UNLIMITED
}

public class Timeouts
{
    [JsonPropertyName("timeoutsStyle")]
    public string TimeoutsStyle { get; set; }
    
    [JsonPropertyName("timeoutsPeriod1")]
    public int TimeoutsPeriod1 { get; set; }
    
    [JsonPropertyName("timeoutsPeriod2")]
    public int TimeoutsPeriod2 { get; set; }
    
    [JsonPropertyName("timeoutsPeriod3")]
    public int TimeoutsPeriod3 { get; set; }
    
    [JsonPropertyName("timeoutsPeriod4")]
    public int TimeoutsPeriod4 { get; set; }
    
    [JsonPropertyName("timeoutsHalf1")]
    public int TimeoutsHalf1 { get; set; }
    
    [JsonPropertyName("timeoutsHalf2")]
    public int TimeoutsHalf2 { get; set; }
    
    [JsonPropertyName("timeoutsExtraTime")]
    public int TimeoutsExtraTime { get; set; }
}

[SocketMessage("setup")]
public class Setup
{
    [JsonPropertyName("periods")]
    public Periods Periods { get; set; }
    
    [JsonPropertyName("shotClock")]
    public int ShotClock { get; set; }
    
    [JsonPropertyName("foulsPersonal")]
    public int FoulsPersonal { get; set; }
    
    [JsonPropertyName("foulsTechnical")]
    public int FoulsTechnical { get; set; }
    
    [JsonPropertyName("foulsBeforeBonus")]
    public int FoulsBeforeBonus { get; set; }
    
    [JsonPropertyName("maxFoulsPersonal")]
    public int MaxFoulsPersonal { get; set; }
    
    [JsonPropertyName("maxFoulsTechnical")]
    public int MaxFoulsTechnical { get; set; }
    
    [JsonPropertyName("timeouts")]
    public Timeouts Timeouts { get; set; }
}
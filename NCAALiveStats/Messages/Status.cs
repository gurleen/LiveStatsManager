using System.Text.Json.Serialization;
using NCAALiveStats.Messages.Helpers;

namespace NCAALiveStats.Messages;

public enum StatusType
{
    READY,
    WARMUP,
    PREMATCH,
    ANTHEM,
    ONCOURT,
    COUNTDOWN,
    INPROGRESS,
    PERIODBREAK,
    INTERRUPTED,
    CANCELLED,
    FINISHED,
    PROTESTED,
    COMPLETE,
    RESCHEDULED,
    DELAYED
}

public class MatchPeriod
{
    public enum PeriodType { REGULAR, OVERTIME }
        
    [JsonPropertyName("current")]
    public int CurrentPeriod { get; set; }
        
    [JsonPropertyName("periodType")]
    public PeriodType Type { get; set; }
}

public enum PeriodStatus
{
    PENDING,
    STARTED,
    ENDED,
    CONFIRMED
}

public class TeamScore
{
    [JsonPropertyName("teamNumber")]
    public int TeamNumber { get; set; }
    
    [JsonPropertyName("score")]
    public int Score { get; set; }
    
    [JsonPropertyName("timeoutsRemaining")]
    public int TimeoutsRemaining { get; set; }
    
    [JsonPropertyName("fouls")]
    public int Fouls { get; set; }
}

[SocketMessage("status")]
public class MatchStatus
{
    [JsonPropertyName("status")]
    public StatusType Status { get; set; }
    
    [JsonPropertyName("period")]
    public MatchPeriod Period { get; set; }
    
    [JsonPropertyName("periodStatus")]
    public PeriodStatus PeriodStatus { get; set; }
    
    [JsonPropertyName("clock")]
    public string Clock { get; set; }
    
    [JsonPropertyName("shotClock")]
    public string ShotClock { get; set; }
    
    [JsonPropertyName("clockRunning")]
    public bool IsClockRunning { get; set; }
    
    [JsonPropertyName("possession")]
    public int Possession { get; set; }
    
    [JsonPropertyName("possessionArrow")]
    public int PossessionArrow { get; set; }
    
    [JsonPropertyName("scores")]
    public List<TeamScore> Scores { get; set; }
}
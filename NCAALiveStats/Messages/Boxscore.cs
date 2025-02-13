using NCAALiveStats.Messages.Helpers;
using Shared.Objects;

namespace NCAALiveStats.Messages;

using System.Collections.Generic;
using System.Text.Json.Serialization;

[SocketMessage("boxscore")]
public class Boxscore
{
    [JsonPropertyName("teams")]
    public List<TeamBox> Teams { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}

public class TeamBox
{
    [JsonPropertyName("teamNumber")]
    public int TeamNumber { get; set; }

    [JsonPropertyName("total")]
    public TeamTotal Total { get; set; }
}

public class TeamTotal
{
    [JsonPropertyName("players")]
    public List<PlayerStats> Players { get; set; }
    
    [JsonPropertyName("team")]
    public TeamStats Team { get; set; }
}

public class TeamStats
{
    [JsonPropertyName("sBenchPoints")]
    public int BenchPoints { get; set; }

    [JsonPropertyName("sBiggestLead")]
    public int BiggestLead { get; set; }

    [JsonPropertyName("sBiggestScoringRun")]
    public int BiggestScoringRun { get; set; }

    [JsonPropertyName("sTimeLeading")]
    public double TimeLeading { get; set; }

    [JsonPropertyName("sLeadChanges")]
    public int LeadChanges { get; set; }

    [JsonPropertyName("sTimesTied")]
    public int TimesTied { get; set; }

    [JsonPropertyName("sReboundsDefensive")]
    public int DefensiveRebounds { get; set; }

    [JsonPropertyName("sReboundsOffensive")]
    public int OffensiveRebounds { get; set; }

    [JsonPropertyName("sReboundsPersonal")]
    public int PersonalRebounds { get; set; }

    [JsonPropertyName("sReboundsTeamDefensive")]
    public int TeamDefensiveRebounds { get; set; }

    [JsonPropertyName("sReboundsTeamOffensive")]
    public int TeamOffensiveRebounds { get; set; }

    [JsonPropertyName("sReboundsTotalDefensive")]
    public int TotalDefensiveRebounds { get; set; }

    [JsonPropertyName("sReboundsTotalOffensive")]
    public int TotalOffensiveRebounds { get; set; }

    [JsonPropertyName("sReboundsTeam")]
    public int TeamRebounds { get; set; }

    [JsonPropertyName("sTurnoversTeam")]
    public int TeamTurnovers { get; set; }

    [JsonPropertyName("sFoulsTeam")]
    public int TeamFouls { get; set; }

    [JsonPropertyName("sAssists")]
    public int Assists { get; set; }

    [JsonPropertyName("sBlocks")]
    public int Blocks { get; set; }

    [JsonPropertyName("sBlocksReceived")]
    public int BlocksReceived { get; set; }

    [JsonPropertyName("sEfficiency")]
    public double Efficiency { get; set; }

    [JsonPropertyName("sFastBreakPointsMade")]
    public int FastBreakPointsMade { get; set; }

    [JsonPropertyName("sFieldGoalsAttempted")]
    public int FieldGoalsAttempted { get; set; }

    [JsonPropertyName("sFieldGoalsMade")]
    public int FieldGoalsMade { get; set; }

    [JsonPropertyName("sFieldGoalsPercentage")]
    public double FieldGoalsPercentage { get; set; }
    
    public string FieldGoalsDisplay => $"{FieldGoalsMade}/{FieldGoalsAttempted}";

    [JsonPropertyName("sFoulsOn")]
    public int FoulsDrawn { get; set; }

    [JsonPropertyName("sFoulsPersonal")]
    public int PersonalFouls { get; set; }

    [JsonPropertyName("sFoulsTechnical")]
    public int TechnicalFouls { get; set; }

    [JsonPropertyName("sFreeThrowsAttempted")]
    public int FreeThrowsAttempted { get; set; }

    [JsonPropertyName("sFreeThrowsMade")]
    public int FreeThrowsMade { get; set; }

    [JsonPropertyName("sFreeThrowsPercentage")]
    public double FreeThrowsPercentage { get; set; }
    
    public string FreeThrowsDisplay => $"{FreeThrowsMade}/{FreeThrowsAttempted}";

    [JsonPropertyName("sMinutes")]
    public double Minutes { get; set; }

    [JsonPropertyName("sPoints")]
    public int Points { get; set; }

    [JsonPropertyName("sPointsFastBreak")]
    public int FastBreakPoints { get; set; }

    [JsonPropertyName("sPointsInThePaint")]
    public int PointsInPaint { get; set; }

    [JsonPropertyName("sPointsInThePaintMade")]
    public int PointsInPaintMade { get; set; }

    [JsonPropertyName("sPointsSecondChance")]
    public int SecondChancePoints { get; set; }

    [JsonPropertyName("sSecondChancePointsMade")]
    public int SecondChancePointsMade { get; set; }

    [JsonPropertyName("sReboundsTotal")]
    public int TotalRebounds { get; set; }

    [JsonPropertyName("sSteals")]
    public int Steals { get; set; }

    [JsonPropertyName("sThreePointersAttempted")]
    public int ThreePointersAttempted { get; set; }

    [JsonPropertyName("sThreePointersMade")]
    public int ThreePointersMade { get; set; }

    [JsonPropertyName("sThreePointersPercentage")]
    public double ThreePointersPercentage { get; set; }
    
    public string ThreePointersDisplay => $"{ThreePointersMade}/{ThreePointersAttempted}";

    [JsonPropertyName("sTurnovers")]
    public int Turnovers { get; set; }

    [JsonPropertyName("sTwoPointersAttempted")]
    public int TwoPointersAttempted { get; set; }

    [JsonPropertyName("sTwoPointersMade")]
    public int TwoPointersMade { get; set; }

    [JsonPropertyName("sTwoPointersPercentage")]
    public double TwoPointersPercentage { get; set; }

    [JsonPropertyName("sPointsFromTurnovers")]
    public int PointsFromTurnovers { get; set; }
}

public class PlayerStats
{
    [JsonPropertyName("sAssists")]
    public int Assists { get; set; }

    [JsonPropertyName("sReboundsDefensive")]
    public int DefensiveRebounds { get; set; }

    [JsonPropertyName("sPoints")]
    public int Points { get; set; }

    [JsonPropertyName("sPointsFastBreak")]
    public int FastBreakPoints { get; set; }

    [JsonPropertyName("sPointsInThePaint")]
    public int PointsInPaint { get; set; }

    [JsonPropertyName("sPointsInThePaintMade")]
    public int PointsInPaintMade { get; set; }

    [JsonPropertyName("sPointsSecondChance")]
    public int SecondChancePoints { get; set; }

    [JsonPropertyName("sReboundsOffensive")]
    public int OffensiveRebounds { get; set; }

    [JsonPropertyName("sSteals")]
    public int Steals { get; set; }

    [JsonPropertyName("sThreePointersAttempted")]
    public int ThreePointersAttempted { get; set; }

    [JsonPropertyName("sThreePointersMade")]
    public int ThreePointersMade { get; set; }

    [JsonPropertyName("sTurnovers")]
    public int Turnovers { get; set; }

    [JsonPropertyName("sTwoPointersMade")]
    public int TwoPointersMade { get; set; }

    [JsonPropertyName("sMinutes")]
    public double Minutes { get; set; }

    [JsonPropertyName("sBlocks")]
    public int Blocks { get; set; }

    [JsonPropertyName("sBlocksReceived")]
    public int BlocksReceived { get; set; }

    [JsonPropertyName("sFastBreakPointsMade")]
    public int FastBreakPointsMade { get; set; }

    [JsonPropertyName("sFieldGoalsAttempted")]
    public int FieldGoalsAttempted { get; set; }

    [JsonPropertyName("sFieldGoalsMade")]
    public int FieldGoalsMade { get; set; }

    [JsonPropertyName("sFoulsOn")]
    public int FoulsDrawn { get; set; }

    [JsonPropertyName("sFoulsPersonal")]
    public int PersonalFouls { get; set; }

    [JsonPropertyName("sFoulsTechnical")]
    public int TechnicalFouls { get; set; }

    [JsonPropertyName("sFreeThrowsAttempted")]
    public int FreeThrowsAttempted { get; set; }

    [JsonPropertyName("sFreeThrowsMade")]
    public int FreeThrowsMade { get; set; }

    [JsonPropertyName("sTwoPointersAttempted")]
    public int TwoPointersAttempted { get; set; }

    [JsonPropertyName("sReboundsTotal")]
    public int TotalRebounds { get; set; }

    [JsonPropertyName("sTwoPointersPercentage")]
    public double TwoPointersPercentage { get; set; }

    [JsonPropertyName("sThreePointersPercentage")]
    public double ThreePointersPercentage { get; set; }

    [JsonPropertyName("sPlusMinusPoints")]
    public int PlusMinusPoints { get; set; }

    [JsonPropertyName("sFreeThrowsPercentage")]
    public double FreeThrowsPercentage { get; set; }

    [JsonPropertyName("sFieldGoalsPercentage")]
    public double FieldGoalsPercentage { get; set; }

    [JsonPropertyName("sEfficiency")]
    public double Efficiency { get; set; }

    [JsonPropertyName("pno")]
    public int PlayerNumber { get; set; }
    
    public string Linescore() => $"{PlayerNumber} {Points}pts {TotalRebounds}reb {Assists}ast {Steals}stl {Blocks}blk {Turnovers}to";
}
namespace NCAALiveStatsListener.Messages;

using System.Collections.Generic;
using System.Text.Json.Serialization;

public class BoxScore
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
    [JsonPropertyName("team")]
    public TeamStats TeamStats { get; set; }

    [JsonPropertyName("players")]
    public List<PlayerStats> Players { get; set; }
}

public class TeamStats
{
    [JsonPropertyName("sAssists")]
    public int Assists { get; set; }

    [JsonPropertyName("sMinutes")]
    public double Minutes { get; set; }

    [JsonPropertyName("sPointsInThePaint")]
    public int PointsInPaint { get; set; }

    [JsonPropertyName("sPointsInThePaintMade")]
    public int PointsInPaintMade { get; set; }

    [JsonPropertyName("sPointsSecondChance")]
    public int SecondChancePoints { get; set; }

    [JsonPropertyName("sReboundsDefensive")]
    public int DefensiveRebounds { get; set; }

    [JsonPropertyName("sReboundsOffensive")]
    public int OffensiveRebounds { get; set; }

    [JsonPropertyName("sFreeThrowsMade")]
    public int FreeThrowsMade { get; set; }

    [JsonPropertyName("sSecondChancePointsMade")]
    public int SecondChancePointsMade { get; set; }

    [JsonPropertyName("sSteals")]
    public int Steals { get; set; }

    [JsonPropertyName("sThreePointersAttempted")]
    public int ThreePointersAttempted { get; set; }

    [JsonPropertyName("sThreePointersMade")]
    public int ThreePointersMade { get; set; }

    [JsonPropertyName("sTurnovers")]
    public int Turnovers { get; set; }

    [JsonPropertyName("sTwoPointersAttempted")]
    public int TwoPointersAttempted { get; set; }

    [JsonPropertyName("sFreeThrowsAttempted")]
    public int FreeThrowsAttempted { get; set; }

    [JsonPropertyName("sFastBreakPointsMade")]
    public int FastBreakPointsMade { get; set; }

    [JsonPropertyName("sBlocks")]
    public int Blocks { get; set; }

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

    [JsonPropertyName("sTwoPointersMade")]
    public int TwoPointersMade { get; set; }

    [JsonPropertyName("sThreePointersPercentage")]
    public double ThreePointersPercentage { get; set; }

    [JsonPropertyName("sPoints")]
    public int Points { get; set; }

    [JsonPropertyName("sFreeThrowsPercentage")]
    public double FreeThrowsPercentage { get; set; }

    [JsonPropertyName("sFieldGoalsPercentage")]
    public double FieldGoalsPercentage { get; set; }

    [JsonPropertyName("sTwoPointersPercentage")]
    public double TwoPointersPercentage { get; set; }

    [JsonPropertyName("sEfficiency")]
    public int Efficiency { get; set; }

    [JsonPropertyName("sReboundsTotal")]
    public int TotalRebounds { get; set; }
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
    public int Efficiency { get; set; }

    [JsonPropertyName("pno")]
    public int PlayerNumber { get; set; }
    
    public string Linescore() => $"{PlayerNumber} {Points}pts {TotalRebounds}reb {Assists}ast {Steals}stl {Blocks}blk {Turnovers}to";
}
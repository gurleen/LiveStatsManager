namespace NCAALiveStats.Objects;

public enum Stat
{
    GamesPlayed,
    AvgMinutes,
    AvgFouls,
    TechnicalFouls,
    Ejections,
    DoubleDoubles,
    TripleDoubles,
    PER,
    Minutes,
    Rebounds,
    Fouls,
    ReboundsPerGame,
    
    PointsPerGame,
    FGMadePerGame,
    FGAttemptedPerGame,
    FieldGoalPercentage,
    ThreePointersMadePerGame,
    ThreePointersAttemptedPerGame,
    ThreePointPercentage,
    FreeThrowsMadePerGame,
    FreeThrowsAttemptedPerGame,
    FreeThrowPercentage,
    AssistsPerGame,
    TurnoversPerGame,
    Points,
    FieldGoalsMade,
    FieldGoalsAttempted,
    ThreePointersMade,
    ThreePointersAttempted,
    FreeThrowsMade,
    FreeThrowsAttempted,
    Assists,
    Turnovers,
    
    StealsPerGame,
    BlocksPerGame,
    Steals,
    Blocks,
}

public static class StatExtensions
{
    public static string ToShortString(this Stat stat) => stat switch
    {
        Stat.GamesPlayed => "GP",
        Stat.AvgMinutes => "MPG",
        Stat.AvgFouls => "FPG",
        Stat.TechnicalFouls => "TF",
        Stat.Ejections => "EJ",
        Stat.DoubleDoubles => "DD",
        Stat.TripleDoubles => "TD",
        Stat.PER => "PER",
        Stat.Minutes => "MIN",
        Stat.Rebounds => "REB",
        Stat.Fouls => "PF",
        Stat.ReboundsPerGame => "REB/G",
        Stat.PointsPerGame => "PTS/G",
        Stat.FGMadePerGame => "FGM/G",
        Stat.FGAttemptedPerGame => "FGA/G",
        Stat.FieldGoalPercentage => "FG%",
        Stat.ThreePointersMadePerGame => "3PM/G",
        Stat.ThreePointersAttemptedPerGame => "3PA/G",
        Stat.ThreePointPercentage => "3P%",
        Stat.FreeThrowsMadePerGame => "FTM/G",
        Stat.FreeThrowsAttemptedPerGame => "FTA/G",
        Stat.FreeThrowPercentage => "FT%",
        Stat.AssistsPerGame => "AST/G",
        Stat.TurnoversPerGame => "TO/G",
        Stat.Points => "PTS",
        Stat.FieldGoalsMade => "FGM",
        Stat.FieldGoalsAttempted => "FGA",
        Stat.ThreePointersMade => "3PM",
        Stat.ThreePointersAttempted => "3PA",
        Stat.FreeThrowsMade => "FTM",
        Stat.FreeThrowsAttempted => "FTA",
        Stat.Assists => "AST",
        Stat.Turnovers => "TO",
        Stat.StealsPerGame => "STL/G",
        Stat.BlocksPerGame => "BLK/G",
        Stat.Steals => "STL",
        Stat.Blocks => "BLK",
        _ => string.Empty
    };
}

public static class StatLists
{
    public static List<Stat> GameStats => 
    [
        Stat.Points,
        Stat.FieldGoalsMade,
        Stat.FieldGoalsAttempted,
        Stat.FieldGoalPercentage,
        Stat.ThreePointersMade,
        Stat.ThreePointersAttempted,
        Stat.ThreePointPercentage,
        Stat.FreeThrowsMade,
        Stat.FreeThrowsAttempted,
        Stat.FreeThrowPercentage,
        Stat.Rebounds,
        Stat.Assists,
        Stat.Turnovers,
        Stat.Steals,
        Stat.Blocks,
        Stat.Fouls
    ];

    public static List<Stat> SeasonStats =>
    [
        Stat.GamesPlayed,
        Stat.AvgMinutes,
        Stat.AvgFouls,
        Stat.TechnicalFouls,
        Stat.Ejections,
        Stat.DoubleDoubles,
        Stat.TripleDoubles,
        Stat.PER,
        Stat.Minutes,
        Stat.Rebounds,
        Stat.Fouls,
        Stat.ReboundsPerGame,
        Stat.PointsPerGame,
        Stat.FGMadePerGame,
        Stat.FGAttemptedPerGame,
        Stat.FieldGoalPercentage,
        Stat.ThreePointersMadePerGame,
        Stat.ThreePointersAttemptedPerGame,
        Stat.ThreePointPercentage,
        Stat.FreeThrowsMadePerGame,
        Stat.FreeThrowsAttemptedPerGame,
        Stat.FreeThrowPercentage,
        Stat.AssistsPerGame,
        Stat.TurnoversPerGame,
        Stat.Points,
        Stat.Assists,
        Stat.Rebounds
    ];
}
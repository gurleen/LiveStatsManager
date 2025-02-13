using System.Text.Json;
using Shared.Extensions;
using Shared.Objects;

namespace LiveStatsManager.Components.Pages.Wrestling;

public enum WrestlingWeightClass
{
    _125,
    _133,
    _141,
    _149,
    _157,
    _165,
    _174,
    _184,
    _197,
    _285
}

public static class WrestlingWeightClassExtensions
{
    public static string ToStringValue(this WrestlingWeightClass cls)
    {
        return cls switch
        {
            WrestlingWeightClass._125 => "125lb",
            WrestlingWeightClass._133 => "133lb",
            WrestlingWeightClass._141 => "141lb",
            WrestlingWeightClass._149 => "149lb",
            WrestlingWeightClass._157 => "157lb",
            WrestlingWeightClass._165 => "165lb",
            WrestlingWeightClass._174 => "174lb",
            WrestlingWeightClass._184 => "184lb",
            WrestlingWeightClass._197 => "197lb",
            WrestlingWeightClass._285 => "285lb",
            _ => string.Empty
        };
    }
}

public record Matchup(Player HomePlayer, Player AwayPlayer, WrestlingWeightClass weight);

public static class MatchupExtensions
{
    public static string ToStringValue(this Matchup matchup)
    {
        return $"{matchup.weight.ToStringValue()} - {matchup.HomePlayer.FullName} vs {matchup.AwayPlayer.FullName}";
    }
}

public class WrestlingViewModel
{
    private readonly string LiveDataDirectory;
    public List<Matchup> Matchups { get; set; } = [];
    public Dictionary<WrestlingWeightClass, Matchup?> _probableStarters = new();
    public Player? FormHomeWrestler { get; set; }
    public string FormHomeWrestlerRank { get; set; } = string.Empty;
    public string HomeWrestlerDisplay => FormHomeWrestlerRank == string.Empty 
        ? FormHomeWrestler?.FullName ?? string.Empty 
        : $"#{FormHomeWrestlerRank} {FormHomeWrestler?.FullName}";
    public Player? FormAwayWrestler { get; set; }
    public string FormAwayWrestlerRank { get; set; } = string.Empty;
    public string AwayWrestlerDisplay => FormAwayWrestlerRank == string.Empty 
        ? FormAwayWrestler?.FullName ?? string.Empty 
        : $"#{FormAwayWrestlerRank} {FormAwayWrestler?.FullName}";
    public WrestlingWeightClass FormWeightClass { get; set; }
    public Matchup? SelectedMatchup { get; set; }
    public int HomeScore { get; set; } = 0;
    public int AwayScore { get; set; } = 0;
    public int PeriodNumber { get; set; } = 1;
    public string Period => PeriodNumber.DisplayWithSuffix();

    public WrestlingViewModel(SettingsProvider settings)
    {
        LiveDataDirectory = settings.LiveDataDirectory;
        foreach(var weight in Enum.GetValues<WrestlingWeightClass>())
        {
            _probableStarters.Add(weight, null);
        }
    }
    
    public void AddMatchup(Player homePlayer, Player awayPlayer, WrestlingWeightClass weight)
    {
        Matchups.Add(new Matchup(homePlayer, awayPlayer, weight));
    }

    public void AddFormMatchup()
    {
        AddMatchup(FormHomeWrestler!, FormAwayWrestler!, FormWeightClass);
    }

    public void RemoveMatchup()
    {
        if(SelectedMatchup is null) return;
        Matchups.Remove(SelectedMatchup);
    }
    
    public List<WrestlingWeightClass> WeightClasses => Enum.GetValues<WrestlingWeightClass>()
        .ToList();
    
    private record CsvData(string HomeWrestler, string HomeScore, 
        string AwayWrestler, string AwayScore, string Period);
    
    private CsvData GetCsvData() => new CsvData(HomeWrestlerDisplay, HomeScore.ToString(),
        AwayWrestlerDisplay, AwayScore.ToString(), Period);
    
    public async Task IncrementHomeScore(int amount)
    {
        if(HomeScore + amount < 0) return;
        HomeScore += amount;
        await WriteToCsv();
    }
    
    public async Task IncrementAwayScore(int amount)
    {
        if(AwayScore + amount < 0) return;
        AwayScore += amount;
        await WriteToCsv();
    }

    public async Task UpdatePeriod(int periodNum)
    {
        PeriodNumber = periodNum;
        await WriteToCsv();
    }

    public async Task WriteToCsv()
    {
        var data = GetCsvData();
        var jsonString = JsonSerializer.Serialize(data);
        var outputFile = Path.Join(LiveDataDirectory, "Wrestling.json");
        await using var fileWriter = new StreamWriter(outputFile);
        await fileWriter.WriteAsync(jsonString);
    }
}
using LiveStatsManager.Models;
using LiveStatsManager.Utilities;
using NCAALiveStats;
using NCAALiveStats.ExternalData;
using NCAALiveStats.ExternalData.Sidearm;
using Optional;
using Optional.Collections;
using Raffinert.FuzzySharp;
using Shared.Enums;
using Shared.Objects;

namespace LiveStatsManager.Services;

public class TeamDataRepository
{
    public readonly List<Team> Teams = [];
    public readonly List<Player> Players = [];
    public readonly List<PlayerStats> StatLines = [];
    public readonly Dictionary<(Sport, string), TeamStats> TeamStats = new();
    
    private static (Sport, string) TeamKey(Sport sport, string id) => (sport, id);
    
    public Team GetTeam(string teamId) => Teams.First(t => t.Info.Id == teamId);
    public Option<Player> GetPlayer(int playerId) => Players.FirstOrNone(p => p.Id == playerId.ToString());
    public List<Player> PlayersForTeam(Sport sport, string teamId) => 
        Players.Where(p => p.TeamId == teamId && p.Sport == sport).ToList();
    
    public PlayerStats? GetPlayerStats(string playerId, Sport sport) => 
        StatLines.FirstOrDefault(s => s.PlayerId == playerId && s.Sport == sport);

    public IEnumerable<PlayerWithStats> StatsBySport(Sport sport)
    {
        return StatLines.Where(s => s.Sport == sport)
            .Join(Players.Where(p => p.Sport == sport), ps => ps.PlayerId, 
                p => p.Id, 
                (ps, p) => new PlayerWithStats(p, ps))
            .DistinctBy(p => p.Player.Id);
    }

    public List<PlayerStats> GetPlayerLines(int playerId)
    {
        var player = GetPlayer(playerId);
        return player.Match(
            p => StatLines.Where(l => l.PlayerId == p.Id).ToList(),
            () => []);
    }

    private void Clear()
    {
        Teams.Clear();
        Players.Clear();
        StatLines.Clear();
    }

    public async Task UpdateSport(Sport sport)
    {
        var parsed = await ESPNConverter.GetPlayersAndStats(sport);
        Players.AddRange(parsed.Players);
        StatLines.AddRange(parsed.Stats);
        var records = await ESPNConverter.GetTeamStats(sport);
        foreach (var record in records)
        {
            var key = TeamKey(record.Sport, record.Id);
            TeamStats[key] = record;
        }
    }

    public async Task UpdateAll()
    {
        Clear();
        var localTeams = new LocalTeamData();
        var teams = await ESPNConverter.GetTeams();
        var customTeams = localTeams.CustomTeams;
        Teams.AddRange(teams);
        Teams.AddRange(customTeams);
        await UpdateSport(Sport.WomensBasketball);
        await UpdateSport(Sport.MensBasketball);
    }
    
    public async Task<List<Team>> SearchTeams(string query)
    {
        return await Task.Run(() =>
        {
            return Process.ExtractAll(query, Teams, t => t.Name)
                .OrderByDescending(r => r.Score)
                .Take(5)
                .Select(r => r.Value)
                .ToList();
        });
    }
    
    public FullTeam GetFullTeam(Sport sport, string teamId)
    {
        var teamKey = TeamKey(sport, teamId);
        var team = GetTeam(teamId);
        var stats = TeamStats.TryGetValue(teamKey, out var stat) 
            ? stat : Shared.Objects.TeamStats.Empty();
        var players = Players.Where(p => p.Sport == sport && p.TeamId == teamId).ToList();
        return new FullTeam { Info = team.Info, Stats = stats, Players = players};
    }
    
    public async Task<List<PlayerHeadshot>> GetHeadshots(Sport sport, string teamId)
    {
        var loader = new SidearmLoader(GetTeam(teamId), sport);
        return await loader.LoadHeadshots();
    }

    public async Task GetRosterFromSidearm(Sport sport, string teamId)
    {
        var loader = new SidearmLoader(GetTeam(teamId), sport);
        var players = await loader.LoadPlayers();
        Players.AddRange(players);
    }

    public async Task<List<GameStatus>> GetScoreboard(Sport sport)
    {
        var response = await ESPNConverter.GetScoreboard(sport);
        var games = response.Events
            .Select(comp => new GameStatus
            {
                Id = comp.Id,
                HomeTeam = GetTeam(comp.Competition.Competitors.First(c => c.HomeAway == "home").Id),
                HomeScore = comp.Competition.Competitors.First(c => c.HomeAway == "home").Score,
                AwayTeam = GetTeam(comp.Competition.Competitors.First(c => c.HomeAway == "away").Id),
                AwayScore = comp.Competition.Competitors.First(c => c.HomeAway == "away").Score,
                ClockDisplay = comp.Competition.Status.ClockDisplay,
                Status = comp.Competition.Status.Type.ShortDetail
            })
            .OrderBy(gs => gs.Id)
            .ToList();
        return games;
    }

    public IEnumerable<FullTeam> GetStandings(Sport sport, int confId) =>
        Teams
            .Where(t => t.Info.Conference.EspnId == confId)
            .Select(t => GetFullTeam(sport, t.Id))
            .OrderByDescending(ft => ft.Stats.Conference.WinPercentage)
            .ThenByDescending(ft => ft.Stats.Overall.WinPercentage);
}
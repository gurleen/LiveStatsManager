using NCAALiveStats;
using NCAALiveStats.ExternalData.ESPN;
using NCAALiveStats.ExternalData.ESPN.Objects;
using Optional;
using Shared.Enums;
using Shared.Objects;

namespace LiveStatsManager.Utilities;

public record PlayersAndStats
{
    public required List<Player> Players { get; init; }
    public required List<PlayerStats> Stats { get; init; }
}

public class ESPNConverter
{
    private static Player BuildPlayer(ESPNPlayer player, ESPNPlayerStatsRecord stats, Sport sport)
    {
        return new Player
        {
            Id = player.Id,
            TeamId = stats.Player.TeamId ?? string.Empty,
            Sport = sport,
            FirstName = player.FirstName,
            LastName = player.LastName,
            JerseyNumber = player.Jersey,
            Position = stats.Player.Position.Abbreviation,
            Experience = player.Experience?.Abbreviation ?? string.Empty,
            Height = player.DisplayHeight ?? string.Empty,
            Hometown = player.BirthPlace?.City ?? string.Empty
        };
    }

    private static Player BuildPlayer(ESPNPlayer player, Sport sport)
    {
        return new Player
        {
            Id = player.Id,
            TeamId = string.Empty,
            Sport = sport,
            FirstName = player.FirstName,
            LastName = player.LastName,
            JerseyNumber = player.Jersey,
            Position = string.Empty,
            Experience = player.Experience?.Abbreviation ?? string.Empty,
            Height = player.DisplayHeight ?? string.Empty,
            Hometown = player.BirthPlace?.City ?? string.Empty
        };
    }

    private static PlayerStats BuildStatLine(ESPNPlayerStatsRecord stats, Sport sport)
    {
        var playerStats = new PlayerStats(stats.Player.Id, stats.Player.TeamId ?? string.Empty, sport);
        var allStats = stats.AllStats();
        foreach (var stat in allStats)
        {
            playerStats.SetStat(stat.Item1, stat.Item2);
        }
        return playerStats;
    }

    public static async Task<PlayersAndStats> GetPlayersAndStats(Sport sport)
    {
        var loader = new ESPNLoader(sport);
        var players = await loader.Players();
        var playerStats = (await loader.PlayerStats()).ToList();

        var joined = players.Join(playerStats, p => p.Id, s => s.Player.Id,
            (p, s) => new { Player = p, Stats = s });

        var parsedPlayers = new List<Player>();
        var parsedStats = new List<PlayerStats>();
        
        foreach (var pair in joined)
        {
            var player = BuildPlayer(pair.Player, pair.Stats, sport);
            var statLine = BuildStatLine(pair.Stats, sport);
            parsedPlayers.Add(player);
            parsedStats.Add(statLine);
        }
        
        var notMatched = players
            .Where(p => playerStats.All(s => s.Player.Id != p.Id))
            .Select((p) => BuildPlayer(p, sport));
        
        parsedPlayers.AddRange(notMatched);
        
        return new PlayersAndStats
        {
            Players = parsedPlayers,
            Stats = parsedStats
        };
    }
    
    private static Team BuildTeam(ESPNTeam team, Dictionary<string, LocalTeamInfo> localTeamLookup)
    {
        var localInfo = localTeamLookup[team.Id];
        var conference = new ConferenceInfo
        {
            Name = localInfo.ConferenceName,
            Abbreviation = localInfo.ConferenceShortName,
            EspnId = localInfo.ConferenceId
        };
        var info = new TeamInfo
        {
            Id = team.Id,
            SchoolName = team.ShortDisplayName,
            TeamName = team.Name,
            Abbreviation = team.Abbreviation,
            TeamLogo = localInfo.LogoName,
            PrimaryColor = "#" + localInfo.Color,
            SecondaryColor = "#" + localInfo.AlternateColor,
            Conference = conference,
            Website = localInfo.Website
        };
        return new Team { Info = info };
        
    }

    public static async Task<List<Team>> GetTeams()
    {
        var localTeamLookup = new LocalTeamData().Teams.ToDictionary(t => t.EspnId.ToString(), t => t);
        var loader = new ESPNLoader(Sport.MensBasketball);
        var teams = await loader.Teams();
        var espnTeams = teams.Sports.First().Leagues.First().Teams;
        return espnTeams
        .Where(t => localTeamLookup.ContainsKey(t.Team.Id))
            .Select(t => BuildTeam(t.Team, localTeamLookup)).ToList();
    }

    private static TeamStats BuildTeamStats(ESPNConferenceStandingsEntry entry, Sport sport)
    {
        return new TeamStats
        {
            Id = entry.Team.Id,
            Sport = sport,
            Overall = new TeamRecord(Get(ESPNTeamStatType.Wins), Get(ESPNTeamStatType.Losses)),
            Conference = new TeamRecord(Get(ESPNTeamStatType.ConferenceWins), Get(ESPNTeamStatType.ConferenceLosses)),
            Home = new TeamRecord(Get(ESPNTeamStatType.HomeWins), Get(ESPNTeamStatType.HomeLosses)),
        };
        
        int Get(string statName) => (int)entry.GetEntry(statName);
    }
    
    public static async Task<List<TeamStats>> GetTeamStats(Sport sport)
    {
        var loader = new ESPNLoader(sport);
        var standings = await loader.Standings();
        return standings
            .Conferences
            .SelectMany(c => c.Teams)
            .Select(t => BuildTeamStats(t, sport))
            .ToList();
    }
    
    public static async Task<ESPNScoreboardResponse> GetScoreboard(Sport sport)
    {
        var loader = new ESPNLoader(sport);
        return await loader.Scoreboard();
    }
}
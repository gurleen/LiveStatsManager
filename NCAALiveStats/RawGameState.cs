using NCAALiveStats.Messages;

namespace NCAALiveStats;

using ShirtNumber = (TeamSide Side, string Shirt);

public enum TeamSide { Home, Away };

public class RawGameState
{
    private readonly SynchronizationContext _syncContext;
    public event Func<Task> OnUpdate;
    
    public RawGameState()
    {
        _syncContext = _syncContext = SynchronizationContext.Current ?? new SynchronizationContext();
    }

    private void NotifyUpdate()
    {
        _syncContext.Post(_ => OnUpdate?.Invoke(), null);
    }
    public TeamInfoMessage HomeTeam { get; private set; } = new();
    public TeamInfoMessage AwayTeam { get; private set; } = new();
    private readonly Dictionary<ShirtNumber, Player> _players = new();
    private readonly Dictionary<ShirtNumber, PlayerStats> _playerStats = new();
    private TeamStats HomeStats { get; set; } = new();
    private TeamStats AwayStats { get; set; } = new();
    public MatchStatus? Status { get; private set; }

    public int HomeScore => HomeStats.Points;
    public int AwayScore => AwayStats.Points;
    
    public TeamStats GetTeamStats(TeamSide side) => side == TeamSide.Home? HomeStats : AwayStats;

    public PlayerStats GetPlayerStats(TeamSide side, string shirt)
    {
        var key = (side, shirt);
        return _playerStats.TryGetValue(key, out var stats) ? stats : new PlayerStats();
    }
    
    public Dictionary<string, PlayerStats> PlayerStatsByShirt(TeamSide side) =>
        _playerStats.Where(kvp => kvp.Key.Side == side).ToDictionary(kvp => kvp.Key.Shirt, kvp => kvp.Value);
    
    private string ShirtFromPno(TeamSide side, int pno) => 
        _players.First(p => p.Key.Side == side && p.Value.PlayerNumber == pno).Key.Shirt;

    public void UpdateTeams(TeamMessage teams)
    {
        lock (_syncContext)
        {
            HomeTeam = teams.Teams.Find(t =>  t.Detail.IsHomeCompetitor)!;
            AwayTeam = teams.Teams.Find(t => !t.Detail.IsHomeCompetitor)!;
            UpdatePlayers(HomeTeam.Players, TeamSide.Home);
            UpdatePlayers(AwayTeam.Players, TeamSide.Away);
        }
        NotifyUpdate();
    }

    private void UpdatePlayers(List<Player> players, TeamSide side)
    {
        foreach (var player in players)
        {
            var key = new ShirtNumber(side, player.ShirtNumber);
            _players[key] = player;
        }
    }
    
    public void UpdateBoxscore(Boxscore boxscore)
    {
        lock (_syncContext)
        {
            var homeBox = boxscore.Teams.Find(tb => tb.TeamNumber == HomeTeam?.TeamNumber);
            HomeStats = homeBox!.Total.Team;
            UpdateTeamBox(homeBox!, TeamSide.Home);
        
            var awayBox = boxscore.Teams.Find(tb => tb.TeamNumber == AwayTeam?.TeamNumber);
            AwayStats = awayBox!.Total.Team;
            UpdateTeamBox(awayBox!, TeamSide.Away);
        }
        NotifyUpdate();
    }

    private void UpdateTeamBox(TeamBox box, TeamSide team)
    {
        foreach (var playerStats in box.Total.Players)
        {
            var shirtNum = ShirtFromPno(team, playerStats.PlayerNumber);
            var key = new ShirtNumber(team, shirtNum);
            _playerStats[key] = playerStats;
        }
    }
    
    public void UpdateStatus(MatchStatus status)
    {
        Status = status;
        NotifyUpdate();
    }
}
using LiveStatsManager.Models;
using LiveStatsManager.Models.DB;
using LiveStatsManager.Services.DataStore;
using NCAALiveStats;
using NCAALiveStats.ExternalData.Sidearm;
using Shared.Enums;
using Shared.GameState;
using Shared.Objects;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace LiveStatsManager.Services;

public class AppState
{
    private readonly TypedDataStore typedDataStore;
    private readonly TeamDataRepository teamRepo;
    private readonly DatabaseRepository dbRepo;
    private readonly IDataStore store;
    public Sport Sport { get; private set; }
    public Team HomeTeam { get; private set; }
    public FullTeam HomeFullTeam { get; private set; }

    private bool _useHomeTeamSecondaryColor = false;
    public bool UseHomeTeamSecondaryColor
    {
        get => _useHomeTeamSecondaryColor;
        set
        {
            _useHomeTeamSecondaryColor = value;
            PushHomeTeamData();
        }
    }
    public Team AwayTeam { get; private set; }
    public FullTeam AwayFullTeam { get; private set; }
    private bool _useAwayTeamSecondaryColor { get; set; } = false;
    public bool UseAwayTeamSecondaryColor
    {
        get => _useAwayTeamSecondaryColor;
        set
        {
            _useAwayTeamSecondaryColor = value;
            PushAwayTeamData();
        }
    }
    public GraphicsData GraphicsData => new(HomeFullTeam, AwayFullTeam);
    public List<Tuple<string, string>> TextSliderPresets = 
    [
        new("VENUE", "DASKALAKIS ATHLETIC CENTER - PHILADELPHIA, PA")
    ];

    public void SetHomeTeam(Team team)
    {
        SaveState(() => HomeTeam = team);
        HomeFullTeam = teamRepo.GetFullTeam(Sport, team.Id);
        PushHomeTeamData();

    }

    private void PushHomeTeamData()
    {
        var color = UseHomeTeamSecondaryColor 
            ? HomeFullTeam.Info.SecondaryColor : HomeFullTeam.Info.PrimaryColor;
        store.Add("img:Home-Logo", HomeFullTeam.Info.KnockoutLogoUrl);
        store.Add("Home-Team-Name", HomeTeam.Info.TeamName);
        store.Add("Home-School-Name", HomeTeam.Info.SchoolName);
        store.Add("Home-Abbr", HomeTeam.Info.Abbreviation);
    }

    public void SetAwayTeam(Team team)
    {
        SaveState(() => AwayTeam = team);
        AwayFullTeam = teamRepo.GetFullTeam(Sport, team.Id);
        PushAwayTeamData();
    }
    
    private void PushAwayTeamData()
    {
        var color = UseAwayTeamSecondaryColor 
            ? AwayFullTeam.Info.SecondaryColor : AwayFullTeam.Info.PrimaryColor;
        store.Add("img:Away-Logo", AwayFullTeam.Info.KnockoutLogoUrl);
        store.Add("Away-Team-Name", AwayTeam.Info.TeamName);
        store.Add("Away-School-Name", AwayTeam.Info.SchoolName);
        store.Add("Away-Abbr", AwayTeam.Info.Abbreviation);
    }

    public void SetTeamColorUsage(TeamSide side, bool useSecondary)
    {
        if (side == TeamSide.Home)
            UseHomeTeamSecondaryColor = useSecondary;
        else
            UseAwayTeamSecondaryColor = useSecondary;
    }
    
    public void SetSport(Sport sport)
    {
        SaveState(() => Sport = sport);
        typedDataStore.GameState = typedDataStore.GameState with
        {
            Sport = sport
        };
    }

    private async Task<List<PlayerHeadshot>> GetHeadshots(Team team)
    {
        var sidearmLoader = new SidearmLoader(team, Sport);
        return await sidearmLoader.LoadHeadshots();
    }
    
    private void SaveState(Action action)
    {
        action();
        SaveStateToDb();
    }
    
    public AppState(IServiceProvider services)
    {
        teamRepo = services.GetRequiredService<TeamDataRepository>();
        dbRepo = services.GetRequiredService<DatabaseRepository>();
        store = services.GetRequiredService<IDataStore>();
        typedDataStore = services.GetRequiredService<TypedDataStore>();

        var savedState = dbRepo.GetSingleton<SavedAppState>();
        savedState.Match(SetTeamsFromDb, SetDefaultTeams);
    }

    private void SaveStateToDb()
    {
        var state = new SavedAppState
        {
            HomeTeamId = HomeTeam.Id,
            AwayTeamId = AwayTeam.Id,
            Sport = Sport
        };
        dbRepo.InsertSingleton(state);
    }

    private void SetTeamsFromDb(SavedAppState state)
    {
        Sport = state.Sport;
        HomeTeam = teamRepo.GetTeam(state.HomeTeamId);
        AwayTeam = teamRepo.GetTeam(state.AwayTeamId);
        HomeFullTeam = teamRepo.GetFullTeam(Sport, HomeTeam.Id);
        AwayFullTeam = teamRepo.GetFullTeam(Sport, AwayTeam.Id);
    }

    private void SetDefaultTeams()
    {
        Sport = Sport.MensBasketball;
        HomeTeam = teamRepo.Teams[0];
        AwayTeam = teamRepo.Teams[1];
    }
}
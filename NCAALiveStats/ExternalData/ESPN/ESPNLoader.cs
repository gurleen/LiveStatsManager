using Flurl;
using Flurl.Http;
using NCAALiveStats.ExternalData.ESPN.Objects;
using Optional;
using Shared.Enums;

namespace NCAALiveStats.ExternalData.ESPN;

public class ESPNLoader
{
    private readonly Sport SelectedSport;
    private readonly string AppDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    private const string CacheDir = "ESPNStatsCache";
    private const string ApiDateFormat = "yyyyMMdd";
    private string CacheFile(string fileName) => Path.Join(AppDataDir, CacheDir, fileName);
    private string SportSlug => SelectedSport == Sport.MensBasketball? "mens-college-basketball" : "womens-college-basketball";
    private string BaseUrl => $"https://site.api.espn.com/apis/site/v2/sports/basketball/{SportSlug}";
    private string BaseUrlV3 => $"https://site.api.espn.com/apis/common/v3/sports/basketball/{SportSlug}";
    private string BaseUrlSports => $"https://site.api.espn.com/apis/v2/sports/basketball/{SportSlug}";
    private const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/141.0.0.0 Safari/537.36";
    
    private string BaseUrlByVersion(bool useV3) => useV3 ? BaseUrlV3 : BaseUrl;
    private Url BuildUrl(string path, bool useV3 = false) => BaseUrlByVersion(useV3).AppendPathSegment(path);

    public ESPNLoader(Sport sport)
    {
        SelectedSport = sport;
        Directory.CreateDirectory(Path.Join(AppDataDir, CacheDir));
    }
    
    private static async Task<IEnumerable<T>> GetPaginated<T>(string url, Func<T, string?> nextChecker)
    {
        var response = await url.GetJsonAsync<T>();
        
        var next = nextChecker(response);
        if (next == null) return [response];
        
        var nextResponse = await GetPaginated(next, nextChecker);
        return new[] { response }.Concat(nextResponse);
    }
    
    private static async Task<CacheWrapper<T>?> TryLoadCachedData<T>(string fileName)
    {
        try
        {
            var json = await File.ReadAllTextAsync(fileName);
            return json.FromJson<T>();
        }
        catch (Exception e)
        {
            return null;
        }
    }
    
    private async Task<T?> CheckForCachedDataAsync<T>(string fileName, TimeSpan maxAge)
    {
        var fullPath = CacheFile(fileName + ".json");
        if(!File.Exists(fullPath)) return default;
        var cachedFile = await TryLoadCachedData<T>(fullPath);
        if (cachedFile == null) return default;
        return cachedFile.IsExpired(maxAge) ? default : cachedFile.Data;
    }
    
    private async Task SaveDataToCacheAsync<T>(string fileName, T data)
    {
        var fullPath = CacheFile(fileName + ".json");
        var cacheWrapper = new CacheWrapper<T>(data);
        var json = cacheWrapper.ToJson();
        await File.WriteAllTextAsync(fullPath, json);
    }
    
    public async Task<ESPNTeamResponse> Teams() => await BuildUrl(Routes.TeamsUrl)
        .AppendQueryParam("limit", "1000")
        .GetJsonAsync<ESPNTeamResponse>();

    public async Task<List<ESPNPlayer>> Players()
    {
        var cacheFileName = $"{nameof(ESPNPlayer)}_{SportSlug}";
        var cached = await CheckForCachedDataAsync<List<ESPNPlayer>>(cacheFileName, 
            TimeSpan.FromDays(7));
        if(cached != null) return cached;

        var response = await Routes.PlayersUrl(SelectedSport)
            .AppendQueryParam("limit", "20000")
            .AppendQueryParam("active", "true")
            .WithHeader("User-Agent", UserAgent)
            .GetJsonAsync<ESPNPlayerResponse>();
        

        await SaveDataToCacheAsync(cacheFileName, response.Players);
        return response.Players;
    }
    
    public async Task<IEnumerable<ESPNPlayerStatsRecord>> PlayerStats()
    {
        var cacheFileName = $"{nameof(ESPNPlayerStatsRecord)}_{SportSlug}";
        var cached = await CheckForCachedDataAsync<IEnumerable<ESPNPlayerStatsRecord>>(cacheFileName, 
            TimeSpan.FromHours(12));
        if(cached != null) return cached;
        
        const string queryString =
            @"?conference=50&contentorigin=espn&isqualified=false&sort=offensive.avgPoints%3Adesc&limit=1000";
        var url = BuildUrl(Routes.PlayerStatsUrl, true) + queryString;
        var responses = await GetPaginated<ESPNPlayerStats>(url, 
            r => r.Pagination.Next);
        var records = responses.SelectMany(r => r.Records).ToList();
        
        await SaveDataToCacheAsync(cacheFileName, records);
        return records;
    }

    public async Task<ESPNStandingsResponse> Standings()
    {
        return await BaseUrlSports.AppendPathSegment(Routes.StandingsUrl)
            .GetJsonAsync<ESPNStandingsResponse>();
    }
    public async Task<ESPNScoreboardResponse> Scoreboard(string groups = "10")
    {
        var dates = DateTime.Now.ToString(ApiDateFormat);
        return await BaseUrl
            .AppendPathSegment(Routes.ScoreboardUrl)
            .AppendQueryParam(nameof(groups), groups)
            .AppendQueryParam(nameof(dates), dates)
            .GetJsonAsync<ESPNScoreboardResponse>();
    }
}
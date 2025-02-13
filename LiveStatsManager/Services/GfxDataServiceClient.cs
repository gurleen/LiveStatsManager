using Flurl;
using Flurl.Http;

namespace LiveStatsManager.Services;

public class GfxDataServiceClient(SettingsProvider settings)
{
    private string BaseUrl => "";

    public async Task<List<KeyValuePair<string, string>>> GetDataStore()
    {
        var response = await BaseUrl
            .AppendPathSegment(Routes.DataStoreRoute)
            .GetJsonAsync<Dictionary<string, string>>();
        return response.ToList();
    }
    
    public async Task UpdateDataStore(string key, string value) =>
        await BaseUrl
            .AppendPathSegment(Routes.UpdateStore)
            .PostJsonAsync(new { key, value });

    public async Task ToggleDataStore(string key) =>
        await BaseUrl
            .AppendPathSegment(Routes.ToggleStore)
            .AppendPathSegment(key)
            .PostAsync();

    private static class Routes
    {
        public const string DataStoreRoute = "/dataStore";
        public const string UpdateStore = "/dataStore/update";
        public const string ToggleStore = "/dataStore/toggle";
    }
}
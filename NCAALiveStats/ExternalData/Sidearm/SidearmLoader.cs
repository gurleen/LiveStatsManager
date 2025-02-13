using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io;
using Flurl;
using Flurl.Http;
using NCAALiveStats.Objects;

namespace NCAALiveStats.ExternalData;

public class PlayerHeadshot(string Number, string ImageUrl);

public class SidearmLoader(Team team, Sport sport)
{
    private const string UserAgent =
        "\"Mozilla/5.0 (iPad; CPU OS 12_2 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148\"";
    private string SportSlug => sport == Sport.MensBasketball ? "mens-basketball" : "womens-basketball";
    private string ApiUrl => $"https://{team.Info.Website}/api/v2/Rosters/bySport/{SportSlug}";

    private string RosterUrl => $"https://{team.Info.Website}"
        .AppendPathSegment("sports")
        .AppendPathSegment(SportSlug)
        .AppendPathSegment("roster");

    private static IBrowsingContext GetContext()
    {
        var requester = new DefaultHttpRequester();
        requester.Headers["User-Agent"] = UserAgent;
        var config = Configuration.Default
            .With(requester)
            .WithDefaultLoader();
        return BrowsingContext.New(config);
    }

    private static PlayerHeadshot? ParsePlayer(IElement element)
    {
        var imageTag = element.QuerySelector("source");
        var image = imageTag?.GetAttribute("srcset");
        var number = element.QuerySelector(".s-stamp__text")?.TextContent;
        if(image is null || number is null)
        {
            return null;
        }

        return new PlayerHeadshot(number, image);
    }

    private async Task<List<PlayerHeadshot>> TryApiUrl()
    {
        await ApiUrl.GetJsonAsync<>()
    }
    
    public async Task<List<PlayerHeadshot>> LoadHeadshots()
    {
        using var context = GetContext();
        var document = await context.OpenAsync(RosterUrl);
        var playerArea = document.QuerySelector("c-rosterpage__players");
        var players = playerArea?.QuerySelectorAll(".s-person-thumbnail");
        return players?.Select(ParsePlayer)
            .Where(p => p is not null)
            .Select(p => p!)
            .ToList() ?? [];
    }
}
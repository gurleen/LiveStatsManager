using System.IO.Compression;
using System.Text.RegularExpressions;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io;
using AngleSharp.Text;
using Flurl;
using Flurl.Http;
using NameParser;
using Shared.Enums;
using Shared.Extensions;
using Shared.Objects;

namespace NCAALiveStats.ExternalData.Sidearm;

public class PlayerHeadshot(string id, string imageUrl)
{
    public string Id { get; } = id;
    public string ImageUrl { get; } = imageUrl;

    public async Task<byte[]> Download()
    {
        return await ImageUrl
            .WithHeader("User-Agent", SidearmLoader.UserAgent)
            .GetBytesAsync();
    }
};

public partial class SidearmLoader(Team team, Sport sport)
{
    public const string UserAgent =
        "\"Mozilla/5.0 (iPad; CPU OS 12_2 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148\"";

    private string SportSlug => sport switch
    {
        Sport.Wrestling => "wrestling",
        Sport.MensBasketball => "mens-basketball",
        _ => "womens-basketball"
    };
    
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

    private async Task<List<PlayerHeadshot>?> TryApiUrl()
    {
        try
        {
            var response = await GetSidearmApiRoster();
            return response.Players
                .Select(p => 
                    new PlayerHeadshot(p.IdForSport(sport), p.Image.Url))
                .ToList();
        }
        catch (Exception e)
        {
            return null;
        }
    }

    private async Task<SidearmRosterResponse> GetSidearmApiRoster()
    {
        var response = await ApiUrl
            .WithHeader("User-Agent", UserAgent)
            .GetJsonAsync<SidearmRosterResponse>();
        return response;
    }

    private async Task<List<Player>> GetPlayersFromSidearmApi()
    {
        var response = await GetSidearmApiRoster();
        var index = 0;
        return response.Players
            .Select(p => new Player
            {
                Id = index++.ToString(),
                FirstName = p.FirstName,
                LastName = p.LastName,
                JerseyNumber = p.JerseyNumber,
                Position = string.Empty,
                Experience = string.Empty,
                Height = string.Empty,
                Hometown = string.Empty,
                TeamId = team.Id,
                Sport = sport
            })
            .ToList();
    }

    private static string CleanName(string rawName)
    {
        return NameCleaner().Replace(rawName, string.Empty);
    }

    private async Task<List<Player>> GetSidearmHtmlRoster()
    {
        using var context = GetContext();
        var document = await context.OpenAsync(RosterUrl);
        var players = document.QuerySelectorAll("div.sidearm-roster-player-name");
        var numbers = document.QuerySelectorAll("span.sidearm-roster-player-jersey-number");
        var positions = document.QuerySelectorAll("div.sidearm-roster-player-position > span.text-bold");
        
        var index = 0;

        return players.Select(playerElement => playerElement.TextContent.CollapseAndStrip())
            .Select(CleanName)
            .Select(nameText => new HumanName(nameText))
            .Select(fullName => new Player
            {
                Id = index.ToString(),
                FirstName = fullName.First,
                LastName = fullName.Last,
                JerseyNumber = numbers[index].TextContent.CollapseAndStrip(),
                Position = positions[index++].TextContent.CollapseAndStrip(),
                Experience = string.Empty,
                Height = string.Empty,
                Hometown = string.Empty,
                TeamId = team.Id,
                Sport = sport
            })
            .ToList();
    }
    
    public async Task<List<Player>> LoadPlayers()
    {
        try
        {
            var fromApi = await GetPlayersFromSidearmApi();
            if (fromApi.Count > 1)
                return fromApi;
        }
        catch (Exception e)
        {
            // ignored
        }

        return await GetSidearmHtmlRoster();
    }

    private string PlayerIdElement => sport switch
    {
        Sport.Wrestling => "div.sidearm-roster-player-name",
        _ => "span.sidearm-roster-player-jersey-number"
    };

    private async Task<List<PlayerHeadshot>> LoadFromHtml()
    {
        using var context = GetContext();
        var document = await context.OpenAsync(RosterUrl);
        var images = document.QuerySelectorAll("div.sidearm-roster-player-image img");
        var playerIdElements = document.QuerySelectorAll(PlayerIdElement);
        var headshots = new List<PlayerHeadshot>();
        
        foreach (var pair in images.Zip(playerIdElements))
        {
            var (image, number) = pair;
            var imageUrl = image.GetAttribute("data-src").RemoveQuery() ?? string.Empty;
            var fullImageUrl = "https://" + team.Info.Website
                .AppendPathSegment(imageUrl);
            var playerId = number.TextContent.CollapseAndStrip();
            playerId = sport == Sport.Wrestling ? playerId.GenerateSlug() : playerId;
            headshots.Add(new PlayerHeadshot(playerId, fullImageUrl));
        }

        return headshots;
    }
    
    public async Task<List<PlayerHeadshot>> LoadHeadshots()
    {
        var fromApi = await TryApiUrl();
        if(fromApi is not null) { return fromApi; }
        return await LoadFromHtml();
    }

    public static async Task<MemoryStream> DownloadImages(List<PlayerHeadshot> headshots)
    {
        var memoryStream = new MemoryStream();
        using var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true);
        foreach (var headshot in headshots)
        {
            var entry = archive.CreateEntry($"{headshot.Id}.png");
            await using var entryStream = entry.Open();
            var image = await headshot.Download();
            await entryStream.WriteAsync(image);
        }

        return memoryStream;
    }

    [GeneratedRegex(@"\bC\b|\d")]
    private static partial Regex NameCleaner();
}
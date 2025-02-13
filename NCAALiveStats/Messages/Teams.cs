using System.Text.Json.Serialization;
using NCAALiveStats.Messages.Helpers;

namespace NCAALiveStats.Messages;

public class TeamDetail
{
    [JsonPropertyName("teamName")]
    public string TeamName { get; set; } = string.Empty;
    
    [JsonPropertyName("teamNameInternational")]
    public string TeamNameInternational { get; set; } = string.Empty;
    
    [JsonPropertyName("teamId")]
    public int TeamId { get; set; }
    
    [JsonPropertyName("externalId")]
    public string ExternalId { get; set; } = string.Empty;
    
    [JsonPropertyName("internationalReference")]
    public string InternationalReference { get; set; } = string.Empty;
    
    [JsonPropertyName("teamNickname")]
    public string TeamNickname { get; set; } = string.Empty;
    
    [JsonPropertyName("teamCode")]
    public string TeamCode { get; set; } = string.Empty;
    
    [JsonPropertyName("teamCodeLong")]
    public string TeamCodeLong { get; set; } = string.Empty;
    
    [JsonPropertyName("teamCodeInternational")]
    public string TeamCodeInternational { get; set; } = string.Empty;
    
    [JsonPropertyName("teamCodeLongInternational")]
    public string TeamCodeLongInternational { get; set; } = string.Empty;
    
    [JsonPropertyName("teamNicknameInternational")]
    public string TeamNicknameInternational { get; set; } = string.Empty;
    
    [JsonPropertyName("countryCode")]
    public string CountryCode { get; set; } = string.Empty;
    
    [JsonPropertyName("countryCodeIOC")]
    public string CountryCodeIOC { get; set; } = string.Empty;
    
    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;
    
    [JsonPropertyName("website")]
    public string Website { get; set; } = string.Empty;
    
    [JsonPropertyName("isHomeCompetitor")]
    public bool IsHomeCompetitor { get; set; }
}

public class Player
{
    [JsonPropertyName("pno")] public int PlayerNumber { get; set; }

    [JsonPropertyName("personId")] public int PersonId { get; set; }

    [JsonPropertyName("familyName")] public string FamilyName { get; set; } = string.Empty;

    [JsonPropertyName("firstName")] public string FirstName { get; set; } = string.Empty;
    
    [JsonPropertyName("internationalFamilyName")] public string InternationalFamilyName { get; set; } = string.Empty;
    
    [JsonPropertyName("internationalFirstName")] public string InternationalFirstName { get; set; } = string.Empty;
    
    [JsonPropertyName("scoreboardName")] public string ScoreboardName { get; set; } = string.Empty;
    
    [JsonPropertyName("TVName")] public string TVName { get; set; } = string.Empty;
    
    [JsonPropertyName("nickName")] public string NickName { get; set; } = string.Empty;
    
    [JsonPropertyName("website")] public string Website { get; set; } = string.Empty;
    
    [JsonPropertyName("dob")]
    public string DateOfBirth { get; set; }
    
    [JsonPropertyName("height")]
    public double Height { get; set; }
    
    [JsonPropertyName("externalId")] public string ExternalId { get; set; } = string.Empty;
    
    [JsonPropertyName("internationalReference")] public string InternationalReference { get; set; } = string.Empty;
    
    [JsonPropertyName("shirtNumber")] public string ShirtNumber { get; set; } = string.Empty;
    
    [JsonPropertyName("playingPosition")] public string PlayingPosition { get; set; } = string.Empty;
    
    [JsonPropertyName("starter")]
    public bool IsStarter { get; set; }
    
    [JsonPropertyName("captain")]
    public bool IsCaptain { get; set; }
    
    [JsonPropertyName("active")]
    public bool IsActive { get; set; }
    
    [JsonPropertyName("nationalityCode")] public string NationalityCode { get; set; } = string.Empty;
    
    [JsonPropertyName("nationalityCodeIOC")] public string NationalityCodeIOC { get; set; } = string.Empty;
    
    [JsonPropertyName("nationality")] public string Nationality { get; set; } = string.Empty;
}

public class Team
{
    [JsonPropertyName("teamNumber")]
    public int TeamNumber { get; set; }

    [JsonPropertyName("detail")] public TeamDetail Detail { get; set; } = new();

    [JsonPropertyName("players")] public List<Player> Players { get; set; } = [];
    
    public string FullName => Detail.TeamName + " " + Detail.TeamNickname;
}

[SocketMessage("teams")]
public class TeamMessage
{
    [JsonPropertyName("teams")] 
    public List<Team> Teams { get; set; } = [];
    
    [JsonPropertyName("messageId")]
    public  int MessageId { get; set; }
}
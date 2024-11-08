using System.Text.Json.Serialization;

namespace NCAALiveStatsListener.Messages;

public class TeamDetail
{
    [JsonPropertyName("teamName")]
    public required string TeamName { get; set; }
    
    [JsonPropertyName("teamNameInternational")]
    public required string TeamNameInternational { get; set; }
    
    [JsonPropertyName("teamId")]
    public int TeamId { get; set; }
    
    [JsonPropertyName("externalId")]
    public required string ExternalId { get; set; }
    
    [JsonPropertyName("internationalReference")]
    public required string InternationalReference { get; set; }
    
    [JsonPropertyName("teamNickname")]
    public required string TeamNickname { get; set; }
    
    [JsonPropertyName("teamCode")]
    public required string TeamCode { get; set; }
    
    [JsonPropertyName("teamCodeLong")]
    public required string TeamCodeLong { get; set; }
    
    [JsonPropertyName("teamCodeInternational")]
    public required string TeamCodeInternational { get; set; }
    
    [JsonPropertyName("teamCodeLongInternational")]
    public required string TeamCodeLongInternational { get; set; }
    
    [JsonPropertyName("teamNicknameInternational")]
    public required string TeamNicknameInternational { get; set; }
    
    [JsonPropertyName("countryCode")]
    public required string CountryCode { get; set; }
    
    [JsonPropertyName("countryCodeIOC")]
    public required string CountryCodeIOC { get; set; }
    
    [JsonPropertyName("country")]
    public required string Country { get; set; }
    
    [JsonPropertyName("website")]
    public required string Website { get; set; }
    
    [JsonPropertyName("isHomeCompetitor")]
    public bool IsHomeCompetitor { get; set; }
}

public class Player
{
    [JsonPropertyName("pno")] public int PlayerNumber { get; set; }

    [JsonPropertyName("personId")] public int PersonId { get; set; }

    [JsonPropertyName("familyName")] public required string FamilyName { get; set; }

    [JsonPropertyName("firstName")]
    public required string FirstName { get; set; }
    
    [JsonPropertyName("internationalFamilyName")]
    public required string InternationalFamilyName { get; set; }
    
    [JsonPropertyName("internationalFirstName")]
    public required string InternationalFirstName { get; set; }
    
    [JsonPropertyName("scoreboardName")]
    public required string ScoreboardName { get; set; }
    
    [JsonPropertyName("TVName")]
    public required string TVName { get; set; }
    
    [JsonPropertyName("nickName")]
    public required string NickName { get; set; }
    
    [JsonPropertyName("website")]
    public required string Website { get; set; }
    
    [JsonPropertyName("dob")]
    public DateTime DateOfBirth { get; set; }
    
    [JsonPropertyName("height")]
    public double Height { get; set; }
    
    [JsonPropertyName("externalId")]
    public required string ExternalId { get; set; }
    
    [JsonPropertyName("internationalReference")]
    public required string InternationalReference { get; set; }
    
    [JsonPropertyName("shirtNumber")]
    public required string ShirtNumber { get; set; }
    
    [JsonPropertyName("playingPosition")]
    public required string PlayingPosition { get; set; }
    
    [JsonPropertyName("starter")]
    public bool IsStarter { get; set; }
    
    [JsonPropertyName("captain")]
    public bool IsCaptain { get; set; }
    
    [JsonPropertyName("active")]
    public bool IsActive { get; set; }
    
    [JsonPropertyName("nationalityCode")]
    public required string NationalityCode { get; set; }
    
    [JsonPropertyName("nationalityCodeIOC")]
    public required string NationalityCodeIOC { get; set; }
    
    [JsonPropertyName("nationality")]
    public required string Nationality { get; set; }
}

public class Team
{
    [JsonPropertyName("teamNumber")]
    public int TeamNumber { get; set; }
    
    [JsonPropertyName("detail")]
    public required TeamDetail Detail { get; set; }
    
    [JsonPropertyName("players")]
    public required List<Player> Players { get; set; }
}

public class TeamMessage
{
    [JsonPropertyName("teams")]
    public required List<Team> Teams { get; set; }
    
    [JsonPropertyName("messageId")]
    public required int MessageId { get; set; }
}
using Newtonsoft.Json;

namespace NFLGamePredictor.Dto
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Address
    {
        public string city { get; set; }
        public string state { get; set; }
        public string zipCode { get; set; }
    }

    public class BoxscoreSource
    {
        public string id { get; set; }
        public string description { get; set; }
        public string state { get; set; }
    }

    public class Broadcasts
    {
        [JsonProperty("$ref")]
        public string @ref { get; set; }
    }

    public class Competition
    {
        [JsonProperty("$ref")]
        public string @ref { get; set; }
        public string id { get; set; }
        public string guid { get; set; }
        public string uid { get; set; }
        public string date { get; set; }
        public int attendance { get; set; }
        public Type type { get; set; }
        public bool timeValid { get; set; }
        public bool dateValid { get; set; }
        public bool neutralSite { get; set; }
        public bool divisionCompetition { get; set; }
        public bool conferenceCompetition { get; set; }
        public bool previewAvailable { get; set; }
        public bool recapAvailable { get; set; }
        public bool boxscoreAvailable { get; set; }
        public bool lineupAvailable { get; set; }
        public bool gamecastAvailable { get; set; }
        public bool playByPlayAvailable { get; set; }
        public bool conversationAvailable { get; set; }
        public bool commentaryAvailable { get; set; }
        public bool pickcenterAvailable { get; set; }
        public bool summaryAvailable { get; set; }
        public bool liveAvailable { get; set; }
        public bool ticketsAvailable { get; set; }
        public bool shotChartAvailable { get; set; }
        public bool timeoutsAvailable { get; set; }
        public bool possessionArrowAvailable { get; set; }
        public bool onWatchESPN { get; set; }
        public bool recent { get; set; }
        public bool bracketAvailable { get; set; }
        public GameSource gameSource { get; set; }
        public BoxscoreSource boxscoreSource { get; set; }
        public PlayByPlaySource playByPlaySource { get; set; }
        public LinescoreSource linescoreSource { get; set; }
        public StatsSource statsSource { get; set; }
        public Venue venue { get; set; }
        public Weather weather { get; set; }
        public List<Competitor> competitors { get; set; }
        public List<object> notes { get; set; }
        public Situation situation { get; set; }
        public Status status { get; set; }
        public Odds odds { get; set; }
        public Broadcasts broadcasts { get; set; }
        public Tickets tickets { get; set; }
        public Leaders leaders { get; set; }
        public List<Link> links { get; set; }
        public Predictor predictor { get; set; }
        public PowerIndexes powerIndexes { get; set; }
        public Format format { get; set; }
        public Drives drives { get; set; }
        public bool hasDefensiveStats { get; set; }
    }

    public class Competitor
    {
        [JsonProperty("$ref")]
        public string @ref { get; set; }
        public string id { get; set; }
        public string uid { get; set; }
        public string type { get; set; }
        public int order { get; set; }
        public string homeAway { get; set; }
        public Team team { get; set; }
        public Score score { get; set; }
        public Roster roster { get; set; }
        public Record record { get; set; }
    }

    public class Drives
    {
        [JsonProperty("$ref")]
        public string @ref { get; set; }
    }

    public class Format
    {
        public Regulation regulation { get; set; }
        public Overtime overtime { get; set; }
    }

    public class GameSource
    {
        public string id { get; set; }
        public string description { get; set; }
        public string state { get; set; }
    }

    public class Image
    {
        public string href { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string alt { get; set; }
        public List<string> rel { get; set; }
    }

    public class Leaders
    {
        [JsonProperty("$ref")]
        public string @ref { get; set; }
    }

    public class League
    {
        [JsonProperty("$ref")]
        public string @ref { get; set; }
    }

    public class LinescoreSource
    {
        public string id { get; set; }
        public string description { get; set; }
        public string state { get; set; }
    }

    public class Link
    {
        public string language { get; set; }
        public List<string> rel { get; set; }
        public string href { get; set; }
        public string text { get; set; }
        public string shortText { get; set; }
        public bool isExternal { get; set; }
        public bool isPremium { get; set; }
    }

    public class Link2
    {
        public string language { get; set; }
        public List<string> rel { get; set; }
        public string href { get; set; }
        public string text { get; set; }
        public string shortText { get; set; }
        public bool isExternal { get; set; }
        public bool isPremium { get; set; }
    }

    public class Odds
    {
        [JsonProperty("$ref")]
        public string @ref { get; set; }
    }

    public class Overtime
    {
        public int periods { get; set; }
        public string displayName { get; set; }
        public string slug { get; set; }
        public double clock { get; set; }
    }

    public class PlayByPlaySource
    {
        public string id { get; set; }
        public string description { get; set; }
        public string state { get; set; }
    }

    public class PowerIndexes
    {
        [JsonProperty("$ref")]
        public string @ref { get; set; }
    }

    public class Predictor
    {
        [JsonProperty("$ref")]
        public string @ref { get; set; }
    }

    public class Record
    {
        [JsonProperty("$ref")]
        public string @ref { get; set; }
    }

    public class Regulation
    {
        public int periods { get; set; }
        public string displayName { get; set; }
        public string slug { get; set; }
        public double clock { get; set; }
    }

    public class EventResponse
    {
 
        public string id { get; set; }
        //public string uid { get; set; }
        //public string date { get; set; }
        public string name { get; set; }
        //public string shortName { get; set; }
        //public Season season { get; set; }
        //public SeasonType seasonType { get; set; }
        //public Week week { get; set; }
        //public bool timeValid { get; set; }
        //public List<Competition> competitions { get; set; }
        //public List<Link> links { get; set; }
        //public List<Venue> venues { get; set; }
        //public Weather weather { get; set; }
        //public League league { get; set; }
    }

    public class Roster
    {
        [JsonProperty("$ref")]
        public string @ref { get; set; }
    }

    public class Score
    {
        [JsonProperty("$ref")]
        public string @ref { get; set; }
    }

    public class Season
    {
        [JsonProperty("$ref")]
        public string @ref { get; set; }
    }

    public class SeasonType
    {
        [JsonProperty("$ref")]
        public string @ref { get; set; }
    }

    public class Situation
    {
        [JsonProperty("$ref")]
        public string @ref { get; set; }
    }

    public class StatsSource
    {
        public string id { get; set; }
        public string description { get; set; }
        public string state { get; set; }
    }

    public class Status
    {
        [JsonProperty("$ref")]
        public string @ref { get; set; }
    }

    public class Team
    {
        [JsonProperty("$ref")]
        public string @ref { get; set; }
    }

    public class Tickets
    {
        [JsonProperty("$ref")]
        public string @ref { get; set; }
    }

    public class Type
    {
        public string id { get; set; }
        public string text { get; set; }
        public string abbreviation { get; set; }
        public string slug { get; set; }
        public string type { get; set; }
    }

    public class Venue
    {
        [JsonProperty("$ref")]
        public string @ref { get; set; }
        public string id { get; set; }
        public string fullName { get; set; }
        public Address address { get; set; }
        public bool grass { get; set; }
        public bool indoor { get; set; }
        public List<Image> images { get; set; }
    }

    public class Venue2
    {
        [JsonProperty("$ref")]
        public string @ref { get; set; }
    }

    public class Weather
    {
        [JsonProperty("$ref")]
        public string @ref { get; set; }
        public string type { get; set; }
        public string displayValue { get; set; }
        public string zipCode { get; set; }
        public string lastUpdated { get; set; }
        public int windSpeed { get; set; }
        public string windDirection { get; set; }
        public int temperature { get; set; }
        public int highTemperature { get; set; }
        public int lowTemperature { get; set; }
        public string conditionId { get; set; }
        public int gust { get; set; }
        public int precipitation { get; set; }
        public Link link { get; set; }
    }

    public class Week
    {
        [JsonProperty("$ref")]
        public string @ref { get; set; }
    }


}

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using Newtonsoft.Json;

public class Category
{
    public string name { get; set; }
    public string displayName { get; set; }
    public string shortDisplayName { get; set; }
    public string abbreviation { get; set; }
    public string summary { get; set; }
    public List<Stat> stats { get; set; }
}

public class TeamStatsResponse
{
    public string id { get; set; }
    public Season season { get; set; }
    public Team team { get; set; }
    public Splits splits { get; set; }
    public SeasonType seasonType { get; set; }
}

public class Season
{
    [JsonProperty("id")]
    public string id { get; set; }
}

public class SeasonType
{
    [JsonProperty("id")]
    public string id { get; set; }
}

public class Splits
{
    public string id { get; set; }
    public string name { get; set; }
    public string abbreviation { get; set; }
    public List<Category> categories { get; set; }
}

public class Stat
{
    public string name { get; set; }
    public string displayName { get; set; }
    public string shortDisplayName { get; set; }
    public string description { get; set; }
    public string abbreviation { get; set; }
    public double value { get; set; }
    public string displayValue { get; set; }
    public int rank { get; set; }
    public string rankDisplayValue { get; set; }
    public double? perGameValue { get; set; }
    public string perGameDisplayValue { get; set; }
}

public class Team
{
    [JsonProperty("id")]
    public string id { get; set; }
}


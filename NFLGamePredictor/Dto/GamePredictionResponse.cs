using Newtonsoft.Json;

namespace NFLGamePredictor.Dto
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AwayTeam
    {
        public Team team { get; set; }
        public List<Statistic> statistics { get; set; }
    }

    public class HomeTeam
    {
        public Team team { get; set; }
        public List<Statistic> statistics { get; set; }
    }

    public class GamePredictionResponse
    {
      
        public string name { get; set; }
        public HomeTeam homeTeam { get; set; }
        public AwayTeam awayTeam { get; set; }
    }

    public class Statistic
    {
        public string name { get; set; }
        public string displayName { get; set; }
        public string shortDisplayName { get; set; }
        public string description { get; set; }
        public string abbreviation { get; set; }
        public double value { get; set; }
        public string displayValue { get; set; }
    }

    //public class Team
    //{
    //    [JsonProperty("$ref")]
    //    public string @ref { get; set; }
    //}


}

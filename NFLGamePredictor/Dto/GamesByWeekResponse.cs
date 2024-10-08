using Newtonsoft.Json;

namespace NFLGamePredictor.Dto
{



    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Item
    {
        public string gameUrl { get; set; }
    }

    public class Meta
    {
        public Parameters parameters { get; set; }
    }

    public class Parameters
    {
        public List<string> week { get; set; }
        public List<string> season { get; set; }
        public List<string> seasontypes { get; set; }
    }

    public class GamesByWeekResponse
    {
        [JsonProperty("$meta")]
        public Meta meta { get; set; }
        public int count { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int pageCount { get; set; }
        public List<Item> items { get; set; }
    }



}
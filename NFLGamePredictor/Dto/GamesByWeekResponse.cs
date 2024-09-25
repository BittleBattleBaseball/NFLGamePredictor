namespace NFLGamePredictor.Dto
{
    //public class 
    //{
    //}

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Item
    {
        public string eventid { get; set; }
    }

    public class GamesByWeekResponse
    {
        public int count { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int pageCount { get; set; }
        public List<Item> items { get; set; }
    }




}

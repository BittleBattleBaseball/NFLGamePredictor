namespace NFLGamePredictor.Dto
{


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AwayTeamOdds
    {
        public bool favorite { get; set; }
        public bool underdog { get; set; }
        public int moneyLine { get; set; }
        public double spreadOdds { get; set; }
        public Open open { get; set; }
        public Close close { get; set; }
        public Current current { get; set; }
        public Team team { get; set; }
    }

    public class Close
    {
        public PointSpread pointSpread { get; set; }
        public Spread spread { get; set; }
        public MoneyLine moneyLine { get; set; }
        public Over over { get; set; }
        public Under under { get; set; }
        public Total total { get; set; }
    }

    public class Current
    {
        public PointSpread pointSpread { get; set; }
        public Spread spread { get; set; }
        public MoneyLine moneyLine { get; set; }
        public Over over { get; set; }
        public Under under { get; set; }
        public Total total { get; set; }
    }

    public class HomeTeamOdds
    {
        public bool favorite { get; set; }
        public bool underdog { get; set; }
        public int moneyLine { get; set; }
        public double spreadOdds { get; set; }
        public Open open { get; set; }
        public Close close { get; set; }
        public Current current { get; set; }
        public Team team { get; set; }
    }

    public class GameOddsItem
    {
        public string id { get; set; }
        public Provider provider { get; set; }
        public string details { get; set; }
        public double overUnder { get; set; }
        public double spread { get; set; }
        public double overOdds { get; set; }
        public double underOdds { get; set; }
        public AwayTeamOdds awayTeamOdds { get; set; }
        public HomeTeamOdds homeTeamOdds { get; set; }
        public List<Link> links { get; set; }
        public bool moneylineWinner { get; set; }
        public bool spreadWinner { get; set; }
        public Open open { get; set; }
        public Close close { get; set; }
        public Current current { get; set; }
    }

    public class MoneyLine
    {
        public double value { get; set; }
        public string displayValue { get; set; }
        public string alternateDisplayValue { get; set; }
        public double @decimal { get; set; }
        public string fraction { get; set; }
        public string american { get; set; }
        public Outcome outcome { get; set; }
    }

    public class Open
    {
        public bool favorite { get; set; }
        public PointSpread pointSpread { get; set; }
        public Spread spread { get; set; }
        public MoneyLine moneyLine { get; set; }
        public Over over { get; set; }
        public Under under { get; set; }
        public Total total { get; set; }
    }

    public class Outcome
    {
        public string type { get; set; }
    }

    public class Over
    {
        public double value { get; set; }
        public string displayValue { get; set; }
        public string alternateDisplayValue { get; set; }
        public double @decimal { get; set; }
        public string fraction { get; set; }
        public string american { get; set; }
        public Outcome outcome { get; set; }
    }

    public class PointSpread
    {
        public string alternateDisplayValue { get; set; }
        public string american { get; set; }
        public double value { get; set; }
        public string displayValue { get; set; }
        public double @decimal { get; set; }
        public string fraction { get; set; }
    }

    public class Provider
    {
        public string id { get; set; }
        public string name { get; set; }
        public int priority { get; set; }
    }

    public class GameOddsResponse
    {
        public int count { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int pageCount { get; set; }
        public List<GameOddsItem> items { get; set; }
    }

    public class Spread
    {
        public double value { get; set; }
        public string displayValue { get; set; }
        public string alternateDisplayValue { get; set; }
        public double @decimal { get; set; }
        public string fraction { get; set; }
        public string american { get; set; }
        public Outcome outcome { get; set; }
    }


    public class Total
    {
        public double value { get; set; }
        public string displayValue { get; set; }
        public string alternateDisplayValue { get; set; }
        public double @decimal { get; set; }
        public string fraction { get; set; }
        public string american { get; set; }
    }

    public class Under
    {
        public double value { get; set; }
        public string displayValue { get; set; }
        public string alternateDisplayValue { get; set; }
        public double @decimal { get; set; }
        public string fraction { get; set; }
        public string american { get; set; }
        public Outcome outcome { get; set; }
    }




}

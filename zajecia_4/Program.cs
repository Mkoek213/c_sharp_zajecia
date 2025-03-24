using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json; 
using System.Linq;     
using System.Xml.Linq;  
using System.Globalization;




class Program
{

    static public void ConvertToXml(List<Tweet> tweets, string outputPath){
        var xml = new XElement("Tweets", tweets.Select(t => new XElement("Tweet",
            new XElement("Text", t.Text),
            new XElement("UserName", t.UserName),
            new XElement("LinkToTweet", t.LinkToTweet),
            new XElement("FirstLinkUrl", t.FirstLinkUrl),
            new XElement("CreatedAt", t.CreatedAt),
            new XElement("TweetEmbedCode", t.TweetEmbedCode)
        )));
        xml.Save(outputPath);
    }

    static public List<Tweet> LoadFromXml(string path){
        var xml = XElement.Load(path);
        return xml.Elements("Tweet").Select(t => new Tweet{
            Text = t.Element("Text").Value,
            UserName = t.Element("UserName").Value,
            LinkToTweet = t.Element("LinkToTweet").Value,
            FirstLinkUrl = t.Element("FirstLinkUrl").Value,
            CreatedAt = DateTime.Parse(t.Element("CreatedAt").Value),
            TweetEmbedCode = t.Element("TweetEmbedCode").Value
        }).ToList();
    }

    static public void sortTweetsByUserName(List<Tweet> tweets){
        tweets.Sort((a, b) => a.UserName.CompareTo(b.UserName));
    }

    static public void sortTweetsByCreatedAt(List<Tweet> tweets){
        tweets.Sort((a, b) => a.CreatedAt.CompareTo(b.CreatedAt));
    }

    public class Tweet
    {
        public string Text { get; set; }
        public string UserName { get; set; }
        public string LinkToTweet { get; set; }
        public string FirstLinkUrl { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime CreatedAt { get; set; }
        public string TweetEmbedCode { get; set; }
    }

    public class CustomDateTimeConverter : JsonConverter
    {
        private readonly string[] dateFormats = new string[]
        {
            "MMMM dd, yyyy 'at' hh:mmtt",  
            "MMMM dd, yyyy hh:mmtt",    
        };

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss"));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var dateString = reader.Value.ToString();
            
            if (DateTime.TryParseExact(dateString, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
            
            throw new JsonReaderException($"Could not parse date: {dateString}");
        }
    }

    static void Main()
    {
        string filePath = "/Users/mikolaj/Desktop/work/c_sharp_zajecia/zajecia_4/favorite-tweets.jsonl";
        List<Tweet> tweets = new List<Tweet>();
        var data = File.ReadAllLines(filePath);
        foreach (var line in data)
        {
                var tweet = JsonConvert.DeserializeObject<Tweet>(line);
            tweet.CreatedAt = DateTime.ParseExact(tweet.CreatedAt.ToString(), "MMMM dd, yyyy 'at' hh:mmtt", CultureInfo.InvariantCulture);
            tweets.Add(tweet);
        }
        Console.WriteLine($"Total Tweets: {tweets.Count}");

        ConvertToXml(tweets, "/Users/mikolaj/Desktop/work/c_sharp_zajecia/zajecia_4/tweets.xml");
        var loadedTweets = LoadFromXml("/Users/mikolaj/Desktop/work/c_sharp_zajecia/zajecia_4/tweets.xml");
        Console.WriteLine($"Total Tweets: {loadedTweets.Count}");

        sortTweetsByCreatedAt(tweets);
        Console.WriteLine($"Najstarszy tweet: {tweets[0].CreatedAt}");
        Console.WriteLine($"Najnowszy tweet: {tweets[tweets.Count - 1].CreatedAt}");

        Dictionary<string, List<Tweet>> tweetsByUser = new Dictionary<string, List<Tweet>>();
        foreach (var tweet in tweets)
        {
            if (!tweetsByUser.ContainsKey(tweet.UserName))
            {
                tweetsByUser[tweet.UserName] = new List<Tweet>();
            }
            tweetsByUser[tweet.UserName].Add(tweet);
        }

        Dictionary<string, int> wordFrequency = new Dictionary<string, int>();
        foreach (var tweet in tweets)
        {
            var words = tweet.Text.Split(' ');
            foreach (var word in words)
            {
                if (!wordFrequency.ContainsKey(word))
                {
                    wordFrequency[word] = 1;
                }
                wordFrequency[word]++;
            }
        }


        var topWords = wordFrequency.Where(w => w.Key.Length >= 5).OrderByDescending(w => w.Value).Take(10);
        foreach (var word in topWords)
        {
            Console.WriteLine($"{word.Key}: {word.Value}");
        }

        Dictionary<string, double> wordIDF = new Dictionary<string, double>();
        foreach(var word in wordFrequency){
            float df = word.Value / tweets.Count;
            wordIDF[word.Key] = Math.Log(1 / df);
        }

        wordIDF = wordIDF.OrderByDescending(w => w.Value).ToDictionary(w => w.Key, w => w.Value).Take(10).ToDictionary(w => w.Key, w => w.Value);;
        foreach (var word in wordIDF)
        {
            Console.WriteLine($"{word.Key}: {word.Value}");
        }
    }
}





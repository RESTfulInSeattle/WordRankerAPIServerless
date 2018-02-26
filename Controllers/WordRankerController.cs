using System.Collections.Generic;

namespace WordRankerAPIServerless.Controllers
{
    public class WordRankerController
    {
        private static string queueURL = "https://sqs.us-west-2.amazonaws.com/792614619693/Assignment3-Queue";
        public static string RankURL(string url)
        {
            if (url.Length > 0)
            {
                string result = DynamoDbActions.RetrieveItem(url);

                if (result.Length == 0)
                {
                    bool success = SQSActions.AddMessage(queueURL, url);
                    if (!success) return "There was a problem contacting SQS";

                    return "This URL has been queued to be ranked, please check back later.";
                }

                return result;

            }
            return "Invalid URL";
        }

        public string Rank(string url)
        {
            if (url.Length > 0)
            {
                URLFetcher uf = new URLFetcher();
                string urlText = uf.GetURLText(url);

                if (urlText.Length > 0)
                {
                    string returnString;
                    WordCollector wc = new WordCollector();
                    char[] delimeters = { ' ' };
                    Dictionary<string, long> words = wc.GetWords(urlText, delimeters);

                    if (words.Keys.Count > 0)
                    {
                        WordRanker wr = new WordRanker();
                        List<KeyValuePair<string, long>> rankedWords = wr.RankWords(words, 10);

                        returnString = "Top 10 Words:" + "\r\n";

                        foreach (KeyValuePair<string, long> word in rankedWords)
                        {
                            returnString += $"Word: {word.Key}  Count: {word.Value}" + "\r\n";
                        }

                        return returnString;
                    }
                    else
                    {
                        return "No words retrieved from URL: " + url;
                    }
                }
                else
                {
                    return "No text retrieved from URL: " + url;
                }


            }
            else
            {
                return "Invalid URL";
            }
        }
    }
    
}

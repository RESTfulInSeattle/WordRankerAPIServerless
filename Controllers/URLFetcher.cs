using System.Net;

namespace WordRankerAPIServerless.Controllers
{
    class URLFetcher
    {
        public string GetURLText(string URL)
        {
            string returnString="";

            if (URL.Length > 0)
            {
                WebClient client = new WebClient();
                returnString = client.DownloadString(URL);
            }

            return returnString;
        }
    }
}

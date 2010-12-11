namespace WikiDesk.Core
{
    using System.IO;
    using System.Net;

    public class Download
    {
        public static string DownloadPage(string url)
        {
            //string url = string.Concat(languageCode, Settings. ExportU)

            // Open a connection
            HttpWebRequest webRequestObject = (HttpWebRequest)WebRequest.Create(url);

            // You can also specify additional header values like
            // the user agent or the referrer:
            webRequestObject.UserAgent	= ".NET Framework/2.0";
            webRequestObject.Referer	= "http://www.example.com/";

            // Request response:
            using (WebResponse response = webRequestObject.GetResponse())
            {
                using (Stream webStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(webStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
    }
}

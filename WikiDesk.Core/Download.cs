namespace WikiDesk.Core
{
    using System.IO;
    using System.Net;

    public class Download
    {
        public static string DownloadPage(string url)
        {
            // Open a connection
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest == null)
            {
                return null;
            }

            webRequest.UserAgent = "WebDesk";
            //webRequest.Referer = "http://www.example.com/";

            // Request response:
            using (WebResponse response = webRequest.GetResponse())
            {
                using (Stream webStream = response.GetResponseStream())
                {
                    if (webStream == null)
                    {
                        return null;
                    }

                    using (StreamReader reader = new StreamReader(webStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
    }
}

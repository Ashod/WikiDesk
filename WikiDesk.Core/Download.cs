namespace WikiDesk.Core
{
    public class Download
    {
        public static string DownloadPage(string url)
        {
            return WebStream.ReadToEnd(url, "WikiDesk", string.Empty);
        }
    }
}

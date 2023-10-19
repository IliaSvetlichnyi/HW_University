using SimpleWebBrowser.Http;
using System;
using System.IO;
using System.Text;

namespace SimpleWebBrowser.Controllers
{
    public class BulkDownloader
    {
        private HttpRequestManager requestManager;

        public BulkDownloader(HttpRequestManager requestManager)
        {
            this.requestManager = requestManager;
        }

        public string PerformBulkDownload(string filePath)
        {
            var urls = File.ReadAllLines(filePath);
            StringBuilder results = new StringBuilder();

            foreach (var url in urls)
            {
                var responseContent = requestManager.FetchHtmlContent(url);
                int byteCount = Encoding.UTF8.GetByteCount(responseContent.HtmlContent);
                results.AppendLine($"{(int)responseContent.StatusCode} {byteCount} {url}");
            }

            return results.ToString();
        }
    }
}

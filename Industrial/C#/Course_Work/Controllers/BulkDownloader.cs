using SimpleWebBrowser.Http;
using System;
using System.IO;
using System.Text;

namespace SimpleWebBrowser.Controllers
{
    public class BulkDownloader
    {
        private const int MAX_URL_COUNT = 1000; // Maximum number of URLs to process
        private HttpRequestManager requestManager;

        // Constructor that initializes the HttpRequestManager
        public BulkDownloader(HttpRequestManager requestManager)
        {
            this.requestManager = requestManager;
        }

        // Performs bulk download using the URLs from the specified file
        public string PerformBulkDownload(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return "Error: File does not exist.";
            }

            var urls = File.ReadAllLines(filePath);

            // Check if the number of URLs exceeds the maximum allowed
            if (urls.Length > MAX_URL_COUNT)
            {
                return $"Error: Too many URLs in file. Please limit to {MAX_URL_COUNT} URLs.";
            }

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

using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace SimpleWebBrowser.Http
{
    // Provides methods to interact with HTTP web resources.
    public class HttpRequestManager
    {
        // Represents the content returned from a web request.
        public class ResponseContent
        {
            // Gets or sets the HTML content or error message of the response.
            public string HtmlContent { get; set; }

            // Gets or sets the status code of the HTTP response.
            public HttpStatusCode StatusCode { get; set; }
        }

        // Fetches the HTML content of a given URL and returns the content along with the HTTP status code.
        // <returns> A ResponseContent object containing the HTML content and the status code.</returns>
        public ResponseContent FetchHtmlContent(string url)
        {
            try
            {
                // Create a web request for the specified URL.
                var request = (HttpWebRequest)WebRequest.Create(url);

                // Obtain the response from the server.
                using (var response = (HttpWebResponse)request.GetResponse())
                using (var dataStream = response.GetResponseStream())
                using (var reader = new StreamReader(dataStream))
                {
                    // Return the response content along with the status code.
                    return new ResponseContent
                    {
                        HtmlContent = reader.ReadToEnd(),
                        StatusCode = response.StatusCode
                    };
                }
            }
            // Handle web exceptions which may contain HTTP error status codes.
            catch (WebException ex) when (ex.Response is HttpWebResponse httpWebResponse)
            {
                return new ResponseContent
                {
                    HtmlContent = $"Error fetching content: {ex.Message}",
                    StatusCode = httpWebResponse.StatusCode
                };
            }
            // Handle other general exceptions.
            catch (Exception ex)
            {
                return new ResponseContent
                {
                    HtmlContent = $"Error fetching content: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        // Extracts the title from the provided HTML content.
        public string ExtractTitleFromHtml(string htmlContent)
        {
            const string defaultTitle = "No title";

            if (string.IsNullOrWhiteSpace(htmlContent))
            {
                return defaultTitle;
            }

            var titleRegex = new Regex(@"<title>\s*(.+?)\s*</title>", RegexOptions.IgnoreCase);
            var match = titleRegex.Match(htmlContent);

            return match.Success ? match.Groups[1].Value : defaultTitle;
        }
    }
}

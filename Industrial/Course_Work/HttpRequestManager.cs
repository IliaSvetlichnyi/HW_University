using System;
using System.IO;
using System.Net;

namespace SimpleWebBrowser.Http
{
    /// <summary>
    /// Provides methods to interact with HTTP web resources.
    /// </summary>
    public class HttpRequestManager
    {
        /// <summary>
        /// Represents the content returned from a web request.
        /// </summary>
        public class ResponseContent
        {
            /// <summary>
            /// Gets or sets the HTML content or error message of the response.
            /// </summary>
            public string HtmlContent { get; set; }

            /// <summary>
            /// Gets or sets the status code of the HTTP response.
            /// </summary>
            public HttpStatusCode StatusCode { get; set; }
        }

        /// <summary>
        /// Fetches the HTML content of a given URL and returns the content along with the HTTP status code.
        /// </summary>
        /// <param name="url">The URL of the web resource to fetch.</param>
        /// <returns>A ResponseContent object containing the HTML content and the status code.</returns>
        public ResponseContent FetchHtmlContent(string url)
        {
            try
            {
                // Create a web request for the specified URL.
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                // Obtain the response from the server.
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    // Retrieve the data stream from the response.
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        // Read the content of the data stream.
                        using (StreamReader reader = new StreamReader(dataStream))
                        {
                            // Return the response content along with the status code.
                            return new ResponseContent
                            {
                                HtmlContent = reader.ReadToEnd(),
                                StatusCode = response.StatusCode
                            };
                        }
                    }
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
    }
}
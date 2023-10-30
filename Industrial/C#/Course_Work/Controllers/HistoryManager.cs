using System;
using System.Collections.Generic;
using System.IO;

namespace SimpleWebBrowser.Controllers
{
    public class HistoryManager
    {
        public class HistoryItem
        {
            public string Url { get; private set; }
            public DateTime Timestamp { get; private set; }

            public HistoryItem(string url, DateTime timestamp)
            {
                Url = url;
                Timestamp = timestamp;
            }
        }

        private const string HistoryFilePath = "history.txt";
        private const int MaxHistoryItems = 1000; // Maximum number of URLs in the history
        private List<HistoryItem> history = new List<HistoryItem>();
        private int currentHistoryIndex = -1;

        // Returns a read-only list of history items
        public IReadOnlyList<HistoryItem> History => history.AsReadOnly();
        public int CurrentHistoryIndex => currentHistoryIndex;

        // Adds a new URL to the history
        public void AddToHistory(string url)
        {
            // Prevent adding duplicate consecutive URLs
            if (history.Count > 0 && history[currentHistoryIndex].Url == url)
                return;

            var newItem = new HistoryItem(url, DateTime.Now); // Using a constructor

            history.Add(newItem);
            currentHistoryIndex++;

            // Ensure history doesn't exceed the limit
            while (history.Count > MaxHistoryItems)
            {
                history.RemoveAt(0);
                currentHistoryIndex--;
            }

            // Append the new URL to the history file
            File.AppendAllText(HistoryFilePath, $"{newItem.Timestamp}|{url}" + Environment.NewLine);
        }

        // Looking for a matching URL in the history file
        public List<string> GetMatchingUrls(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<string>();

            return history
                .Where(item => item.Url.Contains(query, StringComparison.OrdinalIgnoreCase))
                .Select(item => item.Url)
                .Distinct()
                .ToList();
        }


        // Navigate back in the history and return the URL
        public string? MoveBackInHistory()
        {
            if (currentHistoryIndex > 0)
            {
                currentHistoryIndex--;
                return history[currentHistoryIndex].Url; // Returning the URL from HistoryItem
            }
            return null;
        }

        // Navigate forward in the history and return the URL
        public string? MoveForwardInHistory()
        {
            if (currentHistoryIndex < history.Count - 1)
            {
                currentHistoryIndex++;
                return history[currentHistoryIndex].Url; // Returning the URL from HistoryItem
            }
            return null;
        }

        // Load history items from the file
        public void LoadHistoryFromFile()
        {
            if (File.Exists(HistoryFilePath))
            {
                var lines = File.ReadAllLines(HistoryFilePath);
                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length == 2)
                    {
                        var timestamp = DateTime.Parse(parts[0]);
                        var url = parts[1];
                        history.Add(new HistoryItem(url, timestamp)); // Using a constructor
                    }
                }
                currentHistoryIndex = history.Count - 1;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;

namespace SimpleWebBrowser.Controllers
{
    public class HistoryManager
    {
        private const string HistoryFilePath = "history.txt";
        private const int MaxHistoryItems = 1000; // добавим ограничение на количество записей в истории
        private List<string> history = new List<string>();
        private int currentHistoryIndex = -1;

        // Returns a read-only list of history items
        public IReadOnlyList<string> History => history.AsReadOnly();
        public int CurrentHistoryIndex => currentHistoryIndex;

        // Adds a new URL to the history
        public void AddToHistory(string url)
        {
            // Prevent adding duplicate consecutive URLs
            if (history.Count > 0 && history[currentHistoryIndex] == url)
                return;

            history.Add(url);
            currentHistoryIndex++;

            // Ensure history doesn't exceed the limit
            while (history.Count > MaxHistoryItems)
            {
                history.RemoveAt(0);
                currentHistoryIndex--;
            }

            // Append the new URL to the history file
            File.AppendAllText(HistoryFilePath, url + Environment.NewLine);
        }


        // Navigate back in the history and return the URL
        public string? MoveBackInHistory()
        {
            if (currentHistoryIndex > 0)
            {
                currentHistoryIndex--;
                return history[currentHistoryIndex];
            }
            return null;
        }

        // Navigate forward in the history and return the URL
        public string? MoveForwardInHistory()
        {
            if (currentHistoryIndex < history.Count - 1)
            {
                currentHistoryIndex++;
                return history[currentHistoryIndex];
            }
            return null;
        }

        // Load history items from the file
        public void LoadHistoryFromFile()
        {
            if (File.Exists(HistoryFilePath))
            {
                history = new List<string>(File.ReadAllLines(HistoryFilePath));
                currentHistoryIndex = history.Count - 1;
            }
        }
    }
}

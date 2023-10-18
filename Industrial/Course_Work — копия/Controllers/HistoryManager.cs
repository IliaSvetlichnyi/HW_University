using System;
using System.Collections.Generic;
using System.IO;

namespace SimpleWebBrowser.Controllers
{
    public class HistoryManager
    {
        private const string HistoryFilePath = "history.txt";
        private List<string> history = new List<string>();
        private int currentHistoryIndex = -1;

        public IReadOnlyList<string> History => history.AsReadOnly();
        public int CurrentHistoryIndex => currentHistoryIndex;

        public void AddToHistory(string url)
        {
            if (currentHistoryIndex == history.Count - 1 || history.Count == 0)
            {
                history.Add(url);
                currentHistoryIndex++;
            }
            SaveHistoryToFile(); // сохраняем каждый раз, когда добавляем новую запись
        }

        public string? MoveBackInHistory()
        {
            if (currentHistoryIndex > 0)
            {
                currentHistoryIndex--;
                return history[currentHistoryIndex];
            }
            return null;
        }

        public string? MoveForwardInHistory()
        {
            if (currentHistoryIndex < history.Count - 1)
            {
                currentHistoryIndex++;
                return history[currentHistoryIndex];
            }
            return null;
        }

        public void LoadHistoryFromFile()
        {
            if (File.Exists(HistoryFilePath))
            {
                history = new List<string>(File.ReadAllLines(HistoryFilePath));
                currentHistoryIndex = history.Count - 1; // Устанавливаем индекс на последний URL в истории
            }
        }

        public void SaveHistoryToFile()
        {
            File.AppendAllLines(HistoryFilePath, history);
        }
    }
}

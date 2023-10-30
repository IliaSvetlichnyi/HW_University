using System.Collections.Generic;
using System.IO;

namespace SimpleWebBrowser.Controllers
{
    public class FavoritesManager
    {
        private List<KeyValuePair<string, string>> favorites = new List<KeyValuePair<string, string>>();

        // Returns a read-only list of favorites
        public IReadOnlyList<KeyValuePair<string, string>> Favorites => favorites.AsReadOnly();

        // Adds a new favorite if title and url are not empty or null
        public void AddToFavorites(string title, string url)
        {
            if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(url))
            {
                favorites.Add(new KeyValuePair<string, string>(title, url));
            }
        }

        // Removes a favorite based on title
        public void RemoveFromFavorites(string title, string url)
        {
            favorites.RemoveAll(fav => fav.Key == title && fav.Value == url);
        }

        private string favoritesFilePath = "favorites.txt";

        // Saves the favorites to a file
        public void SaveFavoritesToFile()
        {
            List<string> linesToSave = new List<string>();

            foreach (var favorite in favorites)
            {
                linesToSave.Add($"{favorite.Key}||{favorite.Value}");
            }

            File.WriteAllLines(favoritesFilePath, linesToSave);
        }

        // Loads the favorites from a file
        public void LoadFavoritesFromFile()
        {
            if (File.Exists(favoritesFilePath))
            {
                var lines = File.ReadAllLines(favoritesFilePath);

                foreach (var line in lines)
                {
                    var parts = line.Split(new[] { "||" }, StringSplitOptions.None);
                    if (parts.Length == 2)
                    {
                        AddToFavorites(parts[0], parts[1]);
                    }
                }
            }
        }
    }
}

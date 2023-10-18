using System.Collections.Generic;

namespace SimpleWebBrowser.Controllers
{
    public class FavoritesManager
    {
        private List<KeyValuePair<string, string>> favorites = new List<KeyValuePair<string, string>>();

        public IReadOnlyList<KeyValuePair<string, string>> Favorites => favorites.AsReadOnly();

        public void AddToFavorites(string title, string url)
        {
            if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(url))
            {
                favorites.Add(new KeyValuePair<string, string>(title, url));
            }
        }

        public void RemoveFromFavorites(string title)
        {
            favorites.RemoveAll(fav => fav.Key == title);
        }

        private string favoritesFilePath = "favorites.txt";

        public void SaveFavoritesToFile()
        {
            List<string> linesToSave = new List<string>();

            foreach (var favorite in favorites)
            {
                linesToSave.Add($"{favorite.Key}||{favorite.Value}");
            }

            File.WriteAllLines(favoritesFilePath, linesToSave);
        }

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

using Gtk;
using System;
using System.Collections.Generic;

namespace SimpleWebBrowser.Controllers
{
    public static class BrowserDialogs
    {
        // Displays a modal input dialog and returns the entered text.
        public static string? InputDialog(string prompt, string title, Window parent)
        {
            string? result = null;

            using var dialog = new Dialog(title, parent, DialogFlags.Modal);
            dialog.AddButton("Cancel", ResponseType.Cancel);
            dialog.AddButton("OK", ResponseType.Ok);

            var label = new Label(prompt);
            var entry = new Entry();

            var hbox = new HBox(false, 8);
            hbox.BorderWidth = 8;
            hbox.PackStart(label, false, false, 0);
            hbox.PackStart(entry, true, true, 0);

            dialog.ContentArea.PackStart(hbox, true, true, 0);

            dialog.ShowAll();

            if (dialog.Run() == (int)ResponseType.Ok)
            {
                result = entry.Text;
            }

            return result;
        }

        // Show a modal dialog with the user's favorites
        public static void ViewFavorites(Window parent, List<KeyValuePair<string, string>> favorites, Action<string> onFavoriteSelected)
        {
            using var dialog = new Dialog("Favorites", parent, DialogFlags.Modal);
            dialog.SetDefaultSize(400, 300);

            var listView = new TreeView();
            var listStore = new ListStore(typeof(string), typeof(string));

            listView.Model = listStore;

            // Setting up the tree view columns
            var titleColumn = new TreeViewColumn { Title = "Title" };
            var urlColumn = new TreeViewColumn { Title = "URL" };

            var titleCell = new CellRendererText();
            var urlCell = new CellRendererText();

            titleColumn.PackStart(titleCell, true);
            urlColumn.PackStart(urlCell, true);

            listView.AppendColumn(titleColumn);
            listView.AppendColumn(urlColumn);

            titleColumn.AddAttribute(titleCell, "text", 0);
            urlColumn.AddAttribute(urlCell, "text", 1);

            // Populate the tree view with favorites
            foreach (var favorite in favorites)
            {
                listStore.AppendValues(favorite.Key, favorite.Value);
            }

            // Handling favorite selection
            listView.RowActivated += (s, e) =>
            {
                var model = listView.Model;
                TreeIter iter;
                if (model.GetIter(out iter, e.Path))
                {
                    var url = model.GetValue(iter, 1).ToString();
                    onFavoriteSelected(url);
                }
            };

            dialog.ContentArea.PackStart(listView, true, true, 0);
            listView.Show();
            dialog.AddButton("Close", ResponseType.Close);
            dialog.ShowAll();

            dialog.Run();
        }

        // Show a modal dialog with the browser history
        public static void ViewHistory(Window parent, IReadOnlyList<string> history, Action<string> onHistoryItemSelected)
        {
            var dialog = new Dialog("History", parent, DialogFlags.Modal);
            dialog.SetDefaultSize(400, 300);

            var treeView = new TreeView();
            var treeStore = new TreeStore(typeof(string));

            treeView.Model = treeStore;
            treeView.AppendColumn("URL", new CellRendererText(), "text", 0);

            // Populate the tree view with browser history
            foreach (var item in history)
            {
                treeStore.AppendValues(item);
            }

            // Handling history item selection
            treeView.RowActivated += (sender, e) =>
            {
                TreeIter iter;
                treeView.Model.GetIter(out iter, e.Path);
                var selectedUrl = (string)treeView.Model.GetValue(iter, 0);

                onHistoryItemSelected.Invoke(selectedUrl);
            };

            var scrolledWindow = new ScrolledWindow();
            scrolledWindow.Add(treeView);

            dialog.ContentArea.PackStart(scrolledWindow, true, true, 0);
            dialog.AddButton("Close", ResponseType.Close);
            dialog.Response += (sender, e) => dialog.Destroy();

            dialog.ShowAll();
        }
    }
}

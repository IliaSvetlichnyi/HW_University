using Gtk;
using System;
using System.Collections.Generic;

namespace SimpleWebBrowser.Controllers
{
    public static class BrowserDialogs
    {
        [Obsolete]
        public static string? InputDialog(string prompt, string title, Window parent)
        {
            Console.WriteLine("InputDialog called"); // debug
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
                Console.WriteLine($"InputDialog returned: {result}");
            }
            else
            {
                Console.WriteLine("InputDialog was cancelled or closed.");
            }

            // dialog.Destroy();
            return result;
        }

        public static void ViewFavorites(Window parent, List<KeyValuePair<string, string>> favorites, Action<string> onFavoriteSelected)
        {
            Console.WriteLine("ViewFavorites called"); // debug
            using var dialog = new Dialog("Favorites", parent, DialogFlags.Modal);
            dialog.SetDefaultSize(400, 300);

            var listView = new TreeView();
            var listStore = new ListStore(typeof(string), typeof(string));

            listView.Model = listStore;

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


            foreach (var favorite in favorites)
            {
                listStore.AppendValues(favorite.Key, favorite.Value);
            }

            listView.RowActivated += (s, e) =>
            {
                var model = listView.Model;
                TreeIter iter;
                if (model.GetIter(out iter, e.Path))
                {
                    var url = model.GetValue(iter, 1).ToString();
                    onFavoriteSelected(url);
                    // dialog.Destroy();
                }
            };

            dialog.ContentArea.PackStart(listView, true, true, 0);
            listView.Show();
            dialog.AddButton("Close", ResponseType.Close);
            dialog.ShowAll();

            dialog.Run();
            // dialog.Destroy();
        }

        public static void ViewHistory(Window parent, IReadOnlyList<string> history, Action<string> onHistoryItemSelected)
        {
            var dialog = new Dialog("History", parent, DialogFlags.Modal);
            dialog.SetDefaultSize(400, 300);

            var treeView = new TreeView();
            var treeStore = new TreeStore(typeof(string));

            treeView.Model = treeStore;
            treeView.AppendColumn("URL", new CellRendererText(), "text", 0);

            foreach (var item in history)
            {
                treeStore.AppendValues(item);
            }

            treeView.RowActivated += (sender, e) =>
            {
                TreeIter iter;
                treeView.Model.GetIter(out iter, e.Path);
                var selectedUrl = (string)treeView.Model.GetValue(iter, 0);

                onHistoryItemSelected.Invoke(selectedUrl);
                dialog.Destroy();
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

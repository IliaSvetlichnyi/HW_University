using Gtk;
using SimpleWebBrowser.Http;
using System;
using System.Collections.Generic;

namespace SimpleWebBrowser.UI
{
    public class BrowserWindow : Window
    {
        // Fields and properties
        public Entry UrlEntry { get; private set; } = new Entry();
        public Button LoadButton { get; private set; } = new Button("Load");
        public TextView HtmlDisplay { get; private set; } = new TextView();
        public Statusbar Statusbar { get; private set; } = new Statusbar();
        public MenuBar MenuBar { get; private set; } = new MenuBar();
        public Button BackButton { get; private set; } = new Button("Back");
        public Button ForwardButton { get; private set; } = new Button("Forward");
        public Button RefreshButton { get; private set; } = new Button("Refresh");

        private HttpRequestManager requestManager;
        private List<string> history;
        private int currentHistoryIndex;

        private string homePageUrl = "https://www.hw.ac.uk/dubai/";

        private List<KeyValuePair<string, string>> favorites;

        // Constructor
        public BrowserWindow() : base("Simple Web Browser")
        {
            requestManager = new HttpRequestManager();
            history = new List<string>();
            currentHistoryIndex = -1;

            favorites = new List<KeyValuePair<string, string>>();

            InitializeComponents();

            UrlEntry.Text = homePageUrl; // установка домашней страницы в поле ввода URL при запуске
            LoadUrl(); // автоматическая загрузка домашней страницы при запуске
        }

        private void InitializeComponents()
        {
            InitializeEntry();
            InitializeButtons();
            InitializeMenuBar();
            InitializeTextView();
            InitializeStatusbar();
            SetupLayout();
        }

        private void InitializeEntry()
        {

        }

        private void InitializeButtons()
        {
            LoadButton.Clicked += LoadButton_Clicked;
            BackButton.Clicked += BackButton_Clicked;
            ForwardButton.Clicked += ForwardButton_Clicked;
            RefreshButton.Clicked += (sender, e) => LoadUrl();
        }

        private void InitializeMenuBar()
        {
            MenuBar = new MenuBar();

            var homeItem = new MenuItem("Home");
            homeItem.Activated += HomeItem_Activated;

            var setAsHomePageItem = new MenuItem("Set as Home Page");
            setAsHomePageItem.Activated += SetAsHomePageItem_Activated;

            var favoritesItem = new MenuItem("Favorites");
            var historyItem = new MenuItem("History");

            var addToFavoritesItem = new MenuItem("Add to Favorites");
            addToFavoritesItem.Activated += AddToFavoritesItem_Activated;

            var viewFavoritesItem = new MenuItem("View Favorites");
            viewFavoritesItem.Activated += ViewFavoritesItem_Activated;

            var favoritesSubmenu = new Menu();
            favoritesSubmenu.Append(addToFavoritesItem);
            favoritesSubmenu.Append(viewFavoritesItem);

            favoritesItem.Submenu = favoritesSubmenu;


            MenuBar.Append(homeItem);
            MenuBar.Append(setAsHomePageItem);
            MenuBar.Append(favoritesItem);
            MenuBar.Append(historyItem);
        }


        private void InitializeTextView()
        {
            HtmlDisplay = new TextView();
        }

        private void InitializeStatusbar()
        {
            Statusbar = new Statusbar();
        }

        private void HomeItem_Activated(object? sender, EventArgs e)
        {
            UrlEntry.Text = homePageUrl;
            LoadUrl();
        }

        private void SetAsHomePageItem_Activated(object? sender, EventArgs e)
        {
            homePageUrl = UrlEntry.Text;
        }

        private string? InputDialog(string prompt, string title)
        {
            string? result = null;

            using var dialog = new Dialog(title, this, DialogFlags.Modal);
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
                Console.WriteLine($"InputDialog returned: {result}"); // Debug
            }
            else
            {
                Console.WriteLine("InputDialog was cancelled or closed."); // Debug
            }

            dialog.Destroy();
            return result;
        }


        private void AddToFavoritesItem_Activated(object? sender, EventArgs e)
        {
            Console.WriteLine("Entering AddToFavoritesItem_Activated"); // Debug
            string? title = InputDialog("Enter title for favorite", "Add to Favorites");
            if (!string.IsNullOrWhiteSpace(title))
            {
                favorites.Add(new KeyValuePair<string, string>(title, UrlEntry.Text));
                Console.WriteLine($"Added to favorites. Current count: {favorites.Count}"); // Output to the console
                Statusbar.Push(0, $"Added to favorites. Current count: {favorites.Count}"); // Show in the status bar
            }
            else
            {
                Console.WriteLine("No title provided or title is whitespace."); // Debug
            }
            Console.WriteLine("Exiting AddToFavoritesItem_Activated"); // Debug
        }



        private void ViewFavorites()
        {
            using var dialog = new Dialog("Favorites", this, DialogFlags.Modal);
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

            Console.WriteLine("Entering ViewFavorites"); // Debug

            foreach (var favorite in favorites)
            {
                Console.WriteLine($"Appending favorite: {favorite.Key} - {favorite.Value}"); // Debug
                listStore.AppendValues(favorite.Key, favorite.Value);
            }

            Console.WriteLine("Exiting ViewFavorites"); // Debug

            listView.RowActivated += (s, e) =>
            {
                var model = listView.Model;
                TreeIter iter;
                if (model.GetIter(out iter, e.Path))
                {
                    var url = model.GetValue(iter, 1).ToString();
                    UrlEntry.Text = url;
                    LoadUrl();
                    dialog.Destroy();
                }
            };

            dialog.ContentArea.PackStart(listView, true, true, 0);
            listView.Show();
            dialog.AddButton("Close", ResponseType.Close);
            dialog.ShowAll();

            dialog.Run();
            dialog.Destroy();
        }


        private void ViewFavoritesItem_Activated(object? sender, EventArgs e)
        {
            ViewFavorites();
        }


        private void SetupLayout()
        {
            var scrolledWindow = new ScrolledWindow();
            scrolledWindow.Add(HtmlDisplay);

            var hbox = new Box(Orientation.Horizontal, 0);
            hbox.PackStart(BackButton, false, false, 0);
            hbox.PackStart(ForwardButton, false, false, 0);
            hbox.PackStart(RefreshButton, false, false, 0);
            hbox.PackStart(UrlEntry, true, true, 0);
            hbox.PackStart(LoadButton, false, false, 0);

            var vbox = new Box(Orientation.Vertical, 0);
            vbox.PackStart(MenuBar, false, false, 0);
            vbox.PackStart(hbox, false, false, 0);
            vbox.PackStart(scrolledWindow, true, true, 0);
            vbox.PackStart(Statusbar, false, false, 0);

            Add(vbox);
        }

        // Event handlers
        private void LoadButton_Clicked(object sender, EventArgs e)
        {
            LoadUrl();
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            if (currentHistoryIndex > 0)
            {
                currentHistoryIndex--;
                UrlEntry.Text = history[currentHistoryIndex];
                LoadUrl();
            }
        }

        private void ForwardButton_Clicked(object sender, EventArgs e)
        {
            if (currentHistoryIndex < history.Count - 1)
            {
                currentHistoryIndex++;
                UrlEntry.Text = history[currentHistoryIndex];
                LoadUrl();
            }
        }

        // Utility methods
        private void LoadUrl()
        {
            var responseContent = requestManager.FetchHtmlContent(UrlEntry.Text);
            HtmlDisplay.Buffer.Text = responseContent.HtmlContent;
            Statusbar.Push(0, $"Status: {responseContent.StatusCode}");

            if (currentHistoryIndex == history.Count - 1 || history.Count == 0)
            {
                history.Add(UrlEntry.Text);
                currentHistoryIndex++;
            }
        }
    }
}

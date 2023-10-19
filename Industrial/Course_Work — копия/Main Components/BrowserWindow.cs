using Gtk;
using SimpleWebBrowser.Http;
using SimpleWebBrowser.Controllers;
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

        private HomePageManager homePageManager = new HomePageManager(); // Добавлено

        private FavoritesManager favoritesManager = new FavoritesManager(); // Добавлено

        private HistoryManager historyManager = new HistoryManager(); // Добавлено

        private BulkDownloader bulkDownloader;


        // Constructor
        [Obsolete]
        public BrowserWindow() : base("Simple Web Browser")
        {
            requestManager = new HttpRequestManager();

            favoritesManager.LoadFavoritesFromFile();

            InitializeComponents();

            homePageManager.LoadHomePageToEntry(UrlEntry); // Модифицировано
            LoadUrl(); // автоматическая загрузка домашней страницы при запуске

            historyManager.LoadHistoryFromFile();

            bulkDownloader = new BulkDownloader(requestManager);
        }

        [Obsolete]
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

        [Obsolete]
        private void InitializeMenuBar()
        {
            MenuBar = new MenuBar();

            var homeItem = new MenuItem("Home");
            var setAsHomePageItem = new MenuItem("Set as Home Page");
            homePageManager.AttachHomePageEvents(homeItem, setAsHomePageItem, UrlEntry, LoadUrl);

            var addToFavoritesItem = new MenuItem("Add to Favorites");
            var viewFavoritesItem = new MenuItem("View Favorites");

            var viewHistoryItem = new MenuItem("View History");

            addToFavoritesItem.Activated += (sender, e) =>
            {
                var title = BrowserDialogs.InputDialog("Enter title for the favorite:", "Add to Favorites", this);
                if (!string.IsNullOrEmpty(title))
                {
                    favoritesManager.AddToFavorites(title, UrlEntry.Text);
                    favoritesManager.SaveFavoritesToFile();
                }
            };


            viewFavoritesItem.Activated += (sender, e) =>
            {
                BrowserDialogs.ViewFavorites(this, favoritesManager.Favorites.ToList(), (selectedUrl) =>
                {
                    UrlEntry.Text = selectedUrl;
                    LoadUrl();
                });
            };


            var favoritesItem = new MenuItem("Favorites");
            var favoritesSubmenu = new Menu();
            favoritesSubmenu.Append(addToFavoritesItem);
            favoritesSubmenu.Append(viewFavoritesItem);
            favoritesItem.Submenu = favoritesSubmenu;


            viewHistoryItem.Activated += (sender, e) =>
            {
                BrowserDialogs.ViewHistory(this, historyManager.History, (selectedUrl) =>

                {
                    UrlEntry.Text = selectedUrl;
                    LoadUrl();
                });
            };

            var historyItem = new MenuItem("History");
            var historySubmenu = new Menu();
            historySubmenu.Append(viewHistoryItem);
            historyItem.Submenu = historySubmenu;


            
            var bulkDownloadItem = new MenuItem("Bulk Download");
            bulkDownloadItem.Activated += BulkDownloadItem_Activated;

            

            MenuBar.Append(homeItem);
            MenuBar.Append(setAsHomePageItem);
            MenuBar.Append(favoritesItem);
            MenuBar.Append(historyItem);
            MenuBar.Append(bulkDownloadItem);



        }

        private void InitializeTextView()
        {
            HtmlDisplay = new TextView();
        }

        private void InitializeStatusbar()
        {
            Statusbar = new Statusbar();
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
            var previousUrl = historyManager.MoveBackInHistory();
            if (previousUrl != null)
            {
                UrlEntry.Text = previousUrl;
                LoadUrl();
            }
        }

        private void ForwardButton_Clicked(object sender, EventArgs e)
        {
            var nextUrl = historyManager.MoveForwardInHistory();
            if (nextUrl != null)
            {
                UrlEntry.Text = nextUrl;
                LoadUrl();
            }
        }


        private void BulkDownloadItem_Activated(object sender, EventArgs e)
        {
            var fileChooser = new FileChooserDialog(
                "Choose a file for bulk download",
                this,
                FileChooserAction.Open,
                "Cancel", ResponseType.Cancel,
                "Open", ResponseType.Accept);

            if (fileChooser.Run() == (int)ResponseType.Accept)
            {
                var results = bulkDownloader.PerformBulkDownload(fileChooser.Filename);
                HtmlDisplay.Buffer.Text = results;
            }
            fileChooser.Destroy();
        }



        // Utility methods
        private void LoadUrl()
        {
            var responseContent = requestManager.FetchHtmlContent(UrlEntry.Text);

            // Извлечение заголовка
            var title = requestManager.ExtractTitleFromHtml(responseContent.HtmlContent);

            // Добавление заголовка к содержимому (пример)
            HtmlDisplay.Buffer.Text = title + "\n\n" + responseContent.HtmlContent;

            Statusbar.Push(0, $"Status: {responseContent.StatusCode}");
            historyManager.AddToHistory(UrlEntry.Text);
        }
    }
}

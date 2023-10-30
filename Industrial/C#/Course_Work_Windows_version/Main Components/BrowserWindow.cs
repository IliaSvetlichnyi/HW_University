using Gtk;
using SimpleWebBrowser.Http;
using SimpleWebBrowser.Controllers;
using System;

namespace SimpleWebBrowser.UI
{
    public class BrowserWindow : Window
    {
        // Private fields and properties
        private Entry UrlEntry { get; set; } = new Entry();
        private Button LoadButton { get; set; } = new Button("Load");
        private TextView HtmlDisplay { get; set; } = new TextView();
        private Statusbar Statusbar { get; set; } = new Statusbar();
        private MenuBar MenuBar { get; set; } = new MenuBar();
        private Button BackButton { get; set; } = new Button("Back");
        private Button ForwardButton { get; set; } = new Button("Forward");
        private Button RefreshButton { get; set; } = new Button("Refresh");

        private HttpRequestManager requestManager;
        private HomePageManager homePageManager;
        private FavoritesManager favoritesManager;
        private HistoryManager historyManager;
        private BulkDownloader bulkDownloader;
        private ComboBox UrlSuggestions { get; set; } = new ComboBox();



        // Constructor for BrowserWindow
        public BrowserWindow() : base("Simple Web Browser")
        {
            InitializeManagers();
            InitializeComponents();
            LoadHomePage();
        }


        // Initialize managers and load data
        private void InitializeManagers()
        {
            requestManager = new HttpRequestManager();
            homePageManager = new HomePageManager();
            favoritesManager = new FavoritesManager();
            historyManager = new HistoryManager();
            bulkDownloader = new BulkDownloader(requestManager);

            favoritesManager.LoadFavoritesFromFile();
            historyManager.LoadHistoryFromFile();
        }


        // Initialize UI components for the browser window
        private void InitializeComponents()
        {
            InitializeButtons();
            InitializeMenuBar();
            InitializeTextView();
            InitializeUrlSuggestions();
            InitializeStatusbar();
            SetupLayout();
        }


        // Load the default home page
        private void LoadHomePage()
        {
            homePageManager.LoadHomePageToEntry(UrlEntry);
            LoadUrl();
        }


        private void InitializeButtons()
        {
            // Attach button click event handlers
            LoadButton.Clicked += LoadButton_Clicked;
            BackButton.Clicked += BackButton_Clicked;
            ForwardButton.Clicked += ForwardButton_Clicked;
            RefreshButton.Clicked += (sender, e) => LoadUrl();
        }

        private void InitializeMenuBar()
        {
            // Initialize the main menu bar
            MenuBar = new MenuBar();

            // Configure the home page related items
            InitializeHomePageItems();

            // Configure the favorites related items
            var favoritesItem = InitializeFavoritesItems();

            // Configure the history related items
            var historyItem = InitializeHistoryItems();

            // Configure the bulk download item
            var bulkDownloadItem = new MenuItem("Bulk Download");
            bulkDownloadItem.Activated += BulkDownloadItem_Activated;

            // Append all items to the main menu bar
            MenuBar.Append(bulkDownloadItem);
        }

        private void InitializeHomePageItems()
        {
            // Create the Home menu item
            var homeItem = new MenuItem("Home");

            // Create the Set as Home Page menu item
            var setAsHomePageItem = new MenuItem("Set as Home Page");

            // Attach events for home page functionalities
            homePageManager.AttachHomePageEvents(homeItem, setAsHomePageItem, UrlEntry, LoadUrl);

            // Append home related items to the main menu bar
            MenuBar.Append(homeItem);
            MenuBar.Append(setAsHomePageItem);
        }

        private MenuItem InitializeFavoritesItems()
        {
            // Create the Add to Favorites menu item
            var addToFavoritesItem = new MenuItem("Add to Favorites");
            addToFavoritesItem.Activated += (sender, e) =>
            {
                // Show input dialog to get the title for the favorite URL
                var title = BrowserDialogs.InputDialog("Enter title for the favorite:", "Add to Favorites", this);
                if (!string.IsNullOrEmpty(title))
                {
                    // Add the URL to favorites and save it
                    favoritesManager.AddToFavorites(title, UrlEntry.Text);
                    favoritesManager.SaveFavoritesToFile();
                }
            };

            // Create the View Favorites menu item
            var viewFavoritesItem = new MenuItem("View Favorites");
            viewFavoritesItem.Activated += (sender, e) =>
            {
                // Show the list of favorites and allow user to select one
                BrowserDialogs.ViewFavorites(this, favoritesManager.Favorites.ToList(), (selectedUrl) =>
                {
                    UrlEntry.Text = selectedUrl;
                    LoadUrl();
                });
            };

            // Group favorites-related items under a sub-menu
            var favoritesItem = new MenuItem("Favorites");
            var favoritesSubmenu = new Menu();
            favoritesSubmenu.Append(addToFavoritesItem);
            favoritesSubmenu.Append(viewFavoritesItem);
            favoritesItem.Submenu = favoritesSubmenu;

            // Append favorites related items to the main menu bar
            MenuBar.Append(favoritesItem);
            return favoritesItem;
        }

        private MenuItem InitializeHistoryItems()
        {
            // Create the View History menu item
            var viewHistoryItem = new MenuItem("View History");
            viewHistoryItem.Activated += (sender, e) =>
            {
                // Show the browsing history and allow user to select a URL
                BrowserDialogs.ViewHistory(this, historyManager.History, (selectedUrl) =>
                {
                    UrlEntry.Text = selectedUrl;
                    LoadUrl();
                });
            };

            // Group history-related items under a sub-menu
            var historyItem = new MenuItem("History");
            var historySubmenu = new Menu();
            historySubmenu.Append(viewHistoryItem);
            historyItem.Submenu = historySubmenu;

            // Append history related items to the main menu bar
            MenuBar.Append(historyItem);
            return historyItem;
        }


        // Initializes the TextView component where HTML content will be displayed.
        private void InitializeTextView()
        {
            HtmlDisplay = new TextView();
        }

        // Initializes the status bar for displaying brief messages at the bottom of the application.
        private void InitializeStatusbar()
        {
            Statusbar = new Statusbar();
        }

        // Sets up the entire layout of the application including all buttons, the address bar, and the display area.
        private void SetupLayout()
        {
            // Creates a scrollable window and adds the HTML display area inside it.
            var scrolledWindow = new ScrolledWindow();
            scrolledWindow.Add(HtmlDisplay);

            // Creates a horizontal box layout to arrange navigation buttons and the address bar.
            var hbox = new Box(Orientation.Horizontal, 0);
            hbox.PackStart(BackButton, false, false, 0); // Adds the 'Back' navigation button.
            hbox.PackStart(ForwardButton, false, false, 0); // Adds the 'Forward' navigation button.
            hbox.PackStart(RefreshButton, false, false, 0); // Adds the 'Refresh' button.
            hbox.PackStart(UrlEntry, true, true, 0); // Adds the address bar where URLs are entered.
            hbox.PackStart(LoadButton, false, false, 0); // Adds the 'Load' button to initiate the page load.

            // Creates a vertical box layout to stack the menu bar, navigation area, display area, and status bar.
            var vbox = new Box(Orientation.Vertical, 0);
            vbox.PackStart(MenuBar, false, false, 0); // Adds the menu bar at the top.
            vbox.PackStart(hbox, false, false, 0); // Adds the horizontal box containing navigation buttons and address bar.
            vbox.PackStart(UrlSuggestions, false, false, 0);
            vbox.PackStart(scrolledWindow, true, true, 0); // Adds the scrollable window with the HTML display area.
            vbox.PackStart(Statusbar, false, false, 0); // Adds the status bar at the bottom.

            // Adds the entire layout to the main window.
            Add(vbox);
        }



        // Event handler for the 'Load' button. Initiates loading the URL provided in the address bar.
        private void LoadButton_Clicked(object sender, EventArgs e)
        {
            LoadUrl();
        }

        // Event handler for the 'Back' button. Navigates to the previous URL in the browsing history if it exists.
        private void BackButton_Clicked(object sender, EventArgs e)
        {
            var previousUrl = historyManager.MoveBackInHistory();
            if (previousUrl != null)
            {
                UrlEntry.Text = previousUrl;
                LoadUrl();
            }
        }

        // Event handler for the 'Forward' button. Navigates to the next URL in the browsing history if it exists.
        private void ForwardButton_Clicked(object sender, EventArgs e)
        {
            var nextUrl = historyManager.MoveForwardInHistory();
            if (nextUrl != null)
            {
                UrlEntry.Text = nextUrl;
                LoadUrl();
            }
        }

        private void InitializeUrlSuggestions()
        {
            UrlSuggestions.Visible = false; // hidden by default
            UrlEntry.Changed += (sender, e) =>
            {
                var matches = historyManager.GetMatchingUrls(UrlEntry.Text);
                UrlSuggestions.Clear();

                if (matches.Count > 0)
                {
                    var cell = new CellRendererText();
                    UrlSuggestions.PackStart(cell, false);
                    UrlSuggestions.AddAttribute(cell, "text", 0);
                    UrlSuggestions.Model = new ListStore(typeof(string));
                    foreach (var match in matches)
                    {
                        (UrlSuggestions.Model as ListStore).AppendValues(match);
                    }
                    UrlSuggestions.Visible = true;
                }
                else
                {
                    UrlSuggestions.Visible = false;
                }
            };

            UrlSuggestions.Changed += (sender, e) =>
            {
                TreeIter iter;
                if (UrlSuggestions.GetActiveIter(out iter))
                {
                    var selectedUrl = (string)UrlSuggestions.Model.GetValue(iter, 0);
                    UrlEntry.Text = selectedUrl;
                    UrlSuggestions.Visible = false;
                    LoadUrl();
                }
            };
        }



        private void LoadUrl()
        {
            var responseContent = requestManager.FetchHtmlContent(UrlEntry.Text);

            // Extract the title from the HTML content
            var title = requestManager.ExtractTitleFromHtml(responseContent.HtmlContent);

            // Appending the title to the content
            HtmlDisplay.Buffer.Text = title + "\n\n" + responseContent.HtmlContent;

            // Update the status bar with the status code from the response
            Statusbar.Push(0, $"Status: {responseContent.StatusCode}");
            // Add the visited URL to browsing history
            historyManager.AddToHistory(UrlEntry.Text);
        }


        private void BulkDownloadItem_Activated(object sender, EventArgs e)
        {
            var fileChooser = new FileChooserDialog(
                "Choose a file for bulk download",
                this,
                FileChooserAction.Open,
                "Cancel", ResponseType.Cancel,
                "Open", ResponseType.Accept);

            // Check if user has selected a file
            if (fileChooser.Run() == (int)ResponseType.Accept)
            {
                // Perform bulk download based on selected file
                var results = bulkDownloader.PerformBulkDownload(fileChooser.Filename);
                HtmlDisplay.Buffer.Text = results;
            }
            // Destroy the file chooser dialog after use
            fileChooser.Destroy();
        }

    }
}

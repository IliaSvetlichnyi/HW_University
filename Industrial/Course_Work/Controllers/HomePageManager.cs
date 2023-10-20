using Gtk;
using System;

namespace SimpleWebBrowser.Controllers
{
    public class HomePageManager
    {
        private string homePageUrl = "https://www.hw.ac.uk/dubai/";

        // Property to get or set the home page URL
        public string HomePageUrl
        {
            get => homePageUrl;
            private set => homePageUrl = value;
        }

        // Set the home page URL from a given Entry widget
        public void SetHomePageFromEntry(Entry urlEntry)
        {
            homePageUrl = urlEntry.Text;
        }

        // Load the current home page URL into a given Entry widget
        public void LoadHomePageToEntry(Entry urlEntry)
        {
            urlEntry.Text = homePageUrl;
        }

        // Attach events to MenuItems for home page functionalities
        public void AttachHomePageEvents(MenuItem homeItem, MenuItem setAsHomePageItem, Entry urlEntry, System.Action loadUrlAction)
        {
            // When the 'Home' menu item is activated, load the home page URL
            homeItem.Activated += (sender, e) =>
            {
                LoadHomePageToEntry(urlEntry);
                loadUrlAction();
            };

            // When the 'Set As Home Page' menu item is activated, set the current URL as home page
            setAsHomePageItem.Activated += (sender, e) => SetHomePageFromEntry(urlEntry);
        }
    }
}

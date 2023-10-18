using Gtk;
using System;

namespace SimpleWebBrowser.Controllers
{
    public class HomePageManager
    {
        private string homePageUrl = "https://www.hw.ac.uk/dubai/";

        public string HomePageUrl
        {
            get => homePageUrl;
            set => homePageUrl = value;
        }

        public void SetHomePageFromEntry(Entry urlEntry)
        {
            homePageUrl = urlEntry.Text;
        }

        public void LoadHomePageToEntry(Entry urlEntry)
        {
            urlEntry.Text = homePageUrl;
        }

        public void AttachHomePageEvents(MenuItem homeItem, MenuItem setAsHomePageItem, Entry urlEntry, System.Action loadUrlAction)
        {
            homeItem.Activated += (sender, e) =>
            {
                LoadHomePageToEntry(urlEntry);
                loadUrlAction();
            };

            setAsHomePageItem.Activated += (sender, e) => SetHomePageFromEntry(urlEntry);
        }
    }
}

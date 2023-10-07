using Gtk;
using SimpleWebBrowser.UI;
using SimpleWebBrowser.Http;

namespace SimpleWebBrowser
{
    class Program
    {
        static void Main()
        {
            Application.Init();

            var mainWindow = new BrowserWindow();
            mainWindow.ShowAll();

            Application.Run();
        }
    }
}
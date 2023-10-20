// Import necessary libraries.
using Gtk;
using SimpleWebBrowser.UI;

namespace SimpleWebBrowser
{
    class Program
    {
        // Main entry point of the application.
        static void Main()
        {
            // Initialize the Gtk application.
            Application.Init();

            // Create a new instance of the main browser window.
            var mainWindow = new BrowserWindow();

            // Show all elements of the main window.
            mainWindow.ShowAll();

            // Start the Gtk application loop, waiting for user interactions.
            Application.Run();
        }
    }
}

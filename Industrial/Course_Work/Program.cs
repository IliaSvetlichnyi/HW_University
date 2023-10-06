using Gtk;

class Program
{
    static void Main()
    {
        Application.Init();

        var win = new Window("Simple Web Browser");
        win.Resize(800, 600);
        win.DeleteEvent += (sender, e) => Application.Quit();

        var vbox = new VBox(false, 2);

        var urlEntry = new Entry();
        urlEntry.Text = "http://";

        vbox.PackStart(urlEntry, false, false, 0);

        win.Add(vbox);

        win.ShowAll();

        Application.Run();
    }
}

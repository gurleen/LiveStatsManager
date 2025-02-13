using Eto.Forms;
using Eto.Drawing;

namespace StatsFetcher
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            Title = "DragonsTV Stats Fetcher";
            MinimumSize = new Size(200, 200);

            Content = new StackLayout
            {
                Padding = 10,
                Items =
                {
                    "Hello World!",
                }
            };

            var clickMe = new Command { MenuText = "Click Me!", ToolBarText = "Click Me!" };
            clickMe.Executed += (sender, e) => MessageBox.Show(this, "I was clicked!");

            var quitCommand = new Command
                { MenuText = "Quit", Shortcut = Application.Instance.CommonModifier | Keys.Q };
            quitCommand.Executed += (sender, e) => Application.Instance.Quit();

            var aboutCommand = new Command { MenuText = "About..." };
            aboutCommand.Executed += (sender, e) => new AboutDialog().ShowDialog(this);

            Menu = new MenuBar
            {
                Items =
                {
                    new SubMenuItem { Text = "&File", Items = { clickMe } },
                },
                ApplicationItems =
                {
                    new ButtonMenuItem { Text = "&Preferences..." },
                },
                QuitItem = quitCommand,
                AboutItem = aboutCommand
            };

            ToolBar = new ToolBar { Items = { clickMe } };
        }
    }
}
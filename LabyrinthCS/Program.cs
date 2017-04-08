using System.Drawing;
using System.Globalization;

using BearLib;

using Labyrinth.UI;
using Labyrinth.UI.Input;
using Labyrinth.UI.Scenes;

namespace Labyrinth
{
    internal class Program : EventLoop
    {
        private static readonly Key AltF4 = KeyDatabase.Parse("alt f4");
        protected override bool IsRunning => State.IsRunning;

        protected override void React(Scene scene, Event @event)
        {
            if (@event == null)
            {
                return;
            }

            if (@event is CloseEvent)
            {
                State.SignalExit();
                return;
            }

            if (@event is KeyEvent key && (key.Key == AltF4))
            {
                State.SignalExit();
                return;
            }

            base.React(scene, @event);
        }

        private static void Main()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            Terminal.Open();
            Terminal.Set(
                $"window: title=Labyrinth, size={Const.Width}x{Const.Height};" +
                "input: precise-mouse=false, filter=[keyboard, mouse, system], alt-functions=false;" +
                "log: level=fatal;"
            );
            Terminal.Color(Color.White);
            Terminal.BkColor(Color.Black);
            Terminal.Refresh();

            State.SetScene<MainMenuScene>();
            new Program().Run();
        }
    }
}

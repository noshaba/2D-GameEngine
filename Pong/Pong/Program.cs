using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;

namespace Pong {
    class Program {
        static ContextSettings context = new ContextSettings();
        static RenderWindow window;

        static void Main(string[] args) {
            Console.WriteLine("Hello World! :D");
            window = new RenderWindow(new VideoMode(1024, 768), "Pong!", Styles.Default, context);
            window.Closed += window_Closed;
            window.KeyReleased += window_KeyReleased;
            window.SetActive(true);
            window.SetFramerateLimit(60);
            while (window.IsOpen()) {
                window.DispatchEvents();
                window.Clear();

                window.Display();
            }
        }

        static void window_KeyReleased(object sender, KeyEventArgs e) {
            if (e.Code == Keyboard.Key.Escape) {
                window.Close();
            }
        }

        static void window_Closed(object sender, EventArgs e) {
            window.Close();
        }

    }
}

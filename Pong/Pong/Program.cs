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

        static Texture mouse_tex = new Texture("../Content/Mouse.png");
        static Sprite mouse_sprite;

        static void Main(string[] args) {
            /*
             * stopwatch
             * renderwindow
             * setverticalsyncenabled
             * dt = clock.elapsedtimes / (float) stopwatch.frequency
             * clock.restart
             * while dt > tick
             * 
             */
            Console.WriteLine("Hello World! :D");
            window = new RenderWindow(new VideoMode(1024, 768), "Pong!", Styles.Default, context);
            window.Closed += window_Closed;
            window.KeyReleased += window_KeyReleased;
            window.MouseMoved += window_MouseMoved;
            window.MouseEntered += window_MouseEntered;
            window.MouseLeft += window_MouseLeft;
            window.SetActive(true);
            window.SetFramerateLimit(60);
            mouse_sprite = new Sprite(mouse_tex);
            while (window.IsOpen()) {
                window.DispatchEvents();
                window.Clear();

                mouse_sprite.Draw(window, RenderStates.Default);
                window.Display();
            }
        }

        private static void window_MouseMoved(object sender, MouseMoveEventArgs e) {
            mouse_sprite.Position = new Vector2f(e.X,e.Y);
        }

        private static void window_MouseLeft(object sender, EventArgs e) {
            window.SetMouseCursorVisible(true);
        }

        private static void window_MouseEntered(object sender, EventArgs e) {
            window.SetMouseCursorVisible(false);
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

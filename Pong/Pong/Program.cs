using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SFML.Graphics;
using SFML.Window;

namespace Pong {
    class Program {
        const int WIDTH = 1024;
        const int HEIGHT = 576;
        static ContextSettings context = new ContextSettings();
        static RenderWindow window = new RenderWindow(new VideoMode(WIDTH, HEIGHT), "Pong!", Styles.Default, context);

        static Sprite mouseSprite = new Sprite(new Texture("../Content/Mouse.png"));

        static Game pong = new Game(WIDTH,HEIGHT,window);

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
            InitWindow();
            while (window.IsOpen()) {
                window.DispatchEvents();
                window.Clear();

                Draw();
                window.Display();
            }
        }

        private static void InitWindow() {
            window.Closed += window_Closed;
            window.KeyReleased += window_KeyReleased;
            window.MouseMoved += window_MouseMoved;
            window.MouseEntered += window_MouseEntered;
            window.MouseLeft += window_MouseLeft;
            window.SetActive(true);
            window.SetFramerateLimit(60);
        }

        private static void Draw() {
            pong.Draw();
            mouseSprite.Draw(window, RenderStates.Default);
        }

        #region Listener

        private static void window_MouseMoved(object sender, MouseMoveEventArgs e) {
            mouseSprite.Position = new Vector2f(e.X,e.Y);
            pong.MovePlayer(e.Y);
        }

        private static void window_MouseLeft(object sender, EventArgs e) {
            window.SetMouseCursorVisible(true);
        }

        private static void window_MouseEntered(object sender, EventArgs e) {
            window.SetMouseCursorVisible(false);
        }

        static void window_KeyReleased(object sender, KeyEventArgs e) {
            switch (e.Code) { 
                case Keyboard.Key.Escape:
                    window.Close();
                    break;
                case Keyboard.Key.Return:

                    break;
                default:
                    break;
            }
        }

        static void window_Closed(object sender, EventArgs e) {
            window.Close();
        }

        #endregion

    }
}

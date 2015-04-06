using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;

namespace Pong {
    class Program {
        const int WIDTH = 1024;
        const int HEIGHT = 576;
        static ContextSettings context = new ContextSettings();
        static RenderWindow window = new RenderWindow(new VideoMode(WIDTH, HEIGHT), "Pong!", Styles.Default, context);

        static Sprite mouseSprite = new Sprite(new Texture("../Content/Mouse.png"));

        static Paddle player = new Paddle(new Vector2f(50, HEIGHT * 0.5f) , new Vector2f(25, 100), Color.Cyan);
        static Paddle ai = new Paddle(new Vector2f(WIDTH - 50, HEIGHT * 0.5f), new Vector2f(25, 100), Color.Green);
        static Ball ball = new Ball(new Vector2f(WIDTH * 0.5f, 12.5f), 12.5f, Color.Red,1);

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
            window.Draw(player);
            window.Draw(ai);
            window.Draw(ball);
            mouseSprite.Draw(window, RenderStates.Default);
        }

        #region Listener

        private static void window_MouseMoved(object sender, MouseMoveEventArgs e) {
            mouseSprite.Position = new Vector2f(e.X,e.Y);
            player.move(e.Y);
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

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using SFML.Graphics;
using SFML.Window;

namespace Pong {
    class Program {
        const int WIDTH = 1024;
        const int HEIGHT = 576;
        const float FPS = 60.0f;
        const float DT = 1.0f / FPS;
        static float accumulator = 0;

        static ContextSettings context = new ContextSettings();
        static Stopwatch timer = new Stopwatch();
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

            timer.Start();
            float frameStart = timer.ElapsedMilliseconds / 100.0f;

            while (window.IsOpen()) {
                window.DispatchEvents();
                window.Clear();

                //pong.Update(DT);
                Update(ref frameStart);

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

        private static void Update(ref float frameStart) {
            float currentTime = timer.ElapsedMilliseconds / 100.0f;
            accumulator += currentTime - frameStart;
            frameStart = currentTime;

            if (accumulator > 0.2f)
                accumulator = 0.2f;
            
            while (accumulator >= DT) {
                pong.Update(DT);
                accumulator -= DT;
            }
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
                    pong.Start();
                    break;
                case Keyboard.Key.P:
                    pong.ToggleFreeze();
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

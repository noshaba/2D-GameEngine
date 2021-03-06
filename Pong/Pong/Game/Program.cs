﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFML.Audio;

namespace Pong {
    class Program {
        const int WIDTH = 1024;
        const int HEIGHT = 576;
        const float FPS = 60.0f;
        const float MIN_FPS = 20.0f;
        const float DT = 1.0f / FPS;
        const float MAX_DT = 1.0f / MIN_FPS;
        static float accumulator = 0;

        //timers and window
        static ContextSettings context = new ContextSettings();
        static Stopwatch timer = new Stopwatch();
        static Stopwatch FPSClock = new Stopwatch();
        static RenderWindow window;

        static Sprite mouseSprite = new Sprite(new Texture("../Content/Mouse.png"));

        //create game and gui fitting for the window
        static Game pong = new Game(WIDTH, HEIGHT);
        static GUI gui = new GUI(WIDTH, HEIGHT, pong);

        //main method
        static void Main(string[] args) {
            //Console.WriteLine("Hello World!");
            InitWindow();
            timer.Start();
            float frameStart = timer.ElapsedMilliseconds / 1000.0f;
            //game loop
            while (window.IsOpen) {
                window.DispatchEvents();
                window.Clear();
                Update(ref frameStart);
                Draw(accumulator/DT);
                window.Display();
            }
        }

        //initialize window and create event handlers
        private static void InitWindow() {
            context.AntialiasingLevel = 8;
            window = new RenderWindow(new VideoMode(WIDTH, HEIGHT), "Pong!", Styles.Default, context);
            window.Closed += window_Closed;
            window.KeyReleased += window_KeyReleased;
            window.MouseMoved += window_MouseMoved;
            window.MouseEntered += window_MouseEntered;
            window.MouseLeft += window_MouseLeft;
            window.MouseButtonReleased += window_MouseButtonReleased;
            window.MouseButtonPressed += window_MouseButtonPressed;
            window.SetActive(true);
        }

        //update method with time management
        private static void Update(ref float frameStart) {
            float currentTime = timer.ElapsedMilliseconds / 1000.0f;
            accumulator += currentTime - frameStart;
            frameStart = currentTime;
            if (accumulator > MAX_DT) accumulator = MAX_DT;
            while (accumulator >= DT) {
                //update the game as long as the "lag" is not compensated 
                pong.Update(DT);
                accumulator -= DT;
            }
        }

        //draw method
        private static void Draw(float alpha) {
            pong.Draw(window, alpha);
            gui.Draw(window);
            mouseSprite.Draw(window, RenderStates.Default);
        }

        #region Listener

        private static void window_MouseMoved(object sender, MouseMoveEventArgs e) {
            mouseSprite.Position = new Vector2f(e.X,e.Y);
            pong.MovePlayer(e.Y);
            gui.OnHover(e.X, e.Y);
        }

        private static void window_MouseLeft(object sender, EventArgs e) {
            window.SetMouseCursorVisible(true);
        }

        private static void window_MouseEntered(object sender, EventArgs e) {
            window.SetMouseCursorVisible(false);
        }

        private static void window_MouseButtonReleased(object sender, MouseButtonEventArgs e) {
            gui.Released(e.X, e.Y);
        }

        private static void window_MouseButtonPressed(object sender, MouseButtonEventArgs e) {
            gui.Pressed(e.X, e.Y);
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
                case Keyboard.Key.Space:
                    pong.Restart();
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

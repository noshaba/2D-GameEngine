using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class Program
    {
        const int WIDTH = 1200;
        const int HEIGHT = 700;
        const float FPS = 60.0f;
        const float MIN_FPS = 20.0f;
        const float DT = 1.0f / FPS;
        const float MAX_DT = 1.0f / MIN_FPS;
        static float accumulator = 0;

        //timers and window
        static ContextSettings context = new ContextSettings();
        static Stopwatch timer = new Stopwatch();
        static RenderWindow window;
        static View view = new View(new Vector2f(WIDTH * .5f, HEIGHT * .5f), new Vector2f(WIDTH, HEIGHT));
        static View GUIview = new View(new Vector2f(WIDTH * .5f, HEIGHT * .5f), new Vector2f(WIDTH, HEIGHT));

        static Sprite mouseSprite = new Sprite(new Texture("../Content/Mouse.png"));

        //create game and gui fitting for the window
        static Game sEmUp = new Game(WIDTH, HEIGHT);
        static GUI gui = new GUI(WIDTH, HEIGHT, sEmUp);

        //main method
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            InitWindow();
            timer.Start();
            float frameStart = timer.ElapsedMilliseconds / 1000.0f;
            //game loop
            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear();
                Update(ref frameStart);
                Draw(accumulator / DT);
                window.Display();
            }
        }

        //initialize window and create event handlers
        private static void InitWindow()
        {
            context.AntialiasingLevel = 8;
            window = new RenderWindow(new VideoMode(WIDTH, HEIGHT), "Platformer", Styles.Close, context);
            window.Position = new Vector2i(0,0);
            window.SetView(view);
            window.SetActive(true);
            window.Closed += window_Closed;
            window.KeyReleased += window_KeyReleased;
            //window.KeyPressed += window_KeyPressed;
            window.MouseMoved += window_MouseMoved;
            window.MouseEntered += window_MouseEntered;
            window.MouseLeft += window_MouseLeft;
            window.MouseButtonReleased += window_MouseButtonReleased;
            window.MouseButtonPressed += window_MouseButtonPressed;
        }

        //update method with time management
        private static void Update(ref float frameStart)
        {
            float currentTime = timer.ElapsedMilliseconds / 1000.0f;
            accumulator += currentTime - frameStart;
            frameStart = currentTime;
            if (accumulator > MAX_DT) accumulator = MAX_DT;
            while (accumulator >= DT)
            {
               // mouseSprite.Position = window.MapPixelToCoords(Mouse.GetPosition(window), GUIview);
                if (sEmUp.status == Game.GameStatus.Active)
                    sEmUp.physics.Update(DT, view.Center);
                accumulator -= DT;
            }
            if (sEmUp.status == Game.GameStatus.Active)
            {
                ReadInput();
                /*if (sEmUp.player.rigidBody.COM.X >= view.Center.X && sEmUp.player.rigidBody.Velocity.X > 0 && sEmUp.player.rigidBody.COM.X < sEmUp.planet.backgroundSprite.TextureRect.Width-WIDTH/2)
                {*/
                view.Center = sEmUp.player.rigidBody.COM;
                if (view.Center.X < 600) 
                    view.Center = new Vector2f(600,view.Center.Y);
                if (view.Center.X > Game.levelSize.X - 600)
                    view.Center = new Vector2f(Game.levelSize.X - 600, view.Center.Y);
                if (view.Center.Y < 700)
                    view.Center = new Vector2f(view.Center.X, 700);
                if (view.Center.Y > Game.levelSize.Y - 350)
                    view.Center = new Vector2f(view.Center.X, Game.levelSize.Y - 350);
                sEmUp.EarlyUpdate(view.Center);
                sEmUp.LateUpdate(view.Center);
                /*}
                else {
                    view.Center = new Vector2f(view.Center.X, HEIGHT / 2);
                    if (sEmUp.player.rigidBody.COM.X > sEmUp.planet.backgroundSprite.TextureRect.Width - WIDTH / 2)
                    {
                        sEmUp.player.rigidBody.Velocity = new Vector2f(100, 0);
                        sEmUp.levelEnded = true;
                    }
                }*/
            }
            window.SetView(view);
        }

        //draw method
        private static void Draw(float alpha)
        {
            if (sEmUp.status == Game.GameStatus.Nextlevel || sEmUp.status == Game.GameStatus.Credits || sEmUp.status == Game.GameStatus.Start)
            { 
                view = new View(new Vector2f(WIDTH * .5f, HEIGHT * .5f), new Vector2f(WIDTH, HEIGHT));
            }
            window.SetView(view);
            sEmUp.Draw(window, alpha, view.Center);
            window.SetView(GUIview);
            gui.Draw(window);
            mouseSprite.Draw(window, RenderStates.Default);
        }

        private static void ReadInput()
        {
           if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
            {
                sEmUp.MovePlayer(Keyboard.Key.Left);
            }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
            {
                sEmUp.MovePlayer(Keyboard.Key.Right);
            }
            if(Keyboard.IsKeyPressed(Keyboard.Key.Up))
            {
                sEmUp.MovePlayer(Keyboard.Key.Up);
            } 
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
            {
                sEmUp.MovePlayer(Keyboard.Key.Down);
            }
        }

        #region Listener

        private static void window_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            mouseSprite.Position = window.MapPixelToCoords(Mouse.GetPosition(window), GUIview);
         //   gui.OnHover(e.X, e.Y);
        }

        private static void window_MouseLeft(object sender, EventArgs e)
        {
            window.SetMouseCursorVisible(true);
        }

        private static void window_MouseEntered(object sender, EventArgs e)
        {
            window.SetMouseCursorVisible(false);
        }

        private static void window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            gui.Released(e.X, e.Y);
        }

        private static void window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            gui.Pressed(e.X, e.Y);
        }

        static void window_KeyReleased(object sender, KeyEventArgs e)
        {
            switch (e.Code)
            {
                case Keyboard.Key.D:
                    Game.debug = !Game.debug;
                    break;
                case Keyboard.Key.Escape:
                    window.Close();
                    break;
                case Keyboard.Key.Return:
                    //sEmUp.StartGame();
                    break;
                case Keyboard.Key.P:
                    if (sEmUp.status == Game.GameStatus.Active) sEmUp.Pause();
                    break;
                default:
                    sEmUp.player.Release(e.Code);
                    break;
            }
        }

        static void window_KeyPressed(object sender, KeyEventArgs e) {
            if (sEmUp.status == Game.GameStatus.Active) {
                switch(e.Code) {
                    case Keyboard.Key.Left:
                    case Keyboard.Key.Right:
                    case Keyboard.Key.Up:
                    case Keyboard.Key.Down:
                 //       sEmUp.MovePlayer(e.Code);
                        break;
                    case Keyboard.Key.Space:
                  //      sEmUp.Fire();
                        break;
                }
            }
        }

        static void window_Closed(object sender, EventArgs e)
        {
            window.Close();
        }

        #endregion
    }
}

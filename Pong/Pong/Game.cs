using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;

namespace Pong {
    class Game {
        private Paddle ai;
        private Paddle player;
        private Ball ball;
        private RenderWindow window;
        private Physics physics;
        private List<IShape> objects = new List<IShape>();
        private int WIDTH;
        private int HEIGHT;
        private Random random;

        public Game(int width, int height, RenderWindow window) {
            WIDTH = width;
            HEIGHT = height;
            this.window = window;
            random = new Random();
            physics = new Physics();
            // game elements
            player = new Paddle(new Vector2f(50, height * 0.5f), new Vector2f(25, 100), Color.Cyan);
            ai = new Paddle(new Vector2f(width - 50, height * 0.5f), new Vector2f(25, 100), Color.Green);
            ball = new Ball(new Vector2f(width * 0.5f, 30), 12.5f, Color.Red, 10);
            AddObject(player);
            AddObject(ai);
            AddObject(ball);
            // walls
            AddObject(new OBB(new Vector2f(width * 0.5f, 12.5f), new Vector2f(width, 25), 0, Color.White));
            AddObject(new OBB(new Vector2f(width * 0.5f, height - 12.5f), new Vector2f(width, 25), 0, Color.White));
            // obstacles
            for (int i = 0; i < random.Next(10, 15); ++i)
                AddObject(new OBB(new Vector2f(random.Next(width), random.Next(height)), new Vector2f(random.Next(100), random.Next(100)), random.Next(360), Color.White, (float) random.NextDouble()));
        }

        private void AddObject(IShape obj) {
            objects.Add(obj);
            physics.AddObject(obj);
        }

        public void Update(float dt) {
            if (ball.COM.X < 0) {
                ai.score++;
                ball.Reset();
            }
            if (ball.COM.X > WIDTH) {
                player.score++;
                ball.Reset();
            }
            ai.move(ball.COM.Y);
            physics.Update(dt);
        }

        public void Start() {
            ball.Velocity = new Vector2f(-50, 50);
        }

        private void Reset() {
            ball.Reset();
        }

        public void Draw(float alpha) {
            State interpol;
            Transform t;
            RenderStates r = new RenderStates();
            foreach (Shape obj in objects) {
                interpol = (obj as IShape).Interpolation(alpha);
                t = Transform.Identity;
                t.Translate(interpol.position);
                t.Rotate(interpol.DegOrientation);
                r.Transform = t;
                window.Draw(obj, r);
            }
        }

        public void MovePlayer(float y) {
            player.move(y);
        }

        public void ToggleFreeze(){
            physics.frozen = !physics.frozen;
        }
    }
}

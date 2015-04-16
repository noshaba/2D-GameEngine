using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;

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
        private Font font;
        private Text playerScore;
        private Text aiScore;

        public Game(int width, int height, RenderWindow window) {
            WIDTH = width;
            HEIGHT = height;
            this.window = window;
            random = new Random();
            physics = new Physics();
            // game elements
            player = new Paddle(new Vector2f(50, height * 0.5f), new Vector2f(25, 100), Color.Cyan);
            ai = new Paddle(new Vector2f(width - 50, height * 0.5f), new Vector2f(25, 100), Color.Green);
            ball = new Ball(new Vector2f(width * 0.5f, 50), 12.5f, Color.Red, 10);
            AddObject(player);
            AddObject(ai);
            AddObject(ball);
            // walls and obstacles
            AddObject(new OBB(new Vector2f(width * 0.5f, 12.5f), new Vector2f(width, 25), 0, Color.White));
            AddObject(new OBB(new Vector2f(width * 0.5f, height - 12.5f), new Vector2f(width, 25), 0, Color.White));
            AddObject(new OBB(new Vector2f(12.5f, height * 0.5f), new Vector2f(25, height), 0, Color.White));
            AddObject(new OBB(new Vector2f(width - 12.5f, height * 0.5f), new Vector2f(25, width), 0, Color.White));
            AddObstacles();
            // score
            font = new Font("../Content/arial.ttf");
            playerScore = new Text(player.score.ToString(), font, 50);
            playerScore.Position = new Vector2f(width * 0.25f, 50);
            playerScore.Style = Text.Styles.Bold;
            playerScore.Color = Color.Cyan;
            aiScore = new Text(ai.score.ToString(), font, 50);
            aiScore.Position = new Vector2f(width * 0.75f, 50);
            aiScore.Style = Text.Styles.Bold;
            aiScore.Color = Color.Green;
        }

        private void AddObject(IShape obj) {
            objects.Add(obj);
            physics.AddObject(obj);
        }

        private void AddObstacles() {
            // static obstacles
            for (int i = 0; i < random.Next(5, 10); ++i)
                AddObject(new OBB(new Vector2f(random.Next(WIDTH), random.Next(100, HEIGHT)), new Vector2f(random.Next(100), random.Next(100)), random.Next(360), Color.White));
            // moveable obstacles
            for (int i = 0; i < random.Next(3, 6); ++i)
                AddObject(new OBB(new Vector2f(random.Next(WIDTH), random.Next(100, HEIGHT)), new Vector2f(random.Next(100), random.Next(100)), random.Next(360), Color.Blue, random.Next(5, 10)));
        }

        private void ResetObstacles() {
            objects.RemoveRange(7, objects.Count - 7);
            physics.Reset();
            AddObstacles();
        }

        public void Update(float dt) {
            if (ball.COM.X < player.COM.X) {
                ai.score++;
                aiScore.DisplayedString = ai.score.ToString();
                Sounds.scoreSound.Play();
                Reset();
            }
            if (ball.COM.X > ai.COM.X) {
                player.score++;
                playerScore.DisplayedString = player.score.ToString();
                Sounds.scoreSound.Play();
                Reset();
            }
            // if ball too fast or FPS too low
            if (ball.COM.Y > HEIGHT || ball.COM.Y < 0)
                Reset();
            ai.move(ball.COM.Y);
            ball.IncreaseVelocity(dt);
            physics.Update(dt);
        }

        public void Start() {
            ball.Impulse();
        }

        private void Reset() {
            ball.Reset();
            ResetObstacles();
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
            window.Draw(playerScore);
            window.Draw(aiScore);
        }

        public void MovePlayer(float y) {
            player.move(y);
        }

        public void ToggleFreeze(){
            physics.frozen = !physics.frozen;
        }
    }
}

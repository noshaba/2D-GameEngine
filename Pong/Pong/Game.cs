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
        public Ball ball;
        private Physics physics;
        private List<IShape> objects = new List<IShape>();
        private int WIDTH;
        private int HEIGHT;
        private Font font;
        private Text playerScore;
        private Text aiScore;
        private float difficulty;
        private bool soundOn;
        
        //constructor
        public Game(int width, int height) {
            WIDTH = width;
            HEIGHT = height;
            physics = new Physics();
            // game elements
            player = new Paddle(new Vector2f(50, height * 0.5f), 12.5f, 50, Color.Cyan);
            ai = new Paddle(new Vector2f(width - 50, height * 0.5f), 12.5f, 50, Color.Green);
            ball = new Ball(new Vector2f(width * 0.5f, 50), 12.5f, Color.Red, 10);
            //determimes difficulty of the AI enemy 0.0=unbeatable, 1.0=easy 
            difficulty = 0.0f;
            AddObject(player);
            AddObject(ai);
            AddObject(ball);
            // walls and obstacles
            Polygon wall1 = new Polygon(Color.White);
            Polygon wall2 = new Polygon(Color.White);
            Polygon wall3 = new Polygon(Color.White);
            Polygon wall4 = new Polygon(Color.White);
            wall1.SetBox(new Vector2f(width * 0.5f, 12.5f), width * .5f, 12.5f, 0);
            AddObject(wall1);
            wall2.SetBox(new Vector2f(width * 0.5f, height - 12.5f), width * .5f, 12.5f, 0);
            AddObject(wall2);
            wall3.SetBox(new Vector2f(12.5f, height * 0.5f), 12.5f, height * .5f, 0);
            AddObject(wall3);
            wall4.SetBox(new Vector2f(width - 12.5f, height * 0.5f), 12.5f, width * .5f, 0);
            AddObject(wall4);
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

            soundOn = true;
        }

        private void AddObject(IShape obj) {
            objects.Add(obj);
            physics.AddObject(obj);
        }

        //adds random obstacles
        private void AddObstacles() {
            // static obstacles
            for (int i = 0; i < EMath.random.Next(5, 10); ++i)
                AddObject(new Polygon(new Vector2f(EMath.Random(0, WIDTH), EMath.Random(100, HEIGHT)), EMath.Random(0, 360), Color.White));
                //AddObject(new OBB(new Vector2f(random.Next(WIDTH), random.Next(100, HEIGHT)), new Vector2f(random.Next(100), random.Next(100)), random.Next(360), Color.White));
            // moveable obstacles
            for (int i = 0; i < EMath.random.Next(3, 6); ++i)
                AddObject(new Polygon(new Vector2f(EMath.Random(0, WIDTH), EMath.Random(100, HEIGHT)), EMath.Random(0, 360), Color.Yellow, 0.1f));
                //AddObject(new OBB(new Vector2f(random.Next(WIDTH), random.Next(100, HEIGHT)), new Vector2f(random.Next(100), random.Next(100)), random.Next(360), Color.Yellow, random.Next(5, 10)));
        }

        //resets obstacles, leaves 1 ball, 2 paddles and 4 walls
        private void ResetObstacles() {
            objects.RemoveRange(7, objects.Count - 7);
            physics.Reset();
            AddObstacles();
        }

        public void Update(float dt) {
            if (ball.COM.X < player.COM.X) {
                ai.score++;
                aiScore.DisplayedString = ai.score.ToString();
                if (soundOn)
                Sounds.scoreSound.Play();
                Reset();
            }
            if (ball.COM.X > ai.COM.X) {
                player.score++;
                playerScore.DisplayedString = player.score.ToString();
                if (soundOn)
                Sounds.scoreSound.Play();
                Reset();
            }
            // if ball too fast or FPS too low
            if (ball.COM.Y > HEIGHT - 25 || ball.COM.Y < 25) {
                ball.Current = ball.Previous;
                ball.Velocity = new Vector2f(ball.Velocity.X, -ball.Velocity.Y);
            }
            ai.moveAi(ball.COM.Y, ball.Velocity.Y, difficulty);
            player.moveAi(ball.COM.Y, ball.Velocity.Y, difficulty);
            ball.IncreaseVelocity(dt);
            physics.Update(dt);
        }

        //starts the ball
        public void Start() {
            ball.Impulse();
        }

        public void Reset() {
            ball.Reset();
            ResetObstacles();
        }

        public void Draw(RenderWindow window, float alpha) {
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

        public void setSound() {
            soundOn = !soundOn;
        }
    }
}

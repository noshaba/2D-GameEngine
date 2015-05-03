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
        private Physics physics;
        private List<IShape> objects = new List<IShape>();
        private int WIDTH;
        private int HEIGHT;
        private Font font;
        private Text playerScore;
        private Text aiScore;
        private float difficulty;
        private bool soundOn;
        private String phase;
        
        //constructor
        public Game(int width, int height) {
            WIDTH = width;
            HEIGHT = height;
            physics = new Physics();
            // game elements
            player = new Paddle(new Vector2f(50, height * 0.5f), 12.5f, 50, Color.Cyan);
            ai = new Paddle(new Vector2f(width - 50, height * 0.5f), 12.5f, 50, Color.Green);
            ball = new Ball(new Vector2f(width * 0.5f, 50), 12.5f, Color.Red, 1);
            //determimes difficulty of the AI enemy 0.0=unbeatable, 1.0=easy 
            difficulty = 0.5f;
            AddObject(player);
            AddObject(ai);
            AddObject(ball);
            // walls and obstacles
            AddObject(new Wall(new Vector2f( 0, 1), new Vector2f(width * 0.5f, 12.5f), new Vector2f(width, 25.0f), Color.White));
            AddObject(new Wall(new Vector2f( 0,-1), new Vector2f(width * 0.5f, height - 12.5f), new Vector2f(width, 25.0f), Color.White));
            AddObject(new Wall(new Vector2f( 1, 0), new Vector2f(12.5f, height * 0.5f), new Vector2f(25.0f, height), Color.White));
            AddObject(new Wall(new Vector2f(-1, 0), new Vector2f(width - 12.5f, height * 0.5f), new Vector2f(25.0f, width), Color.White));
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
                AddObject(new Obstacle(new Vector2f(EMath.Random(0, WIDTH), EMath.Random(100, HEIGHT)), EMath.Random(0, 360), Color.White));
            // moveable obstacles
            for (int i = 0; i < EMath.random.Next(3, 6); ++i)
                AddObject(new Obstacle(new Vector2f(EMath.Random(0, WIDTH), EMath.Random(100, HEIGHT)), EMath.Random(0, 360), Color.Yellow, 0.01f));
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
                SoundManager.scoreSound.Play();
                Reset();
            }
            if (ball.COM.X > ai.COM.X) {
                player.score++;
                playerScore.DisplayedString = player.score.ToString();
                if (soundOn)
                SoundManager.scoreSound.Play();
                Reset();
            }

            ai.moveAi(ball.COM.Y, ball.Velocity.Y, difficulty);
           // player.moveAi(ball.COM.Y, ball.Velocity.Y, 0);

            ball.IncreaseVelocity(dt);
            physics.Update(dt);
            if (player.score > 4 || ai.score > 4) {
                phase = "end";
            }
        }

        //starts the ball
        public void Start() {
            ball.Impulse();
        }

        public void Reset() {
            phase = "running";
            ball.Reset();
            ResetObstacles();
        }

        public void Draw(RenderWindow window, float alpha) {
            if (phase == "end") {
                Text t = new Text();
                t.Font = font;
                t.CharacterSize = 70;
                t.Position = new Vector2f(300,200);
                if (player.score > 4) {
                    t.DisplayedString = "You Win!";
                } else {
                    t.DisplayedString = "You Loose";
                }
                window.Draw(t);
            } else {
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
        }

        public void MovePlayer(float y) {
            player.move(y);
        }
        public void ToggleFreeze(){
            physics.frozen = !physics.frozen;
        }


        public void toggleSound() {
            this.soundOn = !this.soundOn;

        }

        public void Restart() {
            this.player.score = 0;
            this.ai.score = 0;
            this.playerScore.DisplayedString = this.player.score.ToString();
            this.aiScore.DisplayedString = this.ai.score.ToString();
            this.Reset();
        }
    }
}

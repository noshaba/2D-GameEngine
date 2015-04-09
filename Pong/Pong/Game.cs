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

        public Game(float width, float height, RenderWindow window) {
            physics = new Physics();
            player = new Paddle(new Vector2f(50, height * 0.5f), new Vector2f(25, 100), Color.Cyan);
            ai = new Paddle(new Vector2f(width - 50, height * 0.5f), new Vector2f(25, 100), Color.Green);
            ball = new Ball(new Vector2f(width * 0.5f, 12.5f), 12.5f, Color.Red, 1);
            AddObject(player);
            AddObject(ai);
            AddObject(ball);
            this.window = window;
        }

        private void AddObject(IShape obj) {
            objects.Add(obj);
            physics.AddObject(obj);
        }

        public void Update(float dt) {
            physics.Update(dt);
        }

        public void Start() {
            ball.Velocity = new Vector2f(-50, 50);
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

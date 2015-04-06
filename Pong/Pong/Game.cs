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

        public Game(float width, float height, RenderWindow window) {
            player = new Paddle(new Vector2f(50, height * 0.5f), new Vector2f(25, 100), Color.Cyan);
            ai = new Paddle(new Vector2f(width - 50, height * 0.5f), new Vector2f(25, 100), Color.Green);
            ball = new Ball(new Vector2f(width * 0.5f, 12.5f), 12.5f, Color.Red, 1);
            this.window = window;
        }

        public void Draw() {
            window.Draw(player);
            window.Draw(ai);
            window.Draw(ball);
        }

        public void MovePlayer(float y) {
            player.move(y);
        }
    }
}

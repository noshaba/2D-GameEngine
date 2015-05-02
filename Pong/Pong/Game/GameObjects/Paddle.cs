using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;


namespace Pong {
    class Paddle : Polygon {

        public uint score;

        public Paddle(Vector2f position, float hw, float hh, Color color) : base() {
            SetBox(position, hw, hh, 0);
            FillColor = color;
        }

        public void move(float y) {
            current.position = new Vector2f(COM.X, y); 
        }

        public void moveAi(float y, float veloY, float diff) {
            float expectedBallY = y + veloY * diff;
            current.position = new Vector2f(COM.X, expectedBallY); 
        }
    }
}

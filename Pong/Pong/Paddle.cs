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

        public Paddle(Vector2f position, float hw, float hh, Color color) : base(color) {
            SetBox(position, hw, hh, 0);
        }

        public void move(float y) {
            current.position = new Vector2f(COM.X, y); 
        }

        public void moveAi(float y, float veloY, float diff, int height) {
            if (veloY != 0) {
                float expectedBall = y + veloY * diff;
                if (expectedBall < 0)
                    expectedBall = 0;
                if (expectedBall > height)
                    expectedBall = height;
                current.position = new Vector2f(COM.X, expectedBall); 
            }
        }
    }
}

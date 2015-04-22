using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Pong {
    class Paddle : OBB {

        public uint score;

        public Paddle(Vector2f position, Vector2f size, Color color) : base(position, size, 0, color) {
            score = 0;
        }

        public void move(float y)
        {
            current.position = new Vector2f(COM.X, y); 
        }

        public void moveAi(float y, float veloY, float diff) {
            if (veloY != 0) {
                float expectedBall = y + veloY * diff;
                current.position = new Vector2f(COM.X, expectedBall); 
            }
        }
    }
}

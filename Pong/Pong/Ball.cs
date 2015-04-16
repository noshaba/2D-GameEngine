using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Pong {
    class Ball : Circle {

        public Collision.Type type = Collision.Type.Circle;

        private Vector2f initPosition;

        public Ball(Vector2f position, float radius, Color color, float mass) : base(position, radius, color, mass) {
            initPosition = position;
        }

        public void Reset() {
            current.Reset();
            current.position = initPosition;
            previous = current;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using Physics;

namespace Shoot_em_Up {
    class Wall : GameObject {

        public Wall(Vector2f normal, Vector2f position, Vector2f size, Color color) : base(normal, position, size, 0) {
            (rigidBody as Plane).FillColor = color;
            rigidBody.Restitution = 1.0f;
        }

        public Wall(Vector2f normal, Vector2f position, Vector2f size, Color color, float orientation) : base(normal, position, size, orientation) {
           (rigidBody as Plane).FillColor = color;
           rigidBody.Restitution = 1.0f;
        }
    }
}

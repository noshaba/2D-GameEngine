using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using Physics;

namespace Platformer {
    class Wall : GameObject {

        public Wall(Vector2f normal, Vector2f position, Vector2f size, Color color)
            : base(normal, position, size, 0) 
        {
            RigidBodyParent = this;
            rigidBody.Restitution = 1.0f;
            drawable[0].FillColor = color;
            drawable[0].OutlineThickness = 0;
        }

        public Wall(Vector2f normal, Vector2f position, Vector2f size, Color color, float orientation) 
            : base(normal, position, size, orientation) 
        {
           rigidBody.Restitution = 1.0f;
           drawable[0].FillColor = color;
           drawable[0].OutlineThickness = 0;
        }
    }
}

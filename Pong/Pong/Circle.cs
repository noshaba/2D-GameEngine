using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Pong {
    abstract class Circle : CircleShape, IShape {
        private Collision.Type type = Collision.Type.Circle;

        protected State current;
        protected State previous;

        public Circle(Vector2f position, float radius) : base(radius) {
            Origin = new Vector2f(Position.X + radius * 0.5f, Position.Y + radius * 0.5f);
            Position = position;
        }

        public Collision.Type Type {
            get { return type; }
        }

        public Vector2f Force {
            get { return current.force; }
            set { current.force = value; }
        }

        public float Torque {
            get { return current.torque; }
            set { current.torque = value; }
        }

        public void Update(float dt) {
            previous = current;
            current.Integrate(dt);
            Position = current.position;
            Rotation = current.orientation;
        }
    }
}

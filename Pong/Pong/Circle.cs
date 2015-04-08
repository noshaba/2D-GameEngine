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

        public State current;
        protected State previous;

        public Circle(Vector2f position, float radius) : base(radius) {
            Origin = new Vector2f(Position.X + radius * 0.5f, Position.Y + radius * 0.5f);
            Position = position;
        }

        public Collision.Type Type {
            get { return type; }
        }

        public void ApplyImpulse(Vector2f J, Vector2f r) {
            current.velocity += J * current.inverseMass;
            current.angularVelocity += r.CrossProduct(J) * current.inverseInertiaTensor;
        }

        public float InverseMass {
            get { return current.inverseMass; }
        }

        public float InverseInertia {
            get { return current.inverseInertiaTensor; }
        }

        public Vector2f Velocity {
            get { return current.velocity; }
            set { current.velocity = value; }
        }

        public float AngularVelocity {
            get { return current.angularVelocity; }
            set { current.angularVelocity = value; }
        }

        public void Update(float dt) {
            previous = current;
            current.Integrate(dt);
            Position = current.position;
            Rotation = (float)(current.orientation * 180.0f / Math.PI);
        }
        public void Pull(Vector2f n, float overlap) {
            current.position += n * overlap;
            Position = current.position;
        }
    }
}

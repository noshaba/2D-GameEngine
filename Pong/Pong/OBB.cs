using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Pong{
    class OBB : RectangleShape, IShape {
        private Collision.Type type = Collision.Type.OBB;

        public Vector2f[] axis = { new Vector2f(1,0), new Vector2f(0,1) };
        public float[] hl = new float[2]; // half lengths of axis'

        protected State previous;
        protected State current;

        public OBB(Vector2f position, Vector2f size, float rotation, Color color) : base(size) {
            Origin = new Vector2f(size.X * 0.5f, size.Y * 0.5f);
            hl[0] = size.X * 0.5f;
            hl[1] = size.Y * 0.5f;
            current = new State(position, rotation);
            previous = current;
            FillColor = color;
        }

        public OBB(Vector2f position, Vector2f size, float rotation, Color color, float mass) : base(size) {
            Origin = new Vector2f(size.X * 0.5f, size.Y * 0.5f);
            hl[0] = size.X * 0.5f;
            hl[1] = size.Y * 0.5f;
            current = new State(position, rotation, mass, size.X * size.Y / 6.0f);
            previous = current;
            FillColor = color;
        }

        public Collision.Type Type {
            get { return type; }
        }

        public Vector2f COM {
            get { return current.position; }
        }

        public State Current {
            get { return current; }
            set { current = value; }
        }

        public State Previous {
            get { return previous; }
        }

        public State Interpolation(float alpha) {
            return previous * alpha + current * (1.0f - alpha);
        }

        public float Width {
            get { return Size.X; }
            set { Size = new Vector2f(value, Size.Y); }
        }

        public float Height {
            get { return Size.Y; }
            set { Size = new Vector2f(Size.X, value); }
        }

        public float Mass {
            get { return current.mass; }
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

        public Vector2f Axis(uint i){
            if(current.orientation != 0)
                return axis[i].Rotate(current.orientation);
            return axis[i];
        }

        public Vector2f Axis(int i){
            if (current.orientation != 0)
                return axis[i].Rotate(current.orientation);
            return axis[i];
        }

        public void Update(float dt) {
            previous = current;
            current.Integrate(dt);
        }

        public void ApplyImpulse(Vector2f J, Vector2f r) {
            current.velocity += J * current.inverseMass;
            current.angularVelocity += r.CrossProduct(J) * current.inverseInertiaTensor;
        }

        public void Pull(Vector2f n, float overlap) {
            current.position += n * overlap;
        }
    }
}

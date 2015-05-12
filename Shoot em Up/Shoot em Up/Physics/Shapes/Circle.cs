using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using Maths;
using Shoot_em_Up;

namespace Physics {
    class Circle : CircleShape, IState {
        private Collision.Type type = Collision.Type.Circle;
        private Collision collision;
        private float restitution = (float) EMath.random.NextDouble();
        private float staticFriction = (float) EMath.random.NextDouble();
        private float kineticFriction;

        protected State current;
        protected State previous;

        private Object parent;

        public Circle(Vector2f position, float radius) : base(radius) {
            Origin = new Vector2f(radius, radius);
            current = new State(position, 0);
            previous = current;
            kineticFriction = EMath.Random(0, staticFriction);
            collision = new Collision();
            collision.collision = false;

        }
        
        public Circle(Vector2f position, float radius, float density) : base(radius) {
            Origin = new Vector2f(radius, radius);
            float mass = (float) Math.PI * radius * radius * density;
            current = new State(position, 0, mass, mass * radius * radius);
            previous = current;
            kineticFriction = EMath.Random(0, staticFriction);
            collision = new Collision();
            collision.collision = false;

            // Console.WriteLine("Ball " + mass);
        }


        public Collision.Type Type {
            get { return type; }
        }

        public Object Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public Collision Collision
        {
            get { return collision; }
            set { collision = value; }
        }

        public Vector2f COM {
            get { return current.position; }
        }

        public float Orientation {
            get { return current.orientation; }
        }

        public Mat22f WorldTransform {
            get { return current.worldTransform; }
        }

        public Mat22f LocalTransform {
            get { return current.localTransform; }
        }

        public State Current {
            get { return current; }
        }

        public State Previous {
            get { return previous; }
            set { previous = value; }
        }

        public State Interpolation(float alpha) {
            return current + alpha * (current - previous);
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

        public float Restitution {
            get { return restitution; }
            set { restitution = value; }
        }

        public float StaticFriction {
            get { return staticFriction; }
            set { staticFriction = value; }
        }

        public float KineticFriction {
            get { return kineticFriction; }
            set { kineticFriction = value; }
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

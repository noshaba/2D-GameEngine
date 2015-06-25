using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;
using Maths;

namespace Physics {
    class Plane : RectangleShape, IRigidBody {
        private Collision.Type type = Collision.Type.Plane;
        private float restitution = (float) EMath.random.NextDouble();
        private float staticFriction = (float) EMath.random.NextDouble();
        private float kineticFriction;
        private float dragCoefficient = 0;
        private Vector2f center;
        private Collision collision;

        protected State previous;
        protected State current;

        private Object parent;

        public Vector2f normal;
        public float constant;
        public float thickness;


        public Plane(Vector2f normal, Vector2f position, Vector2f size, float orientation) : base(size) {
            FillColor = Color.Transparent;
            OutlineThickness = 2;
            OutlineColor = Color.White;
            Origin = new Vector2f(size.X * .5f, size.Y * .5f);
            current = new State(position, orientation);
            previous = current;
            thickness = Math.Abs(normal.Dot(size) * .5f);
            this.normal = current.worldTransform * normal;
            kineticFriction = EMath.Random(0, staticFriction);
            collision = new Collision();
            collision.collision = false;
            Radius = (float)Math.Sqrt(size.X * size.X * .25f + size.Y * size.Y * .25f);
            BoundingCircle = new CircleShape(Radius);
            BoundingCircle.Origin = new Vector2f(Radius, Radius);
            BoundingCircle.FillColor = Color.Transparent;
            BoundingCircle.OutlineThickness = 1;
            BoundingCircle.OutlineColor = Color.White;
            COMDrawable = new RectangleShape(new Vector2f(5,5));
            COMDrawable.FillColor = Color.White;
            COMDrawable.Origin = new Vector2f(2.5f, 2.5f);
            this.center = new Vector2f(Size.X * .5f, Size.Y * .5f);
            constant = Center.Dot(this.normal);
        }


        public Collision Collision
        {
            get { return collision; }
            set { collision = value; }
        }

        public Object Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public Collision.Type Type {
            get { return type; }
        }

        public float Radius { get; set; }

        public CircleShape BoundingCircle { get; set; }

        public RectangleShape COMDrawable { get; set; }

        public Vector2f COM {
            get { return current.position; }
            set { current.position = value; previous.position = value; }
        }

        public Vector2f Centroid
        {
            get { return this.Origin; }
            set { this.Origin = new Vector2f(Size.X * .5f, Size.Y * .5f) + value; }
        }

        public Vector2f Center
        {
            get { return current.worldTransform * (center - Centroid) + current.position; }
        }

        public float Orientation {
            get { return current.Orientation; }
            set
            {
                current.Orientation = value;
                previous.Orientation = value;
                normal = current.worldTransform * normal;
                constant = Center.Dot(normal);
            }
        }

        public Mat22f WorldTransform {
            get { return current.worldTransform; }
        }

        public Mat22f LocalTransform {
            get { return current.localTransform; }
        }

        public State Current {
            get { return current; }
            set { current = value; }
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

        public float Inertia
        {
            get { return current.inertiaTensor; }
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

        public float DragCoefficient
        {
            get { return dragCoefficient; }
            set { dragCoefficient = value; }
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

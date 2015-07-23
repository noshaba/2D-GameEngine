using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using Maths;

namespace Physics {
    class Circle : CircleShape, IRigidBody {
        private Collision.Type type = Collision.Type.Circle;
        private Vector2f center;

        protected State current;
        protected State previous;
        
        public Circle(Vector2f position, float rotation, float radius, float density) : base(radius) {
            FillColor = Color.Transparent;
            OutlineThickness = 2;
            OutlineColor = Color.White;
            Origin = new Vector2f(radius, radius);
            float mass = (float) Math.PI * radius * radius * density;
            current = new State(position, rotation, mass, mass * radius * radius);
            previous = current;
            InitBoundingCircle(radius);
            InitCOMDrawable();
            this.center = new Vector2f(radius, radius);
        }

        private void InitBoundingCircle(float radius)
        {
            this.BoundingCircle = new CircleShape(radius);
            this.BoundingCircle.Origin = new Vector2f(radius, radius);
            this.BoundingCircle.FillColor = Color.Transparent;
            this.BoundingCircle.OutlineThickness = 1;
            this.BoundingCircle.OutlineColor = Color.White;
        }

        private void InitCOMDrawable()
        {
            COMDrawable = new RectangleShape(new Vector2f(5, 5));
            COMDrawable.FillColor = Color.White;
            COMDrawable.Origin = new Vector2f(2.5f, 2.5f);
        }

        public Collision.Type Type {
            get { return type; }
        }

        new public float Radius
        {
            get { return base.Radius; }
            set { base.Radius = value; }
        }

        public CircleShape BoundingCircle { get; set; }

        public RectangleShape COMDrawable { get; set; }

        public Vector2f COM {
            get { return current.position; }
            set { current.position = value; previous.position = value; }
        }

        public Vector2f Centroid
        {
            get { return this.Origin; }
            set { this.Origin = new Vector2f(Radius, Radius) + value; }
        }

        public Vector2f Center
        {
            get { return current.worldTransform * (center - Centroid) + current.position;}
        }

        public float Orientation {
            get { return current.Orientation; }
            set 
            { 
                current.Orientation = value;
                previous.Orientation = value;
            }
        }

        public float DegOrientation
        {
            get
            {
                return (float)(Orientation * 180.0 / Math.PI);
            }
            set
            {
                Orientation = (float)(value * Math.PI / 180.0);
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

        public float Inertia
        {
            get { return current.inertiaTensor; }
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

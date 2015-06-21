using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;
using Maths;

namespace Physics
{
    class Body
    {
        private object parent;
        private Collision collision;
        private State current;
        private State previous;
        private float restitution;
        private float staticFriction;
        private float dragCoefficient;
        private float kineticFriction;
        private Vector2f centroid;

        public IRigidBody[] bodies;

        public Body(IRigidBody[] bodies, Vector2f position, float rotation)
        {
            this.bodies = bodies;
            float mass = 0;
            float inertiaTensor = 0;
            foreach (IRigidBody body in bodies)
                mass += body.Mass;
            foreach (IRigidBody body in bodies)
                inertiaTensor += body.Inertia;
            current = new State(position, rotation, mass, inertiaTensor);
            previous = current;
            Restitution = (float)EMath.random.NextDouble();
            StaticFriction = (float)EMath.random.NextDouble();
            KineticFriction = EMath.Random(0, staticFriction);
            DragCoefficient = 0;
            collision = new Collision();
            Orientation = (float)(rotation * Math.PI / 180.0);
            SetCentroid();
            InitBoundingCircle();
            COMDrawable = new RectangleShape(new Vector2f(10, 10));
            COMDrawable.Origin = new Vector2f(5,5);
            COMDrawable.FillColor = Color.Red;
            COM = position;
        }

        private void SetCentroid()
        {
            Vector2f c = new Vector2f();
            foreach (IRigidBody body in bodies)
                c += body.COM;
            c /= bodies.Length;
            Centroid = c;
            foreach (IRigidBody body in bodies)
                body.Centroid = c - body.COM;
        }

        private void InitBoundingCircle()
        {
            float rad;
            Radius = 0;
            foreach (IRigidBody body in bodies)
            {
                rad = (Centroid - body.COM).Length() + body.Radius;
                if (rad > Radius) Radius = rad;
            }
            this.BoundingCircle = new CircleShape(Radius);
            this.BoundingCircle.Origin = new Vector2f(Radius, Radius);
            this.BoundingCircle.FillColor = Color.Transparent;
            this.BoundingCircle.OutlineThickness = 1;
            this.BoundingCircle.OutlineColor = Color.White;
        }

        public float Radius
        {
            get;
            set;
        }

        public CircleShape BoundingCircle
        {
            get;
            set;
        }

        public RectangleShape COMDrawable
        {
            get;
            set;
        }

        public State Current
        {
            get { return this.current; }
        }

        public State Previous
        {
            get { return this.previous; }
        }

        public object Parent
        {
            set 
            { 
                this.parent = value;
                foreach (IRigidBody body in bodies)
                    body.Parent = value;
            }
            get { return this.parent; }
        }

        public Vector2f COM
        {
            set 
            { 
                this.current.position = value;
                this.previous.position = value;
                foreach (IRigidBody body in bodies)
                    body.COM = value;
            }
            get { return this.current.position; }
        }

        public float Orientation
        {
            set
            {
                this.current.Orientation = value;
                this.previous.Orientation = value;
                foreach (IRigidBody body in bodies)
                    body.Orientation += value;
            }
            get { return this.current.Orientation; }
        }

        public Mat22f WorldTransform
        {
            get { return current.worldTransform; }
        }

        public Mat22f LocalTransform
        {
            get { return current.localTransform; }
        }

        public float Mass
        {
            get { return current.mass; }
        }

        public float InverseMass
        {
            get { return current.inverseMass; }
        }

        public float Inertia
        {
            get { return current.inertiaTensor; }
        }

        public float InverseInertia
        {
            get { return current.inverseInertiaTensor; }
        }

        public Vector2f Velocity
        {
            set 
            { 
                this.current.velocity = value;
                foreach (IRigidBody body in bodies)
                    body.Velocity = value;
            }
            get { return this.current.velocity; }
        }

        public float AngularVelocity
        {
            set 
            { 
                this.current.angularVelocity = value;
                foreach (IRigidBody body in bodies)
                    body.AngularVelocity = value;
            }
            get { return this.current.angularVelocity; }
        }

        

        public Collision Collision
        {
            set 
            { 
                this.collision = value;
                foreach (IRigidBody body in bodies)
                    body.Collision = value;
            }
            get { return this.collision; }
        }

        public float Restitution
        {
            get { return this.restitution; }
            set
            {
                this.restitution = value;
                foreach (IRigidBody body in bodies)
                    body.Restitution = value;
            }
        }

        public float StaticFriction
        {
            get { return this.staticFriction; }
            set
            {
                this.staticFriction = value;
                foreach (IRigidBody body in bodies)
                    body.StaticFriction = value;
            }
        }

        public float KineticFriction
        {
            get { return this.kineticFriction; }
            set
            {
                this.kineticFriction = value;
                foreach (IRigidBody body in bodies)
                    body.KineticFriction = value;
            }
        }

        public float DragCoefficient
        {
            get { return this.dragCoefficient; }
            set
            {
                this.dragCoefficient = value;
                foreach (IRigidBody body in bodies)
                    body.DragCoefficient = value;
            }
        }

        public Vector2f Centroid
        {
            set { this.centroid = value; }
            get { return this.centroid; }
        }


        public void Update(float dt)
        {
            previous = current;
            current.Integrate(dt);
            foreach (IRigidBody body in bodies)
                body.Update(dt);
        }

        public void ApplyImpulse(Vector2f J, Vector2f r)
        {
            Velocity += J * current.inverseMass;
            AngularVelocity += r.CrossProduct(J) * current.inverseInertiaTensor;
        }

        public void Pull(Vector2f n, float overlap)
        {
            COM += n * overlap;
        }

        public State Interpolation(float alpha)
        {
            return current + alpha * (current - previous);
        }

        public void Draw(RenderWindow window, float alpha) 
        {
            State interpol = Interpolation(alpha);
            Transform t = Transform.Identity;
            t.Translate(interpol.position);
            t.Rotate(interpol.DegOrientation);
            RenderStates r = new RenderStates(t);
            window.Draw(BoundingCircle, r);
            COMDrawable.Position = COM;
            window.Draw(COMDrawable);
            foreach (IRigidBody body in bodies)
            {
            /*    window.Draw(body as Shape, r);
                body.COMDrawable.Position = body.COM;
                window.Draw(body.COMDrawable);*/
                interpol = body.Interpolation(alpha);
                t = Transform.Identity;
                t.Translate(interpol.position);
                t.Rotate(interpol.DegOrientation);
                r = new RenderStates(t);
                window.Draw(body as Shape, r);
                body.COMDrawable.Position = body.Center;
                window.Draw(body.COMDrawable);
            }
        }

    }
}

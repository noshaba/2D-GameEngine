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
        private List<Collision> collision = new List<Collision>();
        private State current;
        private State previous;
        private float restitution;
        private float staticFriction;
        private float dragCoefficient;
        private float kineticFriction;
        private Vector2f centroid;
        public bool moveable;
        public bool rotateable;
        public bool earlyOut = false;

        public IRigidBody[] bodies;
        public bool isJoint = false;

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
            moveable = InverseMass > 0 ? true : false;
            rotateable = moveable;
            Restitution = (float)EMath.random.NextDouble();
            StaticFriction = (float)EMath.random.NextDouble();
            KineticFriction = EMath.Random(0, staticFriction);
            DragCoefficient = 0;
            Orientation = (float)(rotation * Math.PI / 180.0);
            UpdateCentroid();
            UpdateBoundingCircle();
            COMDrawable = new RectangleShape(new Vector2f(10, 10));
            COMDrawable.Origin = new Vector2f(5,5);
            COMDrawable.FillColor = Color.Red;
            COM = position;
        }

        public Body(object parent, IRigidBody[] bodies, Vector2f position, float rotation)
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
            moveable = InverseMass > 0 ? true : false;
            rotateable = moveable;
            Restitution = (float)EMath.random.NextDouble();
            StaticFriction = (float)EMath.random.NextDouble();
            KineticFriction = EMath.Random(0, staticFriction);
            DragCoefficient = 0;
            Orientation = (float)(rotation * Math.PI / 180.0);
            UpdateCentroid();
            UpdateBoundingCircle();
            COMDrawable = new RectangleShape(new Vector2f(10, 10));
            COMDrawable.Origin = new Vector2f(5, 5);
            COMDrawable.FillColor = Color.Red;
            COM = position;
            this.Parent = parent;
        }

        public bool InsideWindow(Vector2f viewCenter, Vector2f halfSize)
        {
            return COM.X + Radius >= viewCenter.X - halfSize.X &&
                   COM.X - Radius <= viewCenter.X + halfSize.X;
        }

        public void UpdateCentroid()
        {
            Vector2f c = new Vector2f();
            foreach (IRigidBody body in bodies)
                c += body.COM;
            c /= bodies.Length;
            Centroid = c;
            foreach (IRigidBody body in bodies)
                body.Centroid = c - body.COM;
        }

        public void UpdateBoundingCircle()
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
            set { this.current = value; }
            get { return this.current; }
        }

        public State Previous
        {
            set { this.previous = value; }
            get { return this.previous; }
        }

        public object Parent
        {
            set { this.parent = value; }
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

        

        public List<Collision> Collision
        {
            set {  this.collision = value; }
            get { return this.collision; }
        }

        public float Restitution
        {
            get { return this.restitution; }
            set { this.restitution = value; }
        }

        public float StaticFriction
        {
            get { return this.staticFriction; }
            set { this.staticFriction = value; }
        }

        public float KineticFriction
        {
            get { return this.kineticFriction; }
            set { this.kineticFriction = value; }
        }

        public float DragCoefficient
        {
            get { return this.dragCoefficient; }
            set {  this.dragCoefficient = value; }
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
            if(moveable) Velocity += J * current.inverseMass;
            if(rotateable) AngularVelocity += r.CrossProduct(J) * current.inverseInertiaTensor;
        }

        public void ApplyLinearImpulse(Vector2f J)
        {
            if (moveable) Velocity += J * current.inverseMass;
        }

        public void Pull(Vector2f n, float overlap)
        {
            COM += n * overlap;
        }

        public State Interpolation(float alpha)
        {
            return current + alpha * (current - previous);
        }

        public void Draw(RenderTexture buffer, float alpha, Vector2f viewCenter, Vector2f windowHalfSize)
        {
            if (!InsideWindow(viewCenter, windowHalfSize))
                return;
            State interpol = Interpolation(alpha);
            Transform t = Transform.Identity;
            t.Translate(interpol.position);
            t.Rotate(interpol.DegOrientation);
            RenderStates r = new RenderStates(t);
            buffer.Draw(BoundingCircle, r);
            COMDrawable.Position = COM;
            buffer.Draw(COMDrawable);
            foreach (IRigidBody body in bodies)
            {
                interpol = body.Interpolation(alpha);
                t = Transform.Identity;
                t.Translate(interpol.position);
                t.Rotate(interpol.DegOrientation);
                r = new RenderStates(t);
                buffer.Draw(body as Shape, r);
                body.COMDrawable.Position = body.Center;
                buffer.Draw(body.COMDrawable);
                body.BoundingCircle.Position = body.Center;
                buffer.Draw(body.BoundingCircle);
            }
        }

        public void Draw(RenderWindow window, float alpha, Vector2f viewCenter, Vector2f windowHalfSize) 
        {
            if (!InsideWindow(viewCenter, windowHalfSize))
                return;
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
                interpol = body.Interpolation(alpha);
                t = Transform.Identity;
                t.Translate(interpol.position);
                t.Rotate(interpol.DegOrientation);
                r = new RenderStates(t);
                window.Draw(body as Shape, r);
                body.COMDrawable.Position = body.Center;
                window.Draw(body.COMDrawable);
                body.BoundingCircle.Position = body.Center;
                window.Draw(body.BoundingCircle);
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using Maths;

namespace Physics {
    class Polygon : ConvexShape, IRigidBody {
        private int MAXPOLYVERTEXCOUNT = 8;
        private Collision.Type type = Collision.Type.Polygon;
        private float restitution = (float) EMath.random.NextDouble(); 
        private float staticFriction = (float) EMath.random.NextDouble();
        private float dragCoefficient = 0;
        private float kineticFriction;
        private Collision collision;

        protected State previous;
        protected State current;

        private Object parent;

        public Vector2f[] vertices;
        public Vector2f[] normals;
        private Vector2f centroid;

        public Polygon(Vector2f[] vertices, Vector2f position, float rotation, float density) : base() {
            FillColor = Color.Transparent;
            OutlineThickness = 2;
            OutlineColor = Color.White;
            GenerateConvexHull(vertices);
            SetCentroid();
            SetNormals();
            SetRadius();
            InitState(position, rotation, density);
            kineticFriction = EMath.Random(0, staticFriction);
            collision = new Collision();
            InitBoundingCircle();
            InitCOMDrawable();
        }

        public Polygon(Vector2f[] vertices, Vector2f centroid, Vector2f position, float rotation, float density)
        {
            FillColor = Color.Transparent;
            OutlineThickness = 2;
            OutlineColor = Color.White;
            GenerateConvexHull(vertices);
            SetCentroid(centroid);
            SetNormals();
            SetRadius();
            InitState(position, rotation, density);
            kineticFriction = EMath.Random(0, staticFriction);
            collision = new Collision();
            InitBoundingCircle();
            InitCOMDrawable();
        }

        public Polygon() 
        {
            collision = new Collision();
            kineticFriction = EMath.Random(0, staticFriction);
            FillColor = Color.Transparent;
            OutlineThickness = 2;
            OutlineColor = Color.White;
        }

        public void SetBox(Vector2f position, float hw, float hh, float rotation, float density) {
            SetPointCount(4);
            centroid = new Vector2f(hw,hh);
            vertices = new Vector2f[4];
            vertices[0] = new Vector2f(-hw, -hh);
            vertices[1] = new Vector2f(hw, -hh);
            vertices[2] = new Vector2f(hw, hh);
            vertices[3] = new Vector2f(-hw, hh);
            SetPoint(0, vertices[0]);
            SetPoint(1, vertices[1]);
            SetPoint(2, vertices[2]);
            SetPoint(3, vertices[3]);
            normals = new Vector2f[4];
            normals[0] = new Vector2f(0, -1);
            normals[1] = new Vector2f(1, 0);
            normals[2] = new Vector2f(0, 1);
            normals[3] = new Vector2f(-1, 0);
            this.Radius = (float)Math.Sqrt(hw * hw + hh * hh);
            InitState(position, rotation, density);
            InitBoundingCircle();
            InitCOMDrawable();
        }

        private void InitBoundingCircle()
        {
            this.BoundingCircle = new CircleShape(Radius);
            this.BoundingCircle.Origin = new Vector2f(Radius, Radius);
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

        private void InitState(Vector2f position, float rotation, float density) { 
            // Calculate moment of interia
            float area = 0;
            float I = 0;
            const float kInv3 = 1.0f / 3.0f;
            for (int i1 = 0; i1 < vertices.Length; ++i1) {
                // Triangle vertices, third vertex implied as (0, 0)
                Vector2f p1 = vertices[i1];
                int i2 = (i1 + 1) % vertices.Length;
                Vector2f p2 = vertices[i2];

                float D = p1.CrossProduct(p2);
                float triangleArea = .5f * D;

                area += triangleArea;

                float intx2 = p1.X * p1.X + p2.X * p1.X + p2.X * p2.X;
                float inty2 = p1.Y * p1.Y + p2.Y * p1.Y + p2.Y * p2.Y;
                I += (0.25f * kInv3 * D) * (intx2 + inty2);
            }

            for (int i1 = 0; i1 < vertices.Length; ++i1) {
                int i2 = (i1 + 1) % vertices.Length;
                Vector2f face = (vertices[i2] - vertices[i1]).Norm();
            }

            current = new State(position, rotation, density * area, density * I);
            previous = current;
           // Console.WriteLine(Mass);
        }

        private void SetCentroid()
        {
            this.centroid = new Vector2f();

            for (uint i = 0; i < GetPointCount(); ++i)
                centroid += vertices[i];

            centroid /= (float)vertices.Length;

            // Translate vertices to centroid (make the centroid (0, 0) for the polygon in model space)
            for (uint i = 0; i < GetPointCount(); ++i)
            {
                vertices[i] -= centroid;
                SetPoint(i, vertices[i]);
            }
        }

        private void SetCentroid(Vector2f centroid)
        {
            this.centroid = centroid;
            // Translate vertices to centroid (make the centroid (0, 0) for the polygon in model space)
            for (uint i = 0; i < GetPointCount(); ++i)
            {
                vertices[i] -= centroid;
                SetPoint(i, vertices[i]);
            }
        }

        private void GenerateConvexHull(Vector2f[] buffer) {

            // find right most vertex in hull
            uint rightMost = 0;
            float highestXCoord = buffer[0].X;
            for (uint i = 1; i < buffer.Length; ++i)
            {
                float x = buffer[i].X;
                if (x > highestXCoord)
                {
                    highestXCoord = x;
                    rightMost = i;
                }
                else if (x == highestXCoord)
                {
                    // If matching x then take farthest negative y
                    if (buffer[i].Y < buffer[rightMost].Y)
                        rightMost = i;
                }
            }

            uint[] hull = new uint[buffer.Length];
            uint outCount = 0;
            uint indexHull = rightMost;

            while (true)
            {
                hull[outCount] = indexHull;
                // Search for next index that wraps around the hull
                // by computing cross products to find the most counter-clockwise
                // vertex in the set, given the previous hull index
                uint nextHullIndex = 0;
                for (uint i = 0; i < buffer.Length; ++i)
                {
                    // Skip if same coordinate as we need three unique
                    // points in the set to perform a cross product
                    if (nextHullIndex == indexHull)
                    {
                        nextHullIndex = i;
                        continue;
                    }
                    // Cross every set of three unique vertices
                    // Record each counter clockwise third vertex and add
                    // to the output hull
                    // See : http://www.oocities.org/pcgpe/math2d.html
                    Vector2f e1 = buffer[nextHullIndex] - buffer[hull[outCount]];
                    Vector2f e2 = buffer[i] - buffer[hull[outCount]];
                    float c = e1.CrossProduct(e2);
                    if (c < 0)
                        nextHullIndex = i;
                    // Cross product is zero then e vectors are on same line
                    // therefore want to record vertex farthest along that line
                    if (c == 0 && e2.Length2() > e1.Length2())
                        nextHullIndex = i;
                }

                ++outCount;
                indexHull = nextHullIndex;

                // Conclude algorithm upon wrap-around
                if (nextHullIndex == rightMost)
                {
                    SetPointCount(outCount);
                    vertices = new Vector2f[outCount];
                    normals = new Vector2f[outCount];
                    break;
                }
            }

            for (uint i = 0; i < GetPointCount(); ++i)
                vertices[i] = buffer[hull[i]];
        }

        private void SetNormals()
        {
            // Compute face normals
            for (int i1 = 0; i1 < vertices.Length; ++i1)
            {
                int i2 = (i1 + 1) % vertices.Length;
                Vector2f face = vertices[i2] - vertices[i1];
                normals[i1] = new Vector2f(face.Y, -face.X).Norm();
            }
        }

        private void SetRadius()
        {
            // calculate radius
            float rad2 = vertices[0].Length2();
            float len2;
            for (uint i = 1; i < vertices.Length; ++i)
            {
                len2 = vertices[i].Length2();
                if (rad2 < len2) rad2 = len2;
            }
            this.Radius = (float)Math.Sqrt(rad2);
        }

        private void InitVertices() {
            uint count = (uint) EMath.Random(3, MAXPOLYVERTEXCOUNT);
            float e = EMath.Random(50, 100);
            Vector2f[] buffer = new Vector2f[count];
            for (uint i = 0; i < buffer.Length; ++i) {
                buffer[i] = new Vector2f(EMath.Random(-e, e), EMath.Random(-e, e));
            }

            GenerateConvexHull(buffer);
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
            get { return this.centroid; }
            set
            {
                this.centroid = value;
                for (uint i = 0; i < GetPointCount(); ++i)
                {
                    // Translate to world coordinates
                    vertices[i] = Vertex(i);
                    // Translate vertices to centroid (make the centroid (0, 0) for the polygon in model space)
                    vertices[i] -= centroid;
                    SetPoint(i, vertices[i]);
                }
            }
        }

        public float Orientation {
            get { return current.Orientation; }
            set
            {
                current.Orientation = value;
                previous.Orientation = value;
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

        public Vector2f Normal(uint i) {
            return current.worldTransform * normals[i];
        }

        public Vector2f Normal(int i) {
            return current.worldTransform * normals[i];
        }

        public Vector2f Vertex(uint i) {
            return current.worldTransform * vertices[i] + current.position;
        }
        public Vector2f Vertex(int i) {
            return current.worldTransform * vertices[i] + current.position;
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

        public List<Vector2f> GetSupport(Vector2f n) {
            List<Vector2f> support = new List<Vector2f>();
            float bestProjection = float.MaxValue;
            float projection;
            Vector2f vertex;
            for (uint i = 0; i < vertices.Length; ++i) {
                vertex = Vertex(i);
                projection = vertex.Dot(n);
                if (projection < bestProjection) {
                    support.Clear();
                    bestProjection = projection;
                    support.Add(vertex);
                } else if (projection == bestProjection) {
                    support.Add(vertex);
                }
            }
            return support;
        }

        public void GetIntersectedPoints(List<Vector2f> support, ref List<Vector2f> intersects) {
            bool inside = true;
            foreach (Vector2f vertex in support) {
                for (uint i = 0; i < normals.Length; ++i) {
                    if ((vertex - Vertex(i)).Dot(Normal(i)) > 0.1f) {
                        inside = false;
                        break;
                    }
                }
                if (inside)
                    intersects.Add(vertex);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using Maths;

namespace Physics {
    class Polygon : ConvexShape, IShape {
        private int MAXVERTEXCOUNT = 8;
        private Collision.Type type = Collision.Type.Polygon;
        private float restitution = (float) EMath.random.NextDouble(); 
        private float staticFriction = (float) EMath.random.NextDouble();
        private float kineticFriction;

        protected State previous;
        protected State current;



        public Vector2f[] vertices;
        public Vector2f[] normals;


        public Polygon() {}

        public Polygon(Vector2f position, float rotation) : base(){
            InitVertices();
            current = new State(position, rotation);
            previous = current;
            kineticFriction = EMath.Random(0, staticFriction);
        }

        public Polygon(Vector2f position, float rotation, float density) : base() {
            InitVertices();
            InitState(position, rotation, density);
            kineticFriction = EMath.Random(0, staticFriction);
        }

        public Polygon(Vector2f[] vertices, Vector2f position, float rotation) : base() {
            this.vertices = vertices;
            for (uint i = 0; i < vertices.Length; ++i)
                SetPoint(i, vertices[i]);
            current = new State(position, rotation);
            previous = current;
            kineticFriction = EMath.Random(0, staticFriction);
        }

        public Polygon(Vector2f[] vertices, Vector2f position, float rotation, float density) : base() {
            this.vertices = vertices;
            for (uint i = 0; i < vertices.Length; ++i)
                SetPoint(i, vertices[i]);
            InitState(position, rotation, density);
            kineticFriction = EMath.Random(0, staticFriction);
        }

        public void SetBox(Vector2f position, float hw, float hh, float rotation) {
            SetPointCount(4);
            vertices = new Vector2f[4];
            vertices[0] = new Vector2f(-hw, -hh);
            vertices[1] = new Vector2f( hw, -hh);
            vertices[2] = new Vector2f( hw,  hh);
            vertices[3] = new Vector2f(-hw,  hh);
            for (uint i = 0; i < vertices.Length; ++i)
                SetPoint(i, vertices[i]);
            normals = new Vector2f[4];
            normals[0] = new Vector2f( 0, -1);
            normals[1] = new Vector2f( 1,  0);
            normals[2] = new Vector2f( 0,  1);
            normals[3] = new Vector2f(-1,  0);
            current = new State(position, rotation);
            previous = current;
        }

        public void SetBox(Vector2f position, float hw, float hh, float rotation, float density) {
            SetPointCount(4);
            vertices = new Vector2f[4];
            vertices[0] = new Vector2f(-hw, -hh);
            vertices[1] = new Vector2f(hw, -hh);
            vertices[2] = new Vector2f(hw, hh);
            vertices[3] = new Vector2f(-hw, hh);
            normals = new Vector2f[4];
            normals[0] = new Vector2f(0, -1);
            normals[1] = new Vector2f(1, 0);
            normals[2] = new Vector2f(0, 1);
            normals[3] = new Vector2f(-1, 0);
            InitState(position, rotation, density);
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

        private void InitVertices() {
            uint count = (uint) EMath.Random(3, MAXVERTEXCOUNT);
            float e = EMath.Random(50, 100);
            Vector2f[] buffer = new Vector2f[count];
            for (uint i = 0; i < buffer.Length; ++i) {
                buffer[i] = new Vector2f(EMath.Random(-e, e), EMath.Random(-e, e));
            }

            // find right most vertex in hull
            uint rightMost = 0;
            float highestXCoord = buffer[0].X;
            for (uint i = 1; i < buffer.Length; ++i) {
                float x = buffer[i].X;
                if (x > highestXCoord) {
                    highestXCoord = x;
                    rightMost = i;
                } else if(x == highestXCoord){
                    // If matching x then take farthest negative y
                    if (buffer[i].Y < buffer[rightMost].Y)
                        rightMost = i;
                }
            }

            uint[] hull = new uint[MAXVERTEXCOUNT];
            uint outCount = 0;
            uint indexHull = rightMost;

            while (true) {
                hull[outCount] = indexHull;
                // Search for next index that wraps around the hull
                // by computing cross products to find the most counter-clockwise
                // vertex in the set, given the previous hull index
                uint nextHullIndex = 0;
                for (uint i = 0; i < count; ++i) {
                    // Skip if same coordinate as we need three unique
                    // points in the set to perform a cross product
                    if (nextHullIndex == indexHull) {
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
                if (nextHullIndex == rightMost) {
                    SetPointCount(outCount);
                    vertices = new Vector2f[outCount];
                    normals = new Vector2f[outCount];
                    break;
                }
            }

            Vector2f centroid = new Vector2f();

            for (uint i = 0; i < GetPointCount(); ++i) {
                vertices[i] = buffer[hull[i]];
                centroid += vertices[i];
            }

            centroid /= (float) vertices.Length;

            // Translate vertices to centroid (make the centroid (0, 0) for the polygon in model space)
            for (uint i = 0; i < GetPointCount(); ++i) {
                vertices[i] -= centroid;
                SetPoint(i, vertices[i]);
            }

            // Compute face normals
            for (int i1 = 0; i1 < vertices.Length; ++i1) {
                int i2 = (i1 + 1) % vertices.Length;
                Vector2f face = vertices[i2] - vertices[i1];
                normals[i1] = new Vector2f(face.Y, -face.X).Norm();
            }
        }

        public virtual void ReactToCollision(Collision colliInfo) { } 

        public Collision.Type Type {
            get { return type; }
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

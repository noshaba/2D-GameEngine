using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Pong {
    class Polygon : ConvexShape, IShape{
        private int MAXVERTEXCOUNT = 8;
        private Collision.Type type = Collision.Type.Polygon;
        private Random random = new Random();

        public Vector2f[] vertices;
        public Vector2f[] normals;
        public float[] hl;

        protected State previous;
        protected State current;

        public Polygon(Vector2f position, float rotation, Color color) : base(){
            InitVertices();
            current = new State(position, rotation);
            previous = current;
            FillColor = color;
        }

        public Polygon(Vector2f position, float rotation, Color color, float density) : base() {
            InitVertices();
            InitState(position, rotation, density);
            FillColor = color;
        }

        private void InitState(Vector2f position, float rotation, float density) { 
            // Calculate centroid and moment of interia
            Vector2f c = new Vector2f(); // centroid
            float area = 0;
            float I = 0;
            const float kInv3 = 1.0f / 3.0f;
            for (uint i1 = 0; i1 < vertices.Length; ++i1) {
                // Triangle vertices, third vertex implied as (0, 0)
                Vector2f p1 = vertices[i1];
                uint i2 = (i1 + 1) % (uint) vertices.Length;
                Vector2f p2 = vertices[i2];

                float D = p1.CrossProduct(p2);
                float triangleArea = .5f * D;

                area += triangleArea;
                // Use area to weight the centroid average, not just vertex position
                c += triangleArea * kInv3 * (p1 + p2);

                float intx2 = p1.X * p1.X + p2.X * p1.X + p2.X * p2.X;
                float inty2 = p1.Y * p1.Y + p2.Y * p1.Y + p2.Y * p2.Y;
                I += (0.25f * kInv3 * D) * (intx2 + inty2);
            }

            c *= 1.0f / area;

            // Translate vertices to centroid (make the centroid (0, 0) for the polygon in model space)
            for (uint i = 0; i < vertices.Length; ++i) {
                vertices[i] -= c;
                SetPoint(i, vertices[i]);
            }

            for (uint i1 = 0; i1 < vertices.Length; ++i1) {
                uint i2 = (i1 + 1) % (uint)vertices.Length;
                Vector2f face = (vertices[i2] - vertices[i1]).Norm();
                hl[i1] = Math.Abs(face.CrossProduct(-vertices[i1])); // (0,0) - vertex, since COM is at (0,0)
            }

            current = new State(position, rotation, density * area, density * I);
            previous = current;
        }

        private void InitVertices() {
            uint count = (uint) Random(3, MAXVERTEXCOUNT);
            float e = Random(5, 10);
            Vector2f[] buffer = new Vector2f[count];
            for (uint i = 0; i < buffer.Length; ++i) {
                buffer[i] = new Vector2f(Random(-e, e), Random(-e, e));
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
                    hl = new float[outCount];
                    break;
                }
            }

            // Copy vertices into shape's vertices
            for(uint i = 0; i < GetPointCount(); ++i)
                vertices[i] = buffer[hull[i]];

            // Compute face normals
            for (uint i1 = 0; i1 < vertices.Length; ++i1) {
                uint i2 = (i1 + 1) % (uint) vertices.Length;
                Vector2f face = vertices[i2] - vertices[i1];
                normals[i1] = new Vector2f(face.Y, -face.X).Norm();
            }
        }

        private float Random(float l, float h) {
            float a = (float) random.NextDouble();
            a = (h - l) * a + l;
            return a;
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

        public Vector2f Velocity {
            get { return current.velocity; }
            set { current.velocity = value; }
        }

        public float AngularVelocity {
            get { return current.angularVelocity; }
            set { current.angularVelocity = value; }
        }

        public Vector2f Normal(uint i) {
            if (current.orientation != 0)
                return normals[i].Rotate(current.orientation);
            return normals[i];
        }

        public Vector2f Normal(int i) {
            if (current.orientation != 0)
                return normals[i].Rotate(current.orientation);
            return normals[i];
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using SFML.Audio;

namespace Pong{
    class Collision {
        public enum Type {
            Circle, Polygon, Plane
        }

        public bool collision;
        public Vector2f normal;
        public float distance;
        public float overlap;
        public Vector2f[] contacts;
        private const float TOLERANCE = 0;

        public static Collision CheckForCollision(IShape obj1, IShape obj2) {
            Collision colli = new Collision();
            Dispatch[(int) obj1.Type, (int) obj2.Type](obj1, obj2, ref colli);
            return colli;
        }

        private delegate void CollisionType(IShape obj1, IShape obj2, ref Collision colli);

        private static CollisionType[,] Dispatch = {
            { CircleToCircle, CircleToPolygon },
            { PolygonToCircle, PolygonToPolygon }
        };

        private static void CircleToCircle(IShape obj1, IShape obj2, ref Collision colli) {
            Circle cir1 = obj1 as Circle;
            Circle cir2 = obj2 as Circle;
            float r = cir1.Radius + cir2.Radius;

            colli.normal = cir1.COM - cir2.COM;
            colli.distance = colli.normal.Length2();
            colli.collision = colli.distance < r * r;

            if (colli.collision) {
                colli.distance = (float) Math.Sqrt(colli.distance);
                colli.overlap = r - colli.distance;
                colli.normal /= colli.distance;
                PullApart(cir1, cir2, colli.normal, colli.overlap);
                colli.contacts = new Vector2f[1];
                colli.contacts[0] = cir2.COM + colli.normal * cir2.Radius;
            }
        }

        private static void CircleToPolygon(IShape obj1, IShape obj2, ref Collision colli) {
            Circle cir = obj1 as Circle;
            Polygon poly = obj2 as Polygon;

            // Transform circle center to polygon model space
            Vector2f center = poly.WorldTransform.Transponent * (cir.COM - poly.COM);

            colli.collision = false;
            colli.normal = new Vector2f(0, 0);
            colli.overlap = 0;
            float distance;

            for (int i = 0; i < poly.normals.Length; ++i) {
                distance = (center - poly.vertices[i]).Dot(poly.normals[i]);
                distance = distance * distance;
                if (distance < cir.Radius * cir.Radius) {
                    colli.normal += poly.normals[i];
                    colli.normal = colli.normal.Norm();
                    colli.overlap += (float)Math.Pow(cir.Radius - Math.Sqrt(distance), 2);
                    Vector2f v = center - colli.normal * cir.Radius;
                    bool inside = true;
                    for (uint j = 0; j < poly.normals.Length; ++j){
                        if ((v - poly.vertices[j]).Dot(poly.normals[j]) > 0) {
                            inside = false;
                            break;
                        }
                    }
                    if (inside)
                        colli.collision = true;
                }
            }
            if (colli.collision) {
                colli.normal = poly.WorldTransform * colli.normal.Norm();
                colli.overlap = (float) Math.Sqrt(colli.overlap);
                PullApart(cir, poly, colli.normal, colli.overlap);
                colli.contacts = new Vector2f[1];
                colli.contacts[0] = cir.COM - colli.normal * cir.Radius;
            }
        }

        private static void PolygonToCircle(IShape obj1, IShape obj2, ref Collision colli) {
            colli.collision = false;
            return;
        }

        private static void PolygonToPolygon(IShape obj1, IShape obj2, ref Collision colli) {
            Polygon poly1 = obj1 as Polygon;
            Polygon poly2 = obj2 as Polygon;

            Vector2f T = poly2.COM - poly1.COM;
            colli.overlap = float.MaxValue;
            Collision c = new Collision();

            for (uint i = 0; i < poly1.normals.Length; ++i) {
                c = PolyToSepAxis(poly1, poly2, poly1.Normal(i), T);
                if(!c.collision) {
                    colli.collision = false;
                    return;
                }
                if (c.overlap < colli.overlap)
                    colli = c;
            }

            for (uint i = 0; i < poly2.normals.Length; ++i) {
                c = PolyToSepAxis(poly1, poly2, poly2.Normal(i), T);
                if (!c.collision) {
                    colli.collision = false;
                    return;
                }
                if (c.overlap < colli.overlap)
                    colli = c;
            }

            if (colli.collision) {
                PullApart(poly1, poly2, colli.normal, colli.overlap);
                ContactPoints(poly1, poly2, colli.normal, ref colli);
            }
        }

        private static void ContactPoints(Polygon poly1, Polygon poly2, Vector2f n, ref Collision colli) {
            List<Vector2f> support1 = poly1.GetSupport(n);
            List<Vector2f> support2 = poly2.GetSupport(-n);
            List<Vector2f> buffer = new List<Vector2f>();

            poly1.GetIntersectedPoints(support2, ref buffer);
            poly2.GetIntersectedPoints(support1, ref buffer);

            if (buffer.Count >= 2) {
                colli.contacts = new Vector2f[2];
                colli.contacts[0] = buffer[0];
                colli.contacts[1] = buffer[1];
            } else if (buffer.Count == 1) {
                colli.contacts = new Vector2f[1];
                colli.contacts[0] = buffer[0];
            } else {
                colli.collision = false;
            }
        }

        private static float[] Projection(Polygon poly1, Vector2f n) {
            float[] projection = new float[2];
            float value;
            projection[0] = projection[1] = n.Dot(poly1.Vertex(0));
            for (uint i = 1; i < poly1.vertices.Length; ++i) {
                value = n.Dot(poly1.Vertex(i));
                projection[0] = Math.Min(value, projection[0]);
                projection[1] = Math.Max(value, projection[1]);
            }
            return projection;
        }

        private static Collision PolyToSepAxis(Polygon poly1, Polygon poly2, Vector2f n, Vector2f T) {
            Collision colli = new Collision();
            colli.distance = T.Dot(n);
            if (colli.distance > 0) n = -n;
            colli.normal = n;
            colli.distance = Math.Abs(colli.distance);
            float[] proj1 = Projection(poly1, n);
            float[] proj2 = Projection(poly2, n);
            colli.collision = (proj1[1] >= proj2[0] && proj2[1] >= proj1[0]);
            if (colli.collision) {
                if (proj1[1] - proj2[0] <= proj2[1] - proj1[0]) {
                    colli.overlap = proj1[1] - proj2[0];
                } else {
                    colli.overlap = proj2[1] - proj1[0];
                }
            }
            return colli;
        }

        private static void PullApart(IShape obj1, IShape obj2, Vector2f n, float overlap){
            if (obj1.InverseMass > 0 && obj2.InverseMass > 0) {
                obj1.Pull(n,  overlap * 0.5f);
                obj2.Pull(n, -overlap * 0.5f);
            } else {
                obj1.Pull(n,  overlap * obj1.InverseMass * obj1.Mass);
                obj2.Pull(n, -overlap * obj2.InverseMass * obj2.Mass);
            }
        }
    }
}

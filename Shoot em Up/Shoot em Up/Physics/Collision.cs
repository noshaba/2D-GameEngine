using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using Maths;

namespace Physics {
    class Collision {
        public enum Type {
            Circle, Polygon, Plane
        }

        public bool collision;
        public Vector2f normal;
        public float distance;
        public float overlap;
        public Vector2f[] contacts;
        public IRigidBody obj;

        public static Collision CheckForCollision(IRigidBody obj1, IRigidBody obj2) {
            Collision colli = new Collision();
            Dispatch[(int)obj1.Type, (int)obj2.Type](obj1, obj2, ref colli);
            return colli;
        }

        public Collision other(IRigidBody other) {
            Collision colli = new Collision();
            colli.collision = this.collision;
            colli.distance = this.distance;
            colli.overlap = this.overlap;
            colli.normal = -this.normal;
            colli.contacts = this.contacts;
            colli.obj = other;
            return colli;
        }

        private delegate void CollisionType(IRigidBody obj1, IRigidBody obj2, ref Collision colli);

        private static CollisionType[,] Dispatch = {
            { CircleToCircle, CircleToPolygon, CircleToPlane },
            { PolygonToCircle, PolygonToPolygon, PolygonToPlane },
            { PlaneToCircle, PlaneToPolygon, PlaneToPlane }
        };

        private static float PointToPlaneDistance(Vector2f point, Plane plane) {
            return point.Dot(plane.normal) - plane.constant;
        }

        private static void CircleToPlane(IRigidBody obj1, IRigidBody obj2, ref Collision colli) {
            Circle cir = obj1 as Circle;
            Plane plane = obj2 as Plane;
            float r = cir.Radius + plane.thickness;
            colli.distance = PointToPlaneDistance(cir.COM, plane);
            colli.collision = colli.distance < r;
            if (colli.collision) {
                colli.normal = plane.normal;
                colli.overlap = r - colli.distance;
                cir.Pull(colli.normal, colli.overlap);
                colli.contacts = new Vector2f[1];
                colli.contacts[0] = cir.COM - colli.normal * cir.Radius;
                colli.obj = obj2;
            }
            
        }

        private static void PolygonToPlane(IRigidBody obj1, IRigidBody obj2, ref Collision colli) {
            Polygon poly = obj1 as Polygon;
            Plane plane = obj2 as Plane;
            colli.normal = plane.normal;
            colli.collision = false;
            float distance = float.MaxValue;
            colli.distance = float.MaxValue;
            int v = 0;
            // look for closest vertex
            for (int i = 0; i < poly.vertices.Length; ++i) {
                distance = PointToPlaneDistance(poly.Vertex(i), plane);
                if (distance < colli.distance && distance < plane.thickness) {
                    v = i;
                    colli.collision = true;
                    colli.distance = distance;
                    colli.overlap = plane.thickness - colli.distance;
                }
            }
            if (colli.collision) {
                poly.Pull(colli.normal, colli.overlap);
                colli.obj = obj2;
                if ((poly.Vertex((v + 1) % poly.vertices.Length).Dot(colli.normal) - plane.constant) < plane.thickness) {
                    colli.contacts = new Vector2f[2];
                    colli.contacts[0] = poly.Vertex(v);
                    colli.contacts[1] = poly.Vertex((v + 1) % poly.vertices.Length);
                }
                else if ((poly.Vertex((v - 1 + poly.vertices.Length) % poly.vertices.Length).Dot(colli.normal) - plane.constant) < plane.thickness) {
                    colli.contacts = new Vector2f[2];
                    colli.contacts[0] = poly.Vertex(v);
                    colli.contacts[1] = poly.Vertex((v - 1 + poly.vertices.Length) % poly.vertices.Length);
                } else {
                    colli.contacts = new Vector2f[1];
                    colli.contacts[0] = poly.Vertex(v);
                }
            }
        }

        private static void PlaneToCircle(IRigidBody obj1, IRigidBody obj2, ref Collision colli) {
            CircleToPlane(obj2, obj1, ref colli);
            colli.obj = obj2;
        }

        private static void PlaneToPolygon(IRigidBody obj1, IRigidBody obj2, ref Collision colli) {
            PolygonToPlane(obj2, obj1, ref colli);
            colli.obj = obj2;
        }

        private static void PlaneToPlane(IRigidBody obj1, IRigidBody obj2, ref Collision colli) {
            colli.collision = false;
        }


        private static void CircleToCircle(IRigidBody obj1, IRigidBody obj2, ref Collision colli) {
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
                colli.obj = obj2;
            }
        }

        private static void CircleToPolygon(IRigidBody obj1, IRigidBody obj2, ref Collision colli) {
            Circle cir = obj1 as Circle;
            Polygon poly = obj2 as Polygon;
            // Transform circle center to polygon model space
            Vector2f center = poly.WorldTransform.Transponent * (cir.COM - poly.COM);
            colli.distance = float.MinValue;
            float value;
            int normal = 0;
            for (int i = 0; i < poly.vertices.Length; ++i) {
                value = poly.normals[i].Dot(center - poly.vertices[i]);
                if (value >= cir.Radius) {
                    colli.collision = false;
                    return;
                }
                if (value > colli.distance) {
                    colli.distance = value;
                    normal = i;
                }
            }
            // if center is within polygon
            if (colli.distance < EMath.EPSILON) {
                colli.collision = true;
                colli.overlap = cir.Radius - colli.distance;
                colli.normal = poly.Normal(normal);
                PullApart(cir, poly, colli.normal, colli.overlap);
                colli.contacts = new Vector2f[1];
                colli.contacts[0] = cir.COM - colli.normal * cir.Radius;
                colli.obj = obj2;
                return;
            }
            // Determine which voronoi region of the edge center of circle lies within
            Vector2f v1 = poly.vertices[normal];
            Vector2f v2 = poly.vertices[(normal + 1)%poly.vertices.Length];
            float dot1 = (center - v1).Dot(v2 - v1);
            float dot2 = (center - v2).Dot(v1 - v2);
            // closest to v1
            if (dot1 < 0) {
                colli.distance = (center - v1).Length2();
                if (colli.distance >= cir.Radius * cir.Radius) {
                    colli.collision = false;
                    return;
                }
                colli.collision = true;
                colli.distance = (float) Math.Sqrt(colli.distance);
                colli.normal = (poly.WorldTransform * (center - v1)).Norm();
            }
            // closest to v2
            else if (dot2 < 0) {
                colli.distance = (center - v2).Length2();
                if (colli.distance >= cir.Radius * cir.Radius) {
                    colli.collision = false;
                    return;
                }
                colli.collision = true;
                colli.distance = (float) Math.Sqrt(colli.distance);
                colli.normal = (poly.WorldTransform * (center - v2)).Norm();
            }
            // closest to edge
            else {
                colli.distance = (center - v1).Dot(poly.normals[normal]);
                if ((center - v1).Dot(poly.normals[normal]) >= cir.Radius) {
                    colli.collision = false;
                    return;
                }
                colli.collision = true;
                colli.normal = poly.Normal(normal);
            }
            if (colli.collision) {
                colli.overlap = cir.Radius - colli.distance;
                PullApart(cir, poly, colli.normal, colli.overlap);
                colli.contacts = new Vector2f[1];
                colli.contacts[0] = cir.COM - colli.normal * cir.Radius;
                colli.obj = obj2;
            }
        }

        private static void PolygonToCircle(IRigidBody obj1, IRigidBody obj2, ref Collision colli) {
            CircleToPolygon(obj2, obj1, ref colli);
            colli.obj = obj2;
        }

        private static void PolygonToPolygon(IRigidBody obj1, IRigidBody obj2, ref Collision colli) {
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
                colli.obj = obj2;
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

        private static float[] Projection(Polygon poly, Vector2f n) {
            float[] projection = new float[2];
            float value;
            projection[0] = projection[1] = n.Dot(poly.Vertex(0));
            for (uint i = 1; i < poly.vertices.Length; ++i) {
                value = n.Dot(poly.Vertex(i));
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

        private static void PullApart(IRigidBody obj1, IRigidBody obj2, Vector2f n, float overlap){
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

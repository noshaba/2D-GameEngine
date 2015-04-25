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
            Circle, Polygon, OBB
        }

        public bool collision;
        public Vector2f normal;
        public float distance;
        public float overlap;
        public Vector2f point;
        public Vector2f[] contacts;
        public Vector2f rad1;
        public Vector2f rad2;
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
        }

        private static void CircleToPolygon(IShape obj1, IShape obj2, ref Collision colli) {
            Circle cir = obj1 as Circle;
            Polygon poly = obj2 as Polygon;

            Vector2f center = (cir.COM - poly.COM).TransRotate(poly.Orientation);
            float separation = float.MinValue;
            uint faceNormal = 0;
            for (uint i = 0; i < poly.vertices.Length; ++i) {
                float s = poly.normals[i].Dot(center - poly.vertices[i]);
                if (s > cir.Radius) {
                    colli.collision = false;
                    return;
                }
                if (s > separation) {
                    separation = s;
                    faceNormal = i;
                }
            }

            // if circle entirely in polygon
            if (separation < EMath.EPSILON) {
                colli.contacts = new Vector2f[1];
                colli.normal = poly.Normal(faceNormal);
                colli.contacts[0] = -colli.normal * cir.Radius + cir.COM;
                colli.overlap = cir.Radius;
                colli.collision = true;
                //PullApart(cir, poly, colli.normal, colli.overlap, ref colli);
                return;
            }

            Vector2f v1 = poly.vertices[faceNormal];
            Vector2f v2 = poly.vertices[(faceNormal + 1) % (uint)poly.vertices.Length];
            colli.overlap = cir.Radius - separation;

            // Determine which voronoi region of the edge center of circle lies within
            float dot1 = (center - v1).Dot( v2 - v1);
            float dot2 = (center - v2).Dot( v1 - v2);
            colli.overlap = cir.Radius - separation;
            // closest to v1
            if (dot1 <= 0) { 
                if((center - v1).Length2() > cir.Radius * cir.Radius){
                    colli.collision = false;
                    return;
                }
                colli.contacts = new Vector2f[1];
                colli.normal = (v1 - center).Rotate(poly.Orientation).Norm();
                colli.contacts[0] = v1.Rotate(poly.Orientation) + poly.COM;
                colli.collision = true;
                //PullApart(poly, cir, -colli.normal, colli.overlap, ref colli);
            // closest to v2
            } else if (dot2 <= 0) {
                if ((center - v2).Length2() > cir.Radius * cir.Radius) {
                    colli.collision = false;
                    return;
                }
                colli.contacts = new Vector2f[1];
                colli.normal = (v2 - center).Rotate(poly.Orientation).Norm();
                colli.contacts[0] = v2.Rotate(poly.Orientation) + poly.COM;
                colli.collision = true;
                //PullApart(poly, cir, -colli.normal, colli.overlap, ref colli);
            // closest to face
            } else {
                if ((center - v1).Dot(poly.normals[faceNormal]) > cir.Radius) {
                    colli.collision = false;
                    return;
                }
                colli.contacts = new Vector2f[1];
                colli.normal = poly.Normal(faceNormal);
                colli.contacts[0] = -colli.normal * cir.Radius + cir.Position;
                colli.collision = true;
                //PullApart(cir, poly, colli.normal, colli.overlap, ref colli);
            }
        }

        private static void PolygonToCircle(IShape obj1, IShape obj2, ref Collision colli) {
            CircleToPolygon(obj2, obj1, ref colli);
            colli.normal = -colli.normal;
        }

        private static void PolygonToPolygon(IShape obj1, IShape obj2, ref Collision colli) {
            Polygon poly1 = obj1 as Polygon;
            Polygon poly2 = obj2 as Polygon;

            Vector2f T = poly2.COM - poly1.COM;
            Collision c = new Collision();
            for (uint i = 0; i < poly1.normals.Length; ++i) {
               // c = PolyToSepAxis(poly1, poly2, poly1.Normal(i), T);
            }
        }
        /*
        private static bool BiasGreaterThan(float a, float b) {
            float biasRelative = 0.95f;
            float biasAbsolute = 0.01f;
            return a >= b * biasRelative + a * biasAbsolute;
        }

        private static float FindAxisLeastPenetration(ref uint faceIndex, Polygon poly1, Polygon poly2) {
            float bestDistance = float.MinValue;
            uint bestIndex = 0;
            for (uint i = 0; i < poly1.vertices.Length; ++i) {
                Vector2f n = poly1.Normal(i).TransRotate(poly2.Orientation);
                Vector2f s = poly2.GetSupport(-n);
                Vector2f v = poly1.vertices[i].Rotate(poly1.Orientation) + poly1.COM;
                v -= poly2.COM;
                v = v.TransRotate(poly2.Orientation);
                float d = n.Dot(s - v);
                if (d > bestDistance) {
                    bestDistance = d;
                    bestIndex = i;
                }
            }
            faceIndex = bestIndex;
            return bestDistance;
        }

        private static void FindIncidentFace(ref Vector2f[] v, Polygon refPoly, Polygon incPoly, uint referenceIndex) { 
            Vector2f referenceNormal = refPoly.normals[referenceIndex];
            referenceNormal = referenceNormal.Rotate(refPoly.Orientation);
            referenceNormal = referenceNormal.Rotate(incPoly.Orientation);
            uint incidentFace = 0;
            float minDot = float.MaxValue;
            for (uint i = 0; i < incPoly.vertices.Length; ++i) {
                float dot = referenceNormal.Dot(incPoly.normals[i]);
                if (dot < minDot) {
                    minDot = dot;
                    incidentFace = i;
                }
            }
            v[0] = incPoly.vertices[incidentFace].Rotate(incPoly.Orientation) + incPoly.Position;
            incidentFace = (incidentFace + 1) % (uint) incPoly.vertices.Length;
            v[1] = incPoly.vertices[incidentFace].Rotate(incPoly.Orientation) + incPoly.Position;
        }

        private static uint Clip(Vector2f n, float c, ref Vector2f[] faces) {
            uint sp = 0;
            Vector2f[] output = faces;
            float d1 = n.Dot(faces[0]) - c;
            float d2 = n.Dot(faces[1]) - c;
            // If negative (behind plane) clip
            if (d1 <= 0) output[sp++] = faces[0];
            if (d2 <= 0) output[sp++] = faces[1];
            // If the points are on different sides of the plane
            if (d1 * d2 < 0) { 
                float alpha = d1 / (d1 - d2);
                output[sp] = faces[0] + alpha * (faces[1] - faces[0]);
                ++sp;
            }
            faces[0] = output[0];
            faces[1] = output[1];
            if (sp != 3) {
                // Console.WriteLine("NOOOOOOOOOOOOOOOOO");
            }
            return sp;
        }*/

        private static void CircleToOBB(IShape obj1, IShape obj2, ref Collision colli) {
            Circle cir = obj1 as Circle;
            OBB obb = obj2 as OBB;
            Vector2f closest = ClosestPointOnOBB(cir.COM, obb);
            colli.normal = cir.COM - closest;
            colli.distance = colli.normal.Length();
            if (colli.distance != 0) colli.normal /= colli.distance;
            colli.overlap = cir.Radius - colli.distance;
            colli.collision = colli.distance < cir.Radius;
            if (colli.collision) {
                //TODO: remove this and handle sound of ball in ball class
                if (cir is Ball)
                    Sounds.hitSound.Play();
                if (cir.InverseMass > 0 && obb.InverseMass > 0) {
                    cir.Pull(colli.normal,  colli.overlap * 0.5f);
                    obb.Pull(colli.normal, -colli.overlap * 0.5f);
                    colli.point = closest - colli.normal * colli.overlap * 0.5f;
                } else {
                    cir.Pull(colli.normal,  colli.overlap * cir.InverseMass * cir.Mass);
                    obb.Pull(colli.normal, -colli.overlap * obb.InverseMass * obb.Mass);
                    colli.point = closest - colli.normal * colli.overlap * obb.InverseMass * obb.Mass;
                }
                colli.rad1 = closest - cir.COM;
                colli.rad2 = closest - obb.COM;
                // PositionalCorrection(obj1, obj2, colli.normal);
            }
        }

        private static void OBBToCircle(IShape obj1, IShape obj2, ref Collision colli) {
            CircleToOBB(obj2, obj1, ref colli);
            if (colli.collision) {
                Vector2f buffer = colli.rad1;
                colli.rad1 = colli.rad2;
                colli.rad2 = buffer;
                colli.normal = -colli.normal;
            }
        }

        private static void OBBToOBB(IShape obj1, IShape obj2, ref Collision colli) {
            OBB obb1 = obj1 as OBB;
            OBB obb2 = obj2 as OBB;
            colli.overlap = float.MaxValue;
            Vector2f T = obb2.COM - obb1.COM;
            Collision c;
            for (int i = 0; i < obb1.axis.Length; ++i) {
                c = OBBToAxis(obb1, obb2, obb1.Axis(i), T);
                if (!c.collision) {
                    colli.collision = false;
                    return;
                }
                if (c.overlap < colli.overlap)
                    colli = c;
            }
            for (int i = 0; i < obb2.axis.Length; ++i) {
                c = OBBToAxis(obb1, obb2, obb2.Axis(i), T);
                if (!c.collision) {
                    colli.collision = false;
                    return;
                }
                if (c.overlap < colli.overlap)
                    colli = c;
            }
            if (colli.collision) {
                if (obb1.InverseMass > 0 && obb2.InverseMass > 0) {
                    obb1.Pull(colli.normal, colli.overlap * 0.5f);
                    obb2.Pull(colli.normal, -colli.overlap * 0.5f);
                } else {
                    obb1.Pull(colli.normal, colli.overlap * obb1.InverseMass * obb1.Mass);
                    obb2.Pull(colli.normal, -colli.overlap * obb2.InverseMass * obb2.Mass);
                }
                // TODO: actual point of contact generation
                colli.rad1 = ClosestPointOnOBB(obb2.COM, obb1) - obb1.COM;
                colli.rad2 = ClosestPointOnOBB(obb1.COM, obb2) - obb2.COM;
                // PositionalCorrection(obj1, obj2, colli.normal);
            }
        }

        private static void PullApart(IShape obj1, IShape obj2, Vector2f n, float overlap, ref Collision colli){
            if (obj1.InverseMass > 0 && obj2.InverseMass > 0) {
                obj1.Pull(n,  overlap * 0.5f);
                obj2.Pull(n, -overlap * 0.5f);
                for (uint i = 0; i < colli.contacts.Length; ++i)
                    colli.contacts[i] += n * overlap * 0.5f;
            } else {
                obj1.Pull(n,  overlap * obj1.InverseMass * obj1.Mass);
                obj2.Pull(n, -overlap * obj2.InverseMass * obj2.Mass);
                for(uint i = 0; i < colli.contacts.Length; ++i){
                    colli.contacts[i] += n * overlap * obj1.InverseMass * obj1.Mass;
                    colli.contacts[i] -= n * overlap * obj2.InverseMass * obj2.Mass;
                }
            }
        }

        private static Vector2f ClosestPointOnOBB(Vector2f p, OBB obb) {
            Vector2f distanceVec = p - obb.COM;
            Vector2f closest = obb.COM;
            float distance;
            for(uint i = 0; i < obb.axis.Length; ++i){ 
                distance = distanceVec.Dot(obb.Axis(i));
                if (distance >  obb.hl[i]) distance =  obb.hl[i];
                if (distance < -obb.hl[i]) distance = -obb.hl[i];
                closest += distance * obb.Axis(i);
            }
            return closest;
        }

        private static Collision OBBToAxis(OBB obb1, OBB obb2, Vector2f n, Vector2f T) {
            Collision colli = new Collision();
            colli.distance = T.Dot(n);
            if (colli.distance > 0) n = -n;
            colli.normal = n;
            colli.distance = Math.Abs(colli.distance);
            float r = 0;
            for (int i = 0; i < obb1.hl.Length; ++i)
                r += Math.Abs(obb1.hl[i]*obb1.Axis(i).Dot(n));
            for (int i = 0; i < obb2.hl.Length; ++i)
                r += Math.Abs(obb2.hl[i] * obb2.Axis(i).Dot(n));
            colli.overlap = r - colli.distance;
            colli.collision = colli.distance < r;
            return colli;
        }

        private static void PositionalCorrection(IShape obj1, IShape obj2, Vector2f n) {
            if (obj1.InverseMass > 0 && obj2.InverseMass > 0) {
                obj1.Pull(n,  TOLERANCE * 0.5f);
                obj2.Pull(n, -TOLERANCE * 0.5f);
            } else {
                obj1.Pull(n,  TOLERANCE * obj1.InverseMass * obj1.Mass);
                obj2.Pull(n, -TOLERANCE * obj2.InverseMass * obj2.Mass);
            }
        }
    }
}

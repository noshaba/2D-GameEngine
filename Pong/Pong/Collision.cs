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
            Circle, OBB
        }

        public bool collision;
        public Vector2f normal;
        public float distance;
        public float overlap;
        public Vector2f point;
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
            { CircleToCircle, CircleToOBB },
            { OBBToCircle, OBBToOBB }
        };

        private static void CircleToCircle(IShape obj1, IShape obj2, ref Collision colli) {
            Circle cir1 = obj1 as Circle;
            Circle cir2 = obj2 as Circle;
        }

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

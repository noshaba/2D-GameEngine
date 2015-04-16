using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

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
            }
        }

        private static void OBBToCircle(IShape obj1, IShape obj2, ref Collision colli) {
            CircleToOBB(obj2, obj1, ref colli);
        }

        private static void OBBToOBB(IShape obj1, IShape obj2, ref Collision colli) {
            OBB obb1 = obj1 as OBB;
            OBB obb2 = obj2 as OBB;
            /*colli.overlap = float.MaxValue;
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
            }*/

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
            colli.collision = colli.distance <= r;
            return colli;
        }
    }
}

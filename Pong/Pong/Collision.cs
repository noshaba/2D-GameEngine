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
            Vector2f closest = ClosestPointOnOBB(cir.Position, obb);
            colli.normal = cir.Position - closest;
            colli.distance = colli.normal.Length();
            if (colli.distance != 0) colli.normal /= colli.distance;
            colli.overlap = cir.Radius - colli.distance;
            colli.collision = colli.distance <= cir.Radius;
            if (colli.collision) {
                cir.Pull(colli.normal, colli.overlap);
                colli.point = closest;
                colli.rad1 = closest - cir.Position;
                colli.rad2 = closest - obb.Position;
            }
        }

        private static void OBBToCircle(IShape obj1, IShape obj2, ref Collision colli) {
            CircleToOBB(obj2, obj1, ref colli);
        }

        private static void OBBToOBB(IShape obj1, IShape obj2, ref Collision colli) {
            OBB obb1 = obj1 as OBB;
            OBB obb2 = obj2 as OBB;
        }

        private static Vector2f ClosestPointOnOBB(Vector2f p, OBB obb) {
            Vector2f distanceVec = p - obb.Position;
            Vector2f closest = obb.Position;
            float distance;
            for(uint i = 0; i < obb.axis.Length; ++i){ 
                distance = distanceVec.Dot(obb.Axis(i));
                if (distance >  obb.hl[i]) distance =  obb.hl[i];
                if (distance < -obb.hl[i]) distance = -obb.hl[i];
                closest += distance * obb.Axis(i);
            }
            return closest;
        }
    }
}

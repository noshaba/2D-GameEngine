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

        public static Collision CheckForCollision(IShape obj1, IShape obj2) {
            Collision colli = new Collision();
            Dispatch[(int) obj1.Type, (int) obj2.Type](obj1, obj2, ref colli);
            return colli;
        }

        private delegate void CollisionType(IShape obj1, IShape obj2, ref Collision colli);

        private static CollisionType[,] Dispatch = {
            { CircleToCircle, CircleToOBB },
            { OBBToCircle, OBBToOBB}
        };

        private static void CircleToCircle(IShape obj1, IShape obj2, ref Collision colli) {
            Circle cir1 = obj1 as Circle;
            Circle cir2 = obj2 as Circle;
        }

        private static void CircleToOBB(IShape obj1, IShape obj2, ref Collision colli) {
            Circle cir = obj1 as Circle;
            OBB obb = obj2 as OBB;
        }

        private static void OBBToCircle(IShape obj1, IShape obj2, ref Collision colli) {
            CircleToOBB(obj2, obj1, ref colli);
        }

        private static void OBBToOBB(IShape obj1, IShape obj2, ref Collision colli) {
            OBB obb1 = obj1 as OBB;
            OBB obb2 = obj2 as OBB;
        }
    }
}

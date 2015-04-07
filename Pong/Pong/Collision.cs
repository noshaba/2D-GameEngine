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

        public static Collision CheckForCollision(Shape obj1, Shape obj2) {
            Collision colli = new Collision();

            return colli;
        }

        private delegate void CollisionType(Shape obj1, Shape obj2, ref Collision colli);

        private CollisionType[,] Dispatch = {
            { CircleToCircle, CircleToOBB },
            { OBBToCircle, OBBToOBB}
        };

        private static void CircleToCircle(Shape obj1, Shape obj2, ref Collision colli) {
        }

        private static void CircleToOBB(Shape obj1, Shape obj2, ref Collision colli) { 
        }

        private static void OBBToCircle(Shape obj1, Shape obj2, ref Collision colli) {
            CircleToOBB(obj2, obj1, ref colli);
        }

        private static void OBBToOBB(Shape obj1, Shape obj2, ref Collision colli) {
        }
    }
}

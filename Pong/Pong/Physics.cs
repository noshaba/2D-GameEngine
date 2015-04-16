using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Pong{
    class Physics {
        private List<IShape> objects = new List<IShape>();
        public bool frozen = false;

        public void AddObject(IShape obj) {
            objects.Add(obj);
        }

        public void Update(float dt) {
            if (!frozen) {
                for (int i = 0; i < objects.Count(); ++i) {
                    objects[i].Update(dt);
                    ApplyForces(dt, i);
                }
            }
        }

        #region Physical Methods

        private void ApplyForces(float dt, int i) {
            AddCollisionImpulse(i);
        }

        private void AddCollisionImpulse(int i) {
            for (int j = 0; j < objects.Count(); ++j) {
                if (i == j) continue;
                Collision colli = Collision.CheckForCollision(objects[i], objects[j]);
                if (colli.collision) {
                    if (objects[i].InverseMass > 0 || objects[j].InverseMass > 0) {
                        Vector2f J = CollisionImpulse(objects[i], objects[j], colli.rad1, colli.rad2, colli.normal);
                        objects[i].ApplyImpulse(J, colli.rad1);
                        objects[j].ApplyImpulse(-J, colli.rad2);
                    }
                }
            }
        }

        private Vector2f CollisionImpulse(IShape obj1, IShape obj2, Vector2f r1, Vector2f r2, Vector2f n) {
            Vector2f v1 = obj1.Velocity;
            Vector2f v2 = obj2.Velocity;
            float w1 = obj1.AngularVelocity;
            float w2 = obj2.AngularVelocity;
            float iM1 = obj1.InverseMass;
            float iM2 = obj2.InverseMass;
            float iI1 = obj1.InverseInertia;
            float iI2 = obj2.InverseInertia;
            float cv = n.Dot(v1 + w1.CrossProduct(r1) - v2 - w2.CrossProduct(r2));
            float r1CrossN = r1.CrossProduct(n);
            float r2CrossN = r2.CrossProduct(n);
            float invMassSum = (float) (iM1 + iM2 + Math.Pow(r1CrossN, 2) * iI1 + Math.Pow(r2CrossN, 2) * iI2);
            float j = -2 * cv;
            j /= invMassSum;
            return j * n;
        }

        #endregion
    }
}

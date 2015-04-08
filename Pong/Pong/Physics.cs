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
        public void AddObject(IShape obj) {
            objects.Add(obj);
        }
        public void Update(float dt) {
            for (int i = 0; i < objects.Count(); ++i) {
                for (int j = 0; j < objects.Count(); ++j) {
                    if (i == j) continue;
                    Collision colli = Collision.CheckForCollision(objects[i], objects[j]);
                    if (colli.collision) {
                        Vector2f f = CollisionForce(objects[i], objects[j], colli.rad1, colli.rad2, colli.normal);
                        objects[i].Force += f;
                        objects[j].Force -= f;
                        objects[i].Torque += colli.rad1.CrossProduct(f);
                        objects[j].Torque += colli.rad2.CrossProduct(-f);
                    }
                }
                objects[i].Update(dt);
            }
        }
        private Vector2f CollisionForce(IShape obj1, IShape obj2, Vector2f r1, Vector2f r2, Vector2f n) {
            Vector2f v1 = obj1.Velocity;
            Vector2f v2 = obj2.Velocity;
            float w1 = obj1.AngularVelocity;
            float w2 = obj2.AngularVelocity;
            return -n.Dot(v1 + w1.CrossProduct(r1) - v2 - w2.CrossProduct(r2)) * n;
        }
    }
}

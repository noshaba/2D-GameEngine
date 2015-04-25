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

        //why 7? - remove all additional objects except ball, 2 paddles and 4 walls? replace 7 with variable in future
        public void Reset() {
            objects.RemoveRange(7, objects.Count - 7);
        }

        //updates all objects in the list
        public void Update(float dt) {
            //frozen = true;
            if (!frozen) {
                for (int i = 0; i < objects.Count(); ++i) {
                    objects[i].Update(dt);
                    ApplyForces(dt, i);
                }
            }
        }

        #region Physical Methods

        //why have dt in parameters when its not in use?
        private void ApplyForces(float dt, int i) {
            AddCollisionImpulse(i);
        }

        //checks for collision between all objects
        private void AddCollisionImpulse(int i) {
            for (int j = 0; j < objects.Count(); ++j) {
                if (i == j) continue;
                Collision colli = Collision.CheckForCollision(objects[i], objects[j]);
                if (colli.collision) {
                  /*  if (i == 2) { 
                        (objects[j] as Shape).FillColor = Color.Magenta;
                        Program.collision = true;
                        Program.colliPoint = colli.contacts[0];
                        Program.e1 = colli.rad1;
                        Program.e2 = colli.rad2;
                        Console.WriteLine(colli.normal);
                    }*/
                    if (objects[i].InverseMass > 0 || objects[j].InverseMass > 0) {
                        for (uint k = 0; k < colli.contacts.Length; ++k) {
                            Vector2f rad1 = colli.contacts[k] - objects[i].COM;
                            Vector2f rad2 = colli.contacts[k] - objects[j].COM;
                            Vector2f J = CollisionImpulse(objects[i], objects[j], rad1, rad2, colli.normal);
                         /*   Console.WriteLine(i);
                            Console.WriteLine(colli.normal);
                            Console.WriteLine(colli.contacts[k]);
                            Console.WriteLine(rad1);
                            Console.WriteLine(rad2);
                            Console.WriteLine(colli.contacts.Length);
                            frozen = true;*/
                            objects[i].ApplyImpulse(J, rad1);
                            objects[j].ApplyImpulse(-J, rad2);
                        }
                       // Console.WriteLine();
                    }
                }/* else {
                    if (i == 2) { 
                        (objects[j] as Shape).FillColor = objects[j].Colour;
                        //Program.collision = false;
                    }
                }*/
            }
        }

        //determines the collision impulse between to given objects
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

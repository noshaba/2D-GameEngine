using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;
using Maths;

namespace Physics {
    class Physic {
        public List<IState> objects;
        private Vector2f gravity;
        private float damping;
        private bool friction;
        public bool frozen = false;

        public Physic(List<IState> shapes, Vector2f gravity, float damping, bool friction) {
            this.gravity = gravity;
            this.damping = damping;
            this.friction = friction;
            this.objects = shapes;
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

        public void Add(IState obj)
        {
            objects.Add(obj);

        }

        #region Physical Methods

        private void ApplyForces(float dt, int i) {
            Gravity(dt, i);
            Damping(dt, i);
            AddCollisionImpulse(i);
        }

        private void Damping(float dt, int i) {
            objects[i].Velocity -= dt * damping * objects[i].Velocity;
            objects[i].AngularVelocity -= dt * damping * objects[i].AngularVelocity;
        }

        private void Gravity(float dt, int i) {
            objects[i].Velocity += dt * gravity * 10f * objects[i].Mass * objects[i].InverseMass;
        }

        //checks for collision between all objects
        private void AddCollisionImpulse(int i) {
            for (int j = 0; j < objects.Count(); ++j) {
                if (i == j) continue;
                Collision colli = Collision.CheckForCollision(objects[i], objects[j]);
                if (colli.collision) {
                    if (!objects[i].Collision.collision) {
                        objects[i].Collision = colli;
                    }
                    if (objects[i].InverseMass > 0) {
                        for (uint k = 0; k < colli.contacts.Length; ++k) {
                            Vector2f rad1 = colli.contacts[k] - objects[i].COM;
                            Vector2f rad2 = colli.contacts[k] - objects[j].COM;
                            CollisionImpulse(objects[i], objects[j], rad1, rad2, colli.normal, colli.contacts.Length);
                        }
                    }
                }
            }
        }

        private void CollisionImpulse(IState obj1, IState obj2, Vector2f r1, Vector2f r2, Vector2f n, int contacts) {
            // init collision stats
            Vector2f v1 = obj1.Velocity;
            Vector2f v2 = obj2.Velocity;
            float w1 = obj1.AngularVelocity;
            float w2 = obj2.AngularVelocity;
            float iM1 = obj1.InverseMass;
            float iM2 = obj2.InverseMass;
            float iI1 = obj1.InverseInertia;
            float iI2 = obj2.InverseInertia;
            Vector2f rv = v1 + w1.CrossProduct(r1) - v2 - w2.CrossProduct(r2);
            float r1CrossN = r1.CrossProduct(n);
            float r2CrossN = r2.CrossProduct(n);
            float invMassSum = (float) (iM1 + iM2 + Math.Pow(r1CrossN, 2) * iI1 + Math.Pow(r2CrossN, 2) * iI2);
            float e = Math.Min(obj1.Restitution, obj2.Restitution);
            float sf = (float) Math.Sqrt(obj1.StaticFriction * obj2.StaticFriction);
            float kf = (float) Math.Sqrt(obj1.KineticFriction * obj2.KineticFriction);

            // apply collision force
            float cv = n.Dot(rv);
            float j = -(1 + e) * cv;
            j /= (invMassSum * contacts);

            Vector2f J = j*n;
            obj1.ApplyImpulse( J, r1);
            obj2.ApplyImpulse(-J, r2);

            if (friction) {
                // apply friction impule
                Vector2f t = (rv - n * rv.Dot(n)).Norm();
                // j tangent magnitude
                float jt = -rv.Dot(t);
                jt /= (invMassSum * contacts);

                // Don't apply tiny friction impulses
                if (Math.Abs(jt) < EMath.EPSILON)
                    return;
                // Coulumb's law
                Vector2f T;
                if (Math.Abs(jt) < j * sf)
                    T = t * jt;
                else
                    T = t * -j * kf;
                obj1.ApplyImpulse(T, r1);
                obj2.ApplyImpulse(-T, r2);
            }
        }

        #endregion
    }
}

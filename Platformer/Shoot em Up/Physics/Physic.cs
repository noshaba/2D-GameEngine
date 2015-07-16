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
        public List<Body> objects;
        public List<Constraint> joints;
        private Vector2f gravity;
        private float damping;
        public bool frozen = false;
        private Quadtree quadtree;
        private List<IRigidBody> possibleCollisionTargets;
        private Vector2f windowHalfSize;
        private Vector2f viewCenter;

        public Physic(List<Body> shapes, List<Constraint> constraints, Vector2f gravity, float damping, Vector2f windowSize) {
            this.gravity = gravity;
            this.damping = damping;
            this.objects = shapes;
            this.joints = constraints;
            this.windowHalfSize = windowSize * .5f;
        //    this.quadtree = new Quadtree(0, windowSize);
        //    this.possibleCollisionTargets = new List<IRigidBody>();
        }

        public void DrawQuadtree(RenderWindow window)
        {
            this.quadtree.Draw(window);
        }
        //updates all objects in the list
        public void Update(float dt, Vector2f viewCenter) {
            //    quadtree.Clear();
            //    foreach (IRigidBody obj in objects)
            //       quadtree.Insert(obj);
            this.viewCenter = viewCenter;
            if (!frozen)
            {
                Parallel.For(0, objects.Count, i =>
                {
                    if (objects[i].InsideWindow(viewCenter, windowHalfSize))
                    {
                        objects[i].Update(dt);
                        ApplyForces(dt, i);
                    }
                });
                Parallel.ForEach(joints, j => j.Solve());
            }
      /*      if (!frozen)
            {
                for (int i = 0; i < objects.Count; ++i)
                {
                    if (objects[i].InsideWindow(viewCenter, windowHalfSize))
                    {
                        objects[i].Update(dt);
                        ApplyForces(dt, i);
                    }
                }
                foreach (Constraint constraint in joints)
                    constraint.Solve();
            }*/
        }

        #region Physical Methods

        private void ApplyForces(float dt, int i) {
            if (!frozen)
            {
                Gravity(dt, i);
                Drag(dt, i);
                Damping(dt, i);
            }
            AddCollisionImpulse(i);
        }

        private void Damping(float dt, int i) {
            if(objects[i].moveable) objects[i].Velocity -= dt * damping * objects[i].Velocity;
            if(objects[i].rotateable) objects[i].AngularVelocity -= dt * damping * objects[i].AngularVelocity;
        }

        private void Gravity(float dt, int i) {
            if(objects[i].moveable) objects[i].Velocity += dt * gravity * 10f;
        }

        private void Drag(float dt, int i)
        {
            if (objects[i].DragCoefficient == 0 || !objects[i].moveable) return;
            objects[i].Velocity -= dt * objects[i].DragCoefficient * objects[i].InverseMass * objects[i].Velocity.Length2() * objects[i].Velocity.Norm();
        }

        //checks for collision between all objects
        private void AddCollisionImpulse(int i) {
        //    possibleCollisionTargets.Clear();
        //    quadtree.Retrieve(possibleCollisionTargets, objects[i]);

            for (int j = 0; j < objects.Count; ++j)
            {
                if (i == j) continue;
                if (!objects[j].InsideWindow(viewCenter, windowHalfSize)) continue;
                Collision colli = Collision.CheckForCollision(objects[i], objects[j]);
                if (colli.collision) {
                //    if (!objects[i].Collision.collision && !objects[j].Collision.collision)
                //    {
                        objects[i].Collision = colli;
                        objects[j].Collision = colli.other(objects[i]);
                //    }
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

        private void CollisionImpulse(Body obj1, Body obj2, Vector2f r1, Vector2f r2, Vector2f n, int contacts) {
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
            float invMassSum = (float) (iM1 + iM2 + r1CrossN * r1CrossN * iI1 + r2CrossN * r2CrossN * iI2);
            float e = Math.Min(obj1.Restitution, obj2.Restitution);
            float sf = (float) Math.Sqrt(obj1.StaticFriction * obj2.StaticFriction);
            float kf = (float) Math.Sqrt(obj1.KineticFriction * obj2.KineticFriction);

            // apply collision force
            float cv = n.Dot(rv);
            float j = -(1 + e) * cv;
            j /= (invMassSum * contacts);

            if (j < 0) return;

            Vector2f J = j*n;
            obj1.ApplyImpulse( J, r1);
            obj2.ApplyImpulse(-J, r2);

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

        #endregion
    }
}

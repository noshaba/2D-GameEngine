﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;
using Maths;

namespace Physics
{
    class RigidBody
    {
        public static ulong ID = 0;
        private object parent;
        private Collision collision;
        private float radius;
        private CircleShape boundingCircle;
        private State current;
        private State previous;
        private float restitution;
        private float staticFriction;
        private float dragCoefficient;
        private float kineticFriction;

        public IRigidBody[] bodies;

        public RigidBody(IRigidBody[] bodies, Vector2f centroid)
        {
            this.bodies = bodies;
            SetBodiesCentroid(centroid);
            Restitution = (float)EMath.random.NextDouble();
            StaticFriction = (float)EMath.random.NextDouble();
            KineticFriction = EMath.Random(0,staticFriction);
            DragCoefficient = 0;
            RigidBody.ID++;
        }

        public float Restitution
        {
            get { return this.restitution; }
            set
            {
                this.restitution = value;
                for (int i = 0; i < bodies.Length; ++i)
                    bodies[i].Restitution = value;
            }
        }

        public float StaticFriction
        {
            get { return this.staticFriction; }
            set
            {
                this.staticFriction = value;
                for (int i = 0; i < bodies.Length; ++i)
                    bodies[i].StaticFriction = value;
            }
        }

        public float KineticFriction
        {
            get { return this.kineticFriction; }
            set
            {
                this.kineticFriction = value;
                for (int i = 0; i < bodies.Length; ++i)
                    bodies[i].KineticFriction = value;
            }
        }

        public float DragCoefficient
        {
            get { return this.dragCoefficient; }
            set
            {
                this.dragCoefficient = value;
                for (int i = 0; i < bodies.Length; ++i)
                    bodies[i].DragCoefficient = value;
            }
        }

        private void SetBodiesCentroid(Vector2f centroid)
        {
            for (int i = 0; i < bodies.Length; ++i)
                bodies[i].Centroid = centroid;
        }


        public void Update(float dt)
        {
            for (int i = 0; i < bodies.Length; ++i)
                bodies[i].Update(dt);
        }

        public void ApplyImpulse(Vector2f J, Vector2f r)
        {
            for (int i = 0; i < bodies.Length; ++i)
                bodies[i].ApplyImpulse(J,r);
        }

        public void Pull(Vector2f n, float overlap)
        {
            for (int i = 0; i < bodies.Length; ++i)
                bodies[i].Pull(n,overlap);
        }

        public State[] Interpolation(float alpha)
        {
            State[] interpols = new State[bodies.Length];
            for (int i = 0; i < bodies.Length; ++i)
                interpols[i] = bodies[i].Interpolation(alpha);
            return interpols;
        }

    }
}

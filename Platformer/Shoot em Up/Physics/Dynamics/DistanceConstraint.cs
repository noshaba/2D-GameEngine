using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using Maths;

namespace Physics
{
    class DistanceConstraint : Constraint
    {
        private float distance;

        public DistanceConstraint(Body body1, Body body2, float distance)
            : base(body1, body2)
        {
            this.distance = distance;
        }

        public override void Solve(float dt)
        {
            Vector2f n = body2.COM - body1.COM;
            float currentDistance = n.Length();
            n /= currentDistance;

            float rv = (body2.Velocity - body1.Velocity).Dot(n);
            float rd = currentDistance - distance;

            float remove = rv + rd;
            float j = remove / (body1.InverseMass + body2.InverseMass);

            Vector2f J = j * n;

            ApplyLinearImpulse(J);
        }
    }
}

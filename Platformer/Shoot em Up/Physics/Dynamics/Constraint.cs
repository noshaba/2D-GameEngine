using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace Physics
{
    abstract class Constraint
    {
        protected Body body1;
        protected Body body2;

        public Constraint(Body body1, Body body2)
        {
            this.body1 = body1;
            this.body2 = body2;
            this.body1.isJoint = true;
            this.body2.isJoint = true;
        }

        public void ApplyImpulse(Vector2f J, Vector2f r1, Vector2f r2)
        {
            body1.ApplyImpulse(J, r1);
            body2.ApplyImpulse(-J, r2);
        }

        public void ApplyLinearImpulse(Vector2f J)
        {
            body1.ApplyLinearImpulse(J);
            body2.ApplyLinearImpulse(-J);
        }

        public abstract void Solve();
    }
}

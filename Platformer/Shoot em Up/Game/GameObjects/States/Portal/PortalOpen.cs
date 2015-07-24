using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics;

namespace Platformer
{
    class PortalOpen : AnimState
    {
        private new Portal owner;
        public PortalOpen(Portal owner) : base(owner)
        {
            this.sequence = new int[] { 3,3,4,4,5,5,4,4};
            this.owner = owner;
        }

        public override void HandleEvents()
        {
            if (owner.open)
            {
                foreach(Collision collision in owner.rigidBody.Collision)
                    if (collision.obj is Player)
                    {
                        owner.entered = true;
                        break;
                    }
            }
         /*   if (this.owner.open && this.owner.rigidBody.Collision.collision && this.owner.rigidBody.Collision.obj is Player)
            {
                this.owner.entered = true;
            }*/
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class PortalOpen : AnimState
    {
        private new Portal owner;
        public PortalOpen(Portal owner) : base(owner)
        {
            this.sequence = new int[] { 1,2};
            this.owner = owner;
        }

        public override void HandleEvents()
        {

            if (this.owner.open && this.owner.rigidBody.Collision.collision && this.owner.rigidBody.Collision.obj is Player)
            {
                this.owner.entered = true;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class PortalClosed : AnimState
    {
        private new Portal owner;
        public PortalClosed(Portal owner) : base(owner)
        {
            this.sequence = new int[] { 0,0,1,1,2,2,1,1};
            this.owner = owner;
        }

        public override void HandleEvents()
        {
            if (this.owner.open)
            {
                this.owner.currentState = new PortalOpen(this.owner);
            }
        }
    }
}

﻿using System;
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
            this.sequence = owner.openAnim;
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
        }
    }
}

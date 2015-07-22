﻿using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class PlayerJumpLeft : PlayerJump
    {
        public PlayerJumpLeft(GameObject owner) : base (owner)
        {
            this.sequence = new int[] {10};
            this.owner.rigidBody.Velocity = new Vector2f(-this.owner.speed, this.owner.rigidBody.Velocity.Y);
        }

        public override void HandleInput(Keyboard.Key k, bool pressed)
        {
            base.HandleInput(k, pressed);
            if (pressed)
            {
                switch (k)
                {
                    case Keyboard.Key.Right:
                        this.owner.currentState = new PlayerJumpRight(this.owner);
                        break;
                }
            }
        }

    }
}

using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class PlayerJump : AnimState
    {
        public PlayerJump(GameObject owner) : base (owner)
        {
            this.sequence = new int[] {4};
        }

        public override void HandleEvents()
        {
            base.HandleEvents();
            if (this.owner.rigidBody.Collision.Count > 0 )
                this.owner.currentState = new PlayerIdle(this.owner);
        }

        public override void HandleInput(Keyboard.Key k, bool pressed)
        {
            if(pressed) {
                switch (k)
                {
                    case Keyboard.Key.Left :
                        this.owner.currentState = new PlayerJumpLeft(this.owner);
                        break;
                    case Keyboard.Key.Right:
                        this.owner.currentState = new PlayerJumpRight(this.owner);
                        break;
                    case Keyboard.Key.Down:
                        this.owner.currentState = new PlayerShatter(this.owner);
                        break;
                }
            } 
        }
    }
}

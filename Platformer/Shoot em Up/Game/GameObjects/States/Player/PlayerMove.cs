using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class PlayerMove : AnimState
    {
        public PlayerMove(GameObject owner) : base (owner)
        {
        }

        public override void HandleInput(Keyboard.Key k, bool pressed)
        {
            if (pressed)
            {
                switch (k)
                {
                    //jump
                    case Keyboard.Key.Up:
                        this.owner.rigidBody.Velocity = new Vector2f(this.owner.rigidBody.Velocity.X, -this.owner.speed * 2);
                        this.owner.currentState = new PlayerJump(this.owner);
                        break;
                }
            }
            else
            {
                this.owner.currentState = new PlayerIdle(this.owner);
            }
        }
    }
}

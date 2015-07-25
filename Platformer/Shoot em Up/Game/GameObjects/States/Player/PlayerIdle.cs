using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class PlayerIdle : AnimState
    {
        public PlayerIdle(GameObject owner) : base (owner)
        {
            //hardcoded -> bleh
            this.sequence = new int[] { 0, 1, 2, 3, 4, 3, 2, 1 };
            this.owner.rigidBody.Velocity = new Vector2f(0, 0);
        }

        public override void HandleEvents()
        {
            base.HandleEvents();
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
                    //moveLeft
                    case Keyboard.Key.Left:
                        this.owner.currentState = new PlayerMoveLeft(this.owner);
                        break;
                    //moveRight
                    case Keyboard.Key.Right:
                        this.owner.currentState = new PlayerMoveRight(this.owner);
                        break;
                }
            }
        }
    }
}

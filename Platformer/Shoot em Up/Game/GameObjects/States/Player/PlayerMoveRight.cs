using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class PlayerMoveRight : PlayerMove
    {
        public PlayerMoveRight(GameObject owner) : base (owner)
        {
            this.sequence = new int[] { 5, 6, 7, 6 };
        }

        public override void HandleEvents()
        {
            base.HandleEvents();
            this.owner.rigidBody.Velocity = new Vector2f(this.owner.speed, this.owner.rigidBody.Velocity.Y);
        }

        public override void HandleInput(Keyboard.Key k, bool pressed)
        {
            base.HandleInput(k, pressed);
            switch (k)
            {
                case Keyboard.Key.Left:
                    this.owner.currentState = new PlayerMoveLeft(this.owner);
                    break;
            }
        }
    }
}

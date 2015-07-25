using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class PlayerMoveLeft : PlayerMove
    {
        public PlayerMoveLeft(GameObject owner) : base (owner)
        {
            //hardcoded -> bleh

            this.sequence = new int[] { 8, 9, 10, 9, 8 };
        }

        public override void HandleEvents()
        {
            base.HandleEvents();
            this.owner.rigidBody.Velocity = new Vector2f(-this.owner.speed, this.owner.rigidBody.Velocity.Y);
        }

        public override void HandleInput(Keyboard.Key k, bool pressed)
        {
            base.HandleInput(k, pressed);
            switch (k)
            {
                case Keyboard.Key.Right:
                    this.owner.currentState = new PlayerMoveRight(this.owner);
                    break;
            }
        }
    }
}

using Physics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class PlayerShatter : AnimState
    {
        public PlayerShatter(GameObject owner) : base (owner)
        {
            this.sequence = new int[] { 11 };
            this.owner.rigidBody.Velocity = new Vector2f(this.owner.rigidBody.Velocity.X, this.owner.speed * 10);
        }

        public override void HandleEvents()
        {
            if(owner.rigidBody.Collision.Count > 0)
                this.owner.currentState = new PlayerIdle(this.owner);

            foreach (Collision collision in owner.rigidBody.Collision)
            {
                Platform platform = collision.obj as Platform;
                if (platform != null)
                {
                    platform.Shatter();
                    break;
                }
            }
        }
    }
}

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
            if (this.owner.rigidBody.Collision.collision)
            {
                if (this.owner.rigidBody.Collision.obj is Platform || this.owner.rigidBody.Collision.obj is Body)
                {
                    //workaround
                    if (this.owner.rigidBody.Collision.obj is Body)
                    {
                        if ((this.owner.rigidBody.Collision.obj as Body).Parent is Platform)
                        {
                            //((this.owner.rigidBody.Collision.obj as Body).Parent as Platform).Shatter();
                        }
                    }
                    else if (this.owner.rigidBody.Collision.obj is Platform)
                    {
                        (this.owner.rigidBody.Collision.obj as Platform).Shatter();
                    }
                    this.owner.currentState = new PlayerIdle(this.owner);
                }
            }    
        }
    }
}

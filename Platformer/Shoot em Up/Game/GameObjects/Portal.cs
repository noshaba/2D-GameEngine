using Physics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class Portal : GameObject
    {
        public bool open;
        public bool entered;
        public Portal(Collision.Type type, int[] tileSize, int[] tileIndices, float density, int animationIndex, float restitution, float staticFriction, float kineticFriction, String texturePath, int[] spriteSize, Vector2f position, float rotation)
            : base(texturePath, tileSize, spriteSize, tileIndices, animationIndex, position, rotation, density)
        {
            RigidBodyParent = this;
            Rotateable = false;
            this.rigidBody.DragCoefficient = 1;
            this.rigidBody.Restitution = restitution;
            this.rigidBody.StaticFriction = staticFriction;
            this.rigidBody.KineticFriction = kineticFriction;
            
            this.currentState = new PortalClosed(this);
            this.animated = true;

            this.open = false;
            this.entered = false;
        }

        public void Open()
        {
            this.open = true;
        }

        public override void EarlyUpdate()
        {
            base.EarlyUpdate();
            this.UpdateBodies();
            this.rigidBody = this.rigidBodies[this.animationFrame];
            this.drawable = this.drawables[this.animationFrame];
        }
    }
}

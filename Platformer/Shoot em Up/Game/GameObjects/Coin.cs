using Physics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class Coin : KillableObject
    {
        public state status;
        public Coin(Collision.Type type, int[] tileSize, int[] tileIndices, float density, int animationIndex, float restitution, float staticFriction, float kineticFriction, String texturePath, int[] spriteSize, Vector2f position, float rotation, int health, int points, int dmg, Faction faction)
            : base(faction, texturePath, tileSize, spriteSize, tileIndices, animationIndex, position, rotation, density)
        {
            RigidBodyParent = this;
            Rotateable = false;
            this.rigidBody.DragCoefficient = 1;
            this.rigidBody.Restitution = restitution;
            this.rigidBody.StaticFriction = staticFriction;
            this.rigidBody.KineticFriction = kineticFriction;
            this.hp = health;
            this.damage = dmg;
            this.points = points;
            this.status = state.idle;
            this.states = new AnimState[] { new AnimState(new int[] { 0, 1, 2, 3,4,5 })};
            this.animated = true;
        }

        public enum state
        {
            idle
        }

        public override void EarlyUpdate()
        {
            base.EarlyUpdate();
            this.rigidBody = this.rigidBodies[this.animationFrame];
            this.drawable = this.drawables[this.animationFrame];

            if (rigidBody.Collision.collision && rigidBody.Collision.obj is Player) {
                this.hp = 0;
            }
        }
    }
}

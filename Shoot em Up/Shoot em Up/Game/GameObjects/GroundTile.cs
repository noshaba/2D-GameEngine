using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Shoot_em_Up
{
    class GroundTile : GameObject
    {
        public GroundTile(float restitution, float staticFriction, float kineticFriction, Texture texture, Vector2f position, float rotation)
            : base(texture, position, rotation)
        {
            this.rigidBody.Restitution = restitution;
            this.rigidBody.StaticFriction = staticFriction;
            this.rigidBody.KineticFriction = kineticFriction;
        }
    }
}

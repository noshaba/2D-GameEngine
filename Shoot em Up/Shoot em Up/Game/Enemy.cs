using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shoot_em_Up
{
    class Enemy : PvPObject
    {
         public Enemy(Faction faction, Vector2f position, Texture texture)
            : base(faction, texture, position, 1)
        {
            rigidBody.Restitution = 1.0f;
            this.hp = 1000;
            this.maxDamage = 0;
            this.maxPoints = 1000;
            this.drawable.Texture = texture;
        }
    }
}

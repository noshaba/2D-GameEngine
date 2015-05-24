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
            this.rigidBody.Velocity = new Vector2f(50, 0);
        }

         public void move()
         {
             if (this.rigidBody.COM.X >= 300 && this.rigidBody.Velocity.X > 0) {
                 this.rigidBody.Velocity = new Vector2f(-50,0);
             }
             else if (this.rigidBody.COM.X <= 100 )
             {
                this.rigidBody.Velocity = new Vector2f(50, 0);
             }
         }

         public override void Update()
         {
             this.move();
             base.Update();
         }
    }
}

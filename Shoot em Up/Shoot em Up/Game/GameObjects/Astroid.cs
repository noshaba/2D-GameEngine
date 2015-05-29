using Maths;
using Physics;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoot_em_Up
{
    class Astroid : PvPObject
    {
        public Astroid(Faction faction, Texture texture, Vector2f position, float rotation)
            : base(faction, texture, position, rotation, 0.01f)
        {
            rigidBody.Velocity = new Vector2f(EMath.random.Next(-50,50),EMath.random.Next(10,30));
            rigidBody.Restitution = 1.0f;
            this.hp = 70;
            this.maxDamage = 100;
            this.maxPoints = 20;
            this.drawable.Texture = texture;
            this.validateVelocity();
            Console.WriteLine(this.rigidBody.Velocity);
        }

        public void validateVelocity()
        {
            if(this.rigidBody.Velocity.X>0 && this.rigidBody.Velocity.Y >0) {
                this.rigidBody.Velocity = new Vector2f(this.rigidBody.Velocity.X, -this.rigidBody.Velocity.Y);
            }
        }
    }
}

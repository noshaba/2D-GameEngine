using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using Physics;
using Maths;
using SFML.System;

namespace Platformer
{
    class Bullet  : KillableObject
    {

        private Vector2f initPosition;
        private const float MAXVELOCITYSQ = 40000;
        private Vector2f bend;
        public KillableObject shooter;

        public Bullet(KillableObject shooter, int dmg, 
            string bulletPath, int[] spriteTileSize, int[] spriteSize, int[] tileIndices,
            int animationIndex, float density)
            : base(shooter.faction, dmg, 1, bulletPath, spriteTileSize, spriteSize, tileIndices,
            animationIndex, shooter.rigidBody.COM, 0, density)
        {
            this.RigidBodyParent = this;
            this.initPosition = shooter.rigidBody.COM;
            this.Restitution = 1.0f;
            this.shooter = shooter;
        }

        public Bullet(Bullet prototype, Vector2f position, Vector2f speed, Vector2f bend)
            : base(prototype as KillableObject)
        {
            this.RigidBodyParent = this;
            this.initPosition = position + speed + bend;
            this.rigidBody.COM = this.initPosition;
            this.rigidBody.Velocity = speed * 5;
            this.shooter = prototype.shooter;
            this.bend = bend;
        }

        //Faction faction, IRigidBody[] bodies, Vector2f position, float rotation
   /*     public Bullet(KillableObject shooter, Faction faction, Vector2f position, float radius, Color color, float density, float rotation, int dmg, Vector2f speed, Vector2f bend)
            : base(faction, new[] { new Circle(position + speed + bend, 0, radius, density) }, position + speed + bend, rotation)
        {
            RigidBodyParent = this;
            initPosition = position + speed + bend;
            drawable[0].FillColor = color;
            rigidBody.Restitution = 1.0f;
            rigidBody.Velocity = speed*5;
            this.shooter = shooter;
            this.hp = 1;
            this.maxHP = 1;
            this.damage = dmg;
            this.points = 0;
            this.bend = bend;

        }*/

        public override void EarlyUpdate()
        {
            base.EarlyUpdate();
            if (opponents.Count > 0)
            {
                this.hp = 0;
                this.alive = false;
            }
            foreach (KillableObject opponent in opponents)
            {
                if (opponent.hp <= 0 && opponent.alive)
                {
                    shooter.score += (100 - opponent.faction.Reputation[(int)this.faction.ID]) * opponent.points / 100;
                    opponent.alive = false;
                }
            }
        }

        public override void LateUpdate()
        {
            base.LateUpdate();
            rigidBody.Velocity += bend;
        }
    }
}
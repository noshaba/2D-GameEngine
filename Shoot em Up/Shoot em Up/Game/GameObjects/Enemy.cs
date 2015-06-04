using Maths;
using Physics;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shoot_em_Up
{
    class Enemy : KillableObject
    {

         public Collision.Type type;
         public int mPattern;
         public Weapon weapon;
         public Game.GameItem drop;
         private Vector2f initPos;
         private int speed;

       // string texturePath, int[] spriteTileSize, int[] spriteSize, int animationIndex, Vector2f position, float rotation, float density
         public Enemy(Collision.Type type, int[]ts, float density, float restitution, float staticFriction, float kineticFriction, String texture, int[]spriteSize, Vector2f position, int health, int points, int dmg, Faction faction, int pattern, WeaponContract w)
            : base(faction, texture, ts, spriteSize,0, position, 0, density)
         {
             this.initPos = position;
             this.rigidBody.Restitution = restitution;
             this.rigidBody.StaticFriction = staticFriction;
             this.rigidBody.KineticFriction = kineticFriction;
             this.hp = health;
             this.damage = dmg;
             this.points = points;
             this.type = type;
             this.speed = 10;
             this.rigidBody.Velocity = new Vector2f(0, this.speed);
             this.mPattern = pattern;
             this.drop = this.DetermineDrop();
             this.weapon = new Weapon(this, dmg, w.FireRate, 2, new Vector2f(1,0) , this.rigidBody.COM); 
         }

         private Game.GameItem DetermineDrop()
         {
             Game.GameItem i;
             int no = EMath.random.Next(1,100);
             if (no > 90)
             {
                 i = Game.GameItem.Weapon;
             }
             else if (no > 80)
             {
                 i = Game.GameItem.Bomb;
             }
             else if (no > 70)
             {
                 i = Game.GameItem.Heal;
             }
             else if (no > 60)
             {
                 i = Game.GameItem.Points;
             }
             else if (no > 50)
             {
                 i = Game.GameItem.NoPoints;
             }
             else {
                 i = Game.GameItem.None;
             }
             return i;
         }

         public void Move()
         {
             if (this.rigidBody.COM.Y >= this.initPos.Y + 30 && this.rigidBody.Velocity.Y > 0)
             {
                 this.rigidBody.Velocity = new Vector2f(0, -this.speed);
             }
             else if (this.rigidBody.COM.Y <= this.initPos.Y - 30)
             {
                 this.rigidBody.Velocity = new Vector2f(0,this.speed);
             }
         }


         public override void Update()
         {
             this.Move();
             base.Update();
         }
    }
}

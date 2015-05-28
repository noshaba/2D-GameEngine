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

        private Weapon weapon;
        public bool fire;
        private float speed;
        private Vector2f initPos;
        private delegate void Pattern();
        private Dictionary<string, Pattern> movements = new Dictionary<string, Pattern>();
        private String pattern;

         public Enemy(Faction faction, Vector2f position, Texture texture, int hp, int dmg, int speed, String pattern, Color color)
            : base(faction, texture, position, 1)
        {
            rigidBody.Restitution = 1.0f;
            this.initPos = position;
            this.hp = hp;
            this.maxHP = hp;
            this.maxDamage = dmg;
            this.maxPoints = hp*2;
            this.drawable.Texture = texture;
            this.speed = speed;
            this.rigidBody.Velocity = new Vector2f(speed,0);
            this.fire = true;
            this.weapon = new Weapon(this, 20, 1000, 30, "singleShot", new Vector2f(0, 1), new Vector2f(0, this.drawable.Texture.Size.Y/2), color);
            this.pattern = pattern;

            this.movements["stationary"] = stationary;
            this.movements["sideToSide"] = sideToSide;
         }

         public void move()
         {
             this.movements[pattern]();
         }

         private void shoot()
         {
             if (fire) this.weapon.shoot(this.rigidBody.COM);
         }

         public override void Update()
         {
             this.move();
             this.shoot();
             base.Update();
         }

         private void stationary()
         {
             this.rigidBody.Velocity = new Vector2f(0,0);
         }

         private void sideToSide()
         {
             if (this.rigidBody.COM.X >= this.initPos.X + 50 && this.rigidBody.Velocity.X > 0)
             {
                 this.rigidBody.Velocity = new Vector2f(-this.speed, 0);
             }
             else if (this.rigidBody.COM.X <= this.initPos.X - 50)
             {
                 this.rigidBody.Velocity = new Vector2f(this.speed, 0);
             }
         }
    }
}

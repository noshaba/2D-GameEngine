using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Physics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Diagnostics;

namespace Shoot_em_Up
{
    class Player : KillableObject
    {
        private float speed;
       // private Weapon weapon;
        public bool fire;
        Stopwatch clock;
        public String shieldStatus;
        private String texturePath;
        //string texturePath, int[] spriteTileSize, int[] spriteSize, int animationIndex, Vector2f position, float rotation
        public Player(Faction faction, Vector2f position, String texture, int[]tileSize, int[]spriteSize)
            : base(faction, texture+".png", tileSize, spriteSize, 0, position, 0, 0.9f)
        {
            this.rigidBody.Restitution = 1.0f;
            this.speed = 50;
            this.fire = false;
            this.score = 0;
            this.hp = 1000;
            this.maxHP = 1000;
            //this.drawable.Texture = texture;
            //this.weapon = new Weapon(this, 20, 500, 30, "singleShot", new Vector2f(0, -1), new Vector2f(0, -texture.Size.Y / 2), Color.Red);
            this.shield = false;
            this.maxShieldHp = 150;
            this.shieldHp = this.maxShieldHp;
            this.clock = new Stopwatch();
            this.shieldStatus = "sR";
            this.texturePath = texture;
            //this.bodies = new [] { this.rigidBody, new Circle(this.rigidBody.COM, this.drawable.Texture.Size.Y/2) };
            //checkShield();
        }

        public void Move(Keyboard.Key k)
        {
            /*
               switch (k)
               {
                   case Keyboard.Key.Right:
                       this.rigidBody.Velocity = new Vector2f(0, 0);
                       this.rigidBody.AngularVelocity = .5f;
                       break;
                   case Keyboard.Key.Left:
                       this.rigidBody.Velocity = new Vector2f(0, 0);
                       this.rigidBody.AngularVelocity = -.5f;
                       break;
                   case Keyboard.Key.Up:
                       this.rigidBody.Velocity = new Vector2f(0,-speed);
                       break;
                   case Keyboard.Key.Down:
                       this.rigidBody.Velocity = new Vector2f(0, speed);
                       break;
                    
               }*/
            this.rigidBody.AngularVelocity = 0;
            if (this.rigidBody.Orientation > 0)
                this.rigidBody.Orientation -= 0.01f;
            if (this.rigidBody.Orientation < 0)
                this.rigidBody.Orientation += 0.01f;

            if (k == Keyboard.Key.Right )
            {
                this.rigidBody.Velocity = new Vector2f(this.speed, 0);
                
            }
            else if (k == Keyboard.Key.Left)
            {
                this.rigidBody.Velocity = new Vector2f(-this.speed,0);
            }
            if (k == Keyboard.Key.Up)
            {
                this.rigidBody.Velocity = new Vector2f(this.rigidBody.Velocity.X, -this.speed);

            }
            else if (k == Keyboard.Key.Down)
            {
                this.rigidBody.Velocity = new Vector2f(this.rigidBody.Velocity.X, this.speed);

            }
            this.rigidBody.Velocity = this.rigidBody.WorldTransform * this.rigidBody.Velocity;
        }

        public void ToggleShield()
        {
            if (this.shieldHp > 0)
            {
                this.shield = !this.shield;
                this.CheckShield();
            }
        }

        public void CheckShield()
        {
            if (this.shield)
            {
                this.ShieldOn(this.rigidBody.Velocity, this.rigidBody.COM);
                this.drawable.Texture = new Texture(texturePath+"Shield.png");
            }
            else
            {
                this.ShieldOff(this.rigidBody.Velocity, this.rigidBody.COM);
                this.drawable.Texture = new Texture(texturePath+".png");
            }
        }

        public void Stop()
        {
            this.rigidBody.Velocity = new Vector2f(0, 0);
            this.rigidBody.AngularVelocity = 0;
        }

        private void Shoot()
        {
            //if (fire) this.weapon.shoot(this.rigidBody.COM);
        }


        public override void Update()
        {
            if (this.shieldHp <= 0 && this.shield)
            {
                this.shield = false;
                this.clock.Restart();
                this.shieldStatus = "sC";
            }
            if (this.clock.ElapsedMilliseconds / 1000 >= this.maxShieldHp / 10 && !this.shield)
            {
                this.shieldHp = this.maxShieldHp;
                this.clock.Stop();
                this.shieldStatus = "sR";
            }
            this.UpdateBodies();
            this.CheckShield();
            this.Shoot();
            base.Update();
        }
    }
}


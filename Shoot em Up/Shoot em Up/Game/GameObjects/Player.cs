using Physics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoot_em_Up
{
    class Player : PvPObject
    {
        private float speed;
        private Weapon weapon;
        public bool fire;

        public Player(Faction faction, Vector2f position, Texture texture)
            : base(faction, texture, position, 0, 0.9f)
        {
            this.rigidBody.Restitution = 1.0f;
            this.speed = 50;
            this.fire = false;
            this.score = 0;
            this.hp = 1000;
            this.maxHP = 1000;
            this.maxDamage = 0;
            this.maxPoints = 1000;
            this.weapon = new Weapon(this, 20, 500, 30, "tripleShot", new Vector2f(0,-1), new Vector2f(0, -40), Color.Red);
            this.drawable.Texture = texture;
            this.shield = false;
            //this.bodies = new [] { this.rigidBody, new Circle(this.rigidBody.COM, this.drawable.Texture.Size.Y/2) };
            //checkShield();
        }

        public void Move(Keyboard.Key k)
        {
         
            switch (k)
            {
                case Keyboard.Key.Right:
                    //this.rigidBody.Velocity = new Vector2f(this.speed, this.rigidBody.Velocity.Y);
                    this.rigidBody.Orientation += (float)Math.PI/50;
                    break;
                case Keyboard.Key.Left:
                    //this.rigidBody.Velocity = new Vector2f(-this.speed, this.rigidBody.Velocity.Y);
                    this.rigidBody.Orientation += (float)-Math.PI / 50;
                    break;
                case Keyboard.Key.Up:
                    this.rigidBody.Velocity = new Vector2f(0, - this.speed);
                    break;
                case Keyboard.Key.Down:
                    this.rigidBody.Velocity = new Vector2f(0, this.speed);
                    break;
                    
            }
            /*
            if (k == Keyboard.Key.Right)
            {
                this.rigidBody.Orientation += (float)Math.PI / 50;
            } else if(k == Keyboard.Key.Left) {
                this.rigidBody.Orientation += (float)-Math.PI / 50;
            }

            if(k == Keyboard.Key.Up) {
                this.rigidBody.Velocity = new Vector2f(0, -this.speed);
            } else if(k == Keyboard.Key.Down) {
                this.rigidBody.Velocity = new Vector2f(0, this.speed);
            }*/
            this.rigidBody.Velocity = this.Align(this.rigidBody.Velocity, this.rigidBody.Orientation);
        }

        public void ToggleShield()
        {
            this.shield = !this.shield;
            this.checkShield();
        }

        public void checkShield()
        {
            if (this.shield)
            {
                this.shieldOn(this.rigidBody.Velocity, this.rigidBody.COM);
                this.drawable.Texture = new Texture("../Content/ships/1Shield.png");
            }
            else
            {
                this.shieldOff(this.rigidBody.Velocity, this.rigidBody.COM);
                this.drawable.Texture = new Texture("../Content/ships/1.png");
            }
        }

        public void Stop()
        {
            this.rigidBody.Velocity = new Vector2f(0,0);
            this.rigidBody.AngularVelocity = 0;
        }

        private void shoot()
        {
            if(fire) this.weapon.shoot(this.rigidBody.COM);
        }

        public override void Update()
        {
            this.updateBodies();
            this.checkShield();
            this.shoot();
            base.Update();
        }
    }
}

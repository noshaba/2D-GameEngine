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

namespace Platformer
{
    class Player : KillableObject
    {
        private float speed;
       // private Weapon weapon;
        public bool fire;
        Stopwatch clock;
        public String shieldStatus;
        private String texturePath;
        public Weapon weapon;
        public int animationIndex;
        //string texturePath, int[] spriteTileSize, int[] spriteSize, int animationIndex, Vector2f position, float rotation
        public Player(Faction faction, Vector2f position, String texture, int[]tileSize, int[]spriteSize)
            : base(faction, texture+".png", tileSize, spriteSize, 0, new Vector2f(tileSize[0]*.5f, tileSize[1]*.5f), position, 0, 0.9f)
        {
            this.rigidBody.Restitution = 1.0f;
            this.speed = 80;
            this.fire = false;
            this.score = 0;
            this.hp = 1000;
            this.maxHP = 1000;
            //this.drawable.Texture = texture;
            //this.weapon = new Weapon("tripleShot", this, 20, 500, 60, new Vector2f(1, 0), new Vector2f(new Texture(texture+".png").Size.X / 2,0), Color.Red);
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
            this.rigidBody.AngularVelocity = 0;
            if (this.rigidBody.Orientation > 0)
                this.rigidBody.Orientation -= 0.01f;
            if (this.rigidBody.Orientation < 0)
                this.rigidBody.Orientation += 0.01f;

            if (k == Keyboard.Key.Right )
            {
                this.rigidBody.Velocity = new Vector2f(this.speed,0);
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

        public void Stop()
        {
            this.rigidBody.Velocity = new Vector2f(0, 0);
            this.rigidBody.AngularVelocity = 0;
        }

        private void Shoot()
        {
            if (fire) this.weapon.shoot(this.rigidBody.COM);

        }


        public override void Update()
        {
            /*if (this.animationIndex < 5)
            {
                this.animationIndex++;

            }
            else
            {
                this.animationIndex = 0;
            }*/
            base.Update();
            this.UpdateBodies();

            this.rigidBody = this.rigidBodies[this.animationIndex];
            this.drawable = this.drawables[this.animationIndex];
        }
    }
}


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
        public state status;

        //Faction faction, string texturePath, int[] spriteTileSize, int[] spriteSize, int[] tileIndices, int animationIndex, Vector2f position, float rotation, float density
        public Player(Faction faction, Vector2f position, String texture, int[]tileSize, int[]spriteSize, int[]tileIndices)
            : base(faction, texture+".png", tileSize, spriteSize,tileIndices, 0, position, 0, 0.1f)
        {
            foreach (Body body in rigidBodies)
            {
                body.Restitution = 0;
                body.rotateable = false;
                body.Parent = this;
            }
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
            this.status = state.idle;
            this.states = new AnimState[] { new AnimState(new int[] { 1, 1, 1, 1, 1, 1, 1, 1 }), new AnimState(new int[] { 0, 1, 2, 3, 4, 5, 6, 7 }), new AnimState(new int[] { 0, 1, 2, 3, 4, 5, 6, 7 }), new AnimState(new int[] { 0, 1, 2, 3, 4, 5, 6, 7 }), new AnimState(new int[] { 0, 1, 2, 3, 4, 5, 6, 7 }) };
            //this.bodies = new [] { this.rigidBody, new Circle(this.rigidBody.COM, this.drawable.Texture.Size.Y/2) };
            //checkShield();
        }

        public enum state
        {
            runLeft, runRight, jump, jumpStart, idle
        }

        public void Move(Keyboard.Key k)
        {
            if(this.status == state.idle) {
                switch (k)
                {
                    case Keyboard.Key.Left: status = state.runLeft;
                        break;
                    case Keyboard.Key.Right: status = state.runRight;
                        break;
                    case Keyboard.Key.Up: status = state.jumpStart;
                        break;
                }
            } else if(this.status == state.runLeft) {
                switch (k)
                {
                    case Keyboard.Key.Right: status = state.runRight;
                        break;
                    case Keyboard.Key.Up: status = state.jumpStart;
                        break;
                }
            } else if(this.status == state.runRight) {
                switch (k)
                {
                    case Keyboard.Key.Left: status = state.runLeft;
                        break;
                    case Keyboard.Key.Up: status = state.jumpStart;
                        break;
                }
            }
            if (k == Keyboard.Key.Left)
            {
                this.rigidBody.Velocity = new Vector2f(-this.speed, this.rigidBody.Velocity.Y);
            } else if(k == Keyboard.Key.Right) {
                this.rigidBody.Velocity = new Vector2f(this.speed,this.rigidBody.Velocity.Y);
            }
        }

        public void Release(Keyboard.Key k)
        {
            if (status != state.jump && status != state.jumpStart)
            {
                switch (k)
                {
                    case Keyboard.Key.Left: status = state.idle;
                        break;
                    case Keyboard.Key.Right: status = state.idle;
                        break;
                }
            }
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
        //TO-DO move to gameObject later so enemies can use this too



        public override void EarlyUpdate()
        {
            
            base.EarlyUpdate();
            this.UpdateBodies();
            switch (this.status)
            {
                case state.idle: this.rigidBody.Velocity = new Vector2f(0,this.rigidBody.Velocity.Y);
                    break;
                case state.runRight: this.rigidBody.Velocity = new Vector2f(this.speed,this.rigidBody.Velocity.Y);
                    break;
                case state.runLeft: this.rigidBody.Velocity = new Vector2f(-this.speed,this.rigidBody.Velocity.Y);
                    break;
                case state.jumpStart: this.rigidBody.Velocity = new Vector2f(this.rigidBody.Velocity.X, -this.speed ); status = state.jump;
                    break;
                case state.jump:
                    if (rigidBody.Collision.collision)
                    {
                        status = state.idle;
                        break;
                    }
                    this.animationFrame = 2; 
                    break;
            }
            this.rigidBody = this.rigidBodies[this.animationFrame];
            this.drawable = this.drawables[this.animationFrame];
        }
    }
}


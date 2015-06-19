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
        private state status;
        protected AnimState[] states;
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
            this.status = state.idle;
            this.states = new AnimState[]{new AnimState(6,10), new AnimState(0,5), new AnimState(2,2), new AnimState(2,2), new AnimState(0,0)};
            //this.bodies = new [] { this.rigidBody, new Circle(this.rigidBody.COM, this.drawable.Texture.Size.Y/2) };
            //checkShield();
        }

        public enum state
        {
            runLeft, runRight, jump, jumpStart, idle
        }

        public void Move(Keyboard.Key k)
        {
            /*this.rigidBody.AngularVelocity = 0;
            if (this.rigidBody.Orientation > 0)
                this.rigidBody.Orientation -= 0.01f;
            if (this.rigidBody.Orientation < 0)
                this.rigidBody.Orientation += 0.01f;

            if (k == Keyboard.Key.Right )
            {
                this.rigidBody.Velocity = new Vector2f(this.speed, this.rigidBody.Velocity.Y);
            }
            else if (k == Keyboard.Key.Left)
            {
                this.rigidBody.Velocity = new Vector2f(-this.speed, this.rigidBody.Velocity.Y);
            }
            if (k == Keyboard.Key.Up)
            {
                this.rigidBody.Velocity = new Vector2f(this.rigidBody.Velocity.X, -this.speed/2);

            }
            this.rigidBody.Velocity = this.rigidBody.WorldTransform * this.rigidBody.Velocity;

            */
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
        public void AdvanceAnim()
        {
            if (this.animationIndex <= this.states[(int)status].max && this.animationIndex >= this.states[(int)status].min)
            {
                if (this.animationIndex < this.states[(int)status].max)
                {
                    this.animationIndex++;
                }
                else
                {
                    this.animationIndex = this.states[(int)status].min;
                }
            }
            else 
            {
                this.animationIndex = this.states[(int)status].min; 
            }
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
            switch (this.status)
            {
                case state.idle: this.rigidBody.Velocity = new Vector2f(0,this.rigidBody.Velocity.Y);
                    break;
                case state.runRight: this.rigidBody.Velocity = new Vector2f(this.speed,this.rigidBody.Velocity.Y);
                    break;
                case state.runLeft: this.rigidBody.Velocity = new Vector2f(-this.speed,this.rigidBody.Velocity.Y);
                    break;
                case state.jumpStart: this.rigidBody.Velocity = new Vector2f(this.rigidBody.Velocity.X, -this.speed / 2); status = state.jump;
                    break;
                case state.jump: 
                    if (rigidBody.Collision.collision) 
                        status = state.idle; 
                    this.animationIndex = 2; 
                    this.rigidBody.AngularVelocity = 0;
                    break;
            }
            this.rigidBody = this.rigidBodies[this.animationIndex];
            this.drawable = this.drawables[this.animationIndex];
        }
    }
}


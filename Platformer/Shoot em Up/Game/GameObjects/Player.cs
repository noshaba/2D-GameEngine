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
            this.states = new AnimState[] { new AnimState(new int[] { 8, 9, 10, 9, 8 }), new AnimState(new int[] { 5, 6, 7, 6 }), new AnimState(new int[] { 4 }), new AnimState(new int[] { 10 }), new AnimState(new int[] { 7 }), new AnimState(new int[] { 0, 1, 2, 3, 4, 3, 2, 1 }), new AnimState(new int[] { 4 }) };
            //this.bodies = new [] { this.rigidBody, new Circle(this.rigidBody.COM, this.drawable.Texture.Size.Y/2) };
            //checkShield();
            this.animated = true;
        }

        public enum state
        {
            runLeft, runRight, jump, jumpLeft, jumpRight, idle, shatter
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
                    case Keyboard.Key.Up: this.rigidBody.Velocity = new Vector2f(this.rigidBody.Velocity.X, -this.speed); status = state.jump;
                        break;
                }
            } else if(this.status == state.runLeft) {
                switch (k)
                {
                    case Keyboard.Key.Right: status = state.runRight;
                        break;
                    case Keyboard.Key.Up: this.rigidBody.Velocity = new Vector2f(this.rigidBody.Velocity.X, -this.speed); status = state.jumpLeft;
                        break;
                }
            } else if(this.status == state.runRight) {
                switch (k)
                {
                    case Keyboard.Key.Left: status = state.runLeft;
                        break;
                    case Keyboard.Key.Up: this.rigidBody.Velocity = new Vector2f(this.rigidBody.Velocity.X, -this.speed); status = state.jumpRight;
                        break;
                }
            }
            if (k == Keyboard.Key.Left)
            {
                if (this.status == state.jump || this.status == state.jumpLeft || this.status == state.jumpRight)
                    status = state.jumpLeft;
                this.rigidBody.Velocity = new Vector2f(-this.speed, this.rigidBody.Velocity.Y);
            } else if(k == Keyboard.Key.Right) {
                if (this.status == state.jump || this.status == state.jumpLeft || this.status == state.jumpRight)
                    status = state.jumpRight;
                this.rigidBody.Velocity = new Vector2f(this.speed,this.rigidBody.Velocity.Y);
            }
            if (k == Keyboard.Key.Down)
            {
                if (status == state.jump || status == state.jumpLeft || status == state.jumpRight)
                {
                    this.rigidBody.Velocity = new Vector2f(0, this.speed*2);
                    status = state.shatter;
                }
            }
        }

        public void Release(Keyboard.Key k)
        {
            if (status != state.jump && status != state.jumpLeft && status != state.jumpRight)
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
            }
            if(status == state.jump || status == state.jumpLeft || status == state.jumpRight) {
                if (rigidBody.Collision.collision)
                    {
                        status = state.idle;
                    }
            }
            if(status == state.shatter ) {
                if (rigidBody.Collision.obj is Platform)
                    (rigidBody.Collision.obj as Platform).Shatter();
                status = state.idle;
            }
            this.rigidBody = this.rigidBodies[this.animationFrame];
            this.drawable = this.drawables[this.animationFrame];
        }
    }
}


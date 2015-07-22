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
       // private Weapon weapon;
        public bool fire;
        Stopwatch clock;
        public String shieldStatus;
        private String texturePath;
        public Weapon weapon;
        

        //Faction faction, string texturePath, int[] spriteTileSize, int[] spriteSize, int[] tileIndices, int animationIndex, Vector2f position, float rotation, float density
        public Player(Faction faction, Vector2f position, String texture, int[]tileSize, int[]spriteSize, int[]tileIndices)
            : base(faction, texture+".png", tileSize, spriteSize,tileIndices, 0, position, 0, 0.01f)
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
            //this.states = new AnimState[] { new PlayerIdle(new int[] { 0, 1, 2, 3, 4, 3, 2, 1 }, this), new AnimState(new int[] { 5, 6, 7, 6 }, this), new AnimState(new int[] { 4 }, this), new AnimState(new int[] { 10 }, this), new AnimState(new int[] { 7 }, this), new AnimState(new int[] { 8, 9, 10, 9, 8 }, this), new AnimState(new int[] { 11 }, this) };
            this.currentState = new PlayerIdle( this);
            //this.bodies = new [] { this.rigidBody, new Circle(this.rigidBody.COM, this.drawable.Texture.Size.Y/2) };
            //checkShield();
            this.animated = true;
        }

       /* public enum state
        {
            runLeft, runRight, jump, jumpLeft, jumpRight, idle, shatter
        }*/

        public void Move(Keyboard.Key k)
        {
            this.currentState.HandleInput(k, true);
        }

        public void Release(Keyboard.Key k)
        {
            this.currentState.HandleInput(k, false);
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

            this.rigidBody = this.rigidBodies[this.animationFrame];
            this.drawable = this.drawables[this.animationFrame];
        }
    }
}


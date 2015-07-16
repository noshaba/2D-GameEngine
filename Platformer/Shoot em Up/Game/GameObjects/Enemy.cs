using Maths;
using Physics;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    class Enemy : KillableObject
    {

        public Collision.Type type;
        public int mPattern;
        public Weapon weapon;
        public Game.GameItem drop;
        private Vector2f initPos;
        private int speed;
        private bool fire;
        public state status;
        private int attentionRange;

        //Faction faction, string texturePath, int[] spriteTileSize, int[] spriteSize, int[] tileIndices, int animationIndex, Vector2f position, float rotation, float density
        public Enemy(Collision.Type type, int[] tileSize, int[] tileIndices,float density, int animationIndex, float restitution, float staticFriction, float kineticFriction, String texturePath, int[]spriteSize, Vector2f position, float rotation, int health, int points, int dmg, Faction faction, int pattern, WeaponContract w)
            : base(faction, texturePath, tileSize, spriteSize, tileIndices, animationIndex, position, rotation, density)
        {
            RigidBodyParent = this;
            this.initPos = position;
            this.rigidBody.DragCoefficient = 1;
            this.rigidBody.Restitution = restitution;
            this.rigidBody.StaticFriction = staticFriction;
            this.rigidBody.KineticFriction = kineticFriction;
            this.hp = health;
            this.damage = dmg;
            this.points = points;
            this.type = type;
            this.speed = -20;
            this.rigidBody.Velocity = new Vector2f(0, this.speed);
            this.mPattern = pattern;
            this.drop = this.DetermineDrop();
            this.weapon = new Weapon("singleShot", this, dmg, 2000, 60, new Vector2f(-1, 0), new Vector2f(-new Texture(texturePath).Size.X/2, 0), Color.Magenta);
            this.fire = true;
        }


        public Enemy(Collision.Type type, int[] tileSize, int[] tileIndices, float density, int animationIndex, float restitution, float staticFriction, float kineticFriction, String texturePath, int[] spriteSize, Vector2f position, float rotation, int health, int points, int dmg, Faction faction)
            : base(faction, texturePath, tileSize, spriteSize, tileIndices, animationIndex, position, rotation, density)
        {
            RigidBodyParent = this;
            this.initPos = position;
            this.rigidBody.DragCoefficient = 1;
            this.rigidBody.Restitution = restitution;
            this.rigidBody.StaticFriction = staticFriction;
            this.rigidBody.KineticFriction = kineticFriction;
            this.hp = health;
            this.damage = dmg;
            this.points = points;
            this.type = type;
            this.speed = -20;
            this.rigidBody.Velocity = new Vector2f(0, this.speed);
            this.drop = this.DetermineDrop();
            this.fire = false;
            this.status = state.sleep;
            this.states = new AnimState[] { new AnimState(new int[] { 4, 5, 6, 5 }), new AnimState(new int[] { 0 }), new AnimState(new int[] { 0, 1, 2, 3, 4 }), new AnimState(new int[] { 4, 5, 6, 5 }) };
            this.attentionRange = 400; //move to json
            this.animated = true;
        }

        public enum state {
            observe, sleep, awake, attack
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
            }             else if (no > 60)
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
            switch (status) { 
                case state.observe : 
                    if (IsPlayerClose())
                    {
                        this.status = state.attack;
                    }
                    else {
                        this.Observe();
                    }
                    break;
                case state.sleep:
                    if (IsPlayerNear())
                    {
                        this.status = state.awake;
                    }
                    break;
                case state.awake:
                    if (this.animationFrame == this.states[(int)status].sequence[this.states[(int)status].sequence.Length - 1]) {
                        this.status = state.observe;
                    }
                    break;
                case state.attack:
                    if (!IsPlayerNear())
                    {
                        this.status = state.observe;
                    }
                    else {
                        this.Chase();
                    }
                    break;
            }
        }

        private void Observe() { 
        }

        private void Chase() {
            if (Game.playerPos.X >= this.rigidBody.COM.X)
                this.rigidBody.Velocity = new Vector2f(-this.speed, 0);
            else if (Game.playerPos.X <= this.rigidBody.COM.X)
                this.rigidBody.Velocity = new Vector2f(this.speed,0);
        }

        private void Shoot()
        {
            if (fire) this.weapon.shoot(this.rigidBody.COM); 
        }

        public override void EarlyUpdate()
        {
            this.Move();
            this.Shoot();
            base.EarlyUpdate();
            this.UpdateBodies();
            this.rigidBody = this.rigidBodies[this.animationFrame];
            this.drawable = this.drawables[this.animationFrame];
        }

        private bool IsPlayerNear() {
            return Game.playerPos.X >= this.rigidBody.COM.X - this.attentionRange && Game.playerPos.X <= this.rigidBody.COM.X + attentionRange;
        }


        private bool IsPlayerClose() {
            return Game.playerPos.X >= this.rigidBody.COM.X - this.attentionRange/2 && Game.playerPos.X <= this.rigidBody.COM.X + attentionRange/2;
        }
    }
}

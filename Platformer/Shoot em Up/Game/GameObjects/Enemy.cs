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
        private bool fire;
        private int attentionRange;
        public int[][] animations;

        //Faction faction, string texturePath, int[] spriteTileSize, int[] spriteSize, int[] tileIndices, int animationIndex, Vector2f position, float rotation, float density
       /* public Enemy(Collision.Type type,  int[] tileSize, int[] tileIndices,float density, int animationIndex, float restitution, float staticFriction, float kineticFriction, String texturePath, int[]spriteSize, Vector2f position, float rotation, int health, int points, int dmg, Faction faction, int pattern, WeaponContract w)
            : base(faction, texturePath, tileSize, spriteSize, tileIndices, animationIndex, position, rotation, density)
        {
            RigidBodyParent = this;
            Rotateable = false;
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
        }*/


        public Enemy(Collision.Type type, int attentionRange, int[][] animation, int[] tileSize, int[] tileIndices, float density, int animationIndex, float restitution, float staticFriction, float kineticFriction, String texturePath, int[] spriteSize, Vector2f position, float rotation, int health, int points, int dmg, Faction faction)
            : base(faction, texturePath, tileSize, spriteSize, tileIndices, animationIndex, position, rotation, density)
        {
            RigidBodyParent = this;
            Rotateable = false;
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

            this.drop = this.DetermineDrop();
            this.fire = false;

            this.attentionRange = attentionRange;
            this.animations = animation;
            this.animated = true;
            this.animationFrame = 0;
            this.currentState = new EnemySleep(this);
        }

        public enum animType
        {
            sleep, awake, observe, attack
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

        public void Attack() {
            if (Game.playerPos.X >= this.rigidBody.COM.X)
                this.rigidBody.Velocity = new Vector2f(-this.speed, this.rigidBody.Velocity.Y);
            else if (Game.playerPos.X <= this.rigidBody.COM.X)
                this.rigidBody.Velocity = new Vector2f(this.speed, this.rigidBody.Velocity.Y);
        }

        private void Shoot()
        {
            if (fire) this.weapon.shoot(this.rigidBody.COM); 
        }

        public override void EarlyUpdate()
        {
            base.EarlyUpdate();
            this.UpdateBodies();
            this.rigidBody = this.rigidBodies[this.animationFrame];
            this.drawable = this.drawables[this.animationFrame];
        }

        public bool IsPlayerNear() {
            return Game.playerPos.X >= this.rigidBody.COM.X - this.attentionRange && Game.playerPos.X <= this.rigidBody.COM.X + attentionRange && Game.playerPos.Y >= this.rigidBody.COM.Y - this.attentionRange && Game.playerPos.Y <= this.rigidBody.COM.Y + attentionRange;
        }


        public bool IsPlayerClose() {
            return Game.playerPos.X >= this.rigidBody.COM.X - this.attentionRange/2 && Game.playerPos.X <= this.rigidBody.COM.X + attentionRange/2 && Game.playerPos.Y >= this.rigidBody.COM.Y - this.attentionRange/2 && Game.playerPos.Y <= this.rigidBody.COM.Y + attentionRange/2;
        }
    }
}

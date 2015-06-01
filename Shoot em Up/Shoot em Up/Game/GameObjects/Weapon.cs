using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Shoot_em_Up
{
    class Weapon : RectangleShape
    {
        private int bulletSpeed;
        private Stopwatch charge;
        private int fireRate;
        private int damage;
        public string type;
        private Vector2f direction;
        private Vector2f relativePos;

        private delegate void Shot(Vector2f position, Vector2f direction);
        private KillableObject owner;
        private Color color;

        public Weapon(KillableObject owner, int damage, int fireRate, int bulletSpeed, Vector2f direction, Vector2f pos, Texture texture)
            : base(pos)
        {
            this.fireRate = fireRate;
            this.bulletSpeed = bulletSpeed;
            this.damage = damage;
            this.charge = new Stopwatch();
            this.charge.Start();
            this.owner = owner;
            this.direction = direction;
            this.relativePos = pos;
            this.Size = new Vector2f(texture.Size.X, texture.Size.Y);
            this.Texture = texture;
        }

        public void shoot(Vector2f position)
        {
            //this.relativePos = this.owner.rigidBody.WorldTransform * this.relativePos;
            //this.direction = this.owner.rigidBody.WorldTransform * this.direction;
            if (this.charge.ElapsedMilliseconds > this.fireRate)
            {
                //this.weapons[type](this.owner.rigidBody.WorldTransform * this.relativePos + position, this.owner.rigidBody.WorldTransform * this.direction * bulletSpeed);
                this.charge.Restart();
            }
        }
        /*
        private void singleShot(Vector2f position, Vector2f direction)
        {
            Game.Add(new Bullet(this.owner, this.owner.Faction, position, 2, this.color, 0.5f, maxDamage, direction, new Vector2f(0, 0)));
        }

        private void tripleShot(Vector2f position, Vector2f direction)
        {
            Game.Add(new Bullet(this.owner, this.owner.Faction, position, 1.5f, this.color, 0.5f, maxDamage, this.owner.rigidBody.WorldTransform * new Vector2f(this.direction.X * bulletSpeed - 10, this.direction.Y * bulletSpeed), new Vector2f(0, 0)));
            Game.Add(new Bullet(this.owner, this.owner.Faction, position, 1.5f, this.color, 0.5f, maxDamage, this.owner.rigidBody.WorldTransform * new Vector2f(this.direction.X * bulletSpeed, this.direction.Y * bulletSpeed), new Vector2f(0, 0)));
            Game.Add(new Bullet(this.owner, this.owner.Faction, position, 1.5f, this.color, 0.5f, maxDamage, this.owner.rigidBody.WorldTransform * new Vector2f(this.direction.X * bulletSpeed + 10, this.direction.Y * bulletSpeed), new Vector2f(0, 0)));
        }

        private void tripleBentShot(Vector2f position, Vector2f direction)
        {
            Game.Add(new Bullet(this.owner, this.owner.Faction, position, 1.5f, this.color, 0.5f, maxDamage, this.owner.rigidBody.WorldTransform * new Vector2f(this.direction.X - 5, this.direction.Y * bulletSpeed * 2), new Vector2f(-1, 0)));
            Game.Add(new Bullet(this.owner, this.owner.Faction, position, 1.5f, this.color, 0.5f, maxDamage, this.owner.rigidBody.WorldTransform * new Vector2f(this.direction.X * bulletSpeed, this.direction.Y * bulletSpeed), new Vector2f(0, 0)));
            Game.Add(new Bullet(this.owner, this.owner.Faction, position, 1.5f, this.color, 0.5f, maxDamage, this.owner.rigidBody.WorldTransform * new Vector2f(this.direction.X + 5, this.direction.Y * bulletSpeed * 2), new Vector2f(1, 0)));
        }*/
    }
}

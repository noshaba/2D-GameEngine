using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Platformer
{
    class Weapon 
    {
        private int bulletSpeed;
        private Stopwatch charge;
        private int fireRate;
        public int damage;
        public string type;
        private Vector2f direction;
        private Vector2f relativePos;
        private Dictionary<string, Shot> weapons;

        private delegate void Shot(Vector2f position, Vector2f direction);
        private KillableObject owner;
        private Color color;

        public Weapon(KillableObject owner, int damage, int fireRate, int bulletSpeed, Vector2f direction, Vector2f pos)
        {
            this.fireRate = fireRate;
            this.bulletSpeed = bulletSpeed;
            this.damage = damage;
            this.type = "singleShot";
            this.weapons = new Dictionary<string, Shot>();
            this.weapons["singleShot"] = singleShot;
            this.weapons["tripleShot"] = tripleShot;
            this.weapons["tripleBentShot"] = tripleBentShot;
            this.charge = new Stopwatch();
            this.charge.Start();
            this.owner = owner;
            this.direction = direction;
            this.relativePos = pos;
        }

        public Weapon(String type, KillableObject owner, int damage, int fireRate, int bulletSpeed, Vector2f direction, Vector2f pos, Color c)
        {
            this.fireRate = fireRate;
            this.bulletSpeed = bulletSpeed;
            this.damage = damage;
            this.type = type;
            this.weapons = new Dictionary<string, Shot>();
            this.weapons["singleShot"] = singleShot;
            this.weapons["tripleShot"] = tripleShot;
            this.weapons["tripleBentShot"] = tripleBentShot;
            this.charge = new Stopwatch();
            this.charge.Start();
            this.owner = owner;
            this.direction = direction;
            this.relativePos = pos;
            this.color = c;
        }

        public void shoot(Vector2f position)
        {
            //this.relativePos = this.owner.rigidBody.WorldTransform * this.relativePos;
            //this.direction = this.owner.rigidBody.WorldTransform * this.direction;
            if (this.charge.ElapsedMilliseconds > this.fireRate)
            {
                this.weapons[this.type](this.owner.rigidBody.WorldTransform * this.relativePos + position, this.owner.rigidBody.WorldTransform * this.direction * bulletSpeed);
                this.charge.Restart();
            }
        }
        
        private void singleShot(Vector2f position, Vector2f direction)
        {
         //   Game.Add(new Bullet(this.owner, this.owner.faction, position, 2, this.color, 0.5f, this.damage, direction, new Vector2f(0, 0)));
        }

        private void tripleShot(Vector2f position, Vector2f direction)
        {
         //   Game.Add(new Bullet(this.owner, this.owner.faction, position, 1.5f, this.color, 0.5f, this.damage, this.owner.rigidBody.WorldTransform * new Vector2f(this.direction.X * bulletSpeed, this.direction.Y * bulletSpeed - 10), new Vector2f(0, 0)));
         //   Game.Add(new Bullet(this.owner, this.owner.faction, position, 1.5f, this.color, 0.5f, this.damage, this.owner.rigidBody.WorldTransform * new Vector2f(this.direction.X * bulletSpeed, this.direction.Y * bulletSpeed), new Vector2f(0, 0)));
         //   Game.Add(new Bullet(this.owner, this.owner.faction, position, 1.5f, this.color, 0.5f, this.damage, this.owner.rigidBody.WorldTransform * new Vector2f(this.direction.X * bulletSpeed, this.direction.Y * bulletSpeed + 10), new Vector2f(0, 0)));
        }

        private void tripleBentShot(Vector2f position, Vector2f direction)
        {
         //   Game.Add(new Bullet(this.owner, this.owner.faction, position, 1.5f, this.color, 0.5f, this.damage, this.owner.rigidBody.WorldTransform * new Vector2f(this.direction.X * bulletSpeed * 2, this.direction.Y - 5), new Vector2f(0, -1)));
         //   Game.Add(new Bullet(this.owner, this.owner.faction, position, 1.5f, this.color, 0.5f, this.damage, this.owner.rigidBody.WorldTransform * new Vector2f(this.direction.X * bulletSpeed, this.direction.Y * bulletSpeed), new Vector2f(0, 0)));
         //   Game.Add(new Bullet(this.owner, this.owner.faction, position, 1.5f, this.color, 0.5f, this.damage, this.owner.rigidBody.WorldTransform * new Vector2f(this.direction.X * bulletSpeed * 2, this.direction.Y + 5), new Vector2f(0, 1)));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SFML.System;
using SFML.Graphics;

namespace Shoot_em_Up
{
    class Weapon
    {
        private int bulletSpeed;
        private Stopwatch charge;
        private int fireRate;
        private int maxDamage;
        public string type;
        private Vector2f direction;
        private Vector2f relativePos;

        private delegate void Shot(Vector2f position);
        private Dictionary<string, Shot> weapons;
        private PvPObject owner;
        private Color color;

        public Weapon(PvPObject owner, int maxDamage, int fireRate, int bulletSpeed, String type, Vector2f direction, Vector2f pos, Color color) {
            this.fireRate = fireRate;
            this.bulletSpeed = bulletSpeed;
            this.maxDamage = maxDamage;
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
            this.color = color;
        }

        public void shoot(Vector2f position) {
            if (this.charge.ElapsedMilliseconds > this.fireRate) {
                this.weapons[type](position+relativePos);
                this.charge.Restart();
            }
        }

        private void singleShot(Vector2f position) {
            Game.AddObject(new Bullet(this.owner, this.owner.Faction, position, 2, this.color, 0.5f, maxDamage, new Vector2f(this.direction.X*bulletSpeed, this.direction.Y*bulletSpeed), new Vector2f(0, 0)));
        }

        private void tripleShot(Vector2f position)
        {
            Game.AddObject(new Bullet(this.owner, this.owner.Faction, position, 1.5f, this.color, 0.5f, maxDamage, new Vector2f(this.direction.X * bulletSpeed - 10, this.direction.Y * bulletSpeed), new Vector2f(0, 0)));
            Game.AddObject(new Bullet(this.owner, this.owner.Faction, position, 1.5f, this.color, 0.5f, maxDamage, new Vector2f(this.direction.X * bulletSpeed, this.direction.Y * bulletSpeed), new Vector2f(0, 0)));
            Game.AddObject(new Bullet(this.owner, this.owner.Faction, position, 1.5f, this.color, 0.5f, maxDamage, new Vector2f(this.direction.X * bulletSpeed + 10, this.direction.Y * bulletSpeed), new Vector2f(0, 0)));
        }

        private void tripleBentShot(Vector2f position)
        {
            Game.AddObject(new Bullet(this.owner, this.owner.Faction, position, 1.5f, this.color, 0.5f, maxDamage, new Vector2f(this.direction.X - 5, this.direction.Y * bulletSpeed * 2), new Vector2f(-1, 0)));
            Game.AddObject(new Bullet(this.owner, this.owner.Faction, position, 1.5f, this.color, 0.5f, maxDamage, new Vector2f(this.direction.X * bulletSpeed, this.direction.Y * bulletSpeed), new Vector2f(0, 0)));
            Game.AddObject(new Bullet(this.owner, this.owner.Faction, position, 1.5f, this.color, 0.5f, maxDamage, new Vector2f(this.direction.X + 5, this.direction.Y * bulletSpeed * 2), new Vector2f(1, 0)));
        }
    }
}

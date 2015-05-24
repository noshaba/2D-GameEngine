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

        private delegate void Shot(Vector2f position);
        private Dictionary<string, Shot> weapons;
        private Player owner;

        public Weapon(Player owner, int maxDamage, int fireRate, int bulletSpeed, String type) {
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
        }

        public void shoot(Vector2f position) {
            if (this.charge.ElapsedMilliseconds > this.fireRate) {
                this.weapons[type](position);
                this.charge.Restart();
            }
        }

        private void singleShot(Vector2f position) {
            Game.AddObject(new Bullet(this.owner, FactionManager.factions[(int)Faction.Type.Player], position, 2, Color.Yellow, 0.01f, maxDamage, new Vector2f(0, -bulletSpeed), new Vector2f(0, 0)));
        }

        private void tripleShot(Vector2f position)
        {
            Game.AddObject(new Bullet(this.owner, FactionManager.factions[(int)Faction.Type.Player], position, 1.5f, Color.Yellow, 0.01f, maxDamage, new Vector2f(-10, -bulletSpeed), new Vector2f(0, 0)));
            Game.AddObject(new Bullet(this.owner, FactionManager.factions[(int)Faction.Type.Player], position, 1.5f, Color.Yellow, 0.01f, maxDamage, new Vector2f(0, -bulletSpeed), new Vector2f(0, 0)));
            Game.AddObject(new Bullet(this.owner, FactionManager.factions[(int)Faction.Type.Player], position, 1.5f, Color.Yellow, 0.01f, maxDamage, new Vector2f( 10, -bulletSpeed), new Vector2f(0, 0)));
        }

        private void tripleBentShot(Vector2f position)
        {
            Game.AddObject(new Bullet(this.owner, FactionManager.factions[(int)Faction.Type.Player], position, 1.5f, Color.Yellow, 0.01f, maxDamage, new Vector2f(-5, -bulletSpeed*2), new Vector2f(-1, 0)));
            Game.AddObject(new Bullet(this.owner, FactionManager.factions[(int)Faction.Type.Player], position, 1.5f, Color.Yellow, 0.01f, maxDamage, new Vector2f( 0, -bulletSpeed  ), new Vector2f( 0, 0)));
            Game.AddObject(new Bullet(this.owner, FactionManager.factions[(int)Faction.Type.Player], position, 1.5f, Color.Yellow, 0.01f, maxDamage, new Vector2f( 5, -bulletSpeed*2), new Vector2f( 1, 0)));
        }
    }
}

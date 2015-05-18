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
        private Vector2f bulletSpeed;
        private Vector2f bulletBend;
        private Stopwatch charge;
        private int fireRate;
        private int maxDamage;
        public string type;

        private delegate void Shot(Vector2f position);
        private Dictionary<string, Shot> weapons;

        public Weapon(int maxDamage, int fireRate, Vector2f bulletSpeed, Vector2f bulletBend) {
            this.fireRate = fireRate;
            this.bulletSpeed = bulletSpeed;
            this.bulletBend = bulletBend;
            this.maxDamage = maxDamage;
            this.type = "singleShot";
            this.weapons = new Dictionary<string, Shot>();
            this.weapons["singleShot"] = singleShot;
            this.weapons["tripleShot"] = tripleShot;
            this.charge = new Stopwatch();
            this.charge.Start();
        }

        public void shoot(Vector2f position) {
            if (this.charge.ElapsedMilliseconds > this.fireRate) {
                this.weapons[type](position);
                this.charge.Restart();
            }
        }

        private void singleShot(Vector2f position) {
            Game.AddObject(new Bullet(FactionManager.factions[(int)Faction.Type.Player], position + bulletSpeed, 2, Color.Yellow, 0.01f, new Vector2f(0, 0), maxDamage));
        }

        private void tripleShot(Vector2f position)
        {
            Game.AddObject(new Bullet(FactionManager.factions[(int)Faction.Type.Player], position + bulletSpeed, 2, Color.Yellow, 0.01f, new Vector2f(-2, 0), maxDamage));
            Game.AddObject(new Bullet(FactionManager.factions[(int)Faction.Type.Player], position + bulletSpeed, 2, Color.Yellow, 0.01f, new Vector2f(0, 0), maxDamage));
            Game.AddObject(new Bullet(FactionManager.factions[(int)Faction.Type.Player], position + bulletSpeed, 2, Color.Yellow, 0.01f, new Vector2f(2, 0), maxDamage));
        }
    }
}

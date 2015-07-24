using Physics;
using Maths;
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
        private Stopwatch charge;
        private int fireRate;
        private Vector2f direction;
        private Vector2f relativePos;
        private KillableObject owner;
        private BulletShot[] shots;
        private int bulletLifeTime;

        public Weapon(KillableObject owner, int fireRate, Vector2f direction, Vector2f relativePos, BulletShot[] shots, 
            int bulletLifeTime)
        {
            this.owner = owner;
            this.fireRate = fireRate;
            this.charge = new Stopwatch();
            this.charge.Start();
            this.direction = direction;
            this.relativePos = relativePos;
            this.shots = shots;
            this.bulletLifeTime = bulletLifeTime;
            this.charge.Start();
        }

        public void Shoot()
        {
            if (this.charge.ElapsedMilliseconds > this.fireRate)
            {
                Vector2f position = this.relativePos + this.owner.rigidBody.COM;
                foreach(BulletShot shot in shots)
                    Game.Add(new Bullet(this.owner, shot.Damage, shot.Radius, shot.Density, position, 
                        direction.ElemMul(shot.Speed).Add(shot.Offset), new Vector2f(shot.Bend[0], shot.Bend[1]), 
                        new Color(shot.FillColor[0], shot.FillColor[1], shot.FillColor[2]), shot.OutlineThickness,
                        new Color(shot.OutlineColor[0], shot.OutlineColor[1], shot.OutlineColor[2]), this.bulletLifeTime));
                this.charge.Restart();
            }
        }
    }
         
}

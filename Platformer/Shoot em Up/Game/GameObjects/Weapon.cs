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
        private KillableObject owner;
        private BulletShot[] shots;
        private int bulletLifeTime;
     //   public Vector2f direction;

        public Weapon(KillableObject owner, int fireRate, BulletShot[] shots, int bulletLifeTime)
        {
            this.owner = owner;
            this.fireRate = fireRate;
            this.charge = new Stopwatch();
            this.charge.Start();
          //  this.direction = new Vector2f(-1, 0);
            this.shots = shots;
            this.bulletLifeTime = bulletLifeTime;
            this.charge.Start();
        }

        public void Shoot(Vector2f direction)
        {
            if (this.charge.ElapsedMilliseconds > this.fireRate)
            {
                Vector2f position = this.owner.rigidBody.Radius * direction + this.owner.rigidBody.COM;
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

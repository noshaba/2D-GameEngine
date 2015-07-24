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
        private int damage;
        private Vector2f direction;
        private Vector2f relativePos;
        private KillableObject owner;
        private BulletShot[] shots;
        private Bullet[,] cylinder;


        public Weapon(KillableObject owner, int dmg, string bulletPath, int[] spriteTileSize, int[] spriteSize, int[] tileIndices,
            int animationIndex, float bulletDensity, int fireRate, Vector2f direction, Vector2f relativePos, BulletShot[] shots,
            int cylinderSize)
        {
            this.owner = owner;
            this.fireRate = fireRate;
            this.damage = dmg;
            this.charge = new Stopwatch();
            this.charge.Start();
            this.direction = direction;
            this.relativePos = relativePos;
            this.shots = shots;
            this.cylinder = new Bullet[cylinderSize, shots.Length];
            for (int i = 0; i < cylinderSize; ++i) for (int j = 0; j < shots.Length; ++j)
                {
                    Vector2f position = this.relativePos + this.owner.rigidBody.COM;
                    cylinder[i, j] = new Bullet(owner, dmg, bulletPath, spriteTileSize, spriteSize, tileIndices,
                        animationIndex, bulletDensity, position, direction.ElemMul(shots[j].BulletSpeed).Add(shots[j].Offset), 
                        new Vector2f(shots[j].Bend[0], shots[j].Bend[1]));
                }
            this.charge.Start();
        }

        int i = 0;
        public void Shoot()
        {
            if (this.charge.ElapsedMilliseconds > this.fireRate)
            {
                for (int j = 0; j <= cylinder.GetUpperBound(1); ++j)
                {
                    Vector2f position = this.relativePos + this.owner.rigidBody.COM;
                    cylinder[i, j].Charge(position, this.direction.ElemMul(shots[j].BulletSpeed).Add(shots[j].Offset),
                        new Vector2f(shots[j].Bend[0], shots[j].Bend[1]));
                    Console.WriteLine(owner.rigidBody.COM);
                    Console.WriteLine(cylinder[i, j].rigidBody.COM);
                    Game.Add(cylinder[i, j]);
                }
                this.charge.Restart();
                i = (++i) % cylinder.Length;
                Console.WriteLine(i);
            }
        }
    }
         
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using Physics;
using Maths;
using SFML.System;

namespace Shoot_em_Up
{
    class Bullet : KillableObject
    {

        private Vector2f initPosition;
        private const float MAXVELOCITY2 = 40000;
        private Vector2f bend;
        private KillableObject shooter;

        public Bullet(KillableObject shooter, Faction faction, Vector2f position, float radius, Color color, float density, int dmg, Vector2f speed, Vector2f bend)
            : base(faction, position + speed + bend, 0, radius, density)
        {
            initPosition = position + speed + bend;
            (rigidBody as Circle).FillColor = color;
            rigidBody.Restitution = 1.0f;
            rigidBody.Velocity = speed*5;
            this.shooter = shooter;
            this.hp = 1;
            this.maxHP = 1;
            this.damage = dmg;
            this.points = 0;
            this.bend = bend;

        }

        public override void Update()
        {
            base.Update();
            if (rigidBody.Collision.collision)
            {
                //this.hp = 1;
                this.hp = 0;
                this.alive = false;
                if (opponent != null)
                {
                    if (opponent.hp <= 0 && opponent.alive)
                    {
                        shooter.score += (100 - opponent.faction.Reputation[(int)this.faction.ID]) * opponent.points / 100;
                        opponent.alive = false;
                    }
                }
            }
        }

        public override void LateUpdate()
        {
            base.LateUpdate();
            rigidBody.Velocity += bend;
        }
    }
}
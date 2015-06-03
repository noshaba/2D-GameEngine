﻿using Physics;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shoot_em_Up
{
    class Enemy : KillableObject
    {

         public Collision.Type type;
         public int mPattern;
         public Weapon weapon;

       // string texturePath, int[] spriteTileSize, int[] spriteSize, int animationIndex, Vector2f position, float rotation, float density
         public Enemy(Collision.Type type, int[]ts, float density, float restitution, float staticFriction, float kineticFriction, String texture, int[]spriteSize, Vector2f position, int health, int points, int dmg, Faction faction, int pattern, WeaponContract w)
            : base(faction, texture, ts, spriteSize,0, position, 0, density)
         {
             this.rigidBody.Restitution = restitution;
             this.rigidBody.StaticFriction = staticFriction;
             this.rigidBody.KineticFriction = kineticFriction;
             this.hp = health;
             this.damage = dmg;
             this.points = points;
             this.type = type;
             this.mPattern = pattern;
             this.weapon = new Weapon(this, dmg, w.FireRate, 2, new Vector2f(1,0) , this.rigidBody.COM); 
         }
    }
}
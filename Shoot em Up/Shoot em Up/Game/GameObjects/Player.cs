using Physics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoot_em_Up
{
    class Player : PvPObject
    {
        public int score;
        private float speed;
        private Weapon weapon;
        public bool fire;

        public Player(Faction faction, Vector2f position, Texture texture)
            : base(faction, texture, position, 1)
        {
            rigidBody.Restitution = 1.0f;
            this.speed = 25;
            this.fire = false;
            this.score = 0;
            this.hp = 1000;
            this.maxDamage = 0;
            this.maxPoints = 1000;
            this.weapon = new Weapon(this, 20, 500, 30, "singleShot");
            this.drawable.Texture = texture;
        }

        public void Move(Keyboard.Key k)
        {
            switch (k)
            {
                case Keyboard.Key.Right:
                    rigidBody.Velocity = new Vector2f(this.speed, rigidBody.Velocity.Y);
                    break;
                case Keyboard.Key.Left:
                    rigidBody.Velocity = new Vector2f(-this.speed, rigidBody.Velocity.Y);
                    break;
                case Keyboard.Key.Up:
                    rigidBody.Velocity = new Vector2f(rigidBody.Velocity.X, -this.speed);
                    break;
                case Keyboard.Key.Down:
                    rigidBody.Velocity = new Vector2f(rigidBody.Velocity.X, this.speed);
                    break;
            }
        }

        public void Stop()
        {
            rigidBody.Velocity = new Vector2f(0,0);
        }

        private void shoot()
        {
            if(fire) this.weapon.shoot(new Vector2f(this.rigidBody.COM.X, this.rigidBody.COM.Y-20));
        }

        public override void Update()
        {
            this.shoot();
            base.Update();
        }
    }
}

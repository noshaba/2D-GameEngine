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
        public uint score;
        private Vector2f speed;
        private Weapon weapon;
        public bool fire;

        public Player(Faction faction, Vector2f position, float hw, float hh, Color color) : base(faction, Collision.Type.Polygon, position, 1, 0.01f) {
            (state as Polygon).SetBox(position, hw, hh, 0);
            (state as Polygon).FillColor = color;
            state.Restitution = 1.0f;
            this.speed = new Vector2f(50,0);
            this.fire = false;
            this.score = 0;
            this.hp = 1000;
            this.maxDamage = 0;
            this.maxPoints = 1000;
            this.weapon = new Weapon(20, 500, 30, "tripleBentShot");
            //this.shape.Texture = new Texture("../Content/ship.png");
        }

        public void Move(Keyboard.Key k)
        {
            switch (k)
            {
                case Keyboard.Key.Right:
                    state.Velocity = this.speed;
                    break;
                case Keyboard.Key.Left:
                    state.Velocity = -this.speed;
                    break;
            }
        }

        public void Stop()
        {
            state.Velocity = new Vector2f(0,0);
        }

        private void shoot()
        {
            if(fire) this.weapon.shoot(this.state.COM);
        }

        public override void Update()
        {
            this.shoot();
            base.Update();
        }

        public override void LateUpdate()
        {
            // TODO: an bullets anpassen...
            this.score += attacked != null ? (uint)((1 - attacked.Faction.Reputation[(int)this.Faction.ID]) * attacked.maxPoints) : 0;
            base.LateUpdate();
        }
    }
}

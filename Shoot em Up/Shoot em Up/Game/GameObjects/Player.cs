using Physics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoot_em_Up
{
    class Player : Polygon
    {
        public uint score;
        private Vector2f speed;

        public Player(Vector2f position, float hw, float hh, Color color) : base(position, 1, 0.01f) {
            SetBox(position, hw, hh, 0);
            FillColor = color;
            Restitution = 1.0f;
            this.speed = new Vector2f(25,0);
            this.score = 200;
        }

        public void Move(Keyboard.Key k)
        {
            switch (k)
            {
                case Keyboard.Key.Right:
                    Velocity = this.speed;
                    break;
                case Keyboard.Key.Left:
                    Velocity = -this.speed;
                    break;
            }
        }

        public void Stop()
        {
            Velocity = new Vector2f(0,0);
        }

        public override void ReactToCollision(Collision colliInfo)
        {
            //Console.WriteLine("Oh no the ship got hit!");
        }
    }
}

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
        private float speed;

        public Player(Vector2f position, float hw, float hh, Color color) : base(position, 1, 0.01f) {
            SetBox(position, hw, hh, 0);
            FillColor = color;
            this.speed = 25;
            this.score = 200;
        }

        public void move(Keyboard.Key k)
        {
            switch (k)
            {
                case Keyboard.Key.Right:
                    current.velocity = new Vector2f(this.speed,0);
                    break;
                case Keyboard.Key.Left:
                    current.velocity = new Vector2f(this.speed*-1, 0);
                    break;
            }
        }

        public override void reactToCollision(IShape obj)
        {
            Console.WriteLine("Oh no the ship got hit!");
        }
    }
}

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
    class Player : GameObject
    {
        public uint score;
        private Vector2f speed;

        public Player(Vector2f position, float hw, float hh, Color color) : base(Collision.Type.Polygon, position, 1, 0.01f) {
            (shape as Polygon).SetBox(position, hw, hh, 0);
            (shape as Polygon).FillColor = color;
            shape.Restitution = 1.0f;
            this.speed = new Vector2f(50,0);
            this.score = 200;
        }

        public void Move(Keyboard.Key k)
        {
            switch (k)
            {
                case Keyboard.Key.Right:
                    shape.Velocity = this.speed;
                    break;
                case Keyboard.Key.Left:
                    shape.Velocity = -this.speed;
                    break;
            }
        }

        public void Stop()
        {
            shape.Velocity = new Vector2f(0,0);
        }

    }
}

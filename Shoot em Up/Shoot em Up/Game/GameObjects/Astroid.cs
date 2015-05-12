using Maths;
using Physics;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoot_em_Up
{
    class Astroid : GameObject
    {
        public Astroid(int x, int y)
            : base(Collision.Type.Polygon, new Vector2f(x, y), 200, 0.01f)
        {

            (shape as Polygon).Texture = new Texture("../Content/astroid.png");
            shape.Velocity = new Vector2f(EMath.random.Next(-50,50),EMath.random.Next(10,30));
            shape.Restitution = 1.0f;
            this.hp = 100;
        }

        public override void Update()
        {
            
            if(shape.Collision.collision) {
                if (this.hp <= 0) {
                    this.alive = false;
                }
                if (this.hp <= 50) {
                    (this.shape as Polygon).FillColor = Color.Red;
                }
            }
            base.Update();
        }

    }
}

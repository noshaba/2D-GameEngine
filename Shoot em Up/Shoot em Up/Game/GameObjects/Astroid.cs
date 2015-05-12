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

            (state as Polygon).Texture = new Texture("../Content/astroid.png");
            state.Velocity = new Vector2f(EMath.random.Next(-50,50),EMath.random.Next(10,30));
            state.Restitution = 1.0f;
            this.hp = 100;
        }

        public override void Update()
        {
            this.alive = this.hp > 0;

            if (this.hp <= 50)
            {
                this.shape.FillColor = Color.Red;
            }
            if(state.Collision.collision) {
                if (this.state.Collision.obj.Parent is Bullet)
                {
                    this.hp -= (this.state.Collision.obj.Parent as Bullet).damage;
                    (this.state.Collision.obj.Parent as Bullet).alive = false;
                }
            }
            base.Update();
        }

    }
}

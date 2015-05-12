using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using Physics;
using Maths;
using SFML.System;

namespace Shoot_em_Up {
    class Ball : GameObject {

        public Collision.Type type = Collision.Type.Circle;

        private Vector2f initPosition;
        private const float MAXVELOCITY2 = 40000;
        private int damage = 20;

        public Ball(Vector2f position, float radius, Color color, float mass) : base(Collision.Type.Circle, position, radius, mass) {
            initPosition = position;
            (shape as Circle).FillColor = color;
            shape.Restitution = 1.0f;
            shape.Velocity = new Vector2f(0,-50);
        }

        public void Reset() {
            shape.Current.Reset();
            shape.Current.position = initPosition;
            shape.Previous = shape.Current;
        }

        public void Impulse() {
            shape.Current.velocity = new Vector2f(-50, 50);
        }

        public void IncreaseVelocity(float dt) {
            if (shape.Current.velocity.Length2() < MAXVELOCITY2)
                shape.Current.velocity += dt * shape.Current.velocity.Norm() * 50;
        }

        public override void Update()
        {
            if (shape.Collision.collision)
            {
                if (this.shape.Collision.obj.Parent is Astroid)
                {
                    (this.shape.Collision.obj.Parent as Astroid).hp -= this.damage;
                    this.alive = false;
                }
            }
            base.Update();
        }
    }
}

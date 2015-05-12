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
        public int damage = 20;

        public Ball(Vector2f position, float radius, Color color, float mass) : base(Collision.Type.Circle, position, radius, mass) {
            initPosition = position;
            (state as Circle).FillColor = color;
            state.Restitution = 1.0f;
            state.Velocity = new Vector2f(0,-50);
        }

        public void Reset() {
            state.Current.Reset();
            state.Current.position = initPosition;
            state.Previous = state.Current;
        }

        public void Impulse() {
            state.Current.velocity = new Vector2f(-50, 50);
        }

        public void IncreaseVelocity(float dt) {
            if (state.Current.velocity.Length2() < MAXVELOCITY2)
                state.Current.velocity += dt * state.Current.velocity.Norm() * 50;
        }

        public override void Update()
        {
            if (state.Collision.collision)
            {
                this.alive = false;
                if (this.state.Collision.obj.Parent is Astroid)
                {
                    (this.state.Collision.obj.Parent as Astroid).hp -= this.damage;
                }
            }
            base.Update();
        }
    }
}

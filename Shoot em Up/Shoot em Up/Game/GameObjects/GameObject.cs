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
    class GameObject
    {
        public IState state;
        public Shape shape;
        public bool alive = true;
        public int hp;

        public GameObject(IState state)
        {
            this.state = state;
            this.shape = state as Shape;
            this.state.Parent = this; 
        }
        //change name of "radius"
        public GameObject(Collision.Type type, Vector2f position, float var, float density)
        {
            switch (type)
            {
                case Collision.Type.Circle: 
                    this.state = new Circle(position, var, density);
                    this.shape = this.state as Shape;
                    break;
                case Collision.Type.Polygon:
                    this.state = new Polygon(position, var, density);
                    this.shape = this.state as Shape;
                    break;
            }
            this.state.Parent = this;
        }

        public GameObject(Vector2f normal, Vector2f position, Vector2f size, float rotation)
        {
            this.state = new Plane(normal, position, size, rotation);
            this.shape = this.state as Shape;
            this.state.Parent = this;
        }

        public virtual void Update() {
            state.Collision.collision = false;
        }
    }
}

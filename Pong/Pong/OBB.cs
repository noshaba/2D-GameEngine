using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Pong{
    abstract class OBB : RectangleShape {
        public Collision.Type type = Collision.Type.OBB;

        public List<Vector2f> axis = new List<Vector2f>();
        public List<float> hl = new List<float>(); // half lengths of axis'

        protected State previous;
        protected State current;

        public OBB(Vector2f position, Vector2f size) : base(size) {
            Origin = new Vector2f(size.X * 0.5f, size.Y * 0.5f);
            Position = position;
            axis.Add(new Vector2f(1,0));
            axis.Add(new Vector2f(0,1));
            hl.Add(size.X * 0.5f);
            hl.Add(size.Y * 0.5f);
        }

        public float Width {
            get { return Size.X; }
            set { Size = new Vector2f(value, Size.Y); }
        }

        public float Height {
            get { return Size.Y; }
            set { Size = new Vector2f(Size.X, value); }
        }

        public Vector2f Momentum {
            get { return current.momentum; }
            set { current.momentum = value; }
        }

        public void Update(float dt) {
            previous = current;
            current.Integrate(dt);
        }
    }
}

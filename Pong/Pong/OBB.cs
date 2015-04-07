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

        public Vector2f[] axis = { new Vector2f(1,0), new Vector2f(0,1) };
        public float[] hl = new float[2]; // half lengths of axis'

        protected State previous;
        protected State current;

        public OBB(Vector2f position, Vector2f size) : base(size) {
            Origin = new Vector2f(size.X * 0.5f, size.Y * 0.5f);
            Position = position;
            hl[0] = size.X * 0.5f;
            hl[1] = size.Y * 0.5f;
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

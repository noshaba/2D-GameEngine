using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Pong {
    class Paddle : RectangleShape {
        #region Constructors
        public Paddle() : base() { }
        public Paddle(RectangleShape copy) : base(copy) { }
        public Paddle(Vector2f size) : base(size) {
            Origin = new Vector2f(size.X * 0.5f, size.Y * 0.5f);
        }
        public Paddle(Vector2f size, Color color) : base(size) {
            Origin = new Vector2f(size.X * 0.5f, size.Y * 0.5f);
            FillColor = color;
        }
        public Paddle(Vector2f position, Vector2f size) : base(size) {
            Origin = new Vector2f(size.X * 0.5f, size.Y * 0.5f);
            Position = position;
        }
        public Paddle(Vector2f position, Vector2f size, Color color) : base(size) {
            Origin = new Vector2f(size.X * 0.5f, size.Y * 0.5f);
            Position = position;
            FillColor = color;
        }
        #endregion

        public float Width {
            get { return Size.X; }
            set { Size = new Vector2f(value, Size.Y); }
        }

        public float Height {
            get { return Size.Y; }
            set { Size = new Vector2f(Size.X, value); }
        }

        public void move(float y) { 
            Position = new Vector2f(Position.X, y);
        }
    }
}

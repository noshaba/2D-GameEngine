using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;

namespace Pong {
    class Paddle : RectangleShape {
        #region Constructors
        public Paddle() : base() { }
        public Paddle(RectangleShape copy) : base(copy) { }
        public Paddle(Vector2f size) : base(size) {
            base.Origin = new Vector2f(size.X * 0.5f, size.Y * 0.5f);
        }
        public Paddle(Vector2f size, Color color) : base(size) {
            base.Origin = new Vector2f(size.X * 0.5f, size.Y * 0.5f);
            base.FillColor = color;
        }
        public Paddle(Vector2f position, Vector2f size) : base(size) {
            base.Origin = new Vector2f(size.X * 0.5f, size.Y * 0.5f);
            base.Position = position;
        }
        public Paddle(Vector2f position, Vector2f size, Color color) : base(size) {
            base.Origin = new Vector2f(size.X * 0.5f, size.Y * 0.5f);
            base.Position = position;
            base.FillColor = color;
        }
        #endregion
    }
}

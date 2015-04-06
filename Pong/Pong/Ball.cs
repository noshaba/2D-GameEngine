using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Pong {
    class Ball : CircleShape {
        struct State {
            public Vector2f position;
            public Vector2f momentum;
            public Vector2f velocity;
        }
        struct Derivative {
            public Vector2f velocity;
            public Vector2f force;

        }

        public State state;

        #region Constructors
        public Ball() : base() {
            state = new State();
            state.position = base.Position;
            state.velocity = new Vector2f(0,0);
        }
        public Ball(CircleShape copy) : base(copy) { }
        public Ball(float radius) : base(radius) {
            Origin = new Vector2f(Position.X + radius * 0.5f, Position.Y + radius * 0.5f);
        }
        public Ball(float radius, Color color) : base(radius) {
            Origin = new Vector2f(Position.X + radius * 0.5f, Position.Y + radius * 0.5f);
            FillColor = color;
        }
        public Ball(float radius, uint pointCount) : base(radius, pointCount) {
            Origin = new Vector2f(Position.X + radius * 0.5f, Position.Y + radius * 0.5f);
        }
        public Ball(float radius, Color color, uint pointCount) : base(radius, pointCount) {
            Origin = new Vector2f(Position.X + radius * 0.5f, Position.Y + radius * 0.5f);
            FillColor = color;
        }
        public Ball(Vector2f position, float radius) : base(radius) {
            Origin = new Vector2f(Position.X + radius * 0.5f, Position.Y + radius * 0.5f);
            Position = position;
        }
        public Ball(Vector2f position, float radius, uint pointCount) : base(radius, pointCount) {
            Origin = new Vector2f(Position.X + radius * 0.5f, Position.Y + radius * 0.5f);
            Position = position;
        }
        public Ball(Vector2f position, float radius, Color color) : base(radius) {
            Origin = new Vector2f(Position.X + radius * 0.5f, Position.Y + radius * 0.5f);
            Position = position;
            FillColor = color;
        }
        public Ball(Vector2f position, float radius, Color color, uint pointCount) : base(radius, pointCount) {
            Origin = new Vector2f(Position.X + radius * 0.5f, Position.Y + radius * 0.5f);
            Position = position;
            FillColor = color;
        }
        #endregion
    }
}

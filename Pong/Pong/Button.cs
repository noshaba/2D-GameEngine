using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;

namespace Pong {
    // TODO: change rectangleshape to sprite later..
    class Button : RectangleShape, IGraphic {
        public enum Status {
            Pressed, Released, OnHover
        }
        public delegate void ActionListener();
        public Status status;
        public ActionListener listener;
        private bool displayed;
        private IGraphic parentView;

        public Button(Vector2f position, Vector2f size, Color color, ActionListener listener) : base(size) {
            this.listener = listener;
            Position = position;
            FillColor = color;
            status = Status.Released;
            displayed = true;
        }

        public IGraphic ParentView {
            get { return parentView; }
            set { parentView = value; }
        }

        public bool Displayed {
            get { return displayed; }
            set { displayed = value; }
        }

        // for sprite
        // public Button(Vector2f position, string file) : base() {  
        // }

        public void Draw(RenderWindow window) {
            if (displayed)
                window.Draw(this);
        }

        public void Released(float X, float Y) {
            if (displayed && GetGlobalBounds().Contains(X, Y)) {
                status = Status.Released;
                if (listener != null)
                    listener();
            }
        }

        public void Pressed(float X, float Y) {
            if (displayed && GetGlobalBounds().Contains(X, Y))
                status = Status.Pressed;
        }

        public void OnHover(float X, float Y) {
            if (displayed && GetGlobalBounds().Contains(X, Y))
                status = Status.OnHover;
        }
    }
}

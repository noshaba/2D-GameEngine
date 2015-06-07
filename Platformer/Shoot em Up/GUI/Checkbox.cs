using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace GUI {
    class Checkbox : RectangleShape, IGraphic {
        public enum Status {
            Pressed, Released, OnHover
        }
        public bool check;
        public Status status;
        private bool displayed;
        private IGraphic parentView;

        public Checkbox(Vector2f position, Vector2f size, Color color) : base(size) {
            Position = position;
            FillColor = color;
            check = false;
            status = Status.Released;
            displayed = true;
            parentView = null;
        }

        public Checkbox(Vector2f position, Vector2f size, Color color, bool check) : base(size) {
            this.check = check;
            status = Status.Released;
            Position = position;
            FillColor = color;
            displayed = true;
            parentView = null;
        }

        public IGraphic ParentView {
            get { return parentView; }
            set { parentView = value; }
        }

        public bool Displayed {
            get { return displayed; }
            set { displayed = value; }
        }

        public void Draw(RenderWindow window) {
            if (displayed)
                window.Draw(this);
        }

        public void Released(float X, float Y) {
            if (displayed && GetGlobalBounds().Contains(X, Y)) {
                status = Status.Released;
                check = !check;
            }
        }

        public void Pressed(float X, float Y) {
            if (displayed && GetGlobalBounds().Contains(X, Y)) {
                status = Status.Pressed;
            }
        }

        public void OnHover(float X, float Y) {
            if (displayed) {
                if (GetGlobalBounds().Contains(X, Y))
                    status = Status.OnHover;
                else
                    status = Status.Released;
            }
        }

    }
}

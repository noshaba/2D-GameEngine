using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace Shoot_em_Up {
    // TODO: change rectangleshape to sprite later..
    class Button : GUIElement, IGraphic {
        public enum Status {
            Pressed, Released, OnHover
        }
        public delegate void ActionListener();
        public Status status;
        public ActionListener listener;
        private bool displayed;
        private IGraphic parentView;

        public Button(Vector2f position, Vector2f size, ActionListener listener) : base((uint)position.X, (uint)position.Y, (uint)size.X, (uint)size.Y, "../Content/ButtonActive.png") {
            this.listener = listener;
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
            if (displayed) {
                if (GetGlobalBounds().Contains(X, Y))
                    status = Status.OnHover;
                else
                    status = Status.Released;
            }
        }
    }
}

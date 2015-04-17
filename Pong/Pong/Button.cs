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
        public string name;
        public enum Status {
            Pressed, Released, OnHover
        }
        public Status status;
        private bool displayed;

        public Button(string name, Vector2f position, Vector2f size, Color color) : base(size) {
            this.name = name;
            Position = position;
            FillColor = color;
            status = Status.Released;
            displayed = true;
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
            if (displayed && GetGlobalBounds().Contains(X, Y)){
                Console.WriteLine(name);
            }
        }

        public void Pressed(float X, float Y) {
        }
    }
}

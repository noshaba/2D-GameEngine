using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Pong {
    class Checkbox : RectangleShape, IGraphic {
        public string name;
        public bool check;
        private bool displayed;

        public Checkbox(string name, Vector2f position, Vector2f size, Color color) : base(size) {
            this.name = name;
            Position = position;
            FillColor = color;
            check = false;
            displayed = true;
        }

        public Checkbox(string name, Vector2f position, Vector2f size, Color color, bool check) : base(size) {
            this.name = name;
            this.check = check;
            Position = position;
            FillColor = color;
            displayed = true;
        }

        public bool Displayed {
            get { return displayed; }
            set { displayed = value; }
        }

        public void Draw(RenderWindow window) {
            if (displayed)
                window.Draw(this);
        }
    }
}

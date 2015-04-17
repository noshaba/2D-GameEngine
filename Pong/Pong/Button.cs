using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;

namespace Pong {
    // TODO: change rectangleshape to sprite later..
    class Button : RectangleShape {
        public string name;

        public Button(string name, Vector2f position, Vector2f size, Color color) : base(size) {
            this.name = name;
            Position = position;
            FillColor = color;
        }

        // for sprite
        // public Button(Vector2f position, string file) : base() {  
        // }
    }
}

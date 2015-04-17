using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;

namespace Pong {
    class GUI : View {
        Button options;

        public GUI(int width, int height) : base(new Vector2f(0, 0), new Vector2f(width, height)) {
            options = new Button(new Vector2f(0, 0), new Vector2f(50, 50), Color.Blue, DisplayOptions);
            Add(options);
        }

        private void DisplayOptions() {
            Console.WriteLine("lol");
        }
    }
}

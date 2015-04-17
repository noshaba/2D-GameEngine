using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;

namespace Pong {
    class HUD {
        private List<Button> buttons = new List<Button>();

        public HUD() {
            buttons.Add(new Button("Options", new Vector2f(0, 0), new Vector2f(50, 50), Color.Blue));
        }

        public void ClickButton(float X, float Y) {
            foreach (Button button in buttons) {
                if (button.GetGlobalBounds().Contains(X, Y))
                    Console.WriteLine(button.name);
            }
        }

        public void Draw(RenderWindow window){
            foreach (Button button in buttons)
                window.Draw(button);
        }
    }
}

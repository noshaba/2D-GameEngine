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
        Button sound;
        Menue opsMenue;
        Game game;

        public GUI(int width, int height, Game g) : base(new Vector2f(0, 0), new Vector2f(width, height)) {
            game = g;
            options = new Button(new Vector2f(0, 0), new Vector2f(50, 50), Color.Cyan, DisplayOptions);
            opsMenue = new Menue(new Vector2f(100, 100), new Vector2f(300, 200), Color.Red);
            sound = new Button(new Vector2f(110, 110), new Vector2f(70, 50), Color.Cyan, ToggleSound);
            Add(options);
            Add(opsMenue);
            opsMenue.Add(sound);
        }

        private void DisplayOptions() {
            if (opsMenue.Displayed)
            {
                opsMenue.Displayed = false;
            }
            else
            {
                opsMenue.Displayed = true;
            }
        }

        private void ToggleSound()
        {
            game.setSound();
        }
    }
}

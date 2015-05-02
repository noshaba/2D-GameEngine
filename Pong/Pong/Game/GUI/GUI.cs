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
        Button restart;
        GUIElement test;
        Menue opsMenue;
        Game game;

        public GUI(int width, int height, Game g) : base(new Vector2f(0, 0), new Vector2f(width, height)) {
            game = g;
            options = new Button(new Vector2f(0, 0), new Vector2f(80, 80), DisplayOptions);
            opsMenue = new Menue(new Vector2f(100, 100), new Vector2f(300, 200));
            restart = new Button(new Vector2f(110, 110), new Vector2f(150, 100), StartNewGame);
            Add(options);
            Add(opsMenue);
            opsMenue.Add(restart);
        }

        private void DisplayOptions() {
            opsMenue.Displayed = !opsMenue.Displayed;
        }



        private void StartNewGame() {
            this.game.Restart();
        }
    }
}

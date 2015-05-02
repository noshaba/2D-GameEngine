using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace Shoot_em_Up {
    class GUI : View {
        Menue menue;
        Button start;
        Game game;

        public GUI(int width, int height, Game g) : base(new Vector2f(0, 0), new Vector2f(width, height)) {
            game = g;
            menue = new Menue(new Vector2f(0,0), new Vector2f(480, 20));
            menue.Displayed = true;
            start = new Button(new Vector2f(10,10), new Vector2f(30,10), startGame);
            menue.Add(start);
            Add(menue);
        }

        private void startGame()
        {
            Console.WriteLine("start Game");
        }

    }
}

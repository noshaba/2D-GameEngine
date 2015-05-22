using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using GUI;

namespace Shoot_em_Up {
    class GUI : GUIView {
        Menue menue;
        Button start;
        Label label;
        Game game;
        private List<IGraphic> welcome = new List<IGraphic>();
        private List<IGraphic> inGame = new List<IGraphic>();

        public GUI(int width, int height, Game g) : base(new Vector2f(0, 0), new Vector2f(width, height)) {
            game = g;
            menue = new Menue(new Vector2f(0,0), new Vector2f(480, 60));
            menue.Displayed = true;
            start = new Button(10, 10, "Start Game!", StartGame);
            menue.Add(start);
            this.welcome.Add(menue);
            this.children = this.welcome;
        }

        private void StartGame()
        {
            game.StartGame();
        }

        public void Draw(RenderWindow window)
        {
            //decide which elements to draw depending on game status
            switch(this.game.status) {
                case Game.GameStatus.Active: this.children = this.inGame;
                    break;
                case Game.GameStatus.Welcome: this.children = this.welcome;
                    break;
            }
            base.Draw(window);
        }
    }
}

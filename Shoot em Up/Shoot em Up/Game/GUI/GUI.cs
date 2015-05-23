﻿using System;
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
        Button start;
        Button restart;
        Button toMain;
        Menue menue;
        Screen title;
        Game game;
        private List<IGraphic> welcome = new List<IGraphic>();
        private List<IGraphic> inGame = new List<IGraphic>();
        private List<IGraphic> credits = new List<IGraphic>();

        public GUI(int width, int height, Game g) : base(new Vector2f(0, 0), new Vector2f(width, height)) {
            game = g;

            menue = new Menue(new Vector2f(0,0), new Vector2f(480,50));
            toMain = new Button(10,10, "Main Menue", ShowWelcome);
            menue.Add(toMain);
            this.inGame.Add(menue);

            start = new Button(new Vector2f(150, 350), new Vector2f(200,50), "Start Game!", 24, StartGame);
            title = new Screen(0,0,"../Content/title.png");
            this.welcome.Add(title);
            this.welcome.Add(start);

            restart = new Button(new Vector2f(150, 350), new Vector2f(200, 50), "Play Again!", 24, StartGame);
            this.credits.Add(restart);

            this.children = this.welcome;
        }

        private void StartGame()
        {
            game.StartGame();
        }

        private void ShowWelcome()
        {
            game.Reset();
            game.status = Game.GameStatus.Welcome;
        }

        public void Draw(RenderWindow window)
        {
            //decide which elements to draw depending on game status
            switch(this.game.status) {
                case Game.GameStatus.Active: this.children = this.inGame;
                    break;
                case Game.GameStatus.Welcome: this.children = this.welcome;
                    break;
                case Game.GameStatus.Credits: this.children = this.credits;
                    break;
            }
            base.Draw(window);
        }
    }
}

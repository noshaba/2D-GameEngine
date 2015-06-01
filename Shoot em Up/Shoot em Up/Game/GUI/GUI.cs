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
        Color color;
        Button start;
        Button restart;
        Button toMain;
        Menue menue;
        Screen title;
        Game game;
        Label scoreLabel;
        Label resultScore;
        Label hp;
        Label lvl;
        Menue results;
        Screen shield;
        private List<IGraphic> welcome = new List<IGraphic>();
        private List<IGraphic> inGame = new List<IGraphic>();
        private List<IGraphic> credits = new List<IGraphic>();

        public GUI(int width, int height, Game g) : base(new Vector2f(0, 0), new Vector2f(width, height)) {
            game = g;
            this.color = new Color(255,255,255);
           // start = new Button(new Vector2f(150, 640), new Vector2f(200,50), "Start Game!", this.color, 24, StartGame);
            title = new Screen(0,0,"../Content/title.png", this.color);
            this.welcome.Add(title);
            this.welcome.Add(start);
            

            restart = new Button(new Vector2f(150, 440), new Vector2f(200, 50), "Play Again!", this.color, 24, StartGame);
            this.results = new Menue(new Vector2f(0, 0), new Vector2f(width, 60), this.color);
            this.credits.Add(restart);
            this.resultScore = new Label(new Vector2f(80, 100), "Your Score: ", this.color, 24);
            this.results.Add(resultScore);
            this.credits.Add(results);

            this.children = this.credits;
        }

        private void StartGame()
        {
            //game.StartGame();
            Console.WriteLine("Yayayayaya");
        }
        /*
        private void ShowWelcome()
        {
            game.Reset();
            game.status = Game.GameStatus.Welcome;
        }
        */
        public new void Draw(RenderWindow window)
        {
            //decide which elements to draw depending on game status
          /*  switch(this.game.status) {
                case Game.GameStatus.Active: this.children = this.inGame;
                    this.createInGame();
                    break;
                case Game.GameStatus.Welcome: this.children = this.welcome;
                    break;
                case Game.GameStatus.Credits: this.children = this.credits;
                    break;
            }*/
            this.children = this.credits;
            base.Draw(window);
        }/*

        public void createInGame()
        {
            if (this.inGame.Count == 0)
            {
                menue = new Menue(new Vector2f(0, 0), new Vector2f(480, 75), this.color);
                toMain = new Button(10, 10, "Main Menue", this.color, ShowWelcome);
                Label l = new Label(new Vector2f(350,15), "Score", this.color);
                scoreLabel = new Label(new Vector2f(390, 15), this.game.player.score.ToString(), this.color);
                Label l2 = new Label(new Vector2f(350, 25), "HP", this.color);
                hp = new Label(new Vector2f(390, 25), this.game.player.hp.ToString() + "/" + this.game.player.maxHP.ToString(), this.color);
                lvl = new Label(new Vector2f(240, 20), this.game.level.ToString(), this.color);
                shield = new Screen(450, 45, "../Content/"+this.game.player.shieldStatus+".png", this.color);
                menue.Add(toMain);
                menue.Add(scoreLabel);
                menue.Add(l);
                menue.Add(l2);
                menue.Add(hp);
                menue.Add(lvl);
                menue.Add(shield);
                this.inGame.Add(menue);
            }
            else
            {
                scoreLabel.DisplayedString = this.game.player.score.ToString();
                hp.DisplayedString = this.game.player.hp.ToString() + "/" + this.game.player.maxHP.ToString();
                lvl.DisplayedString = this.game.level.ToString();
                shield.setImage("../Content/" + this.game.player.shieldStatus + ".png");
                this.resultScore.DisplayedString = "Your Score: " + this.game.player.score.ToString();
            }
        }*/
    }
}

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
        Color color;
        Color textColor;
        Button start;
        Button restart;
        Button toMain;
        Menue menue;
        Picture title;
        Game game;
        Label scoreLabel;
        Label resultScore;
        Label hp;
        Label lvl;
        Menue results;
        Picture shield;
        private List<IGraphic> welcome = new List<IGraphic>();
        private List<IGraphic> inGame = new List<IGraphic>();
        private List<IGraphic> credits = new List<IGraphic>();
        private List<IGraphic> nextLevel = new List<IGraphic>();

        public GUI(int width, int height, Game g) : base(new Vector2f(0, 0), new Vector2f(width, height)) {
            game = g;
            this.color = new Color(255,180,255);
            this.textColor = new Color(245,220,253);
            start = new Button(new Vector2f(150, 640), new Vector2f(200,50), "Start Game!", this.color, this.textColor, 24, StartGame);
            title = new Picture(0,0,"../Content/title.png", this.color);
            this.welcome.Add(title);
            this.welcome.Add(start);
            this.nextLevel.Add(title);
            this.nextLevel.Add(new Button(new Vector2f(150, 640), new Vector2f(200, 50), "Start next Level!", this.color,this.textColor, 24, NextLevel));

            restart = new Button(new Vector2f(150, 440), new Vector2f(200, 50), "Play Again!", this.color, this.textColor, 24, ShowWelcome);
            this.results = new Menue(new Vector2f(0, 0), new Vector2f(width, 60), this.color);
            this.credits.Add(restart);
            this.resultScore = new Label(new Vector2f(80, 100), "Your Score: ", this.textColor, 24);
            this.results.Add(resultScore);
            this.credits.Add(results);

            this.children = this.credits;
        }

        private void StartGame()
        {
            //game.StartGame();
            game.startGame();
        }

        private void NextLevel()
        {
            game.NextLevel();
        }
        private void ShowWelcome()
        {
            //game.Reset();
            game.status = Game.GameStatus.Start;
        }

        public new void Draw(RenderWindow window)
        {
            //decide which elements to draw depending on game status
            switch(this.game.status) {
                case Game.GameStatus.Active: this.children = this.inGame;
                    this.createInGame();
                    break;
                case Game.GameStatus.Start: this.children = this.welcome;
                    break;
                case Game.GameStatus.Credits: this.children = this.credits;
                    break;
                case Game.GameStatus.Nextlevel: this.children = this.nextLevel;
                    break;
            }
            base.Draw(window);
        }

        public void createInGame()
        {
            if (this.inGame.Count == 0)
            {
                menue = new Menue(new Vector2f(0, 0), new Vector2f(1200, 75), this.color);
                toMain = new Button(10, 10, "Main Menue", this.color, this.textColor, ShowWelcome);
                Label l = new Label(new Vector2f(350,15), "Score", this.textColor);
                scoreLabel = new Label(new Vector2f(390, 15), this.game.player.score.ToString(), this.textColor);
                Label l2 = new Label(new Vector2f(350, 25), "HP", this.textColor);
                hp = new Label(new Vector2f(390, 25), this.game.player.hp.ToString() + "/" + this.game.player.maxHP.ToString(), this.textColor);
                lvl = new Label(new Vector2f(240, 20), this.game.Level.ToString(), this.textColor);
                shield = new Picture(450, 45, "../Content/"+this.game.player.shieldStatus+".png", this.color);
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
                lvl.DisplayedString = this.game.Level.ToString();
                shield.setImage("../Content/" + this.game.player.shieldStatus + ".png");
                this.resultScore.DisplayedString = "Your Score: " + this.game.player.score.ToString();
            }
        }
    }
}

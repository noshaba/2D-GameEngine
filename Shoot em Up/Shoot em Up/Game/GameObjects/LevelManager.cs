using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoot_em_Up
{
    class LevelManager
    {
        public Spawn[] spawns;
        private Game game;
        private int handled;
        public LevelManager(Game g)
        {
            game = g;
        }
        public void LoadLevel(int lvl)
        {

            using (StreamReader sr = new StreamReader("../Content/"+lvl+".json"))
            {
                this.handled = 0;
                String line = sr.ReadToEnd();
                spawns = JSONManager.deserializeJson<Spawn[]>(line);
            }
        }

        public void Progress(uint time)
        {
            this.game.levelEnded = this.handled >= this.spawns.Length;
            for (int i = this.handled; i<this.spawns.Length; i++)
            {
                if(spawns[i].spawnAt == time/1000) {
                    this.TranslateToGame(spawns[i]);
                    this.handled++;
                }
            }
        }

        public void TranslateToGame(Spawn spwn)
        {
            this.game.numberOfFoes++;
            switch(spwn.type) {
                case "Astroid": game.GenerateAstroid(); 
                    break;
                case "Enemy": game.AddEnemy(spwn.x, spwn.y);
                    break;
                case "MeanEnemy": game.AddMeanEnemy(spwn.x, spwn.y);
                    break;
            }
        }
    }
}

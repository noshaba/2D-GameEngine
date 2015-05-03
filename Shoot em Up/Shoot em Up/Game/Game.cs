using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;
using SFML.System;

namespace Shoot_em_Up {
    class Game {
        private Paddle ai;
        private Paddle player;
        private Ball ball;
        private Physics physics;
        private List<IShape> objects = new List<IShape>();
        private int WIDTH;
        private int HEIGHT;

        private Astroid a;
        
        //constructor
        public Game(int width, int height) {
            WIDTH = width;
            HEIGHT = height;
            physics = new Physics();
            this.startGame();
        }



        public void Update(float dt) {
            //all the updating
            a.Update(dt);
        }


        public void Draw(RenderWindow window, float alpha) {
            //all the drawing
            foreach (Shape obj in objects) {
                window.Draw(obj);
            }
        }

        public void startGame()
        {
            objects.Clear();
            a = new Astroid();
            objects.Add(a);

        }

    }
}

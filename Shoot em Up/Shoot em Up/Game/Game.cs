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
        
        //constructor
        public Game(int width, int height) {
            WIDTH = width;
            HEIGHT = height;
            physics = new Physics();

        }



        public void Update(float dt) {
            //all the updating
        }


        public void Draw(RenderWindow window, float alpha) {
            //all the drawing
        }

    }
}

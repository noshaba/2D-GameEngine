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

        private void AddObject(IShape obj)
        {
            objects.Add(obj);
            physics.AddObject(obj);
        }

        public void Update(float dt) {
            //all the updating
            physics.Update(dt);
        }


        public void Draw(RenderWindow window, float alpha) {
            //all the drawing
            State interpol;
            Transform t;
            foreach (Shape obj in objects) {
                interpol = (obj as IShape).Interpolation(alpha);
                t = Transform.Identity;
                t.Translate(interpol.position);
                t.Rotate(interpol.DegOrientation);
                window.Draw(obj, new RenderStates(t));
            }
        }

        public void startGame()
        {
            objects.Clear();
            physics.Reset();
            this.AddObject(new Astroid(350, 140));
            this.AddObject(new Astroid(50, 200));

        }

    }
}

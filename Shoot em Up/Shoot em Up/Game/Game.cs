using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using Physics;

namespace Shoot_em_Up {
    class Game {
        private Paddle ai;
        private Paddle player;
        private Ball ball;
        private Physic physics;
        private List<IShape> objects = new List<IShape>();
        private int WIDTH;
        private int HEIGHT;
        private int MIN_OBJECTS;

        private Wall top;
        private Wall left;
        private Wall right;
        private Wall bottom;
        
        //constructor
        public Game(int width, int height) {
            WIDTH = width;
            HEIGHT = height;
            MIN_OBJECTS = 2;
            physics = new Physic(new Vector2f(0, 0), .1f, false);
            //this.top = new Wall(new Vector2f(0,width), 1,1,Color.Black);
            this.right = new Wall(new Vector2f(-1, 0), new Vector2f(width - 0.5f, height * 0.5f), new Vector2f(1.0f, height), Color.Black);
            this.left = new Wall(new Vector2f( 1, 0), new Vector2f(0.5f, height * 0.5f), new Vector2f(1.0f, height), Color.Black);
            AddObject(this.right);
            AddObject(this.left);
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
            this.reset();
            this.AddObject(new Astroid(350, 140));
            this.AddObject(new Astroid(50, 200));

        }

        public void reset()
        {
            objects.RemoveRange(MIN_OBJECTS, objects.Count - MIN_OBJECTS);
            physics.Reset(MIN_OBJECTS);
        }

    }
}

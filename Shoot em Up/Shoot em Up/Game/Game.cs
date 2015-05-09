using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using Maths;
using Physics;
using System.Diagnostics;

namespace Shoot_em_Up {
    class Game {
        private Physic physics;
        private List<GameObject> objects = new List<GameObject>();
        private List <IShape> shapes = new List<IShape>();
        private int WIDTH;
        private int HEIGHT;
        private int MIN_OBJECTS;

        private Player p;

        private Wall left;
        private Wall right;

        private Stopwatch clock;
        private int chance;


        
        //constructor
        public Game(int width, int height) {
            WIDTH = width;
            HEIGHT = height;
            MIN_OBJECTS = 2; //2 walls
            physics = new Physic(shapes, new Vector2f(0, 0), .1f, false);

            //this.top = new Wall(new Vector2f(0,width), 1,1,Color.Black);
            this.right = new Wall(new Vector2f(-1, 0), new Vector2f(width - 0.5f, height * 0.5f), new Vector2f(1.0f, height), Color.Black);
            this.left = new Wall(new Vector2f( 1, 0), new Vector2f(0.5f, height * 0.5f), new Vector2f(1.0f, height), Color.Black);

            AddObject(this.right);
            AddObject(this.left);

            this.clock = new Stopwatch();
            this.StartGame();
        }

        private void AddObject(GameObject obj)
        {

            objects.Add(obj);
            shapes.Add(obj.shape);
        }

        public void Update(float dt) {
            //all the updating
            physics.Update(dt);
            LookForNewAstroids();
            //each astroid has to check if it has left the screen, when it does the player looses points(colliding with player is a different matter)
            for (int i = 0; i < objects.Count; i++ )
            {
                objects[i].Update();
                if (objects[i] is Astroid && (objects[i] as Astroid).shape.COM.Y > this.HEIGHT)
                {
                    //objects.RemoveAt(i);
                    if (this.p.score >= 100)
                    {
                        this.p.score -= 100;
                    }
                }
            }

        }


        public void Draw(RenderWindow window, float alpha) {
            //all the drawing
            State interpol;
            Transform t;
            foreach (GameObject obj in objects) {
                interpol = obj.shape.Interpolation(alpha);
                t = Transform.Identity;
                t.Translate(interpol.position);
                t.Rotate(interpol.DegOrientation);
                window.Draw(obj.shape as Shape, new RenderStates(t));
            }
        }

        public void StartGame()
        {
            this.Reset();

            this.p = new Player(new Vector2f(this.WIDTH / 2, 600), 20, 10, Color.Yellow);
            AddObject(p);

            this.clock.Start();
            this.chance = 10;
            this.GenerateAstroid();
            
        }

        public void GenerateAstroid() {
            this.AddObject(new Astroid(this.WIDTH/2, 0));
        }

        public void LookForNewAstroids()
        {
            //every second try to create a new astroid, if not raise chance to create one next time
            if (EMath.random.Next(1, 100) < this.chance && this.clock.ElapsedMilliseconds > 600)
            {
                this.clock.Restart();
                this.GenerateAstroid();
                this.chance = 0;
            }
            else if (this.chance < 100 && this.clock.ElapsedMilliseconds > 6000)
            {
                this.clock.Restart();
                this.chance += 10;
            }
        }

        public void MovePlayer(Keyboard.Key k)
        {
            this.p.Move(k);
        }

        public void StopPlayer() {
            this.p.Stop();
        }

        public void Reset()
        {
            objects.RemoveRange(MIN_OBJECTS, objects.Count - MIN_OBJECTS);
        }

    }
}

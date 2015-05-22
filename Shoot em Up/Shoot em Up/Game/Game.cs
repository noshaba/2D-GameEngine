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
        private static List<GameObject> objects = new List<GameObject>();
        private static List<IState> shapes = new List<IState>();
        private int WIDTH;
        private int HEIGHT;
        private int MIN_OBJECTS;

        private Player player;

        private Wall left;
        private Wall right;

        private Stopwatch clock;
        private int chance;
        public GameStatus status;

        public enum GameStatus
        {
            Active, Welcome, Credits
        }
        
        //constructor
        public Game(int width, int height) {
            WIDTH = width;
            HEIGHT = height;
            MIN_OBJECTS = 2; //2 walls
            physics = new Physic(shapes, new Vector2f(0, 0), 0, false);

            this.right = new Wall(new Vector2f(-1, 0), new Vector2f(width - 0.5f, height * 0.5f), new Vector2f(1.0f, height), Color.Black);
            this.left = new Wall(new Vector2f( 1, 0), new Vector2f(0.5f, height * 0.5f), new Vector2f(1.0f, height), Color.Black);

            AddObject(this.right);
            AddObject(this.left);

            FactionManager.LoadJSON();

            this.clock = new Stopwatch();
            this.status = GameStatus.Welcome;
            //this.StartGame();
        }

        public static void AddObject(GameObject obj)
        {
            objects.Add(obj);
            shapes.Add(obj.state);
        }

        public void Update(float dt) {
            if (!this.physics.frozen)
            {
                if (this.status == GameStatus.Active)
                {
                    if (!this.player.display)
                    {
                        this.status = GameStatus.Credits;
                        this.Reset();
                    }
                    //all the updating
                    LookForNewAstroids();
                    physics.Update(dt);

                    for (int i = 0; i < objects.Count; ++i)
                    {
                        objects[i].Update();
                    }

                    for (int i = 0; i < objects.Count; ++i)
                    {
                        objects[i].LateUpdate();
                        if (objects[i].state.COM.Y < 0 || objects[i].state.COM.Y > this.HEIGHT || objects[i].state.COM.X < 0 || objects[i].state.COM.X > this.WIDTH)
                        {
                            //add pooling later
                            objects[i].display = false;
                        }
                        if (!objects[i].display)
                        {
                            objects.RemoveAt(i);
                            shapes.RemoveAt(i);
                        }
                    }
                }
            }
        }

        public void Draw(RenderWindow window, float alpha) {
            //all the drawing
            State interpol;
            Transform t;
            foreach (GameObject obj in objects) {
                interpol = obj.state.Interpolation(alpha);
                t = Transform.Identity;
                t.Translate(interpol.position);
                t.Rotate(interpol.DegOrientation);
                window.Draw(obj.shape, new RenderStates(t));
            }
        }

        public void StartGame()
        {
            this.Reset();

            this.player = new Player(FactionManager.factions[(int) Faction.Type.Player],new Vector2f(this.WIDTH / 2, this.HEIGHT-40), new []{new Vector2f(40,100), new Vector2f(80,60), new Vector2f(60,60), new Vector2f(100,100)});
            AddObject(player);

            this.clock.Start();
            this.chance = 10;
            this.GenerateAstroid();
            this.status = GameStatus.Active;
        }

        public void GenerateAstroid() {
            AddObject(new Astroid(FactionManager.factions[(int) Faction.Type.None], this.WIDTH/2, 0));
        }

        public void LookForNewAstroids()
        {
            //every second try to create a new astroid, if not raise chance to create one next time
            if (EMath.random.Next(1, 100) < this.chance && this.clock.ElapsedMilliseconds > 600 && !this.physics.frozen)
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
            if(status == GameStatus.Active) this.player.Move(k);
        }

        public void StopPlayer() {
            if (status == GameStatus.Active) this.player.Stop();
        }

        public void Fire()
        {
            if (status == GameStatus.Active) this.player.fire = !this.physics.frozen;
        }

        public void StopFire()
        {
            if (status == GameStatus.Active) this.player.fire = false;
        }

        public void Reset()
        {
            objects.RemoveRange(MIN_OBJECTS, objects.Count - MIN_OBJECTS);
            shapes.RemoveRange(MIN_OBJECTS, shapes.Count - MIN_OBJECTS);
        }

        public void Pause()
        {
            if (status == GameStatus.Active)
            {
                this.player.fire = false;
                this.physics.frozen = !this.physics.frozen;
            }
        }

    }
}

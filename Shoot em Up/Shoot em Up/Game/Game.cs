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
        private static List<IRigidBody> shapes = new List<IRigidBody>();
        private int WIDTH;
        private int HEIGHT;
        private int MIN_OBJECTS;

        public Player player;

        private Wall left;
        private Wall right;

        private Stopwatch clock;
        public GameStatus status;
        public int level = 1;
        private int maxLevel = 2;
        private LevelManager progressor;
        public bool levelEnded;
        public int numberOfFoes = 0;

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
            this.progressor = new LevelManager(this);
            this.levelEnded = false;
        }

        public static void AddObject(GameObject obj)
        {
            objects.Add(obj);
            shapes.Add(obj.rigidBody);
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
                    if (levelEnded)
                        CheckFinal();
                    //all the updating
                    this.progressor.Progress((uint)this.clock.ElapsedMilliseconds);
                    physics.Update(dt);

                    for (int i = 0; i < objects.Count; ++i)
                    {
                        objects[i].Update();
                    }

                    for (int i = 0; i < objects.Count; ++i)
                    {
                        objects[i].LateUpdate();
                        if (objects[i].rigidBody.COM.Y < 0 || objects[i].rigidBody.COM.Y > this.HEIGHT || objects[i].rigidBody.COM.X < 0 || objects[i].rigidBody.COM.X > this.WIDTH)
                        {
                            objects[i].display = false;
                        }
                        if (!objects[i].display)
                        {
                            if(!(objects[i] is Player || objects[i] is Bullet)) {
                                this.numberOfFoes--;
                            }
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
                interpol = obj.rigidBody.Interpolation(alpha);
                t = Transform.Identity;
                t.Translate(interpol.position);
                t.Rotate(interpol.DegOrientation);
                window.Draw(obj.drawable, new RenderStates(t));
            }
        }

        public void StartGame()
        {
            this.Reset();

            this.player = new Player(FactionManager.factions[(int) Faction.Type.Player],new Vector2f(this.WIDTH / 2, this.HEIGHT-40), new Texture("../Content/ship.png"));
            AddObject(player);
            this.clock.Start();
            this.status = GameStatus.Active;
            this.progressor.LoadLevel(this.level);
        }

        public void CheckFinal()
        {
            //if all objects from the lvl have been destroyed
            if(levelEnded && this.numberOfFoes == 0) {
                if (this.level < this.maxLevel)
                {
                    this.level++;
                    this.progressor.LoadLevel(this.level);
                    this.clock.Restart();
                } else if (this.level == this.maxLevel)
                {
                    this.status = GameStatus.Credits;
                }
            }
        }

        public void GenerateAstroid() {
            AddObject(new Astroid(FactionManager.factions[(int) Faction.Type.None], this.WIDTH/2, 0));
        }


        public void AddEnemy(float x, float y)
        {
            AddObject(new Enemy(FactionManager.factions[(int)Faction.Type.AI], new Vector2f(x, y), new Texture("../Content/enemy.png"), 250, 10, 20));
        }

        public void AddMeanEnemy(float x, float y)
        {
            AddObject(new Enemy(FactionManager.factions[(int)Faction.Type.AI], new Vector2f(x, y), new Texture("../Content/enemy2.png"), 500, 10, 30));
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

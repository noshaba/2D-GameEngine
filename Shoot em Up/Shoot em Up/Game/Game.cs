using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using Maths;
using Physics;
using System.Diagnostics;

namespace Shoot_em_Up 
{
    class Game
    {
        private Physic physics;
        public Planet planet;
        private static List<GameObject> objects = new List<GameObject>();
        private static List<IRigidBody> rigidBodies = new List<IRigidBody>();
        public static int WIDTH;
        public static int HEIGHT;
        public static Faction[] factions;
        public static bool debug = false;
        private int level;
        private int maxLevel = 2;
        public Player player;
        public bool levelEnded;
        public GameStatus status;
        public Game(int width, int height)
        {
            WIDTH = width;
            HEIGHT = height;
            this.status = GameStatus.Start;
        }

        public void startGame()
        {
            this.Reset();
            this.status = GameStatus.Active;
            this.levelEnded = false;
            Level = 1;
            this.player = new Player(factions[1], new Vector2f(100, 100), "../Content/cuteship", new int[] { 100, 89 }, new int[] { 100, 89 });
            Add(this.player);
        }

        public void NextLevel()
        {
            this.Reset();
            this.levelEnded = false;
            this.status = GameStatus.Active;
            Level++;
            this.player.rigidBody.COM = new Vector2f(100, 100);
            Add(this.player);
        }

        public enum GameStatus
        {
            Start,Active,Nextlevel,Credits
        }

        public static void Add(GameObject obj)
        {
            objects.Add(obj);
            rigidBodies.Add(obj.rigidBody);
        }

        public int Level
        {
            get
            {
                return level;
            }
            set
            {
                level = value;
                LoadLevel();
            }
        }

        private void LoadLevel()
        {
            using (StreamReader sr = new StreamReader("../Content/" + level + "/Planet.json"))
            {
                String json = sr.ReadToEnd();
                planet = JSONManager.deserializeJson<Planet>(json);
                planet.Init();
            }
            physics = new Physic(rigidBodies, new Vector2f(planet.Gravity[0], planet.Gravity[1]), planet.Damping, planet.Friction, (FloatRect) planet.backgroundSprite.TextureRect);
            planet.AddGround();
            using (StreamReader sr = new StreamReader("../Content/" + level + "/Factions.json"))
            {
                String json = sr.ReadToEnd();
                factions = JSONManager.deserializeJson<Faction[]>(json);
            }
            using (StreamReader sr = new StreamReader("../Content/" + level + "/Obstacles.json"))
            {
                ObstacleContract[] obstacles;
                String json = sr.ReadToEnd();
                obstacles = JSONManager.deserializeJson<ObstacleContract[]>(json);
                for (int i = 0; i < obstacles.Length; i++)
                {
                    obstacles[i].Init();
                }  
            }
            using (StreamReader sr = new StreamReader("../Content/" + level + "/Enemies.json"))
            {
                EnemyContract[] enemies;
                String json = sr.ReadToEnd();
                enemies = JSONManager.deserializeJson<EnemyContract[]>(json);
                for (int i = 0; i < enemies.Length; i++)
                {
                    enemies[i].Init();
                }
            }
        }

        public void Update(float dt) 
        {
            //physics.Update(dt);
            if (this.status == GameStatus.Active)
                {
                /*
                    if (!this.player.display)
                    {
                        this.status = GameStatus.Credits;
                        this.Reset();
                    }

                    if (levelEnded)
                        CheckFinal();
                    //all the updating
                    this.progressor.Progress((uint)this.clock.ElapsedMilliseconds);*/
                    physics.Update(dt);

                    for (int i = 0; i < objects.Count; ++i)
                    {
                        objects[i].Update();
                        //there exists a better way for this???
                        rigidBodies[i] = objects[i].rigidBody;

                        objects[i].LateUpdate();

                        if(!objects[i].display) {
                            objects.RemoveAt(i);
                            rigidBodies.RemoveAt(i);
                        }
                    }
                    if(this.levelEnded) {
                        if (this.player.rigidBody.COM.X > this.planet.Length && this.level + 1 <= this.maxLevel)
                        {
                            this.status = GameStatus.Nextlevel;
                        }
                        else if (this.level + 1 > this.maxLevel)
                        {
                            this.status = GameStatus.Credits;
                        }
                    }
                    /*for (int i = 0; i < objects.Count; ++i)
                    {
                        objects[i].LateUpdate();
                        if (objects[i].rigidBody.COM.Y < 0 || objects[i].rigidBody.COM.Y > this.HEIGHT || objects[i].rigidBody.COM.X < 0 || objects[i].rigidBody.COM.X > this.WIDTH)
                        {
                            objects[i].display = false;
                        }
                        if (!objects[i].display)
                        {
                            if (objects[i] is Enemy)
                            {
                                AddItem((objects[i] as Enemy).drop, objects[i].rigidBody.COM);
                            }
                            if ((objects[i] is Enemy || objects[i] is Astroid))
                            {
                                this.numberOfFoes--;
                            }
                            objects.RemoveAt(i);
                            shapes.RemoveAt(i);
                        }
                    }*/
                //}
            }
        }

        public void Draw(RenderWindow window, float alpha)
        {
            //all the drawing
            if(status == GameStatus.Active) {
                window.Draw(planet.backgroundSprite);
                State interpol;
                Transform t;
                if(debug) physics.DrawQuadtree(window);
                foreach (GameObject obj in objects)
                {
                    interpol = obj.rigidBody.Interpolation(alpha);
                    t = Transform.Identity;
                    t.Translate(interpol.position);
                    t.Rotate(interpol.DegOrientation);
                    window.Draw(obj.drawable, new RenderStates(t));
                    if (debug)
                    {
                        window.Draw(obj.rigidBody as Shape, new RenderStates(t));
                        window.Draw(obj.rigidBody.BoundingCircle, new RenderStates(t));
                    }
                }
            }
        }

        public void MovePlayer(Keyboard.Key k)
        {
            this.player.Move(k);
        }

        public void Fire()
        {
            this.player.fire = !this.physics.frozen;
        }

        public void StopFire()
        {
            this.player.fire = false;
        }

        public void Reset()
        {
            objects.RemoveRange(0, objects.Count);
            rigidBodies.RemoveRange(0,rigidBodies.Count);
        }

    }
}
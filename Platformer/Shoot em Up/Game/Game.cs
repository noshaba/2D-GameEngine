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
using ImageProcessing;

namespace Platformer 
{
    class Game
    {
        private Physic physics;
        public Planet planet;
        private static List<GameObject> objects = new List<GameObject>();
        private static List<Body> rigidBodies = new List<Body>();
        public static int WIDTH;
        public static int HEIGHT;
        public static Faction[] factions;
        public static bool debug = false;
        private int level;
        private const int MAXLEVEL = 1;
        public Player player;
        public bool levelEnded;
        public GameStatus status;
        public Stopwatch clock;
        Texture normalMap = new Texture("../Content/textures/car_normal.tga");
        Shader shader = new Shader(null, "../Content/shaders/procedural.frag");
        Sprite sprite = new Sprite(new Texture("../Content/textures/car_colour.png"));

        public enum GameStatus
        {
            Start, Active, Nextlevel, Credits
        }

        public enum GameItem
        {
            Heal, Bomb, Points, NoPoints, Weapon, None
        }
        public Game(int width, int height)
        {
            WIDTH = width;
            HEIGHT = height;
            sprite.Position = new Vector2f(400, HEIGHT * .5f);
            this.status = GameStatus.Start;

            SoundManager.Play(SoundManager.ambient);
            SoundManager.ambient.Loop = true;

            this.clock = new Stopwatch();
            this.clock.Start();
        }


        public void startGame()
        {
            this.Reset();
            this.status = GameStatus.Active;
            this.levelEnded = false;
            Level = 1;
            this.player = new Player(factions[1], new Vector2f(250, 250), "../Content/catNew", 
                new int[] { 130, 80 }, new int[] { 130, 960 }, new int[]{0});
            Add(this.player);
            //Platform test
           /* Texture tile = new Texture("../Content/platform.png", new IntRect(0, 0, 100, 100));
            Polygon p =  new Polygon(CV.AlphaEdgeDetection(tile.CopyToImage().Pixels, tile.Size.X, tile.Size.Y, 254), new Vector2f(50,50), new Vector2f(800,500), 0, 0);
            tile = new Texture("../Content/platform.png", new IntRect(100, 0, 100, 100));
            Polygon p2 = new Polygon(CV.AlphaEdgeDetection(tile.CopyToImage().Pixels, tile.Size.X, tile.Size.Y, 254), new Vector2f(50,50), new Vector2f(800,500), 0, 0);
            Add(new Platform(new IRigidBody[]{p,p2}, new Vector2f(800, 500), 0,100));*/

        }

        public void NextLevel()
        {
            this.Reset();
            this.levelEnded = false;
            this.status = GameStatus.Active;
            Level++;
            this.player.rigidBody.COM = new Vector2f(250, 250);
            Add(this.player);
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
            Add(new Wall(new Vector2f(1,0),new Vector2f(0,HEIGHT*.5f),new Vector2f(.1f,HEIGHT),Color.Transparent));
            using (StreamReader sr = new StreamReader("../Content/" + level + "/Planet.json"))
            {
                String json = sr.ReadToEnd();
                planet = JSONManager.deserializeJson<Planet>(json);
                planet.Init();
            }
            using (StreamReader sr = new StreamReader("../Content/" + level + "/Platforms.json"))
            {
                PlatformContract[] platforms;
                String json = sr.ReadToEnd();
                platforms = JSONManager.deserializeJson<PlatformContract[]>(json);
                for (int i = 0; i < platforms.Length; i++)
                {
                    platforms[i].Init();
                }
            }
            physics = new Physic(rigidBodies, new Vector2f(planet.Gravity[0], planet.Gravity[1]), planet.Damping, 
                planet.Friction, (FloatRect) planet.backgroundSprite.TextureRect);
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
                physics.Update(dt);
                if (this.clock.ElapsedMilliseconds > 100 && !this.physics.frozen)
                {
                    //this.player.animationIndex = (this.player.animationIndex + 1) % this.player.rigidBodies.Length;
                    this.player.AdvanceAnim();
                    this.clock.Restart();
                }
                //physics.frozen = true;

                for (int i = 0; i < objects.Count; ++i)
                {
                    objects[i].Update();
                    //there exists a better way for this???
                    rigidBodies[i] = objects[i].rigidBody;

                    objects[i].LateUpdate();

                    if(!objects[i].display) {
                        if (objects[i] is Enemy)
                        {
                            AddItem((objects[i] as Enemy).drop, objects[i].rigidBody.COM);
                        }
                        objects.RemoveAt(i);
                        rigidBodies.RemoveAt(i);
                    }
                }
                if (this.player.hp <= 0)
                {
                    this.status = GameStatus.Credits;
                }
                if(this.levelEnded) {
                    if (this.player.rigidBody.COM.X > this.planet.Length && 
                        this.level + 1 <= MAXLEVEL)
                    {
                        this.status = GameStatus.Nextlevel;
                    }
                    else if (this.player.rigidBody.COM.X > this.planet.Length && 
                        this.level + 1 > MAXLEVEL)
                    {
                        this.status = GameStatus.Credits;
                    }
                }
            }
        }

        public void Draw(RenderWindow window, float alpha)
        {
            //all the drawing
            if(status == GameStatus.Active) {
                window.Draw(planet.backgroundSprite);
            //    if(debug) physics.DrawQuadtree(window);
                foreach (GameObject obj in objects)
                {
                    obj.Draw(window, alpha);
                    if (debug)
                        obj.rigidBody.Draw(window, alpha);
                }
            }
            shader.SetParameter("normalMap", normalMap);
            shader.SetParameter("lightPosition", Mouse.GetPosition(window).X, HEIGHT - Mouse.GetPosition(window).Y, 0.04f);
            RenderStates s = new RenderStates(Transform.Identity);
            s.Shader = shader;
            window.Draw(sprite,s);
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

        public void Pause()
        {
            this.physics.frozen = !this.physics.frozen;
        }

        public void Reset()
        {
            objects.RemoveRange(0, objects.Count);
            rigidBodies.RemoveRange(0,rigidBodies.Count);
        }

        public void AddItem(Game.GameItem item, Vector2f pos)
        {
            switch (item)
            {
                case Game.GameItem.Heal: 
                 //   Add(new Item("../Content/items/heal.png", Heal, pos));
                    break;
                case Game.GameItem.Bomb: 
                 //   Add(new Item("../Content/items/bomb.png", Bomb, pos));
                    break;
                case Game.GameItem.Points: 
                 //   Add(new Item("../Content/items/points.png", Points, pos));
                    break;
                case Game.GameItem.NoPoints: 
                 //   Add(new Item("../Content/items/noPoints.png", NoPoints, pos));
                    break;
                case Game.GameItem.Weapon: 
                 //   Add(new Item("../Content/items/weapon.png", Weapon, pos));
                    break;
                default:
                    break;

            }
        }

        // TODO: gehört in die item klasse und nich hier..!!

        private void Heal()
        {
            this.player.hp += 200;
        }

        private void Bomb()
        {
            this.player.hp -= 200;
        }

        private void Points()
        {
            this.player.score += 100;
        }

        private void NoPoints()
        {
            this.player.score -= 100;
        }

        private void Weapon()
        {
            this.player.weapon.damage += 20;
        }

    }
}
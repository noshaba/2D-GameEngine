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
        public Physic physics;
        public Planet planet;
        private static List<GameObject> objects = new List<GameObject>();
        private static List<Body> rigidBodies = new List<Body>();
        private static List<Constraint> joints = new List<Constraint>();
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

        Shader light = new Shader(null, "../Content/shaders/light.frag");
        Shader sceneBufferShader = new Shader(null, "../Content/shaders/scene_buffer.frag");
        Shader shadow = new Shader(null, "../Content/shaders/shadow.frag");

        Sprite sprite1 = new Sprite(new Texture("../Content/textures/car_colour.png"));
        Sprite sprite2 = new Sprite(new Texture("../Content/textures/car_colour.png"));
        RenderTexture sceneBuffer;
        Sprite scene = new Sprite();

        public static Platform breakable;

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

            light.SetParameter("normalMap", new Texture("../Content/textures/car_normal.tga"));
            light.SetParameter("specularMap", new Texture("../Content/textures/car_specular.png"));
            light.SetParameter("reflectMap", new Texture("../Content/textures/car_reflect.png"));

            sprite1.Position = new Vector2f(400, HEIGHT * .5f);
            sprite2.Position = new Vector2f(350, HEIGHT * .5f);
         
            this.status = GameStatus.Start;

            SoundManager.Play(SoundManager.ambient);
            SoundManager.ambient.Loop = true;

            this.clock = new Stopwatch();
            this.clock.Start();
        }


        public void startGame()
        {
            this.Reset();
            this.levelEnded = false;
            Level = 1;
            GameObject cir1 = new GameObject(new IRigidBody[] { new Circle(new Vector2f(), 0, 25, 0.01f) }, new Vector2f(1000, HEIGHT * .5f - 300), 0);
            GameObject cir2 = new GameObject(new IRigidBody[] { new Circle(new Vector2f(), 0, 25, 0.01f) }, new Vector2f(1000, HEIGHT * .5f - 200), 0);
            GameObject cir3 = new GameObject(new IRigidBody[] { new Circle(new Vector2f(), 0, 25, 0.01f) }, new Vector2f(1000, HEIGHT * .5f - 100), 0);
            cir1.Moveable = false;
            Add(cir1);
            Add(cir2);
            Add(cir3);
            DistanceConstraint joint1 = new DistanceConstraint(cir1.rigidBody, cir2.rigidBody, 100);
            DistanceConstraint joint2 = new DistanceConstraint(cir2.rigidBody, cir3.rigidBody, 100);
            joints.Add(joint1);
            joints.Add(joint2);
            this.status = GameStatus.Active;

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
            player.rigidBody.COM = new Vector2f(250, 250);
            Add(player);
        }

        public static void Add(GameObject obj)
        {
            objects.Add(obj);
            rigidBodies.Add(obj.rigidBody);
        }

        public static void Remove(GameObject obj)
        {
            objects.Remove(obj);
            rigidBodies.Remove(obj.rigidBody);
        }

        public static void Clear()
        {
            objects.Clear();
            rigidBodies.Clear();
        }

        public static void AddRange(List<GameObject> objs, List<Body> bodies)
        {
            objects.AddRange(objs);
            rigidBodies.AddRange(bodies);
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
            using (StreamReader sr = new StreamReader("../Content/" + level + "/Factions.json"))
            {
                String json = sr.ReadToEnd();
                factions = JSONManager.deserializeJson<Faction[]>(json);
            }
            using (StreamReader sr = new StreamReader("../Content/" + level + "/Planet.json"))
            {
                String json = sr.ReadToEnd();
                planet = JSONManager.deserializeJson<Planet>(json);
                planet.Init();
                player = new Player(factions[1], new Vector2f(250, 250), "../Content/catNew",
                    new int[] { 130, 80 }, new int[] { 130, 960 }, new int[] { 0 });

                sceneBuffer = new RenderTexture((uint)planet.Length, (uint)HEIGHT, true);
                scene.Texture = sceneBuffer.Texture;
            }
            physics = new Physic(rigidBodies, joints, new Vector2f(planet.Gravity[0], planet.Gravity[1]), planet.Damping,
                (FloatRect)planet.backgroundSprite.TextureRect);
            Add(player);

            Add(new Wall(new Vector2f(1, 0), new Vector2f(0, HEIGHT * .5f), new Vector2f(.1f, HEIGHT), Color.Transparent));
            using (StreamReader sr = new StreamReader("../Content/" + level + "/Platforms.json"))
            {
                PlatformContract[] platforms;
                String json = sr.ReadToEnd();
                platforms = JSONManager.deserializeJson<PlatformContract[]>(json);
                for (int i = 0; i < platforms.Length; i++)
                {
                    platforms[i].Init();
                }
                //breakable = objects.Last() as Platform;
            }
            planet.AddGround();
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

        public void EarlyUpdate(float dt)
        {
            for (int i = 0; i < objects.Count; ++i)
                objects[i].EarlyUpdate();
        }

        public void LateUpdate(float dt) 
        {
            if (this.clock.ElapsedMilliseconds > 100 && !this.physics.frozen)
            {
                //this.player.animationIndex = (this.player.animationIndex + 1) % this.player.rigidBodies.Length;
                player.AdvanceAnim();
                this.clock.Restart();
            }
            //physics.frozen = true;

            for (int i = 0; i < objects.Count; ++i)
            {
                //there exists a better way for this???
                rigidBodies[i] = objects[i].rigidBody;

                objects[i].LateUpdate();

            /*    if(!objects[i].display) {
                    if (objects[i] is Enemy)
                    {
                        AddItem((objects[i] as Enemy).drop, objects[i].rigidBody.COM);
                    }
                    objects.RemoveAt(i);
                    rigidBodies.RemoveAt(i);
                }*/
            }
            if (player.hp <= 0)
            {
                this.status = GameStatus.Credits;
            }
            if(this.levelEnded) {
                if (player.rigidBody.COM.X > this.planet.Length && 
                    this.level + 1 <= MAXLEVEL)
                {
                    this.status = GameStatus.Nextlevel;
                }
                else if (player.rigidBody.COM.X > this.planet.Length && 
                    this.level + 1 > MAXLEVEL)
                {
                    this.status = GameStatus.Credits;
                }
            }
        }

        public void Draw(RenderWindow window, float alpha)
        {
            // shader.SetParameter("lightPosition", Mouse.GetPosition(window).X, HEIGHT - Mouse.GetPosition(window).Y, 0.04f);
            // shaderBG.SetParameter("lightPosition", Mouse.GetPosition(window).X, HEIGHT - Mouse.GetPosition(window).Y, 0.04f);
            RenderStates s = new RenderStates(Transform.Identity);
            if(status == GameStatus.Active) {
                // s.Shader = shaderBG;
                sceneBuffer.Clear(Color.Transparent);
                // window.Draw(planet.backgroundSprite, s);
                foreach (GameObject obj in objects)
                {
                    // obj.Draw(window, alpha);
                    obj.Draw(sceneBuffer, alpha);
                    if (debug)
                        obj.rigidBody.Draw(sceneBuffer, alpha);
                        // obj.rigidBody.Draw(window, alpha);
                }

                sceneBuffer.Draw(sprite1, s);
                sceneBuffer.Draw(sprite2, s);

                sceneBufferShader.SetParameter("rt_scene", sceneBuffer.Texture);
                s.Shader = sceneBufferShader;
                sceneBuffer.Draw(scene, s);
                sceneBuffer.Display();

                //godsRay.SetParameter("lightPosition", 0,0,1);
                shadow.SetParameter("texture", scene.Texture);
                s.Shader = shadow;

                window.Draw(planet.backgroundSprite);
                window.Draw(scene, s);
            }

            //window.Draw(sprite1,s);
            //window.Draw(sprite2,s);
        }

        public void MovePlayer(Keyboard.Key k)
        {
            player.Move(k);
        }

        public void Fire()
        {
            player.fire = !this.physics.frozen;
        }

        public void StopFire()
        {
            player.fire = false;
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
            player.hp += 200;
        }

        private void Bomb()
        {
            player.hp -= 200;
        }

        private void Points()
        {
            player.score += 100;
        }

        private void NoPoints()
        {
            player.score -= 100;
        }

        private void Weapon()
        {
            player.weapon.damage += 20;
        }

    }
}
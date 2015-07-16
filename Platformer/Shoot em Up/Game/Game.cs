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
        public static List<Spawner> spawners = new List<Spawner>();
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
        public static Vector2f playerPos;

        Shader light = new Shader(null, "../Content/shaders/light.frag");
        Shader sceneBufferShader = new Shader(null, "../Content/shaders/scene_buffer.frag");
        Shader shadow = new Shader(null, "../Content/shaders/shadow.frag");

        RenderTexture shadowBuffer;
        RenderTexture sceneBuffer;
        Sprite shadowScene = new Sprite();
        Sprite scene = new Sprite();
        Vector2f windowHalfSize;
        Vector3f lightPosition;

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
            windowHalfSize = new Vector2f(WIDTH * .5f, HEIGHT * .5f);

            light.SetParameter("normalMap", new Texture("../Content/textures/ReflectMap.png"));
         
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
            // string texturePath, int[] spriteTileSize, int[] spriteSize, int[] tileIndices, int animationIndex, Vector2f position, float rotation, float density
            GameObject cir1 = new GameObject("../Content/Pendulum.png", new[] { 50, 50 }, new[] { 50, 50 }, new[] { 0 }, 0, new Vector2f(1000, HEIGHT * .5f - 300), 0, 0.01f);
            GameObject cir2 = new GameObject("../Content/Pendulum.png", new[] { 50, 50 }, new[] { 50, 50 }, new[] { 0 }, 0, new Vector2f(1000, HEIGHT * .5f - 200), 0, 0.01f);
            GameObject cir3 = new GameObject("../Content/Pendulum.png", new[] { 50, 50 }, new[] { 50, 50 }, new[] { 0 }, 0, new Vector2f(1000, HEIGHT * .5f - 100), 0, 0.01f);
            cir1.Moveable = false;
            Add(cir1);
            Add(cir2);
            Add(cir3);
            DistanceConstraint joint1 = new DistanceConstraint(cir1.rigidBody, cir2.rigidBody, 100);
            DistanceConstraint joint2 = new DistanceConstraint(cir2.rigidBody, cir3.rigidBody, 100);
            joints.Add(joint1);
            joints.Add(joint2);

            objects.Sort(delegate(GameObject o1, GameObject o2)
            {
                return o1.rigidBody.COM.X.CompareTo(o2.rigidBody.COM.X);
            });
            rigidBodies.Sort(delegate(Body b1, Body b2)
            {
                return b1.COM.X.CompareTo(b2.COM.X);
            });

            this.status = GameStatus.Active;
            //this.enemy = new Enemy(Collision.Type.Polygon, new int[] { 70, 70 }, new int[] { 0 }, 1, 0, 0, 0.1f, 0.1f, "../Content/blobSprite.png", new int[] { 70, 490 }, new Vector2f(750, 450),
              //      0, 100, 10, 10, factions[0]);
            //Collision.Type type, int[] tileSize, int[] tileIndices,float density, int animationIndex, float restitution, float staticFriction, float kineticFriction, String texturePath, int[]spriteSize, Vector2f position, float rotation, int health, int points, int dmg, Faction faction
            //Add(enemy);

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
                player = new Player(factions[1], new Vector2f(250, 250), "../Content/ghostSpriteBlack",
                    new int[] { 100, 100 }, new int[] { 100, 800 }, new int[] { 0 });

                lightPosition = new Vector3f(planet.Length * 0.5f, HEIGHT * 0.5f, 0.04f);
                light.SetParameter("lightPosition", 
                    lightPosition.X, HEIGHT - lightPosition.Y, lightPosition.Z);
                light.SetParameter("resolution",
                    planet.Length * 10, planet.backgroundSprite.Texture.Size.Y * 10);
                shadowBuffer = new RenderTexture((uint)planet.Length, (uint)HEIGHT);
                shadowScene.Texture = shadowBuffer.Texture;

                sceneBuffer = new RenderTexture((uint)planet.Length, (uint)HEIGHT);
                scene.Texture = sceneBuffer.Texture;
            }
            physics = new Physic(rigidBodies, joints, new Vector2f(planet.Gravity[0], planet.Gravity[1]), planet.Damping,
                new Vector2f(WIDTH, HEIGHT));
            Add(player);
            playerPos = player.rigidBody.COM;

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

        public void EarlyUpdate(Vector2f viewCenter)
        {
            for (int i = 0; i < objects.Count; ++i)
                if(objects[i].InsideWindow(viewCenter, windowHalfSize))
                    objects[i].EarlyUpdate();
            for (int i = 0; i < spawners.Count; i++)
            {
                if (spawners[i].NearPlayer(player.rigidBody.COM.X, WIDTH / 2))
                {
                    spawners[i].Spawn();
                    spawners.RemoveAt(i);
                }
            }
        }

        public void LateUpdate(Vector2f viewCenter) 
        {
            playerPos = player.rigidBody.COM;
            if (this.clock.ElapsedMilliseconds > 100 && !this.physics.frozen)
            {
                //this.player.animationIndex = (this.player.animationIndex + 1) % this.player.rigidBodies.Length;
                foreach (GameObject obj in objects) {
                    if (obj.animated && obj is Player)
                    {
                        obj.AdvanceAnim((int)(obj as Player).status);
                    }
                    else if (obj.animated && obj is Enemy)
                    {
                        Console.WriteLine("here");
                        obj.AdvanceAnim((int)(obj as Enemy).status);
                    }
                }
                this.clock.Restart();
            }
            //physics.frozen = true;

            for (int i = 0; i < objects.Count; ++i)
            {
                //there exists a better way for this???
                rigidBodies[i] = objects[i].rigidBody;

                if (!objects[i].InsideWindow(viewCenter, windowHalfSize))
                    continue;

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

        public void Draw(RenderWindow window, float alpha, Vector2f viewCenter)
        {   
            if(status == GameStatus.Active) {
                shadow.SetParameter("lightPosition", lightPosition.X / planet.Length, 
                    1 - lightPosition.Y / HEIGHT);
                shadowBuffer.Clear(Color.Transparent);
                sceneBuffer.Clear(Color.Transparent);
                foreach (GameObject obj in objects)
                {
                    obj.Draw(sceneBuffer, alpha, viewCenter, windowHalfSize);
                    obj.Draw(shadowBuffer, alpha, viewCenter, windowHalfSize);
                    if (debug)
                        obj.rigidBody.Draw(sceneBuffer, alpha, viewCenter, windowHalfSize);
                }
                RenderStates s = new RenderStates(Transform.Identity);
                s.Shader = light;
                window.Draw(planet.backgroundSprite, s);

                sceneBufferShader.SetParameter("rt_scene", shadowBuffer.Texture);
                s.Shader = sceneBufferShader;
                shadowBuffer.Draw(shadowScene, s);
                shadowBuffer.Display();

                shadow.SetParameter("texture", shadowScene.Texture);
                s.Shader = shadow;
                window.Draw(shadowScene, s);

                sceneBuffer.Display();
                window.Draw(scene);
            }
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
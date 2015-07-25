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
        public static bool screenShake = false;
        private const int MAXSHAKERATE = 50;
        private int shakeRate = MAXSHAKERATE;
        private int level;
        private const int MAXLEVEL = 2;
        public Player player;
        public bool levelEnded;
        public GameStatus status;
        public static Vector2f playerPos;
        public static Vector2f levelSize;
        private Portal portal;
        private int requiredScore;
        private int numberOfEnemies;
        private float killPercentage;
        private int killedEnemies;
        public static View view;

        Shader light = new Shader(null, "../Content/shaders/light.frag");
        Shader sceneBufferShader = new Shader(null, "../Content/shaders/scene_buffer.frag");
        Shader shadow = new Shader(null, "../Content/shaders/shadow.frag");

        RenderTexture shadowBuffer;
        RenderTexture shadowBufferQuaterSize;
        RenderTexture sceneBuffer;
        Sprite shadowScene = new Sprite();
        Sprite scene = new Sprite();
        Vector2f windowHalfSize;
        Vector3f lightPosition;


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
            view = new View(new Vector2f(WIDTH, HEIGHT) *.5f, new Vector2f(WIDTH, HEIGHT));
        }


        public void startGame()
        {
            this.Reset();
            this.levelEnded = false;
            Level = 2;
            player = new Player(factions[1], new Vector2f(250, 1250), "../Content/ghostSpriteBlueGlow.png",
                    new int[] { 100, 100 }, new int[] { 100, 1200 }, new int[] { 0 });
            playerPos = player.rigidBody.COM;
            Add(player);
            this.status = GameStatus.Active;
        }

        public void NextLevel()
        {
            this.Reset();
            this.levelEnded = false;
            this.status = GameStatus.Active;
            Level++;
            player.rigidBody.COM = new Vector2f(250, 1250);
            Add(player);
        }

        public static void Add(GameObject obj)
        {
            objects.Add(obj);
            rigidBodies.Add(obj.rigidBody);
        }

        public static void AddJoint(Constraint joint)
        {
            joints.Add(joint);
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
                this.killPercentage = planet.KillPercentage;
                this.requiredScore = planet.RequiredPoints;
                this.portal = new Portal(Collision.Type.Polygon, planet.PortalOpen, planet.PortalClosed, planet.PortalTileSize, new int[] { 0 }, 0, 0, 0, 0, 0, planet.PortalSprite, planet.PortalSpriteSize, new Vector2f(planet.PortalPosition[0], planet.PortalPosition[1]), 0);
                Add(portal);
                levelSize = new Vector2f(planet.Size[0], planet.Size[1]);

                lightPosition = new Vector3f(WIDTH*2, HEIGHT * 0.5f, 0.04f);

                light.SetParameter("lightPosition", 
                    lightPosition.X, HEIGHT - lightPosition.Y, lightPosition.Z);
                light.SetParameter("resolution",
                    planet.Size[0] * 10, HEIGHT * 10);

                shadowBuffer = new RenderTexture((uint)WIDTH, (uint)HEIGHT);
                sceneBuffer = new RenderTexture((uint)WIDTH, (uint)HEIGHT);
                shadowBufferQuaterSize = new RenderTexture((uint) WIDTH / 2, (uint) HEIGHT / 2);
                shadowScene.Texture = shadowBuffer.Texture;
                scene.Texture = sceneBuffer.Texture;
            }
            physics = new Physic(rigidBodies, joints, new Vector2f(planet.Gravity[0], planet.Gravity[1]), planet.Damping,
                new Vector2f(WIDTH, HEIGHT));

            Add(new Wall(new Vector2f(0, 1), new Vector2f(levelSize.X * .5f, 0),
                new Vector2f(levelSize.X, .1f), Color.Transparent));
            Add(new Wall(new Vector2f(0,-1), new Vector2f(levelSize.X * .5f, levelSize.Y),
                new Vector2f(levelSize.X, .1f), Color.Transparent));
            Add(new Wall(new Vector2f( 1,0), new Vector2f(0, levelSize.Y * .5f), 
                new Vector2f(.1f, levelSize.Y), Color.Transparent));
            Add(new Wall(new Vector2f(-1,0), new Vector2f(levelSize.X, levelSize.Y * .5f), 
                new Vector2f(.1f, levelSize.Y), Color.Transparent));
            planet.AddGround();
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
            using (StreamReader sr = new StreamReader("../Content/" + level + "/Pendulums.json"))
            {
                PendulumContract[] pendulums;
                String json = sr.ReadToEnd();
                pendulums = JSONManager.deserializeJson<PendulumContract[]>(json);
                for (int i = 0; i < pendulums.Length; ++i)
                    pendulums[i].Init();
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
                this.numberOfEnemies = enemies.Length;
                for (int i = 0; i < enemies.Length; i++)
                {
                    enemies[i].Init();
                }
            }
            using (StreamReader sr = new StreamReader("../Content/" + level + "/Coins.json"))
            {
                CoinContract[] coins;
                String json = sr.ReadToEnd();
                coins = JSONManager.deserializeJson<CoinContract[]>(json);
                this.numberOfEnemies = coins.Length;
                for (int i = 0; i < coins.Length; i++)
                {
                    coins[i].Init();
                }
            }
        }

        public void FixedUpdate(float dt)
        {
            if (status == GameStatus.Active)
                physics.Update(dt, view.Center);
        }

        public void EarlyUpdate()
        {
            if (this.player.score >= this.requiredScore && this.killedEnemies >= this.numberOfEnemies*(this.killPercentage/100)) {
                this.portal.Open();
            }
            for (int i = 0; i < objects.Count; ++i)
                if(objects[i].InsideWindow(view.Center, windowHalfSize))
                    objects[i].EarlyUpdate();
            for (int i = 0; i < spawners.Count; i++)
            {
                if (spawners[i].NearPlayer(playerPos.X, WIDTH * .5f))
                {
                    spawners[i].Spawn();
                    spawners.RemoveAt(i);
                }
            }
        }

        public void LateUpdate() 
        {
            if (physics.frozen) 
                return;

            playerPos = player.rigidBody.COM;

            view.Center = playerPos;
            if (view.Center.X < windowHalfSize.X)
                view.Center = new Vector2f(windowHalfSize.X, view.Center.Y);
            if (view.Center.X > Game.levelSize.X - windowHalfSize.X)
                view.Center = new Vector2f(Game.levelSize.X - windowHalfSize.X, view.Center.Y);
            if (view.Center.Y < windowHalfSize.Y)
                view.Center = new Vector2f(view.Center.X, windowHalfSize.Y);
            if (view.Center.Y > Game.levelSize.Y - windowHalfSize.Y)
                view.Center = new Vector2f(view.Center.X, Game.levelSize.Y - windowHalfSize.Y);

            if (screenShake)
            {
                view.Center += new Vector2f(EMath.random.Next(shakeRate), EMath.random.Next(shakeRate));
                shakeRate--;
            }

            if (shakeRate < 0)
            {
                screenShake = false;
                shakeRate = MAXSHAKERATE;
            }

            for (int i = 0; i < objects.Count; ++i)
            {
                //there exists a better way for this???
                rigidBodies[i] = objects[i].rigidBody;

                if (!objects[i].InsideWindow(view.Center, windowHalfSize))
                    continue;

                objects[i].LateUpdate();

                if(!objects[i].display) {
                    KillableObject killable = objects[i] as KillableObject;
                    if (killable != null)
                    {
                        player.score += killable.points;
                        if (objects[i] is Enemy)
                            this.killedEnemies++;
                    }
                    objects.RemoveAt(i);
                    rigidBodies.RemoveAt(i);
                }
            }

            if (player.hp <= 0)
            {
                this.status = GameStatus.Credits;
            }

            if(this.portal.entered) {
                if (this.level + 1 <= MAXLEVEL)
                {
                    this.status = GameStatus.Nextlevel;
                }
                else if (this.level + 1 > MAXLEVEL)
                {
                    this.status = GameStatus.Credits;
                }
            }
        }

        public void Draw(RenderWindow window, float alpha)
        {   
            if(status == GameStatus.Active) {
                shadowBuffer.Clear(Color.Transparent);
                sceneBuffer.Clear(Color.Transparent);
                shadowBuffer.SetView(view);
                sceneBuffer.SetView(view);

                planet.sky.Position = view.Center;
                sceneBuffer.Draw(planet.sky);
                foreach (GameObject obj in objects)
                {
                    obj.Draw(sceneBuffer, alpha, view.Center, windowHalfSize);
                    obj.Draw(shadowBuffer, alpha, view.Center, windowHalfSize);
                    if (debug)
                        obj.rigidBody.Draw(sceneBuffer, alpha, view.Center, windowHalfSize);
                }

           //     shadow.SetParameter("lightPosition", lightPosition.X / planet.Size[0],
           //         1 - lightPosition.Y / HEIGHT);

                RenderStates s = new RenderStates(Transform.Identity);

                sceneBufferShader.SetParameter("sceneTex", shadowBuffer.Texture);
                s.Shader = sceneBufferShader;
                shadowBuffer.Display();

           //     shadow.SetParameter("texture", shadowScene.Texture);
           //     s.Shader = light;
            //    s.Shader = shadow;
            //    window.Draw(shadowScene, s);


             //   shadowBuffer.Display();
                sceneBuffer.Display();
                window.Draw(scene);
         /*       shadowScene.Scale = new Vector2f(0.5f, 0.5f);
                shadowBufferQuaterSize.Clear();
                shadowBufferQuaterSize.Draw(shadowScene, s);
                shadowBufferQuaterSize.Display();
                window.Draw(new Sprite(shadowBufferQuaterSize.Texture));
                scene.Scale = new Vector2f(1,1);*/
            }
        }

        public void MovePlayer(Keyboard.Key k)
        {
            player.Move(k);
        }

        public void Pause()
        {
            this.physics.frozen = !this.physics.frozen;
        }

        public void Reset()
        {
            objects.Clear();
            rigidBodies.Clear();
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

    }
}
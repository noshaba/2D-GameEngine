﻿using System;
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
        private const float MOONRADIUS = 150;
        private Vector2f MOONPOSUV = new Vector2f(0.5f,0.6f);
        private Color MOONCOLOR = new Color(100,145,200,100);
        private Color SKYCOLOR = new Color(0,30,97);
        private int level;
        private const int MAXLEVEL = 3;
        public Player player;
        public bool levelEnded;
        public GameStatus status;
        public static Vector2f playerPos;
        public static Vector2f levelSize;
        private Portal portal;
        private int requiredScore;
        private int numberOfEnemies;
        private float killPercentage;
        private float killedEnemies;
        public static View view;
        private CircleShape moon;

        Shader stencilShader = new Shader(null, "../Content/shaders/scene_buffer.frag");
        Shader lightShader = new Shader(null, "../Content/shaders/light.frag");

        RenderTexture lightBuffer;
        RenderTexture lightBufferQuaterSize;
        RenderTexture sceneBuffer;
        Sprite lightBufferSprite = new Sprite();
        Sprite sceneBufferSprite = new Sprite();
        Vector2f windowHalfSize;


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
         
            this.status = GameStatus.Start;

            SoundManager.Play(SoundManager.ambient);
            SoundManager.ambient.Loop = true;
            view = new View(new Vector2f(WIDTH, HEIGHT) *.5f, new Vector2f(WIDTH, HEIGHT));
            moon = new CircleShape(MOONRADIUS, 100);
            moon.Origin = new Vector2f(MOONRADIUS, MOONRADIUS);
            moon.FillColor = MOONCOLOR;
            stencilShader.SetParameter("moonPosition", MOONPOSUV.X, MOONPOSUV.Y);
            stencilShader.SetParameter("moonRadius", MOONRADIUS);
            stencilShader.SetParameter("resolution", WIDTH, HEIGHT);
            stencilShader.SetParameter("moonColour", MOONCOLOR.R/255f, MOONCOLOR.G/255f, MOONCOLOR.B/255f);
            stencilShader.SetParameter("skyColour", SKYCOLOR.R/255f, SKYCOLOR.G/255f, SKYCOLOR.B/255f);
            lightShader.SetParameter("lightPosition", MOONPOSUV.X, MOONPOSUV.Y);
        }


        public void startGame()
        {
            this.Reset();
            this.levelEnded = false;
            Level = 1;
            player = new Player(factions[1], new Vector2f(150, 850), "../Content/ghostSpriteBlueGlow.png",
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
            this.killedEnemies = 0;
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
                this.killPercentage = planet.KillPercentage / 100.0f;
                this.requiredScore = planet.RequiredPoints;
                this.portal = new Portal(Collision.Type.Polygon, planet.PortalOpen, planet.PortalClosed, planet.PortalTileSize, new int[] { 0 }, 0, 0, 0, 0, 0, planet.PortalSprite, planet.PortalSpriteSize, new Vector2f(planet.PortalPosition[0], planet.PortalPosition[1]), 0);
                Add(portal);
                levelSize = new Vector2f(planet.Size[0], planet.Size[1]);

                lightBuffer = new RenderTexture((uint)WIDTH, (uint)HEIGHT);
                sceneBuffer = new RenderTexture((uint)WIDTH, (uint)HEIGHT);
                lightBufferQuaterSize = new RenderTexture((uint) WIDTH / 4, (uint) HEIGHT / 4);
                lightBufferSprite.Texture = lightBuffer.Texture;
                sceneBufferSprite.Texture = sceneBuffer.Texture;
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
            if (this.player.score >= this.requiredScore)
            {
                if (this.numberOfEnemies == 0)
                {
                    this.portal.Open();
                } else if ((this.killedEnemies / this.numberOfEnemies) >= this.killPercentage)
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
                lightBuffer.Clear(Color.Transparent);
                sceneBuffer.Clear();
                lightBuffer.SetView(view);
                sceneBuffer.SetView(view);

                planet.sky.Position = view.Center;
                sceneBuffer.Draw(planet.sky);
                
                moon.Position = view.Center - windowHalfSize + new Vector2f(WIDTH * MOONPOSUV.X, HEIGHT * (1 - MOONPOSUV.Y));
                sceneBuffer.Draw(moon);

                foreach (GameObject obj in objects)
                {
                    obj.Draw(sceneBuffer, alpha, view.Center, windowHalfSize);
                    obj.Draw(lightBuffer, alpha, view.Center, windowHalfSize);
                    if (debug)
                        obj.rigidBody.Draw(sceneBuffer, alpha, view.Center, windowHalfSize);
                }
                RenderStates s = new RenderStates(Transform.Identity);
                // stencil image
                stencilShader.SetParameter("sceneTex", lightBuffer.Texture);
                s.Shader = stencilShader;
                lightBuffer.Display();
                // scale stenciled image to 4th of it's resolution
                lightBufferSprite.Scale = new Vector2f(0.25f, 0.25f);
                lightBufferQuaterSize.Clear();
                lightBufferQuaterSize.Draw(lightBufferSprite, s);
                lightBufferQuaterSize.Display();
                lightBufferQuaterSize.Texture.Smooth = true;
                // use the god's ray shader on the stenciled image with lower resultion
                lightShader.SetParameter("texture", lightBufferQuaterSize.Texture);
                s.Shader = lightShader;
                sceneBuffer.Display();
                sceneBuffer.SetView(window.GetView());
                sceneBuffer.Draw(sceneBufferSprite, s);
                // draw the whole scene
                window.Draw(sceneBufferSprite);
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

        //a)-> items not in use and b) the effects of the items belong into item class
        /*
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
        }*/

    }
}
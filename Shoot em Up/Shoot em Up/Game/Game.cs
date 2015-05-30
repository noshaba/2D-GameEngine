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

        public Player player;

        private Stopwatch clock;
        public GameStatus status;
        public int level;
        private int maxLevel = 2; //number of levels to play(../Content/levels for available levels)
        private LevelManager progressor;
        public bool levelEnded;
        public int numberOfFoes = 0;

        #region enums
        public enum GameStatus
        {
            Active, Welcome, Credits
        }

        public enum GameItem
        {
            Heal, Bomb
        }

#endregion
        
        //constructor
        public Game(int width, int height) {
            WIDTH = width;
            HEIGHT = height;
            physics = new Physic(shapes, new Vector2f(0, 0), 0, false);

            FactionManager.LoadJSON();

            this.clock = new Stopwatch();
            this.status = GameStatus.Welcome;
            this.progressor = new LevelManager(this);
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
                        //there exists a better way for this???
                        shapes[i] = objects[i].rigidBody;
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
                            if (objects[i] is Enemy)
                            {
                                AddItem((objects[i] as Enemy).drop, objects[i].rigidBody.COM);
                            }
                            if((objects[i] is Enemy || objects[i] is Astroid)) {
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
            this.level = 1;
            this.levelEnded = false;
            this.numberOfFoes = 0;
            this.player = new Player(FactionManager.factions[(int) Faction.Type.Player],new Vector2f(this.WIDTH / 2, this.HEIGHT-40), new Texture("../Content/ships/1.png"));
            AddObject(player);
            //AddObject(new Item("../Content/item.png", heal));
            this.clock.Restart();
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
                } else if (this.level == this.maxLevel )
                {
                    //System.Threading.Thread.Sleep(3000);
                    this.status = GameStatus.Credits;
                }
            }
        }

        #region ItemFunctions
        private void heal()
        {
            this.player.hp += 150;
        }

        private void bomb()
        {
            this.player.hp -= 150;
        }
        #endregion

        #region ObjectAdders

        public static void AddObject(GameObject obj)
        {
            objects.Add(obj);
            shapes.Add(obj.rigidBody);
        }
        public void GenerateAstroid(float x, float y) {
            Random r = new Random();
            int no = r.Next(1,5);
            AddObject(new Astroid(FactionManager.factions[(int)Faction.Type.None], new Texture("../Content/astroids/"+no+".png"), new Vector2f(x,y), EMath.Random(0,360)));
        }

        public void AddItem(Game.GameItem item, Vector2f pos)
        {
            switch (item)
            {
                case Game.GameItem.Heal: AddObject(new Item("../Content/items/heal.png", heal, pos));
                    break;
                case Game.GameItem.Bomb: AddObject(new Item("../Content/items/bomb.png", bomb, pos));
                    break;
                default:
                    break;

            }
        }

        public void AddEnemy(float x, float y)
        {
            AddObject(new Enemy(FactionManager.factions[(int)Faction.Type.AI], new Vector2f(x, y), "5", 250, 10, 20, "sideToSide",  new Color(99,0,78)));
        }

        public void AddMeanEnemy(float x, float y)
        {
            AddObject(new Enemy(FactionManager.factions[(int)Faction.Type.AI], new Vector2f(x, y), "5", 500, 10, 30, "path",  new Color(99,0,78)));
        }
        #endregion

        #region InputHandlers
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



        public void Pause()
        {
            if (status == GameStatus.Active)
            {
                this.player.fire = false;
                this.physics.frozen = !this.physics.frozen;
            }
        }
        #endregion
        public void Reset()
        {
            objects.RemoveRange(0,objects.Count);
            shapes.RemoveRange(0,shapes.Count);
        }

    }
}

using Physics;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class Platform : GameObject
    {
       Vector2f globalPosition;
       private bool breakable;
       private Vector2f[] positions;
       private int currentPosition;
       private int nextPosition;
       private bool moveable;
       private int[] tileSize;
       private int[] spriteSize;
       private string imagePath;
       private int[] animation;
       private int[] tiles;
       private float density;
       // SpritePath, Position, Rotation, SpriteSize, SpriteTileSize, Tiles, KineticFriction, StaticFriction, Restitution, Density
       public Platform(int[] animation, bool breakable, bool rotateable, int[] movement, String path, Vector2f position, float rotation, int[] spriteSize, int[] tileSize, int[] tiles,
           float kineticFriction, float staticFriction, float restitution, float density)
            : base(path, tileSize, spriteSize, tiles, 0, position, rotation, density)
        {
            this.tileSize = tileSize;
            this.spriteSize = spriteSize;
            this.imagePath = path;
            this.tiles = tiles;
            this.density = density;
            this.DetermineMoves(movement);
            this.breakable = breakable;
            globalPosition = position;
            this.animated = true;
            this.animation = animation;
            this.currentState = new AnimState(animation, this);
            foreach (Body body in rigidBodies)
            {
                body.rotateable = rotateable;
                body.moveable = false;
                body.Restitution = restitution;
                body.StaticFriction = staticFriction;
                body.KineticFriction = kineticFriction;
                body.Parent = this;
                for (int i = 0; i < body.bodies.Length; ++i)
                {
                    body.bodies[i].COM += new Vector2f(i * tileSize[0], 0);
                }
                body.UpdateCentroid();
                body.COM = position;
                body.UpdateBoundingCircle();
            }
        }

        public void Shatter()
        {
            if (this.breakable)
            {
                SoundManager.Play(SoundManager.shatter);
                Game.screenShake = true;
                Game.Remove(this);

                Vector2f pos;
                for (int i = 0; i < rigidBody.bodies.Length; ++i)
                {
                    pos = rigidBody.bodies[i].Center;
                    rigidBody.bodies[i].Centroid = -rigidBody.bodies[i].Centroid;
                 /*   Game.Add(new Obstacle(false, true, animation, this.tileSize, new int[] { this.tiles[i] }, 0, density, 
                        rigidBody.Restitution, rigidBody.StaticFriction, rigidBody.KineticFriction, imagePath, spriteSize, 
                        pos, rigidBody.bodies[i].DegOrientation, 100, 0, 100, Game.factions[0]));*/
                    Game.Add(new KillableObject(Game.factions[0], 100, 100, new[] { rigidBody.bodies[i] }, new[] { drawables[0][i] }, pos, 0));
                }
            }
            else
            {
                SoundManager.Play(SoundManager.rebound);
            }
        }

        private void DetermineMoves(int[] movement)
        {
            if(movement.Length < 2) {
                this.moveable = false;
                return;
            }
            this.moveable = true;
            this.positions = new Vector2f[movement.Length / 2];
            int i = 0;
            for(int j = 0; j< movement.Length; j+=2) {
                this.positions[i] = new Vector2f(movement[j], movement[j+1]);
                i++;
            }
            this.currentPosition = 0;
            this.nextPosition = 1;
        }

        //needs some adjustments, speed has to be defined in json and then has to be calculated correctly taking the distnace bewteen the two points into account
        private void Move() {
            Vector2f direction = new Vector2f(positions[nextPosition].X - positions[currentPosition].X, positions[nextPosition].Y - positions[currentPosition].Y);
            if (this.rigidBody.COM.X >= positions[nextPosition].X * 0.99f && this.rigidBody.COM.X <= positions[nextPosition].X * 1.01f
                && this.rigidBody.COM.Y >= positions[nextPosition].Y * 0.99f && this.rigidBody.COM.Y <= positions[nextPosition].Y * 1.01f)
            {
                if (nextPosition+1 < positions.Length)
                {
                    currentPosition = nextPosition;
                    nextPosition = currentPosition + 1;
                }
                else 
                {
                    currentPosition = nextPosition;
                    nextPosition = 0;
                }
            }
            else {
                this.rigidBody.Velocity = new Vector2f(direction.X / 20, direction.Y / 20);
            }
           /* if (this.currentPosition < positions.Length)
            {
                //get currentPosition
                //get nextPosition
                direction = new Vector2f( positions[currentPosition+1].X - positions[currentPosition].X, positions[currentPosition+1].Y- positions[currentPosition].Y );
                if (this.rigidBody.COM.X <= positions[currentPosition + 1].X * 1.05f && this.rigidBody.COM.X >= positions[currentPosition + 1].X * 0.95f)
                {
                    this.currentPosition++;
                }
            }
            else if(currentPosition > 0){
                direction = new Vector2f(positions[0].X - positions[currentPosition].X, positions[0].Y - positions[currentPosition].Y);
                //nextPosition is at 0
                if (this.rigidBody.COM.X <= positions[0].X * 1.05f && this.rigidBody.COM.X >= positions[0].X * 0.95f)
                {
                    this.currentPosition = 0;
                }
            }
            this.rigidBody.Velocity = new Vector2f(direction.X/20, direction.Y/20);*/
            //calculate direction
            //set velocity in that direction
        }

        public override void EarlyUpdate()
        {
            base.EarlyUpdate();
            if (this.moveable)
            {
                this.Move();
                base.EarlyUpdate();
            }
        }
    }
}

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
       private bool moveable;
       // SpritePath, Position, Rotation, SpriteSize, SpriteTileSize, Tiles, KineticFriction, StaticFriction, Restitution, Density
       public Platform(bool breakable, bool rotateable, int[] movement, String path, Vector2f position, float rotation, int[] spriteSize, int[] tileSize, int[] tiles,
           float kineticFriction, float staticFriction, float restitution, float density)
            : base(path, tileSize, spriteSize, tiles, 0, position, rotation,density)
        {
            this.DetermineMoves(movement);
            this.breakable = breakable;
            this.Rotateable = rotateable;
            globalPosition = position;
            foreach (Body body in rigidBodies)
            {
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
                Game.Remove(this);

                Vector2f pos;
                for (int i = 0; i < rigidBody.bodies.Length; ++i)
                {
                    pos = rigidBody.bodies[i].Center;
                    rigidBody.bodies[i].Centroid = -rigidBody.bodies[i].Centroid;
                    Game.Add(new KillableObject(Game.factions[0], 500, 100, new[] { rigidBody.bodies[i] }, new[] { drawables[0][i] }, pos, 0));
                }
            }
        }

        private void DetermineMoves(int[] movement)
        {
            if(movement.Length < 1) {
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
        }

        private void Move() {
            this.rigidBody.Velocity = new Vector2f(10,0);
           /* if (this.currentPosition < positions.Length)
            {
                //get currentPosition
                //get nextPosition
            }
            else { 
                //nextPosition is at 0
            }*/
            //calculate direction
            //set velocity in that direction
        }

        public override void EarlyUpdate()
        {
            if (this.moveable)
            {
                this.Move();
                base.EarlyUpdate();
            }
        }
    }
}

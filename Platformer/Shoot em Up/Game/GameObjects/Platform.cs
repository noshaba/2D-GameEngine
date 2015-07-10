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
        //string texturePath, int[] spriteTileSize, int[] spriteSize, int[] tileIndices, int animationIndex, Vector2f position, float rotation, float density
        public Platform(String path, Vector2f position, float rotation, int tileSize, int[] tiles)
            : base(path, new[] {tileSize,tileSize}, new[] {300,100}, tiles, 0, position, rotation,0)
        {
            foreach (Body body in rigidBodies)
            {
                for (int i = 0; i < body.bodies.Length; ++i)
                {
                    body.bodies[i].COM += new Vector2f(i * tileSize, 0);
                }
                body.UpdateCentroid();
                body.COM = position;
                body.UpdateBoundingCircle();
            }
        }
    }
}

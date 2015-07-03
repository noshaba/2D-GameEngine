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
           
        }
    }
}

using ImageProcessing;
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
    class PlatformContract
    {
        IRigidBody[] bodies;
        public void Init()
        {
            String path = "../Content/platform.png";
            Vector2f pos = new Vector2f(800,500);
            int tileSize = 100;
            int[] tiles = new int[] { 0,1,1,1,1,2};

            /*
            //number of platforms in json
            for (int i = 0; i < 1; i++)
            {
                bodies = new IRigidBody[tiles.Length];
                //number of tiles
                for (int j = 0; j < tiles.Length; j++)
                {
                    //tileSize * j needs to be tileSize * tileIndex e.g. content of tile Array
                    Console.WriteLine(tiles[j] * 100);
                    Texture tile = new Texture(path, new IntRect(tileSize*tiles[j], 0, tileSize, tileSize));
                    
                    bodies[j] = new Polygon(CV.AlphaEdgeDetection(tile.CopyToImage().Pixels, tile.Size.X, tile.Size.Y, 254), new Vector2f(tileSize/2,tileSize/2), new Vector2f(pos.X+j*100,pos.Y), 0, 0);
                }
            }*/
            Game.Add(new Platform(path, pos, 0, tileSize, tiles));
        }
    }
}

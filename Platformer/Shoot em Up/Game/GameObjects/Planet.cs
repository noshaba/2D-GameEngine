using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using SFML.Graphics;
using SFML.System;

namespace Platformer
{
    [DataContract]
    class Planet
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string BackgroundMusicPath { get; set; }
        [DataMember]
        public float[] Gravity { get; set; }
        [DataMember]
        public float Damping { get; set; }
        [DataMember]
        public int[] Size { get; set; }
        [DataMember]
        public byte[] SkyColor { get; set; }
        [DataMember]
        public GroundType[] GroundTypes { get; set; }
        [DataMember]
        public int[] Ground { get; set; }
        [DataMember]
        public int[] GroundTiles { get; set; }
        [DataMember]
        public int[] GroundTileHeight { get; set; }

        public Sprite sky;

        public void Init()
        {
            RenderTexture buffer = new RenderTexture((uint)Size[0], (uint)Size[1]);
            RectangleShape colorRect = new RectangleShape(new Vector2f(Size[0], Size[1]));
            colorRect.FillColor = Color.Red;
            buffer.Clear();
            buffer.Draw(colorRect);
            buffer.Display();
            sky = new Sprite(buffer.Texture);
        }

        public void AddGround()
        {
           for (int i = 0; i < Ground.Length; ++i)
            {
               Game.Add(GroundTypes[Ground[i]].GetTile(i, GroundTiles[i], GroundTileHeight[i]));
               for (int j = 0; j < GroundTileHeight[i] - 1; ++j)
                   Game.Add(GroundTypes[Ground[i]].GetTile(i, 0, j+1));
            }
        }
    }
}

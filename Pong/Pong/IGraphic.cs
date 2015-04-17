using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace Pong {
    interface IGraphic{
        bool Displayed { get; set; }
        void Draw(RenderWindow window);
    }
}

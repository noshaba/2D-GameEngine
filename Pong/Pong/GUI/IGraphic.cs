using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Pong {
    interface IGraphic{
        bool Displayed { get; set; }
        void Draw(RenderWindow window);
        void Released(float X, float Y);
        void Pressed(float X, float Y);
        void OnHover(float X, float Y);
        Vector2f Position { get; set; }
        IGraphic ParentView { get; set; }
    }
}

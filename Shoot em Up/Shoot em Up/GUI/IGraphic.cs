using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace Shoot_em_Up {
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

using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    class Label : Text 
    {
        public Label(String text)
        {
            this.DisplayedString = text;
            this.CharacterSize = 12;
            this.Color = Color.Black;
            this.Font = new Font("../Content/arial.ttf");
        }

        public Label(Vector2f position, String text)
        {
            this.DisplayedString = text;
            this.Position = position;
            this.CharacterSize = 12;
            this.Color = Color.Black;
        }

    }
}

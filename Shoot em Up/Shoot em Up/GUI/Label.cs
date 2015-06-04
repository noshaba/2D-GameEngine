using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    class Label : Text, IGraphic
    {

        private bool displayed = true;
        private IGraphic parentView;
        public Label(String text, Color c)
        {
            this.DisplayedString = text;
            this.CharacterSize = 18;
            this.Color = c;
            this.Font = new Font("../Content/arial.ttf");
        }

        public Label(Vector2f position, String text, Color c, uint size)
        {
            this.Position = position;
            this.DisplayedString = text;
            this.CharacterSize = size;
            this.Color = c;
            this.Font = new Font("../Content/arial.ttf");
        }

        public Label(Vector2f position, String text, Color c)
        {
            this.DisplayedString = text;
            this.Position = new Vector2f(this.Position.X + this.GetGlobalBounds().Left / 2, this.Position.Y);
            this.CharacterSize = 18;
            this.Color = c;
            this.Font = new Font("../Content/arial.ttf");
        }

        public IGraphic ParentView
        {
            get { return parentView; }
            set { parentView = value; }
        }

        public bool Displayed
        {
            get { return displayed; }
            set { displayed = value; }
        }

        public void Draw(RenderWindow window)
        {
            if (displayed)
                window.Draw(this);
        }

        public void Released(float X, float Y)
        {

        }

        new public void Pressed(float X, float Y)
        {

        }

        new public void OnHover(float X, float Y)
        {

        }

    }
}

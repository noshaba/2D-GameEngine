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

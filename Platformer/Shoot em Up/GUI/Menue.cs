using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUI
{
    //unfinished - needs work - includes lots of workarounds and unneeded repititions
    class Menue : GUIElement, IGraphic
    {
        private List<IGraphic> elements = new List<IGraphic>();
        private bool displayed;
        private IGraphic parentView;

        public Menue(Vector2f position, Vector2f size, Color c)
            : base((uint)position.X, (uint)position.Y, (uint)size.X, (uint)size.Y, "../Content/ButtonActive.png", c)
        {
            displayed = true;
            Position = position;
        }

        new public bool Displayed
        {
            get { return displayed; }
            set { displayed = value; }
        }
        new public void Draw(RenderWindow window)
        {
            if (this.displayed) {
                window.Draw(this);
                foreach (IGraphic elem in elements)
                    elem.Draw(window);
            }
        }

        new public void Released(float X, float Y)
        {
            if (displayed && GetGlobalBounds().Contains(X, Y))
            {
                foreach (IGraphic elem in elements)
                    elem.Released(X, Y);
            }
        }

        new public void Pressed(float X, float Y)
        {
            if (displayed && GetGlobalBounds().Contains(X, Y))
            {
                foreach (IGraphic elem in elements)
                    elem.Pressed(X, Y);
            }
        }

        new public void OnHover(float X, float Y)
        {
            if (displayed && GetGlobalBounds().Contains(X, Y))
            {
                foreach (IGraphic elem in elements)
                    elem.OnHover(X, Y);
            }
        }
        new public IGraphic ParentView
        {
            get { return parentView; }
            set { parentView = value; }
        }

        public void Add(IGraphic graphic)
        {
            graphic.Position += Position;
            graphic.ParentView = this;
            elements.Add(graphic);
        }
    }
}

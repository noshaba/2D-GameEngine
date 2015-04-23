using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pong
{
    class Menue : RectangleShape, IGraphic
    {
        private List<IGraphic> elements = new List<IGraphic>();
        private bool displayed;
        private IGraphic parentView;

        public Menue(Vector2f position, Vector2f size, Color c)
            : base(size)
        {
            displayed = false;
            FillColor = c;
            Position = position;
        }

        public bool Displayed
        {
            get { return displayed; }
            set { displayed = value; }
        }
        public void Draw(RenderWindow window)
        {
            if (this.displayed) {
                window.Draw(this);
                foreach (IGraphic elem in elements)
                    elem.Draw(window);
            }
        }

        public void Released(float X, float Y)
        {
            if (displayed && GetGlobalBounds().Contains(X, Y))
            {
                foreach (IGraphic elem in elements)
                    elem.Released(X, Y);
            }
        }

        public void Pressed(float X, float Y)
        {
            if (displayed && GetGlobalBounds().Contains(X, Y))
            {
                foreach (IGraphic elem in elements)
                    elem.Pressed(X, Y);
            }
        }

        public void OnHover(float X, float Y)
        {
            if (displayed && GetGlobalBounds().Contains(X, Y))
            {
                foreach (IGraphic elem in elements)
                    elem.OnHover(X, Y);
            }
        }
        public IGraphic ParentView
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

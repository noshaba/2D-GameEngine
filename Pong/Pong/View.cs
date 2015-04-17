using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Pong {
    class View : RectangleShape, IGraphic{
        private List<IGraphic> children = new List<IGraphic>();
        private bool displayed;

        public View(Vector2f position, Vector2f size) : base(size) {
            displayed = true;
        }

        public bool Displayed{
            get { return displayed; }
            set { 
                displayed = value; 
                foreach (IGraphic child in children) {
                    child.Displayed = value;
                }
            }
        }

        public void Add(IGraphic graphic) {
            graphic.Position += Position;
            children.Add(graphic);
        }

        public void Remove(IGraphic graphic) {
            children.Remove(graphic);
        }

        public void Remove(int i) {
            children.RemoveAt(i);
        }

        public void Draw(RenderWindow window) {
            foreach (IGraphic child in children)
                child.Draw(window);
        }

        public void Released(float X, float Y) {
            if (displayed) {
                foreach (IGraphic child in children)
                    child.Released(X, Y);
            }
        }

        public void Pressed(float X, float Y) {
            if (displayed) {
                foreach (IGraphic child in children)
                    child.Pressed(X, Y);
            }
        }
    }
}

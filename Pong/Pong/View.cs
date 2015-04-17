using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace Pong {
    class View : RectangleShape, IGraphic{
        private List<IGraphic> children = new List<IGraphic>();
        private bool displayed;

        public View() : base() {
            displayed = true;
        }

        public bool Displayed{
            get { return displayed; }
            set { displayed = value; }
        }

        public void Add(IGraphic graphic) {
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
    }
}

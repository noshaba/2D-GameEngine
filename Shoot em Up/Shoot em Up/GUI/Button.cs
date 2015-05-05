using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using Shoot_em_Up;

namespace GUI {
    // TODO: change rectangleshape to sprite later..
    class Button : GUIElement, IGraphic {
        public enum Status {
            Pressed, Released, OnHover
        }
        public delegate void ActionListener();
        public Status status;
        public Label label;
        public ActionListener listener;
        private bool displayed;
        private IGraphic parentView;

        public Button(Vector2f position, Vector2f size, ActionListener listener) : base((uint)position.X, (uint)position.Y, (uint)size.X, (uint)size.Y, "../Content/ButtonActive.png") {
            this.listener = listener;
           
            status = Status.Released;
            displayed = true;
        }

        public Button(uint x, uint y, String text, ActionListener listener) : base(x,y)
        {
            this.listener = listener;
            status = Status.Released;
            displayed = true;

            this.label = new Label(text);
            FloatRect labelPos = this.label.GetLocalBounds();

            uint margin = 5;

            this.height = (uint)labelPos.Height + margin*2;
            this.width = (uint)labelPos.Width + margin*2;

            if (this.height < 10)
                this.height = 10 + margin*2;

            if (this.width < 10)
                this.width = 10 + margin * 2;

            this.path = "../Content/ButtonActive.png";
            this.img = new Texture(path);
            
            this.label.Color = Color.Black;

            float py = (this.height - labelPos.Height) / 2 - 2;
            float px = (this.width - labelPos.Width) / 2;

            this.label.Position = new Vector2f(x+px, y+py);

            this.scaleImage();
            
        }

        public IGraphic ParentView {
            get { return parentView; }
            set { parentView = value; }
        }

        public bool Displayed {
            get { return displayed; }
            set { displayed = value; }
        }

        // for sprite
        // public Button(Vector2f position, string file) : base() {  
        // }

        public void Draw(RenderWindow window) {
            if (displayed)
                window.Draw(this);
                if (this.label != null)
                window.Draw(this.label);
        }

        public void Released(float X, float Y) {
            if (displayed && GetGlobalBounds().Contains(X, Y)) {
                status = Status.Released;
                if (listener != null)
                    listener();
            }
        }

        public void Pressed(float X, float Y) {
            if (displayed && GetGlobalBounds().Contains(X, Y))
                status = Status.Pressed;
        }

        public void OnHover(float X, float Y) {
            if (displayed) {
                if (GetGlobalBounds().Contains(X, Y))
                    status = Status.OnHover;
                else
                    status = Status.Released;
            }
        }
    }
}

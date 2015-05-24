using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace GUI {
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

        //Button manual size
        public Button(Vector2f position, Vector2f size, String text, uint cSize, ActionListener listener) : base((uint)position.X, (uint)position.Y, (uint)size.X, (uint)size.Y, "../Content/ButtonActive.png") {
            this.listener = listener;
            this.label = new Label(text);
            this.label.Color = Color.Black;
            this.label.CharacterSize = cSize;
            this.centerLabel(this.label.GetLocalBounds(), position, size);
            //this.label.Position = position;
            status = Status.Released;
            displayed = true;
        }

        //Button auto-size
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
                this.width = 10 + margin*2;

            this.path = "../Content/ButtonActive.png";
            this.img = new Texture(path);
            
            this.label.Color = Color.Black;

            float py = this.height/2 - (labelPos.Height/2+labelPos.Top);
            float px = this.width/2 - (labelPos.Width/2+labelPos.Left);
            this.label.Position = new Vector2f(x+px, y+py);

            this.ScaleImage();
            
        }

        private void centerLabel(FloatRect labelSize, Vector2f btnPos, Vector2f btnSize)
        {
            float y = ((btnSize.Y / 2)-(labelSize.Height/2+labelSize.Top)) + btnPos.Y;
            float x = ((btnSize.X / 2) - (labelSize.Width / 2 + labelSize.Left)) + btnPos.X;
            this.label.Position = new Vector2f(x, y);
        }

        new public IGraphic ParentView {
            get { return parentView; }
            set { parentView = value; }
        }

        new public bool Displayed {
            get { return displayed; }
            set { displayed = value; }
        }

        new public void Draw(RenderWindow window) {
            if (displayed)
                window.Draw(this);
            if (this.label != null)
                this.label.Draw(window);
        }

        new public void Released(float X, float Y) {
            if (displayed && GetGlobalBounds().Contains(X, Y)) {
                status = Status.Released;
                if (listener != null)
                    listener();
            }
        }

        new public void Pressed(float X, float Y) {
            if (displayed && GetGlobalBounds().Contains(X, Y))
                status = Status.Pressed;
        }

        new public void OnHover(float X, float Y) {
            if (displayed) {
                if (GetGlobalBounds().Contains(X, Y))
                    status = Status.OnHover;
                else
                    status = Status.Released;
            }
        }
    }
}

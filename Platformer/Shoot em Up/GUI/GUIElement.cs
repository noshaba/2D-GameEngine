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
    class GUIElement : Transformable, Drawable
    {
        protected Texture img;
        protected uint width;
        protected uint height;
        private IGraphic parentView;
        private RectangleShape topLeftDest;
        private RectangleShape topCenterDest;
        private RectangleShape topRightDest;
        private RectangleShape centerLeftDest;
        private RectangleShape centerCenterDest;
        private RectangleShape centerRightDest;
        private RectangleShape bottomLeftDest;
        private RectangleShape bottomCenterDest;
        private RectangleShape bottomRightDest;
        protected RectangleShape image;
        protected String path;
        public Color color;

        public GUIElement(Color c)
        {
            this.color = c;
        }

        public GUIElement(uint x, uint y, Color c)
        {
            this.Position = new Vector2f(x, y);
            this.color = c;
        }
        public GUIElement(uint x, uint y, uint w, uint h, String path, Color c)
        {
            this.color = c;
            if (w <10) {
                w = 10;
            }
            if (h <10) {
                h = 10;
            }
            this.width = w;
            this.height = h;
            this.path = path;
            this.img = new Texture(path);
            this.Position = new Vector2f(x,y);
            this.ScaleImage();
        }


        public virtual void Draw(RenderTarget target, RenderStates states)
        {
            if (this.topLeftDest != null)
            {
                target.Draw(this.topLeftDest, states);
                target.Draw(this.topCenterDest, states);
                target.Draw(this.topRightDest, states);
                target.Draw(this.centerLeftDest, states);
                target.Draw(this.centerCenterDest, states);
                target.Draw(this.centerRightDest, states);
                target.Draw(this.bottomLeftDest, states);
                target.Draw(this.bottomCenterDest, states);
                target.Draw(this.bottomRightDest, states);
            }
            else
            {
                target.Draw(this.image, states);
            }
        }

        public bool Displayed { get; set; }
        public void Draw(RenderWindow window)
        {
            RenderStates r = new RenderStates(BlendMode.Alpha);
            RenderTarget t = window;
            this.Draw(t,r);
        }
        public void Released(float X, float Y)
        {

        }
        public void Pressed(float X, float Y)
        {

        }
        public void OnHover(float X, float Y)
        {

        }
        public IGraphic ParentView
        {
            get { return parentView; }
            set { parentView = value; }
        }

        protected void ScaleImage()
        {
            Vector2u size = this.img.Size;

            uint width = size.X;
            uint height = size.Y;
            uint margin = 4;

            //calculate slice coordinates on Image

            uint leftX = 0;
            uint rightX = width - margin;
            uint centerX = 0 + margin;

            uint topY = 0;
            uint bottomY = height - margin;
            uint centerY = margin;

            uint topHeight = margin;
            uint bottomHeight = margin;
            uint centerHeight = height - margin*2; 

            uint leftWidth = margin;
            uint rightWidth = margin;
            uint centerWidth = width - margin*2;

            //determine slice bounds on Image

            IntRect topLeftSrc = new IntRect((int)leftX, (int)topY, (int)leftWidth, (int)topHeight);
            IntRect topCenterSrc = new IntRect((int)centerX, (int)topY, (int)centerWidth, (int)topHeight);
            IntRect topRightSrc = new IntRect((int)rightX, (int)topY, (int)rightWidth, (int)topHeight);

            IntRect bottomLeftSrc = new IntRect((int)leftX, (int)bottomY, (int)leftWidth, (int)bottomHeight);
            IntRect bottomCenterSrc = new IntRect((int)centerX, (int)bottomY, (int)centerWidth, (int)bottomHeight);
            IntRect bottomRightSrc = new IntRect((int)rightX, (int)bottomY, (int)rightWidth, (int)bottomHeight);

            IntRect centerLeftSrc = new IntRect((int)leftX, (int)centerY, (int)leftWidth, (int)centerHeight);
            IntRect centerCenterSrc = new IntRect((int)centerX, (int)centerY, (int)centerWidth, (int)centerHeight);
            IntRect centerRightSrc = new IntRect((int)rightX, (int)centerY, (int)rightWidth, (int)centerHeight);

            //calculate slice positions on the GUI element
           
            //x positions for left, right and center slices
            leftX = 0 + (uint)this.Position.X;
            rightX = this.width - margin + (uint)this.Position.X;
            centerX = margin + (uint)this.Position.X;

            //y positions for top, bottom and center slices
            topY = 0 + (uint)this.Position.Y;
            bottomY = this.height - margin + (uint)this.Position.Y;
            centerY = margin + (uint)this.Position.Y;

            //heights for left, right and center slices
            topHeight = margin;
            bottomHeight = margin;
            centerHeight = this.height - margin*2; 

            //widths for top, bottom and center slices
            leftWidth = margin;
            rightWidth = margin;
            centerWidth = this.width - margin*2;

            //determine slice bounds on GUIElement
            this.topLeftDest = new RectangleShape(new Vector2f(leftWidth, topHeight));
            this.topLeftDest.Position = new Vector2f(leftX, topY);
            this.topCenterDest = new RectangleShape(new Vector2f(centerWidth, topHeight));
            this.topCenterDest.Position = new Vector2f(centerX, topY);
            this.topRightDest = new RectangleShape(new Vector2f(rightWidth, topHeight));
            this.topRightDest.Position = new Vector2f(rightX, topY);

            this.bottomLeftDest = new RectangleShape(new Vector2f(leftWidth, bottomHeight));
            this.bottomLeftDest.Position = new Vector2f(leftX, bottomY);
            this.bottomCenterDest = new RectangleShape(new Vector2f(centerWidth, bottomHeight));
            this.bottomCenterDest.Position = new Vector2f(centerX, bottomY);
            this.bottomRightDest = new RectangleShape(new Vector2f(rightWidth, bottomHeight));
            this.bottomRightDest.Position = new Vector2f(rightX, bottomY);

            this.centerLeftDest = new RectangleShape(new Vector2f(leftWidth, centerHeight));
            this.centerLeftDest.Position = new Vector2f(leftX, centerY);
            this.centerCenterDest = new RectangleShape(new Vector2f(centerWidth, centerHeight));
            this.centerCenterDest.Position = new Vector2f(centerX, centerY);
            this.centerRightDest = new RectangleShape(new Vector2f(rightWidth, centerHeight));
            this.centerRightDest.Position = new Vector2f(rightX, centerY);
            
            //place slices on GUIElement
            this.topLeftDest.Texture = new Texture(path, topLeftSrc);
            this.topCenterDest.Texture = new Texture(path, topCenterSrc);
            this.topRightDest.Texture = new Texture(path, topRightSrc);
            this.centerLeftDest.Texture = new Texture(path, centerLeftSrc);
            this.centerCenterDest.Texture = new Texture(path, centerCenterSrc);
            this.centerRightDest.Texture = new Texture(path, centerRightSrc);
            this.bottomLeftDest.Texture = new Texture(path, bottomLeftSrc);
            this.bottomCenterDest.Texture = new Texture(path, bottomCenterSrc);
            this.bottomRightDest.Texture = new Texture(path, bottomRightSrc);

            this.topLeftDest.FillColor = this.color;
            this.topCenterDest.FillColor = this.color;
            this.topRightDest.FillColor = this.color;
            this.centerLeftDest.FillColor = this.color;
            this.centerCenterDest.FillColor = this.color;
            this.centerRightDest.FillColor = this.color;
            this.bottomLeftDest.FillColor = this.color;
            this.bottomCenterDest.FillColor = this.color;
            this.bottomRightDest.FillColor = this.color;

        }

        public FloatRect GetGlobalBounds()
        {
            return new FloatRect(this.Position.X, this.Position.Y, this.width, this.height);
        }
    }
}

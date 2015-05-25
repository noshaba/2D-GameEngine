﻿using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUI
{
    class Screen : GUIElement, IGraphic
    {

        private bool displayed;
        private IGraphic parentView;
        public Screen(uint x, uint y, String src) : base(x,y)
        {
            this.img = new Texture(src);
            this.image = new RectangleShape(new Vector2f(this.img.Size.X, this.img.Size.Y));
            this.image.Texture = this.img;
            this.displayed = true;
        }

        new public IGraphic ParentView
        {
            get { return parentView; }
            set { parentView = value; }
        }

        new public bool Displayed
        {
            get { return displayed; }
            set { displayed = value; }
        }

        new public void Draw(RenderWindow window)
        {
            if (displayed)
                window.Draw(this);
        }

        new public void Released(float X, float Y)
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
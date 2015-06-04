using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using Maths;

namespace Shoot_em_Up
{
    class ImpulseSlider
    {
        private RectangleShape sliderBar;
        private RectangleShape slider;
        private float min, max, minY, maxY;
        private Vector2f direction;

        public ImpulseSlider(Texture sliderTxt, Texture buttonTxt, float min, float max)
        {
            this.sliderBar = new RectangleShape((Vector2f) sliderTxt.Size);
            this.sliderBar.Origin = new Vector2f(sliderTxt.Size.X * .5f, sliderTxt.Size.Y * .5f);
            this.sliderBar.Position = new Vector2f(50, 100 + sliderBar.Size.Y * .5f);
            this.sliderBar.Texture = sliderTxt;
            this.slider = new RectangleShape((Vector2f) buttonTxt.Size);
            this.slider.Origin = new Vector2f(buttonTxt.Size.X * .5f, buttonTxt.Size.Y * .5f);
            this.slider.Position = new Vector2f(sliderBar.Position.X, sliderBar.Position.Y + sliderBar.Size.Y * .5f - slider.Size.Y * .5f);
            this.slider.Texture = buttonTxt;
            this.min = min;
            this.max = max;
            this.minY = this.slider.Position.Y;
            this.maxY = this.sliderBar.Position.Y - sliderBar.Size.Y * .5f;
            this.direction = new Vector2f(0,-500);
        }

        public void Update(float dt)
        {
            this.slider.Position += dt * this.direction;
            if (this.slider.Position.Y < this.maxY)
            {
                this.slider.Position = new Vector2f(this.slider.Position.X, this.maxY);
                this.direction = -this.direction;
            }
            if (this.slider.Position.Y > this.minY)
            {
                this.slider.Position = new Vector2f(this.slider.Position.X, this.minY);
                this.direction = -this.direction;
            }
        }

        public float Value
        {
            get { return (slider.Position - new Vector2f(0,minY)).Length() * (this.max - this.min) / this.sliderBar.Size.Y ;}
        }

        public void Draw(RenderWindow window) 
        {
            window.Draw(sliderBar);
            window.Draw(slider);
        }
    }
}

using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shoot_em_Up
{
    class Item : GameObject
    {
        public delegate void ActionListener();
        public ActionListener listener;
        public Item(String texture, ActionListener listener): base(new Texture(texture), new Vector2f(200,400), 1, 1) 
        {
            this.listener = listener;
            this.drawable.Texture = new Texture(texture);
        }

        public override void Update()
        {
            base.Update();
            if (rigidBody.Collision.collision)
            {
                this.display = false;
                this.listener();
            }
        }
    }
}

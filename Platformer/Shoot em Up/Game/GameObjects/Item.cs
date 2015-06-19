using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    class Item : GameObject
    {
        public delegate void ActionListener();
        private Vector2f initPos;
        public ActionListener listener;
        public Item(String texture, ActionListener listener, Vector2f pos)
            : base(new Texture(texture), pos, 1, 0.01f)
        {
            this.listener = listener;
            this.initPos = pos;
            this.rigidBody.DragCoefficient = 2;
        }

        public override void Update()
        {
            base.Update();
            //this.rigidBody.COM = this.initPos;
            if (rigidBody.Collision.collision && rigidBody.Collision.obj.Parent is Player)
            {
                this.display = false;
                this.listener();
            }
        }
    }
}
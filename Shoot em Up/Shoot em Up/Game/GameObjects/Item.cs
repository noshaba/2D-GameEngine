﻿using SFML.Graphics;
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
        public Item(String texture, ActionListener listener, Vector2f pos)
            : base(new Texture(texture), pos, 1, 0.01f)
        {
            this.listener = listener;
            this.drawable.Texture = new Texture(texture);
            this.rigidBody.DragCoefficient = 2;
        }

        public override void Update()
        {
            base.Update();
            if (rigidBody.Collision.collision && rigidBody.Collision.obj.Parent is Player)
            {
                this.display = false;
                this.listener();
            }
        }
    }
}
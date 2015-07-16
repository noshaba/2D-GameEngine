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
        //string texturePath, int[] spriteTileSize, int[] spriteSize, int[] tileIndices, int animationIndex, Vector2f position, float rotation, float density
        public Item(String texturePath, int[] spriteTileSize, int[] spriteSize, int[] tileIndices, int animationIndex, Vector2f position, float rotation, float density, ActionListener listener)
            : base(texturePath, spriteTileSize, spriteSize, tileIndices, animationIndex, position, rotation, density)
        {
            this.listener = listener;
            this.initPos = position;
            this.rigidBody.DragCoefficient = 2;
        }

        public override void EarlyUpdate()
        {
            base.EarlyUpdate();
            //this.rigidBody.COM = this.initPos;
            if (rigidBody.Collision.collision && rigidBody.Collision.obj is Player)
            {
                this.display = false;
                this.listener();
            }
        }
    }
}
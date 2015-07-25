using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Physics;

namespace Platformer
{
    //Not used here but works
    class Item : GameObject
    {
        public delegate void ActionListener();
        private Vector2f initPos;
        public ActionListener listener;
        public Item(String texturePath, int[] spriteTileSize, int[] spriteSize, int[] tileIndices, int animationIndex, Vector2f position, float rotation, float density, ActionListener listener)
            : base(texturePath, spriteTileSize, spriteSize, tileIndices, animationIndex, position, rotation, density)
        {
            RigidBodyParent = this;
            this.listener = listener;
            this.initPos = position;
            this.rigidBody.DragCoefficient = 2;
        }

        public override void EarlyUpdate()
        {
            base.EarlyUpdate();
            foreach(Collision collision in rigidBody.Collision)
                if (collision.obj is Player)
                {
                    this.display = false;
                    this.listener();
                }
        }
    }
}
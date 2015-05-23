﻿using Physics;
using SFML.Graphics;
using SFML.System;
using System;
using System.Text;
using System.Threading.Tasks;
using ImageProcessing;

namespace Shoot_em_Up
{
    class GameObject
    {
        public bool display = true;
        public IRigidBody rigidBody;
        public Shape drawable;

        public GameObject(IRigidBody rigidBody)
        {
            this.rigidBody = rigidBody;
            this.drawable = rigidBody as Shape;
            this.rigidBody.Parent = this; 
        }

        public GameObject(Collision.Type type, Vector2f position, float var, float density)
        {
            switch (type)
            {
                case Collision.Type.Circle: 
                    this.rigidBody = new Circle(position, var, density);
                    this.drawable = this.rigidBody as Shape;
                    break;
                case Collision.Type.Polygon:
                    this.rigidBody = new Polygon(position, var, density);
                    this.drawable = this.rigidBody as Shape;
                    break;
            }
            this.rigidBody.Parent = this;
        }

        public GameObject(Vector2f normal, Vector2f position, Vector2f size, float rotation)
        {
            this.rigidBody = new Plane(normal, position, size, rotation);
            this.drawable = this.rigidBody as Shape;
            this.rigidBody.Parent = this;
        }

        public GameObject(Vector2f[] vertices, Vector2f position, float rotation)
        {
            this.rigidBody = new Polygon(vertices, position, rotation);
            this.drawable = this.rigidBody as Shape;
            this.rigidBody.Parent = this;
        }

        public GameObject(Texture texture, Vector2f position, float rotation)
        {
            this.rigidBody = new Polygon(CV.AlphaEdgeDetection(texture.CopyToImage().Pixels, texture.Size.X, texture.Size.Y, 0), position, rotation);
            this.rigidBody.Parent = this;
            this.drawable = new RectangleShape((Vector2f)texture.Size);
            this.drawable.Origin = new Vector2f(texture.Size.X * .5f, texture.Size.Y * .5f);
        }

        public virtual void Update() {
        }

        public virtual void LateUpdate() {
            rigidBody.Collision.collision = false;
        }
    }
}

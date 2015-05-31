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
        public IRigidBody[] rigidBodies;
        public Shape[] drawables;
        public IRigidBody rigidBody;
        public Shape drawable;

        public GameObject(IRigidBody rigidBody)
        {
            this.rigidBodies = new IRigidBody[] { rigidBody };
            this.drawables = new Shape[] { rigidBody as Shape };
            this.rigidBodies[0].Parent = this;
            this.rigidBody = rigidBodies[0];
            this.drawable = this.drawables[0];
        }

        public GameObject(Vector2f normal, Vector2f position, Vector2f size, float rotation)
        {
            this.rigidBodies = new IRigidBody[] { new Plane(this, normal, position, size, rotation) };
            this.drawables = new Shape[] { this.rigidBodies[0] as Shape };
            this.rigidBody = rigidBodies[0];
            this.drawable = this.drawables[0];
        }

        public GameObject(Vector2f position, float rotation, float radius)
        {
            this.rigidBodies = new IRigidBody[] { new Circle(this, position, rotation, radius) };
            this.drawables = new Shape[] { this.rigidBodies[0] as Shape };
            this.rigidBody = rigidBodies[0];
            this.drawable = this.drawables[0];        
        }

        public GameObject(Vector2f position, float rotation, float radius, float density)
        {
            this.rigidBodies = new IRigidBody[] { new Circle(this, position, rotation, radius, density) };
            this.drawables = new Shape[] { this.rigidBodies[0] as Shape };
            this.rigidBody = rigidBodies[0];
            this.drawable = this.drawables[0];
        }

        public GameObject(Vector2f[] vertices, Vector2f position, float rotation)
        {
            this.rigidBodies = new IRigidBody[] { new Polygon(this, vertices, position, rotation) };
            this.drawables = new Shape[] { this.rigidBodies[0] as Shape };
            this.rigidBody = rigidBodies[0];
            this.drawable = this.drawables[0];
        }

        public GameObject(Vector2f[] vertices, Vector2f position, float rotation, float density)
        {
            this.rigidBodies = new IRigidBody[] { new Polygon(this, vertices, position, rotation, density) };
            this.drawables = new Shape[] { this.rigidBodies[0] as Shape };
            this.rigidBody = rigidBodies[0];
            this.drawable = this.drawables[0];
        }

        public GameObject(Texture texture, Vector2f position, float rotation)
        {
            this.rigidBodies = new IRigidBody[] { new Polygon(this, CV.AlphaEdgeDetection(texture.CopyToImage().Pixels, texture.Size.X, texture.Size.Y, 0), position, rotation) };
            this.drawables = new Shape[] { new RectangleShape((Vector2f)texture.Size) };
            for (int i = 0; i < drawables.Length; ++i)
            {
                this.drawables[i].Origin = this.rigidBodies[i].Centroid;
                this.drawables[i].Texture = texture;
                this.drawables[i].Texture.Smooth = true;
            }
            this.rigidBody = rigidBodies[0];
            this.drawable = this.drawables[0];
            Console.WriteLine(texture.CopyToImage().Pixels.Length);
        }

        public GameObject(Texture texture, Vector2f position, float rotation, float density)
        {
            this.rigidBodies = new IRigidBody[] { new Polygon(this, CV.AlphaEdgeDetection(texture.CopyToImage().Pixels, texture.Size.X, texture.Size.Y, 0), position, rotation, density) };
            this.drawables = new Shape[] { new RectangleShape((Vector2f)texture.Size) };
            for (int i = 0; i < drawables.Length; ++i)
            {
                this.drawables[i].Origin = this.rigidBodies[i].Centroid;
                this.drawables[i].Texture = texture;
                this.drawables[i].Texture.Smooth = true;
            }
            this.rigidBody = rigidBodies[0];
            this.drawable = this.drawables[0];
        }



        public virtual void Update() {
        }

        public virtual void LateUpdate() {
            rigidBody.Collision.collision = false;
        }
    }
}

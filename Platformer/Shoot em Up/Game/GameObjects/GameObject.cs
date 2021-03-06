﻿using Physics;
using SFML.Graphics;
using SFML.System;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ImageProcessing;

namespace Platformer
{
    class GameObject
    {
        public bool display = true;
        public Body[] rigidBodies;
        public Shape[][] drawables;
        public Body rigidBody;
        public Shape[] drawable;
        public int animationFrame;
        public bool animated = false;
        public AnimState currentState;
        public float speed;
        private Stopwatch clock = new Stopwatch();

        public bool Moveable
        {
            get { return rigidBody.moveable; }
            set
            {
                foreach (Body body in rigidBodies)
                    body.moveable = value;
            }
        }

        public bool Rotateable
        {
            get { return rigidBody.rotateable; }
            set
            {
                foreach (Body body in rigidBodies)
                    body.rotateable = value;
            }
        }

        public float Restitution
        {
            get { return rigidBody.Restitution; }
            set
            {
                foreach (Body body in rigidBodies)
                    body.Restitution = value;
            }
        }

        public float DragCoefficient
        {
            get { return rigidBody.DragCoefficient; }
            set
            {
                foreach (Body body in rigidBodies)
                    body.DragCoefficient = value;
            }
        }

        public float KineticFriction
        {
            get { return rigidBody.KineticFriction; }
            set
            {
                foreach (Body body in rigidBodies)
                    body.KineticFriction = value;
            }
        }

        public float StaticFriction
        {
            get { return rigidBody.StaticFriction; }
            set
            {
                foreach (Body body in rigidBodies)
                    body.StaticFriction = value;
            }
        }

        public object RigidBodyParent
        {
            get { return rigidBody.Parent; }
            set
            {
                foreach (Body body in rigidBodies)
                    body.Parent = value;
            }
        }

        public GameObject(Vector2f normal, Vector2f position, Vector2f size, float rotation)
        {
            this.rigidBodies = new Body[] { new Body(this, new[] { new Plane(normal, new Vector2f(), size, 0) }, position, rotation) };
            this.drawables = new Shape[][] { Array.ConvertAll(rigidBodies[0].bodies, body => (Shape)body) };
            this.rigidBody = rigidBodies[0];
            this.drawable = this.drawables[0];
            this.clock.Start();
        }

        public GameObject(IRigidBody[] bodies, Vector2f position, float rotation)
        {
            this.rigidBodies = new Body[] { new Body(this, bodies, position, rotation) };
            this.drawables = new Shape[][] { Array.ConvertAll(rigidBodies[0].bodies, body => (Shape)body) };
            this.rigidBody = rigidBodies[0];
            this.drawable = this.drawables[0];
            this.clock.Start();
        }

        public GameObject(IRigidBody[] bodies, Shape[] shapes, Vector2f position, float rotation)
        {
            this.rigidBodies = new Body[] { new Body(this, bodies, position, rotation) };
            this.drawables = new Shape[][] { shapes };
            this.rigidBody = rigidBodies[0];
            this.drawable = this.drawables[0];
            this.clock.Start();
        }

        public GameObject(string texturePath, int[] spriteTileSize, int[] spriteSize, int[] tileIndices, 
            int animationIndex, Vector2f position, float rotation, float density)
        {
            int cols = tileIndices.Length;
            int rows = spriteSize[1] / spriteTileSize[1];

            this.rigidBodies = new Body[rows];
            this.drawables = new Shape[rows][];
            Vector2f origin = new Vector2f(spriteTileSize[0] * .5f, spriteTileSize[1] * .5f);
            Texture tile;
            int tileIndex;
            for (int i = 0; i < rows; ++i)
            {
                IRigidBody[] bodies = new IRigidBody[cols];
                drawables[i] = new Shape[cols];
                for (int j = 0; j < cols; ++j)
                {
                    tileIndex = tileIndices[j];
                    tile = new Texture(texturePath,
                        new IntRect(tileIndex * spriteTileSize[0], i * spriteTileSize[1], 
                            spriteTileSize[0], spriteTileSize[1]));
                    bodies[j] = new Polygon(CV.AlphaEdgeDetection(tile.CopyToImage().Pixels, tile.Size.X, tile.Size.Y, 0), 
                        origin, new Vector2f(), 0, density);
                    drawables[i][j] = new RectangleShape(new Vector2f(spriteTileSize[0], spriteTileSize[1]));
                    drawables[i][j].Origin = origin;
                    drawables[i][j].Texture = tile;
                    drawables[i][j].Texture.Smooth = true;
                }
                rigidBodies[i] = new Body(bodies, position, rotation);
            }
            this.rigidBody = this.rigidBodies[animationIndex];
            this.drawable = this.drawables[animationIndex];
            this.clock.Start();
        }

        public bool InsideWindow(Vector2f center, Vector2f windowHalfSize)
        {
            return rigidBody.InsideWindow(center, windowHalfSize);
        }

        public virtual void EarlyUpdate()
        {
            if (this.animated)
            {
                this.currentState.HandleEvents();
                foreach (Body body in this.rigidBodies)
                {
                    body.Current = rigidBody.Current;
                    body.Previous = rigidBody.Previous;
                    for (int i = 0; i < body.bodies.Length; ++i)
                    {
                        body.bodies[i].Current = this.rigidBody.bodies[i].Current;
                        body.bodies[i].Previous = this.rigidBody.bodies[i].Previous;
                    }
                }
                this.rigidBody = this.rigidBodies[this.animationFrame];
                this.drawable = this.drawables[this.animationFrame];
            }
        }

        public virtual void LateUpdate()
        {
            if (this.clock.ElapsedMilliseconds > 100)
            {
                if (animated)
                    AdvanceAnim();
                this.clock.Restart();
            }
        }

        public void Draw(RenderTexture buffer, float alpha, Vector2f viewCenter, Vector2f windowHalfSize)
        {
            if (!InsideWindow(viewCenter, windowHalfSize))
                return;
            State interpol;
            Transform t;
            RenderStates r;
            for (int i = 0; i < rigidBody.bodies.Length; ++i)
            {
                interpol = rigidBody.bodies[i].Interpolation(alpha);
                t = Transform.Identity;
                t.Translate(rigidBody.bodies[i].Center);
                t.Rotate(interpol.DegOrientation);
                r = new RenderStates(t);
                buffer.Draw(drawable[i], r);
            }
        }

        public void Draw(RenderWindow buffer, float alpha, Vector2f viewCenter, Vector2f windowHalfSize)
        {
            if (!InsideWindow(viewCenter, windowHalfSize))
                return;
            State interpol;
            Transform t;
            RenderStates r;
            for (int i = 0; i < rigidBody.bodies.Length; ++i)
            {
                interpol = rigidBody.bodies[i].Interpolation(alpha);
                t = Transform.Identity;
                t.Translate(rigidBody.bodies[i].Center);
                t.Rotate(interpol.DegOrientation);
                r = new RenderStates(t);
                buffer.Draw(drawable[i], r);
            }
        }

        public void AdvanceAnim()
        {
            if (this.currentState.index  < this.currentState.sequence.Length)
            {
                this.animationFrame = this.currentState.sequence[this.currentState.index++];
            }
            else
            {
                this.currentState.index = 0;
                this.animationFrame = this.currentState.sequence[this.currentState.index];
            }
        }
    }
}

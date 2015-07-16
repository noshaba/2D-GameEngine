using Physics;
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
        protected int animationFrame;
        protected AnimState[] states;

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
        }

        public GameObject(IRigidBody[] bodies, Vector2f position, float rotation)
        {
            this.rigidBodies = new Body[] { new Body(this, bodies, position, rotation) };
            this.drawables = new Shape[][] { Array.ConvertAll(rigidBodies[0].bodies, body => (Shape)body) };
            this.rigidBody = rigidBodies[0];
            this.drawable = this.drawables[0];
        }

        public GameObject(IRigidBody[] bodies, Shape[] shapes, Vector2f position, float rotation)
        {
            this.rigidBodies = new Body[] { new Body(this, bodies, position, rotation) };
            this.drawables = new Shape[][] { shapes };
            this.rigidBody = rigidBodies[0];
            this.drawable = this.drawables[0];
        }

        public GameObject(string texturePath, int[] spriteTileSize, int[] spriteSize, int[] tileIndices, int animationIndex, Vector2f position, float rotation, float density)
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
        }

        public bool InsideWindow(Vector2f center, Vector2f windowHalfSize)
        {
            return rigidBody.InsideWindow(center, windowHalfSize);
        }

        public virtual void EarlyUpdate()
        {
        }

        public virtual void LateUpdate()
        {
            rigidBody.Collision.collision = false;
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

        public void Draw(RenderTexture buffer, float alpha, Vector2f viewCenter, Vector2f windowHalfSize, Shader s)
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
                r.Shader = s;
                buffer.Draw(drawable[i], r);
            }
        }

        public void AdvanceAnim(int status)
        {
            this.animationFrame = this.states[status].sequence[this.states[status].index];

            if (this.states[status].index < this.states[status].sequence.Length - 1)
            {
                this.states[status].index++;
            }
            else
            {
                this.states[status].index = 0;
            }
        }
    }
}

using Physics;
using SFML.Graphics;
using SFML.System;
using System;
using System.Text;
using System.Threading.Tasks;
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

        public GameObject(Body rigidBody)
        {
            this.rigidBodies = new Body[] { rigidBody };
            this.drawables = new Shape[][] { Array.ConvertAll(rigidBody.bodies, body => (Shape)body) };
            this.rigidBodies[0].Parent = this;
            this.rigidBody = rigidBodies[0];
            this.drawable = this.drawables[0];
        }

        public GameObject(Vector2f normal, Vector2f position, Vector2f size, float rotation)
        {
            this.rigidBodies = new Body[] { new Body(this, new []{ new Plane(normal, new Vector2f(), size, 0)}, position, rotation) };
            this.drawables = new Shape[][] { Array.ConvertAll(rigidBodies[0].bodies, body => (Shape)body) };
            this.rigidBody = rigidBodies[0];
            this.drawable = this.drawables[0];
        }

        public GameObject(Vector2f position, float rotation, float radius)
        {
            this.rigidBodies = new Body[] { new Body(this, new []{ new Circle(new Vector2f(), 0, radius) }, position, rotation) };
            this.drawables = new Shape[][] { Array.ConvertAll(rigidBodies[0].bodies, body => (Shape)body) };
            this.rigidBody = rigidBodies[0];
            this.drawable = this.drawables[0];
        }

        public GameObject(Vector2f position, float rotation, float radius, float density)
        {
            this.rigidBodies = new Body[] { new Body(this, new []{ new Circle(new Vector2f(), 0, radius, density) }, position, rotation) };
            this.drawables = new Shape[][] { Array.ConvertAll(rigidBodies[0].bodies, body => (Shape)body) };
            this.rigidBody = rigidBodies[0];
            this.drawable = this.drawables[0];
        }

        public GameObject(Vector2f[] vertices, Vector2f position, float rotation, float density)
        {
            this.rigidBodies = new Body[] { new Body(this, new []{ new Polygon(vertices, new Vector2f(), 0, density) }, position, rotation) };
            this.drawables = new Shape[][] { Array.ConvertAll(rigidBodies[0].bodies, body => (Shape)body) };
            this.rigidBody = rigidBodies[0];
            this.drawable = this.drawables[0];
        }

        public GameObject(string texturePath, float radius, int[] spriteTileSize, int[] spriteSize, int animationIndex, Vector2f position, float rotation)
        {
            int tileNumber = (spriteSize[0] / spriteTileSize[0]) * (spriteSize[1] / spriteTileSize[1]);
            this.rigidBodies = new Body[tileNumber];
            this.drawables = new Shape[tileNumber][];
            Texture tile;
            for (int i = 0; i < tileNumber; ++i)
            {
                tile = new Texture(texturePath, new IntRect((i * spriteTileSize[0]) % spriteSize[0], (i * spriteTileSize[0]) / spriteSize[0] * spriteTileSize[1], spriteTileSize[0], spriteTileSize[1]));
                this.rigidBodies[i] = new Body(this, new[] { new Circle(new Vector2f(), 0, radius) }, position, rotation);
                this.drawables[i] = new[] { new RectangleShape(new Vector2f(spriteTileSize[0], spriteTileSize[1])) };
                this.drawables[i][0].Origin = this.rigidBodies[i].bodies[0].Center;
                this.drawables[i][0].Texture = tile;
                this.drawables[i][0].Texture.Smooth = true;
            }
            this.rigidBody = this.rigidBodies[animationIndex];
            this.drawable = this.drawables[animationIndex];
        }

        public GameObject(string texturePath, float radius, int[] spriteTileSize, int[] spriteSize, int animationIndex, Vector2f position, float rotation, float density)
        {
            int tileNumber = (spriteSize[0] / spriteTileSize[0]) * (spriteSize[1] / spriteTileSize[1]);
            this.rigidBodies = new Body[tileNumber];
            this.drawables = new Shape[tileNumber][];
            Texture tile;
            for (int i = 0; i < tileNumber; ++i)
            {
                tile = new Texture(texturePath, new IntRect((i * spriteTileSize[0]) % spriteSize[0], (i * spriteTileSize[0]) / spriteSize[0] * spriteTileSize[1], spriteTileSize[0], spriteTileSize[1]));
                this.rigidBodies[i] = new Body(this, new[] { new Circle(new Vector2f(), 0, radius, density) }, position, rotation);
                this.drawables[i] = new[] { new RectangleShape(new Vector2f(spriteTileSize[0], spriteTileSize[1])) };
                this.drawables[i][0].Origin = new Vector2f(spriteTileSize[0] * .5f, spriteTileSize[1] * .5f);
                this.drawables[i][0].Texture = tile;
                this.drawables[i][0].Texture.Smooth = true;
            }
            this.rigidBody = this.rigidBodies[animationIndex];
            this.drawable = this.drawables[animationIndex];
        }

        public GameObject(IRigidBody[] bodies, Vector2f position, float rotation)
        {
            this.rigidBodies = new Body[] { new Body(this, bodies, position, rotation) };
            this.drawables = new Shape[][] { Array.ConvertAll(rigidBodies[0].bodies, body => (Shape)body) };
           // this.rigidBodies[0].Parent = this;
            this.rigidBody = rigidBodies[0];
            this.drawable = this.drawables[0];
        }

      /*  public GameObject()
        {

        }*/

        public GameObject(string texturePath, int[] spriteTileSize, int[] spriteSize, int animationIndex, Vector2f position, float rotation, float density)
        {
            int tileNumber = (spriteSize[0] / spriteTileSize[0]) * (spriteSize[1] / spriteTileSize[1]);
            this.rigidBodies = new Body[tileNumber];
            this.drawables = new Shape[tileNumber][];
            Texture tile;
            for (int i = 0; i < tileNumber; ++i)
            {
                tile = new Texture(texturePath, new IntRect((i * spriteTileSize[0]) % spriteSize[0], (i * spriteTileSize[0]) / spriteSize[0] * spriteTileSize[1], spriteTileSize[0], spriteTileSize[1]));
                this.rigidBodies[i] = new Body(this, new [] { new Polygon(CV.AlphaEdgeDetection(tile.CopyToImage().Pixels, tile.Size.X, tile.Size.Y, 254), new Vector2f(), 0, density) }, position, rotation);
                this.drawables[i] = new[] { new RectangleShape(new Vector2f(spriteTileSize[0], spriteTileSize[1])) };
                this.drawables[i][0].Origin = this.rigidBodies[i].bodies[0].Center;
                this.drawables[i][0].Texture = tile;
                this.drawables[i][0].Texture.Smooth = true;
            }
            this.rigidBody = this.rigidBodies[animationIndex];
            this.drawable = this.drawables[animationIndex];
        }
        //Player constructor
        public GameObject(string texturePath, int[] spriteTileSize, int[] spriteSize, int animationIndex, Vector2f centroid, Vector2f position, float rotation, float density)
        {
            int tileNumber = (spriteSize[0] / spriteTileSize[0]) * (spriteSize[1] / spriteTileSize[1]);
            this.rigidBodies = new Body[tileNumber];
            this.drawables = new Shape[tileNumber][];
            Texture tile;
            Console.WriteLine(tileNumber);
            for (int i = 0; i < tileNumber; ++i)
            {
                tile = new Texture(texturePath, new IntRect((i * spriteTileSize[0]) % spriteSize[0], (i * spriteTileSize[0]) / spriteSize[0] * spriteTileSize[1], spriteTileSize[0], spriteTileSize[1]));
                this.rigidBodies[i] = new Body(this, new []{ new Polygon(CV.AlphaEdgeDetection(tile.CopyToImage().Pixels, tile.Size.X, tile.Size.Y, 254), centroid, new Vector2f(), 0, density) }, position, rotation);
                this.drawables[i] = new[] { new RectangleShape(new Vector2f(spriteTileSize[0], spriteTileSize[1])) };
                this.drawables[i][0].Origin = centroid;
                this.drawables[i][0].Texture = tile;
                this.drawables[i][0].Texture.Smooth = true;
            }
            this.rigidBody = this.rigidBodies[animationIndex];
            this.drawable = this.drawables[animationIndex];
        }

        public GameObject(Texture texture, Vector2f position, float rotation, float density)
        {
            this.rigidBody = new Body(this, new [] { new Polygon(CV.AlphaEdgeDetection(texture.CopyToImage().Pixels, texture.Size.X, texture.Size.Y, 0), new Vector2f(), 0, density) }, position, rotation);
            this.rigidBody.Parent = this;
            this.drawable = new [] { new RectangleShape((Vector2f)texture.Size) };
            this.drawable[0].Texture = texture;
            this.drawable[0].Origin = new Vector2f(texture.Size.X * .5f, texture.Size.Y * .5f);
        }


        public virtual void Update()
        {
        }

        public virtual void LateUpdate()
        {
            rigidBody.Collision.collision = false;
        }

        public void Draw(RenderWindow window, float alpha)
        {
            State interpol;
            Transform t;
            RenderStates r;
            for (int i = 0; i < rigidBody.bodies.Length; ++i)
            {
                interpol = rigidBody.bodies[i].Interpolation(alpha);
                t = Transform.Identity;
                t.Translate(interpol.position);
                t.Rotate(interpol.DegOrientation);
                r = new RenderStates(t);
                window.Draw(drawable[i], new RenderStates(t));
            }
        }
    }
}

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

       /* public GameObject(Vector2f[] vertices, Vector2f position, float rotation)
        {
            this.rigidBodies = new IRigidBody[] { new Polygon(this, vertices, position, rotation) };
            this.drawables = new Shape[] { this.rigidBodies[0] as Shape };
            this.rigidBody = rigidBodies[0];
            this.drawable = this.drawables[0];
        }*/

        public GameObject(Vector2f[] vertices, Vector2f position, float rotation, float density)
        {
            this.rigidBodies = new IRigidBody[] { new Polygon(this, vertices, position, rotation, density) };
            this.drawables = new Shape[] { this.rigidBodies[0] as Shape };
            this.rigidBody = rigidBodies[0];
            this.drawable = this.drawables[0];
        }

        public GameObject(string texturePath, float radius, int[] spriteTileSize, int[] spriteSize, int animationIndex, Vector2f position, float rotation)
        {
            int tileNumber = (spriteSize[0] / spriteTileSize[0]) * (spriteSize[1] / spriteTileSize[1]);
            this.rigidBodies = new IRigidBody[tileNumber];
            this.drawables = new Shape[tileNumber];
            Texture tile;
            for (int i = 0; i < tileNumber; ++i)
            {
                tile = new Texture(texturePath, new IntRect((i * spriteTileSize[0]) % spriteSize[0], (i * spriteTileSize[0]) / spriteSize[0] * spriteTileSize[1], spriteTileSize[0], spriteTileSize[1]));
                this.rigidBodies[i] = new Circle(this, position, rotation, radius);
                this.drawables[i] = new CircleShape(radius);
                this.drawables[i].Origin = this.rigidBodies[i].Centroid;
                this.drawables[i].Texture = tile;
                this.drawables[i].Texture.Smooth = true;
            }
            this.rigidBody = this.rigidBodies[animationIndex];
            this.drawable = this.drawables[animationIndex];
        }

        public GameObject(string texturePath, float radius, int[] spriteTileSize, int[] spriteSize, int animationIndex, Vector2f position, float rotation, float density)
        {
            int tileNumber = (spriteSize[0] / spriteTileSize[0]) * (spriteSize[1] / spriteTileSize[1]);
            this.rigidBodies = new IRigidBody[tileNumber];
            this.drawables = new Shape[tileNumber];
            Texture tile;
            for (int i = 0; i < tileNumber; ++i)
            {
                tile = new Texture(texturePath, new IntRect((i * spriteTileSize[0]) % spriteSize[0], (i * spriteTileSize[0]) / spriteSize[0] * spriteTileSize[1], spriteTileSize[0], spriteTileSize[1]));
                this.rigidBodies[i] = new Circle(this, position, rotation, radius, density);
                this.drawables[i] = new CircleShape(radius);
                this.drawables[i].Origin = this.rigidBodies[i].Centroid;
                this.drawables[i].Texture = tile;
                this.drawables[i].Texture.Smooth = true;
            }
            this.rigidBody = this.rigidBodies[animationIndex];
            this.drawable = this.drawables[animationIndex];
        }

        public GameObject(string texturePath, int[] spriteTileSize, int[] spriteSize, int animationIndex, Vector2f position, float rotation, float density)
        {
            int tileNumber = (spriteSize[0] / spriteTileSize[0]) * (spriteSize[1] / spriteTileSize[1]);
            this.rigidBodies = new IRigidBody[tileNumber];
            this.drawables = new Shape[tileNumber];
            Texture tile;
            for (int i = 0; i < tileNumber; ++i)
            {
                tile = new Texture(texturePath, new IntRect((i * spriteTileSize[0]) % spriteSize[0], (i * spriteTileSize[0]) / spriteSize[0] * spriteTileSize[1], spriteTileSize[0], spriteTileSize[1]));
                this.rigidBodies[i] = new Polygon(this, CV.AlphaEdgeDetection(tile.CopyToImage().Pixels, tile.Size.X, tile.Size.Y, 254), position, rotation, density);
                this.drawables[i] = new RectangleShape(new Vector2f(spriteTileSize[0], spriteTileSize[1]));
                this.drawables[i].Origin = this.rigidBodies[i].Centroid;
                this.drawables[i].Texture = tile;
                this.drawables[i].Texture.Smooth = true;
            }
            this.rigidBody = this.rigidBodies[animationIndex];
            this.drawable = this.drawables[animationIndex];
        }
        public GameObject(Texture texture, Vector2f position, float rotation, float density)
        {
            this.rigidBody = new Polygon(this, CV.AlphaEdgeDetection(texture.CopyToImage().Pixels, texture.Size.X, texture.Size.Y, 0), position, rotation, density);
            this.rigidBody.Parent = this;
            this.drawable = new RectangleShape((Vector2f)texture.Size);
            this.drawable.Origin = new Vector2f(texture.Size.X * .5f, texture.Size.Y * .5f);
        }



        public virtual void Update()
        {
        }

        public virtual void LateUpdate()
        {
            rigidBody.Collision.collision = false;
        }
    }
}

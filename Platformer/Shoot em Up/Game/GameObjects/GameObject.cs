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
        public GameObject(string texturePath, int[] spriteTileSize, int[] spriteSize, int animationIndex, Vector2f centroid, Vector2f position, float rotation, float density)
        {
            int tileNumber = (spriteSize[0] / spriteTileSize[0]) * (spriteSize[1] / spriteTileSize[1]);
            this.rigidBodies = new IRigidBody[tileNumber];
            this.drawables = new Shape[tileNumber];
            Texture tile;
            for (int i = 0; i < tileNumber; ++i)
            {
                tile = new Texture(texturePath, new IntRect((i * spriteTileSize[0]) % spriteSize[0], (i * spriteTileSize[0]) / spriteSize[0] * spriteTileSize[1], spriteTileSize[0], spriteTileSize[1]));
                this.rigidBodies[i] = new Polygon(this, CV.AlphaEdgeDetection(tile.CopyToImage().Pixels, tile.Size.X, tile.Size.Y, 254), centroid, position, rotation, density);
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
            this.drawable.Texture = texture;
            this.drawable.Origin = new Vector2f(texture.Size.X * .5f, texture.Size.Y * .5f);
        }
        public GameObject(String path, Vector2f position, Vector2f size, float rotation, float density)
        {
            Texture texture = ScaleImage(path, position, (uint)size.X, (uint)size.Y);
           // Texture texture = new Texture(path);
            this.rigidBody = new Polygon(this, CV.AlphaEdgeDetection(texture.CopyToImage().Pixels, texture.Size.X, texture.Size.Y, 0), position, rotation, density);
            this.rigidBody.Parent = this;
            this.drawable = new RectangleShape((Vector2f)texture.Size);
            this.drawable.Texture = texture;
            this.drawable.Origin = new Vector2f(texture.Size.X * .5f, texture.Size.Y * .5f);
        }

        protected Texture ScaleImage(String path, Vector2f position, uint tWidth, uint tHeight)
        {
            Vertex topLeftDest;
            RectangleShape topCenterDest;
            RectangleShape topRightDest;
            RectangleShape centerLeftDest;
            RectangleShape centerCenterDest;
            RectangleShape centerRightDest;
            RectangleShape bottomLeftDest;
            RectangleShape bottomCenterDest;
            RectangleShape bottomRightDest;

            Vector2u size = new Texture(path).Size;

            uint width = size.X;
            uint height = size.Y;
            uint margin = 20;

            //calculate slice coordinates on Image

            uint leftX = 0;
            uint rightX = width - margin;
            uint centerX = 0 + margin;

            uint topY = 0;
            uint bottomY = height - margin;
            uint centerY = margin;

            uint topHeight = margin;
            uint bottomHeight = margin;
            uint centerHeight = height - margin * 2;

            uint leftWidth = margin;
            uint rightWidth = margin;
            uint centerWidth = width - margin * 2;

            //determine slice bounds on Image

            IntRect topLeftSrc = new IntRect((int)leftX, (int)topY, (int)leftWidth, (int)topHeight);
            IntRect topCenterSrc = new IntRect((int)centerX, (int)topY, (int)centerWidth, (int)topHeight);
            IntRect topRightSrc = new IntRect((int)rightX, (int)topY, (int)rightWidth, (int)topHeight);

            IntRect bottomLeftSrc = new IntRect((int)leftX, (int)bottomY, (int)leftWidth, (int)bottomHeight);
            IntRect bottomCenterSrc = new IntRect((int)centerX, (int)bottomY, (int)centerWidth, (int)bottomHeight);
            IntRect bottomRightSrc = new IntRect((int)rightX, (int)bottomY, (int)rightWidth, (int)bottomHeight);

            IntRect centerLeftSrc = new IntRect((int)leftX, (int)centerY, (int)leftWidth, (int)centerHeight);
            IntRect centerCenterSrc = new IntRect((int)centerX, (int)centerY, (int)centerWidth, (int)centerHeight);
            IntRect centerRightSrc = new IntRect((int)rightX, (int)centerY, (int)rightWidth, (int)centerHeight);

            //calculate slice positions on the GUI element

            //x positions for left, right and center slices
            leftX = 0 + (uint)position.X;
            rightX = tWidth - margin + (uint)position.X;
            centerX = margin + (uint)position.X;

            //y positions for top, bottom and center slices
            topY = 0 + (uint)position.Y;
            bottomY = tHeight - margin + (uint)position.Y;
            centerY = margin + (uint)position.Y;

            //heights for left, right and center slices
            topHeight = margin;
            bottomHeight = margin;
            centerHeight = tHeight - margin * 2;

            //widths for top, bottom and center slices
            leftWidth = margin;
            rightWidth = margin;
            centerWidth = tWidth - margin * 2;

            //determine slice bounds on GUIElement
            topLeftDest = new Vertex(new Vector2f(leftWidth, topHeight));
            topLeftDest.Position = new Vector2f(leftX, topY);
            topCenterDest = new RectangleShape(new Vector2f(centerWidth, topHeight));
            topCenterDest.Position = new Vector2f(centerX, topY);
            topRightDest = new RectangleShape(new Vector2f(rightWidth, topHeight));
            topRightDest.Position = new Vector2f(rightX, topY);

            bottomLeftDest = new RectangleShape(new Vector2f(leftWidth, bottomHeight));
            bottomLeftDest.Position = new Vector2f(leftX, bottomY);
            bottomCenterDest = new RectangleShape(new Vector2f(centerWidth, bottomHeight));
            bottomCenterDest.Position = new Vector2f(centerX, bottomY);
            bottomRightDest = new RectangleShape(new Vector2f(rightWidth, bottomHeight));
            bottomRightDest.Position = new Vector2f(rightX, bottomY);

            centerLeftDest = new RectangleShape(new Vector2f(leftWidth, centerHeight));
            centerLeftDest.Position = new Vector2f(leftX, centerY);
            centerCenterDest = new RectangleShape(new Vector2f(centerWidth, centerHeight));
            centerCenterDest.Position = new Vector2f(centerX, centerY);
            centerRightDest = new RectangleShape(new Vector2f(rightWidth, centerHeight));
            centerRightDest.Position = new Vector2f(rightX, centerY);

            //place slices on GUIElement
            //topLeftDest.Texture = new Texture(path, topLeftSrc);
            topCenterDest.Texture = new Texture(path, topCenterSrc);
            topRightDest.Texture = new Texture(path, topRightSrc);
            centerLeftDest.Texture = new Texture(path, centerLeftSrc);
            centerCenterDest.Texture = new Texture(path, centerCenterSrc);
            centerRightDest.Texture = new Texture(path, centerRightSrc);
            bottomLeftDest.Texture = new Texture(path, bottomLeftSrc);
            bottomCenterDest.Texture = new Texture(path, bottomCenterSrc);
            bottomRightDest.Texture = new Texture(path, bottomRightSrc);


            return topCenterDest.Texture; //actually needs to be combined texture of the nine pieces
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

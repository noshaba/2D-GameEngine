using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Physics
{
    //Not in use yet, needs some work - especially concerning objects that touch more than one quadrant 
    class Quadtree
    {
        private int MAX_OBJECTS = 10;
        //private int MAX_HEIGHT = 1000;

        private int height;
        private List<IRigidBody> objects;
        private Quadtree[] nodes;
        private FloatRect bounds;
        public RectangleShape drawable;

        public Quadtree(int height, FloatRect bounds)
        {
            this.height = height;
            this.bounds = bounds;
            this.drawable = new RectangleShape(new Vector2f(bounds.Width,bounds.Height));
            this.drawable.Position = new Vector2f(bounds.Left, bounds.Top);
            this.drawable.FillColor = Color.Transparent;
            this.drawable.OutlineThickness = 1;
            this.drawable.OutlineColor = Color.White;
            this.objects = new List<IRigidBody>();
            this.nodes = new Quadtree[4];
        }

        public void Clear()
        {
            objects.Clear();
            for (uint i = 0; i < nodes.Length; ++i)
            {
                if (nodes[i] != null)
                {
                    nodes[i].Clear();
                    nodes[i] = null;
                }
            }
        }

        private void Split()
        {
            int subWidth = (int)bounds.Width / 2;
            int subHeight = (int)bounds.Height / 2;
            int x = (int)bounds.Left;
            int y = (int)bounds.Top;

            nodes[0] = new Quadtree(height+1, new FloatRect(x + subWidth, y, subWidth, subHeight));
            nodes[1] = new Quadtree(height+1, new FloatRect(x, y, subWidth, subHeight));
            nodes[2] = new Quadtree(height+1, new FloatRect(x, y + subHeight, subWidth, subHeight));
            nodes[3] = new Quadtree(height+1, new FloatRect(x + subWidth, y + subHeight, subWidth, subHeight));
        }

        private int GetIndex(IRigidBody obj)
        {
            int index = -1; // Object part of parent node
            float verticalMidpoint = bounds.Left + bounds.Width * .5f;
            float horizontalMidpoint = bounds.Top + bounds.Height * .5f;

            float objLeft = obj.COM.X - obj.Radius;
            float objTop  = obj.COM.Y + obj.Radius;
            float objDiameter = obj.Radius * 2;
            // Object can completely fit within the top quadrants
            bool topQuadrant = objTop < horizontalMidpoint && 
                               objTop + objDiameter < horizontalMidpoint;
            // Object can completely fit within the bottom quadrants
            bool bottomQuadrant = objTop > horizontalMidpoint;
            // Object can completely fit within the left quadrants
            if (objLeft < verticalMidpoint && objLeft + objDiameter < verticalMidpoint)
            {
                if (topQuadrant)
                {
                    index = 1;
                }
                else if (bottomQuadrant)
                {
                    index = 2;
                }
            }
            // Object can completely fit within the right quadrants
            else if (objLeft > verticalMidpoint)
            {
                if (topQuadrant)
                {
                    index = 0;
                }
                else if (bottomQuadrant)
                {
                    index = 3;
                }
            }
            return index;
        }

        public void Insert(IRigidBody obj)
        {
            if (nodes[0] != null)
            {
                int index = GetIndex(obj);
                if (index != -1)
                {
                    nodes[index].Insert(obj);
                    return;
                }
            }
            objects.Add(obj);
            if (objects.Count > MAX_OBJECTS)
            {
                if (nodes[0] == null)
                    Split();

                int i = 0;
                while (i < objects.Count)
                {
                    int index = GetIndex(objects[i]);
                    if (index != -1)
                    {
                        nodes[index].Insert(objects[i]);
                        objects.RemoveAt(i);
                    }
                    else
                    {
                        ++i;
                    }
                }
            }
        }

        public List<IRigidBody> Retrieve(List<IRigidBody> returnObjects, IRigidBody obj)
        {
            int index = GetIndex(obj);
            if (index != -1 && nodes[0] != null)
                nodes[index].Retrieve(returnObjects, obj);

            returnObjects.AddRange(objects);
            return returnObjects;
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(this.drawable);
            for (int i = 0; i < nodes.Length; ++i)
                if(nodes[i] != null) nodes[i].Draw(window);
        }
    }
}

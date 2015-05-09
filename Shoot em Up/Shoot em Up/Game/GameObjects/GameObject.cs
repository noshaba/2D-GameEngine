﻿using Physics;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoot_em_Up
{
    class GameObject
    {
        public IShape shape;

        public GameObject(IShape shape)
        {
            this.shape = shape;
        }
        //change name of "radius"
        public GameObject(Collision.Type type, Vector2f position, float var, float density)
        {
            switch (type)
            {
                case Collision.Type.Circle: 
                    this.shape = new Circle(position, var, density); 
                    break;
                case Collision.Type.Polygon:
                    this.shape = new Polygon(position, var, density);
                    break;
            }  
        }

        public GameObject(Vector2f normal, Vector2f position, Vector2f size, float rotation)
        {
            this.shape = new Plane(normal, position, size, rotation);
        }

        public virtual void Update() {
            /*if (shape.Collision.collision) {
                Console.WriteLine("nnn");
            }*/
            shape.Collision.collision = false;
        }
    }
}
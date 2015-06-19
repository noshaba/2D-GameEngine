﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics;
using SFML.System;
using SFML.Graphics;
using SFML.Window;

namespace BodyTest
{
    class Program
    {
        static void Main(string[] args)
        {
            float dt = .001f;
            const int WIDTH = 1000, HEIGHT = 500;
            RenderWindow window = new RenderWindow(new VideoMode(WIDTH, HEIGHT), "Bodies Test", Styles.Close, new ContextSettings());
            
            List<Body> bodies = new List<Body>();

            Physic phy = new Physic(bodies, new Vector2f(0,0), 0, false, new FloatRect(0,0,WIDTH,HEIGHT));

            Polygon p1 = new Polygon();
            p1.SetBox(new Vector2f(50,50), 50, 50, 45, .1f);
            Polygon p2 = new Polygon();
            p2.SetBox(new Vector2f(-50,50), 50, 50, 0, .1f);
            IRigidBody[] pbodies = { p1, p2 };
            Body p = new Body(pbodies, new Vector2f(300, 250), 0);
            bodies.Add(p);

            Polygon g1 = new Polygon();
            g1.SetBox(new Vector2f(50, 50), 50, 50, 45, .1f);
            Polygon g2 = new Polygon();
            g2.SetBox(new Vector2f(-50, 50), 50, 50, 0, .1f);
            IRigidBody[] gbodies = { g1, g2 };
            Body g = new Body(gbodies, new Vector2f(700, 50), 45);
            bodies.Add(g);
            g.Velocity = new Vector2f(-1,1);

            /*for (int i = 0; i < p1.normals.Length; ++i)
                Console.WriteLine(p1.Normal(i));
            Console.WriteLine();
            for (int i = 0; i < p2.normals.Length; ++i)
                Console.WriteLine(p2.Normal(i));
            Console.WriteLine();
            for (int i = 0; i < g1.normals.Length; ++i)
                 Console.WriteLine(g1.Normal(i));
            Console.WriteLine();
            for (int i = 0; i < g2.normals.Length; ++i)
                Console.WriteLine(g2.Normal(i));
            Console.WriteLine();*/

            //p.Velocity = new Vector2f(50,50);

            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear();
                phy.Update(dt);
                foreach (Body body in bodies)
                    body.Draw(window, 0);
                window.Display();
            }


        }
    }
}

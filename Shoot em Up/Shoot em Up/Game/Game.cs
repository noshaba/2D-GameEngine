using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using Maths;
using Physics;
using System.Diagnostics;

namespace Shoot_em_Up 
{
    class Game
    {
        private Physic physics;
        private Planet planet;
        private static List<GameObject> objects = new List<GameObject>();
        private static List<IRigidBody> rigidBodies = new List<IRigidBody>();
        public static int WIDTH;
        public static int HEIGHT;
        private int level;
        public Game(int width, int height)
        {
            WIDTH = width;
            HEIGHT = height;
            Level = 1;
        }

        public static void Add(GameObject obj)
        {
            objects.Add(obj);
            rigidBodies.Add(obj.rigidBody);
        }

        private int Level
        {
            get
            {
                return level;
            }
            set
            {
                level = value;
                LoadLevel();
            }
        }

        private void LoadLevel()
        {
            using (StreamReader sr = new StreamReader("../Content/" + level + "/Planet.json"))
            {
                String json = sr.ReadToEnd();
                planet = JSONManager.deserializeJson<Planet>(json);
                planet.Init();
            }
            physics = new Physic(rigidBodies, new Vector2f(planet.Gravity[0], planet.Gravity[1]), planet.Damping, planet.Friction);
            planet.AddGround();
        }

        public void Update(float dt) 
        {
            physics.Update(dt);
        }

        public void Draw(RenderWindow window, float alpha)
        {
            //all the drawing
            window.Draw(planet.backgroundSprite);
            State interpol;
            Transform t;
            foreach (GameObject obj in objects)
            {
                interpol = obj.rigidBody.Interpolation(alpha);
                t = Transform.Identity;
                t.Translate(interpol.position);
                t.Rotate(interpol.DegOrientation);
                window.Draw(obj.drawable, new RenderStates(t));
                window.Draw(obj.rigidBody as Shape, new RenderStates(t));
            }
        }

    }
}
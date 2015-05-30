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
        private int WIDTH;
        private int HEIGHT;
        private int level;
        public Game(int width, int height)
        {
            WIDTH = width;
            HEIGHT = height;
            level = 1;
            LoadLevel();
        }

        private void LoadLevel()
        {
            using (StreamReader sr = new StreamReader("../Content/" + level + "/Planet.json"))
            {
                String json = sr.ReadToEnd();
                planet = JSONManager.deserializeJson<Planet>(json);
            }
        }

        public void Update(float dt) 
        {

        }

        public void Draw(RenderWindow window, float alpha)
        {
            //all the drawing
            State interpol;
            Transform t;
            foreach (GameObject obj in objects)
            {
                interpol = obj.rigidBody.Interpolation(alpha);
                t = Transform.Identity;
                t.Translate(interpol.position);
                t.Rotate(interpol.DegOrientation);
                window.Draw(obj.drawable, new RenderStates(t));
            }
        }

    }
}
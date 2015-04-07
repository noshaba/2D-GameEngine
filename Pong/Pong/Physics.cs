using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace Pong{
    class Physics {
        private List<Shape> objects = new List<Shape>();
        public void AddObject(Shape obj) {
            objects.Add(obj);
        }
        public void Update(float dt) {
            for (int i = 0; i < objects.Count(); ++i) {
                for (int j = 0; j < objects.Count(); ++j) {
                    if (i == j) continue;
                    Collision.CheckForCollision(objects[i],objects[j]);
                }
            }
        }
    }
}

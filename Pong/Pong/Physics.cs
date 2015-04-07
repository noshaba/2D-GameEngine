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
            for (uint i = 0; i < objects.Count(); ++i) {
                for (uint j = 0; j < objects.Count(); ++j) {
                    if (i == j) continue;
                }
            }
        }
    }
}

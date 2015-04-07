using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace Pong{
    class Physics {
        private List<IShape> objects = new List<IShape>();
        public void AddObject(IShape obj) {
            objects.Add(obj);
        }
        public void Update(float dt) {
            for (int i = 0; i < objects.Count(); ++i) {
                objects[i].Update(dt);
                for (int j = 0; j < objects.Count(); ++j) {
                    if (i == j) continue;
                    Collision.CheckForCollision(objects[i],objects[j]);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;

namespace Pong {
    class Plane {
        public Vector2f normal;
        public float constant;

        public Plane(Vector2f normal, float constant) {
            this.normal = normal;
            this.constant = constant;
        }
    }
}

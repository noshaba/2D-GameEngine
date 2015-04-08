using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;

namespace Pong {
    interface IShape {
        Collision.Type Type{ get; }
        Vector2f Force { get; set; }
        float Torque { get; set; }
        Vector2f Velocity { get; set; }
        float AngularVelocity { get; set; }
        void Update(float dt);
        void Pull(Vector2f n, float overlap);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.System;

namespace Shoot_em_Up {
    interface IShape {
        Collision.Type Type{ get; }
        Vector2f COM { get; }
        float Orientation { get; }
        Mat22f WorldTransform { get; }
        Mat22f LocalTransform { get; }
        State Current { get; set; }
        State Previous { get; }
        float Mass { get; }
        float InverseMass { get; }
        float InverseInertia { get; }
        void ApplyImpulse(Vector2f J, Vector2f r);
        Vector2f Velocity { get; set; }
        float AngularVelocity { get; set; }
        void Update(float dt);
        void Pull(Vector2f n, float overlap);
        State Interpolation(float alpha);
    }
}

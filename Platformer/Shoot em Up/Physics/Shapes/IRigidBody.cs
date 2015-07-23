using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;
using Maths;

namespace Physics {
    interface IRigidBody {
        Collision.Type Type{ get; }
        float Radius { get; set; }
        CircleShape BoundingCircle { get; set; }
        RectangleShape COMDrawable { get; set; }
        Vector2f COM { get; set; }
        Vector2f Centroid { get; set; }
        Vector2f Center { get; }
        float Orientation { get; set; }
        float DegOrientation { get; set; }
        Mat22f WorldTransform { get; }
        Mat22f LocalTransform { get; }
        State Current { get; set; }
        State Previous { get; set; }
        float Mass { get; }
        float InverseMass { get; }
        float Inertia { get; }
        float InverseInertia { get; }
        void ApplyImpulse(Vector2f J, Vector2f r);
        Vector2f Velocity { get; set; }
        float AngularVelocity { get; set; }
        void Update(float dt);
        void Pull(Vector2f n, float overlap);
        State Interpolation(float alpha);
    }
}

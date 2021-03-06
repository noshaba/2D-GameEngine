﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using Maths;
using Physics;

namespace Pong {
    class Ball : Circle {

        public Collision.Type type = Collision.Type.Circle;

        private Vector2f initPosition;
        private const float MAXVELOCITY2 = 40000;

        public Ball(Vector2f position, float radius, Color color, float density) : base(position, radius, density) {
            initPosition = position;
            FillColor = color;
            Restitution = 1.0f;
        }

        public void Reset() {
            current.Reset();
            current.position = initPosition;
            previous = current;
        }

        public void Impulse() {
            current.velocity = new Vector2f(-50, 50);
        }

        public void IncreaseVelocity(float dt) {
            if (current.velocity.Length2() < MAXVELOCITY2)
                current.velocity += dt * current.velocity.Norm() * 50;
        }
    }
}

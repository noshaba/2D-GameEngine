﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using Physics;

namespace Pong {
    class Obstacle : Polygon {
        public Obstacle(Vector2f position, float radius, Color color) : base(position, radius) {
            FillColor = color;
            Restitution = 1.0f;
        }

        public Obstacle(Vector2f position, float radius, Color color, float density) : base(position, radius, density) {
            FillColor = color;
            Restitution = 1.0f;
        }
    }
}

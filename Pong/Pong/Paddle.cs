﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Pong {
    class Paddle : OBB {

        public Paddle(Vector2f position, Vector2f size, Color color) : base(position, size) {
            current = new State(position);
            previous = current;
            FillColor = color;
        }

        public void move(float y) { 
            Position = new Vector2f(Position.X, y);
            current.position = Position;
        }
    }
}

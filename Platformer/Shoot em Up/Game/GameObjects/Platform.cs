using Physics;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class Platform : GameObject
    {
        public Platform(IRigidBody[] bodies, Vector2f position, float rotation, float tileSize)
            : base("../Content/platform.png", new[] {100,100}, new[] {300,100}, bodies, position, rotation)
        {

        }
    }
}

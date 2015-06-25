using Physics;
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
        public Platform(IRigidBody[] bodies, Vector2f position, float rotation)
            : base(bodies, position, rotation)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace Platformer
{
    struct BulletShot
    {
        public Vector2f bulletSpeed, bend, offset;
        public BulletShot(Vector2f bulletSpeed, Vector2f bend, Vector2f offset)
        {
            this.bulletSpeed = bulletSpeed;
            this.bend = bend;
            this.offset = offset;
        }
    }
}

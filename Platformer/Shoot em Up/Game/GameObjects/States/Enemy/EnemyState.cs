using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class EnemyState : AnimState
    {
        protected new Enemy owner;
        public EnemyState(Enemy owner) : base (owner)
        {
            this.owner = owner;
        }
    }
}

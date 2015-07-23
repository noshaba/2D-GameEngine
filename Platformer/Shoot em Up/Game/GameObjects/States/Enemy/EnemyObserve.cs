using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class EnemyObserve : EnemyState
    {
        public EnemyObserve(Enemy owner) : base (owner)
        {
            this.sequence = new int[] { 4,5,6,5 };
        }

        public override void HandleEvents()
        {
            base.HandleEvents();
        }
    }
}

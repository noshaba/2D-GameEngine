using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class EnemyAwake : EnemyState
    {
        public EnemyAwake(Enemy owner) : base (owner)
        {
            this.sequence = new int[] { 0,1,2,3,4 };
        }

        public override void HandleEvents()
        {
            base.HandleEvents();
            if(this.index == this.sequence.Length-1) {
                this.owner.currentState = new EnemyObserve(this.owner);
            }
        }
    }
}

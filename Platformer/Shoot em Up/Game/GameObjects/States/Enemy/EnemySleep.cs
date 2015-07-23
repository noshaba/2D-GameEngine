using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer {
    class EnemySleep : EnemyState
    {
        public EnemySleep(Enemy owner) : base (owner)
        {
            this.sequence = new int[] { 0 };
        }

        public override void HandleEvents()
        {
            base.HandleEvents();
            if(this.owner.IsPlayerNear()) {
                this.owner.currentState = new EnemyAwake(this.owner);
            }
        }
    }
}

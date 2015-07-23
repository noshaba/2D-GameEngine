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
            this.sequence = this.owner.animations[(int)Enemy.animType.observe];
        }

        public override void HandleEvents()
        {
            base.HandleEvents();
            if (this.owner.IsPlayerClose())
                this.owner.currentState = new EnemyAttack(this.owner);
        }
    }
}

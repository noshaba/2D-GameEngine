using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class EnemyAttack : EnemyState
    {
        public EnemyAttack(Enemy owner) : base (owner)
        {
            this.sequence = this.owner.animations[(int)Enemy.animType.attack];
        }

        public override void HandleEvents()
        {
            base.HandleEvents();
            this.owner.Chase();
            if (!this.owner.IsPlayerNear())
                this.owner.currentState = new EnemyObserve(this.owner);
        }
    }
}

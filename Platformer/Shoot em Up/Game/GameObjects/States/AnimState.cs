using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class AnimState
    {
        public int[] sequence;
        public int index;
        protected GameObject owner;

        public AnimState(int[] sequence, GameObject owner)
        {
            this.sequence = sequence;
            this.index = 0;
            this.owner = owner;
        }

        public AnimState(GameObject owner)
        {
            this.index = 0;
            this.owner = owner;
        }

        public virtual void HandleEvents()
        {
            
        }

        public virtual void HandleInput(Keyboard.Key k, bool pressed)
        {

        }
    }
}

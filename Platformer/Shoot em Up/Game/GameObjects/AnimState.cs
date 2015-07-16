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

        public AnimState(int[] sequence)
        {
            this.sequence = sequence;
            this.index = 0;
        }
    }
}

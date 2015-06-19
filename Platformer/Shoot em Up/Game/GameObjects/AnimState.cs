using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class AnimState
    {
        public int min;
        public int max;

        public AnimState(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
    }
}

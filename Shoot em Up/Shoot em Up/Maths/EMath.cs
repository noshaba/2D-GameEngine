using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maths {
    public static class EMath {
        public static float EPSILON = 0.0001f;
        public static Random random = new Random();
        public static float Random(float l, float h) {
            float a = (float)random.NextDouble();
            a = (h - l) * a + l;
            return a;
        }
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}

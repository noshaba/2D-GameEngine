using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing
{
    static class CV
    {
        public static void alphaThresholding(ref byte[] dst, byte[] src, uint width, uint height, uint threshold)
        {
            for (uint i = 3; i < width * height * 4; i+=4) 
            {
                dst[i] = 255;
                if (src[i] > threshold) {
                    dst[i - 1] = 0;
                    dst[i - 2] = 0;
                    dst[i - 3] = 0;
                }
                else
                {
                    dst[i - 1] = 255;
                    dst[i - 2] = 255;
                    dst[i - 3] = 255;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace ImageProcessing
{
    static class CV
    {
        public static void AlphaThresholding(ref byte[] dst, byte[] src, uint cols, uint rows, uint threshold)
        {
            for (uint i = 3; i < cols * rows * 4; i+=4) 
            {
                if (src[i] > threshold) {
                    dst[i] = 255;   //A
                }
                else
                {
                    dst[i] = 0;     //A
                }

                dst[i - 1] = 255;   //B
                dst[i - 2] = 255;   //G
                dst[i - 3] = 255;   //R
            }
        }

        public static void AlphaEdgeDetection(ref byte[] dst, byte[] src, uint cols, uint rows, uint threshold) {

            AlphaThresholding(ref src, src, cols, cols, threshold);

            // first row
            for (uint x = 0; x < cols * 4; x += 4)
            {
                dst[x + 0] = 255;   //R
                dst[x + 1] = 255;   //G
                dst[x + 2] = 255;   //B
                dst[x + 3] = 0;     //A
            }

            for (uint y = 1; y < rows - 1; ++y)
            {
                // first pixel in row
                dst[cols * 4 * y + 0] = 255;  //R
                dst[cols * 4 * y + 1] = 255;  //G
                dst[cols * 4 * y + 2] = 255;  //B
                dst[cols * 4 * y + 3] = 0;    //A

                for (uint x = 7; x < (cols - 1) * 4; x+=4)
                {
                    dst[cols * 4 * y + x - 3] = 255;    //R
                    dst[cols * 4 * y + x - 2] = 255;    //G
                    dst[cols * 4 * y + x - 1] = 255;    //B

                    // detect horizontal edges
                    int sX = -src[cols * 4 * (y - 1) + x - 4] - (src[cols * 4 * y + x - 4] << 1) - src[cols * 4 * (y + 1) + x - 4]
                             + src[cols * 4 * (y - 1) + x + 4] + (src[cols * 4 * y + x + 4] << 1) + src[cols * 4 * (y + 1) + x + 4];

                    // detect vertical edges
                    int sY = -src[cols * 4 * (y - 1) + x - 4] - (src[cols * 4 * (y - 1) + x] << 1) - src[cols * 4 * (y - 1) + x + 4]
                            + src[cols * 4 * (y + 1) + x - 4] + (src[cols * 4 * (y + 1) + x] << 1) + src[cols * 4 * (y + 1) + x + 4];

                    dst[cols * 4 * y + x] = SobelCode(sX, sY);  //A
                }

                // last pixel in row
                dst[cols * 4 * y + cols * 4 - 4] = 255;   //R
                dst[cols * 4 * y + cols * 4 - 3] = 255;   //G
                dst[cols * 4 * y + cols * 4 - 2] = 255;   //B
                dst[cols * 4 * y + cols * 4 - 1] = 0;     //A
            }

            // last row
            for (uint x = 0; x < cols * 4; x+=4)
            {
                dst[cols * 4 * (rows - 1) + x + 0] = 255;   //R
                dst[cols * 4 * (rows - 1) + x + 1] = 255;   //G
                dst[cols * 4 * (rows - 1) + x + 2] = 255;   //B
                dst[cols * 4 * (rows - 1) + x + 3] = 0;     //A
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte SobelCode(int sobelX, int sobelY)
        {
            int sobelLen = (int)(Math.Sqrt(sobelX * sobelX + sobelY * sobelY));
            if (sobelLen < 0) sobelLen = 0;
            if (sobelLen > 255) sobelLen = 255;
            return (byte) sobelLen;
        }
    }
}

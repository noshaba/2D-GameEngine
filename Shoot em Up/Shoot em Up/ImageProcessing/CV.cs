using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace ImageProcessing
{
    static class CV
    {
        public static void AlphaThresholding(ref byte[] dst, byte[] src, uint cols, uint rows, uint threshold)
        {
            for (uint i = 0; i < cols * rows * 4; i+=4) 
            {
                // R, G, B
                dst[i] = dst[i + 1] = dst[i + 2] = 255;
                if (src[i + 3] > threshold) {
                    dst[i + 3] = 255;   //A
                }
                else
                {
                    dst[i + 3] = 0;     //A
                }
            }
        }

        public static void AlphaEdgeDetection(ref byte[] dst, byte[] src, uint cols, uint rows, uint threshold) {

            AlphaThresholding(ref src, src, cols, rows, threshold);

            // bit shifting
            // num << x = num * x^2
            // num >> x = floor(num / x^2)
            uint pxCols = cols << 2;

            // first row
            for (uint x = 0; x < pxCols; x += 4)
                dst[x] = dst[x + 1] = dst[x + 2] = dst[x + 3] = 0; //R,G,B,A

            for (uint y = 1; y < rows - 1; ++y)
            {
                // first pixel in row R, G, B, A
                dst[pxCols * y] = dst[pxCols * y + 1] = dst[pxCols * y + 2] = dst[pxCols * y + 3] = 0;

                for (uint x = 7; x < pxCols - 4; x+=4)
                {
                    // R, G, B
                    dst[pxCols * y + x - 3] = dst[pxCols * y + x - 2] = dst[pxCols * y + x - 1] = 255;

                    // detect horizontal edges
                    int sX = -src[pxCols * (y - 1) + x - 4] - (src[pxCols * y + x - 4] << 1) - src[pxCols * (y + 1) + x - 4]
                            + src[pxCols * (y - 1) + x + 4] + (src[pxCols * y + x + 4] << 1) + src[pxCols * (y + 1) + x + 4];

                    // detect vertical edges
                    int sY = -src[pxCols * (y - 1) + x - 4] - (src[pxCols * (y - 1) + x] << 1) - src[pxCols * (y - 1) + x + 4]
                            + src[pxCols * (y + 1) + x - 4] + (src[pxCols * (y + 1) + x] << 1) + src[pxCols * (y + 1) + x + 4];

                    //A
                    if (sX == 0 && sY == 0)
                    {
                        dst[pxCols * y + x] = 0;
                    }
                    else
                    {
                        dst[pxCols * y + x] = 255;
                    }
                }

                // last pixel in row
                dst[pxCols * y + pxCols - 4] =    //R
                dst[pxCols * y + pxCols - 3] =    //G
                dst[pxCols * y + pxCols - 2] =    //B
                dst[pxCols * y + pxCols - 1] = 0; //A
            }

            // last row
            for (uint x = 0; x < pxCols; x+=4)
                dst[pxCols * (rows - 1) + x] =        //R
                dst[pxCols * (rows - 1) + x + 1] =    //G
                dst[pxCols * (rows - 1) + x + 2] =    //B
                dst[pxCols * (rows - 1) + x + 3] = 0; //A
        }
    }
}

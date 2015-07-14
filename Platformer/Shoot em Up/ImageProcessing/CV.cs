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
        public static void ChannelThresholding(ref byte[] dst, byte[] src, uint cols, uint rows, uint threshold, uint channel)
        {
            --channel;
            uint idx;

            for (uint i = 0; i < cols * rows * 4; i += 4)
            {
                idx = i + channel;

                if (src[idx] > threshold)
                {
                    dst[idx] = 255;
                }
                else
                {
                    dst[idx] = 0;
                }

                dst[(idx + 1) % 4] = dst[(idx + 2) % 4] = dst[(idx + 2) % 4] = 255;
            }
        }

        private static void InitSrcTmp(ref byte[] src_tmp, byte[] src, uint cols, uint rows)
        {
            uint pxSrcCols = (cols + 2) << 2;
            uint pxCols = cols << 2;

            // first row
            for (uint x = 0; x < pxSrcCols; x += 4)
                src_tmp[x] = src_tmp[x + 1] = src_tmp[x + 2] = src_tmp[x + 3] = 0; //R,G,B,A

            // save src in src_tmp
            for (uint y = 1; y < rows; ++y)
            {
                // first pixel in row R, G, B, A
                src_tmp[pxSrcCols * y + 0] =
                src_tmp[pxSrcCols * y + 1] =
                src_tmp[pxSrcCols * y + 2] =
                src_tmp[pxSrcCols * y + 3] = 0;

                for (uint x = 4; x < pxSrcCols - 4; x += 4)
                {
                    src_tmp[pxSrcCols * y + x + 0] = src[pxCols * (y - 1) + x + 0 - 4]; //R
                    src_tmp[pxSrcCols * y + x + 1] = src[pxCols * (y - 1) + x + 1 - 4]; //G
                    src_tmp[pxSrcCols * y + x + 2] = src[pxCols * (y - 1) + x + 2 - 4]; //B
                    src_tmp[pxSrcCols * y + x + 3] = src[pxCols * (y - 1) + x + 3 - 4]; //A
                }

                // last pixel in row
                src_tmp[pxSrcCols * y + pxSrcCols - 4] =    //R
                src_tmp[pxSrcCols * y + pxSrcCols - 3] =    //G
                src_tmp[pxSrcCols * y + pxSrcCols - 2] =    //B
                src_tmp[pxSrcCols * y + pxSrcCols - 1] = 0; //A
            }

            // last row
            for (uint x = 0; x < pxSrcCols; x += 4)
                src_tmp[pxSrcCols * rows + x + 0] =    //R
                src_tmp[pxSrcCols * rows + x + 1] =    //G
                src_tmp[pxSrcCols * rows + x + 2] =    //B
                src_tmp[pxSrcCols * rows + x + 3] = 0; //A
        }

        // also initializes dst image to show (for debugging)
        public static Vector2f[] AlphaEdgeDetection(ref byte[] dst, byte[] src, uint cols, uint rows, uint threshold)
        {

            ChannelThresholding(ref src, src, cols, rows, threshold, 4);

            List<Vector2f> indexBuffer = new List<Vector2f>();

            
            byte[] src_tmp = new byte[(cols + 2) * (rows + 2) << 2];

            uint pxSrcCols = (cols + 2) << 2;
            uint pxCols = cols << 2;

            InitSrcTmp(ref src_tmp, src, cols, rows);

            // dst img
            for (uint y = 0; y < rows; ++y) 
            {
                for (uint x = 0; x < pxCols; x += 4) 
                {
                    // R, G, B
                    dst[pxCols * y + x] = dst[pxCols * y + x + 1] = dst[pxCols * y + x + 2] = 255;
                    // detect horizontal edges
                    int sX = -src_tmp[pxSrcCols * y + x +  3] - (src_tmp[pxSrcCols * (y + 1) + x +  3] << 1) - src_tmp[pxSrcCols * (y + 2) + x +  3]
                            + src_tmp[pxSrcCols * y + x + 11] + (src_tmp[pxSrcCols * (y + 1) + x + 11] << 1) + src_tmp[pxSrcCols * (y + 2) + x + 11];
                    // detect vertical edges
                    int sY = -src_tmp[pxSrcCols * (y + 0) + x + 3] - (src_tmp[pxSrcCols * (y + 0) + x + 7] << 1) - src_tmp[pxSrcCols * (y + 0) + x + 11]
                            + src_tmp[pxSrcCols * (y + 2) + x + 3] + (src_tmp[pxSrcCols * (y + 2) + x + 7] << 1) + src_tmp[pxSrcCols * (y + 2) + x + 11];

                    //A
                    if (sX == 0 && sY == 0)
                    {
                        dst[pxCols * y + x + 3] = 0;
                    }
                    else
                    {
                        dst[pxCols * y + x + 3] = 255;
                        indexBuffer.Add(new Vector2f(x, y));
                    }
                }
            }
            return indexBuffer.ToArray();
        }

        // just returns the needed index buffer and no dst image to show
        public static Vector2f[] AlphaEdgeDetection(byte[] src, uint cols, uint rows, uint threshold)
        {
            ChannelThresholding(ref src, src, cols, rows, threshold, 4);

            List<Vector2f> indexBuffer = new List<Vector2f>();
            byte[] src_tmp = new byte[(cols + 2) * (rows + 2) << 2];

            uint pxSrcCols = (cols + 2) << 2;
            uint pxCols = cols << 2;

            InitSrcTmp(ref src_tmp, src, cols, rows);

            for (uint y = 0; y < rows; ++y)
            {
                for (uint x = 0; x < pxCols; x += 4)
                {
                    // detect horizontal edges
                    int sX = -src_tmp[pxSrcCols * y + x +  3] - (src_tmp[pxSrcCols * (y + 1) + x +  3] << 1) - src_tmp[pxSrcCols * (y + 2) + x +  3]
                            + src_tmp[pxSrcCols * y + x + 11] + (src_tmp[pxSrcCols * (y + 1) + x + 11] << 1) + src_tmp[pxSrcCols * (y + 2) + x + 11];
                    // detect vertical edges
                    int sY = -src_tmp[pxSrcCols * (y + 0) + x + 3] - (src_tmp[pxSrcCols * (y + 0) + x + 7] << 1) - src_tmp[pxSrcCols * (y + 0) + x + 11]
                            + src_tmp[pxSrcCols * (y + 2) + x + 3] + (src_tmp[pxSrcCols * (y + 2) + x + 7] << 1) + src_tmp[pxSrcCols * (y + 2) + x + 11];

                    //A
                    if (sX != 0 && sY != 0)
                        indexBuffer.Add(new Vector2f(x >> 2, y));
                }
            }
            return indexBuffer.ToArray();
        }
    }
}

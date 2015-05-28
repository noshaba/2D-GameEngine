using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace Maths
{
    class BezierSpline
    {
        private List<Vector2f> subdividedPolygon = new List<Vector2f>();
        private List<Vector2f> leftSubdivision = new List<Vector2f>();
        private List<Vector2f> rightSubdivision = new List<Vector2f>();
        // for Casteljau Algorithm
        private List<Vector2f> tmpSubdivision1 = new List<Vector2f>();
        private List<Vector2f> tmpSubdivision2 = new List<Vector2f>();
        // bezier curve
        private Vector2f[] bezierCurve;

        public Vector2f[] BezierSpline(List<Vector2f> anchors, uint depth, float t)
        {
            BezierSubdivision(anchors, depth, t);
            return bezierCurve;
        }

        private void BezierSubdivision(List<Vector2f> anchors, uint depth, float t)
        {
            uint devided = 0;

            if (depth > 0)
            {
                leftSubdivision.Clear();
                rightSubdivision.Clear();

                leftSubdivision.Add(anchors.First());
                rightSubdivision.Add(anchors.Last());

                tmpSubdivision1 = anchors;
                tmpSubdivision2.Clear();

                // Casteljau Algorithm
                while(tmpSubdivision1.Count > 1)
                {
                    for(int i = 0; i < tmpSubdivision1.Count; ++i)
                        tmpSubdivision2.Add((1 - t) * tmpSubdivision1[i] + t * tmpSubdivision1[i + 1]);

                    // first and last value of the algorithm are saved
                    leftSubdivision.Add(tmpSubdivision2.First());
                    rightSubdivision.Add(tmpSubdivision2.Last());

                    tmpSubdivision1 = tmpSubdivision2;
                    tmpSubdivision2.Clear();
                }

                for (int i = 0; i < leftSubdivision.Count; ++i)
                    subdividedPolygon.Add(leftSubdivision[i]);
                for (int i = rightSubdivision.Count - 1; i > -1; i--)
                    subdividedPolygon.Add(rightSubdivision[i]);

                BezierSubdivision(subdividedPolygon, depth - 1, t);
                ++devided;
            }

            if(devided == depth) bezierCurve = subdividedPolygon.ToArray();
        }
    }
}

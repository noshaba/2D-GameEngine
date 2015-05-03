using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.System;

namespace Maths
{
    class Mat22f {
        private float[,] mat = new float[2, 2];
        public Mat22f(float m00, float m01, float m10, float m11){
            mat[0, 0] = m00;
            mat[0, 1] = m01;
            mat[1, 0] = m10;
            mat[1, 1] = m11;
        }
        public Mat22f(Vector2f vec1, Vector2f vec2) {
            mat[0, 0] = vec1.X;
            mat[0, 1] = vec2.X;
            mat[1, 0] = vec1.Y;
            mat[1, 1] = vec2.Y;
        }

        public Vector2f this[int i] {
            get { return new Vector2f(mat[0, i], mat[1, i]); }
        }

        public float this[int i, int j] {
            get { return mat[i, j]; }
        }

        public static Mat22f Eye() {
            return new Mat22f(1, 0, 0, 1);
        }

        public static Mat22f RotationMatrix(float radians) {
            float c = (float)Math.Cos(radians);
            float s = (float)Math.Sin(radians);
            return new Mat22f(c, -s, s, c);
        }

        public static Mat22f operator *(float s, Mat22f mat) {
            return new Mat22f(s * mat[0], s * mat[1]);
        }
        public static Mat22f operator *(Mat22f mat, float s) {
            return s * mat;
        }
        public static Vector2f operator *(Mat22f mat, Vector2f vec) {
            return new Vector2f(mat[0, 0] * vec.X + mat[0, 1] * vec.Y, mat[1, 0] * vec.X + mat[1, 1] * vec.Y);
        }
        public static Mat22f operator *(Mat22f mat1, Mat22f mat2) {
            float m00 = mat1[0, 0] * mat2[0, 0] + mat1[0, 1] * mat2[1, 0];
            float m01 = mat1[0, 0] * mat2[0, 1] + mat1[0, 1] * mat2[1, 1];
            float m10 = mat1[1, 0] * mat2[0, 0] + mat1[1, 1] * mat2[1, 0];
            float m11 = mat1[1, 0] * mat2[0, 1] + mat1[1, 1] * mat2[1, 1];
            return new Mat22f(m00, m01, m10, m11);
        }
        public static Mat22f operator -(Mat22f mat) {
            return new Mat22f(-mat[0], -mat[1]);
        }
        public static Mat22f operator +(Mat22f mat1, Mat22f mat2) {
            return new Mat22f(mat1[0] + mat2[0], mat1[1] + mat2[1]);
        }
        public static Mat22f operator -(Mat22f mat1, Mat22f mat2) {
            return new Mat22f(mat1[0] - mat2[0], mat1[1] - mat2[1]);
        }
        public static Mat22f operator %(Mat22f mat1, Mat22f mat2) {
            float m00 = mat1[0, 0] * mat2[0, 0];
            float m01 = mat1[0, 1] * mat2[0, 1];
            float m10 = mat1[1, 0] * mat2[1, 0];
            float m11 = mat1[1, 1] * mat2[1, 1];
            return new Mat22f(m00, m01, m10, m11);
        }
        public static Mat22f operator ~(Mat22f mat) {
            float invDet = 1.0f / mat.Det;
            return new Mat22f(mat[1, 1], -mat[0, 1], -mat[1, 0], mat[0, 0]) * invDet;
        }
        public float Det {
            get { return mat[0, 0] * mat[1, 1] - mat[0, 1] * mat[1, 0]; }
        }
        public Mat22f Transponent {
            get {
                return new Mat22f(mat[0, 0], mat[1, 0], mat[0, 1], mat[1, 1]);
            }
        }
    }
}

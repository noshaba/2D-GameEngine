using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;

namespace Pong{
    public class Mat22f{
        private float[,] mat = new float[2,2];
        public Mat22f(float m00, float m01, float m10, float m11){
            mat[0,0] = m00;
            mat[0,1] = m01;
            mat[1,0] = m10;
            mat[1,1] = m11;
        }
        public Mat22f(Vector2f vec1, Vector2f vec2) {
            mat[0,0] = vec1.X;
            mat[0,1] = vec1.Y;
            mat[1,0] = vec2.X;
            mat[1,1] = vec2.Y;
        }
        public float this[int i, int j]{
            get { return mat[i,j]; }
        }
    }
    public static class EMath {
        public static float EPSILON = 0.0001f;
        public static Random random = new Random();
        public static float Random(float l, float h) {
            float a = (float)random.NextDouble();
            a = (h - l) * a + l;
            return a;
        }
        public static float Dot(this Vector2f vec1, Vector2f vec2) {
            return vec1.X * vec2.X + vec1.Y * vec2.Y;
        }
        public static float Length2(this Vector2f vec) {
            return vec.X * vec.X + vec.Y * vec.Y;
        }
        public static float Length(this Vector2f vec){
            return (float) Math.Sqrt(vec.Length2());
        }
        public static float CrossProduct(this Vector2f vec1, Vector2f vec2) {
            return (vec1.X * vec2.Y) - (vec1.Y * vec2.X);
        }

        public static Vector2f CrossProduct(this Vector2f vec, float s) {
            return new Vector2f(s * vec.Y, -s * vec.X);
        }

        public static Vector2f CrossProduct(this float s, Vector2f vec) {
            return new Vector2f(-s * vec.Y, s * vec.X);
        }

        public static Vector2f Rotate(this Vector2f vec, float radians) {
            float c = (float) Math.Cos(radians);
            float s = (float) Math.Sin(radians);
            return new Vector2f(c * vec.X - s * vec.Y, s * vec.X + c * vec.Y);
        }

        public static Vector2f TransRotate(this Vector2f vec, float radians) {
            float c = (float) Math.Cos(radians);
            float s = (float) Math.Sin(radians);
            return new Vector2f(c * vec.X + s * vec.Y, -s * vec.X + c * vec.Y);
        }

        public static Vector2f Norm(this Vector2f vec) {
            if (vec.Length2() == 0) return vec;
            return vec / vec.Length();
        }
    }
}

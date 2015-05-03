using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace Pong {
    public static class Vec2fMath {
        public static float Dot(this Vector2f vec1, Vector2f vec2) {
            return vec1.X * vec2.X + vec1.Y * vec2.Y;
        }
        public static float Length2(this Vector2f vec) {
            return vec.X * vec.X + vec.Y * vec.Y;
        }
        public static float Length(this Vector2f vec) {
            return (float)Math.Sqrt(vec.Length2());
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

using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class CoordPoint {
        internal CoordPoint UnaryVector {
            get {
                var length = (float)Math.Sqrt(X * X + Y * Y);
                return this / length;
            }
        }

        public float Length {
            get {
                return Distance(new CoordPoint(), this);
            }
        }
        public float X { get; set; }
        public float Y { get; set; }

        public CoordPoint() {
            X = 0;
            Y = 0;
        }
        public CoordPoint(CoordPoint vector) {
            X = vector.X;
            Y = vector.Y;
        }
        public CoordPoint(float x, float y) {
            X = x;
            Y = y;
        }

        public static float Distance(CoordPoint p1, CoordPoint p2) {
            return (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
        public override string ToString() {
            return string.Format("({0}:{1})", X, Y);
        }

        public static CoordPoint operator -(CoordPoint p1, CoordPoint p2) {
            return new CoordPoint(p1.X - p2.X, p1.Y - p2.Y);
        }
        public static CoordPoint operator +(CoordPoint p1, CoordPoint p2) {
            return new CoordPoint(p1.X + p2.X, p1.Y + p2.Y);
        }
        public static CoordPoint operator /(CoordPoint vector, float factor) {
            return new CoordPoint(vector.X / factor, vector.Y / factor);
        }
        public static CoordPoint operator *(CoordPoint vector, float factor) {
            return new CoordPoint(vector.X * factor, vector.Y * factor);
        }
    }
}

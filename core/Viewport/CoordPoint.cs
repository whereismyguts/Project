using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class CoordPoint {
        float x;
        float y;

        public static float Distance(CoordPoint p1, CoordPoint p2) {
            return (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
        public float Length {
            get { return Distance(new CoordPoint(), this); }
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
        internal CoordPoint UnaryVector {
            get {
                float length = (float)Math.Sqrt(x * x + y * y);
                return this / length;
            }
        }

        public static CoordPoint operator -(CoordPoint p1, CoordPoint p2) {
            return new CoordPoint(p1.X - p2.X, p1.Y - p2.Y);
        }
        public float X
        {
            get { return x; }
            set { x = value; }
        }
        public float Y
        {
            get { return y; }
            set { y = value; }
        }
        public CoordPoint(float X, float Y) {
            x = X;
            y = Y;
        }
        public CoordPoint() {
            x = 0;
            y = 0;
        }

        public CoordPoint(CoordPoint vector) {
            x = vector.X;
            y = vector.Y;
        }

        public override string ToString() {
            return string.Format("({0}:{1})", x, y);
        }

    }
}

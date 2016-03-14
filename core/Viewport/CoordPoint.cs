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

        public float Angle {
            get {
                var x2 = 0f;
                var y2 = -1;
                var dot = X * x2 + Y * y2;
                var det = X * y2 - Y * x2;
                var angle = Math.Atan2(det, dot);

                //var angle = (Math.Atan2(0 - X, Y - (-1)));
                return -(float)angle;
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
        public CoordPoint(double x, double y) {
            X = (float)x;
            Y = (float)y;
        }

        internal void Rotate(float angle) {
            var newX = X * Math.Cos(angle) - Y * Math.Sin(angle);
            var newY = X * Math.Sin(angle) + Y * Math.Cos(angle);
            X = (float)newX;
            Y = (float)newY;
        }

        public static float Distance(CoordPoint p1, CoordPoint p2) {
            return (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
        public override string ToString() {
            return string.Format("(X:{0}, Y:{1}, L:{2})", X, Y, Length);
        }

        public static CoordPoint operator -(CoordPoint p1, float a) {
            return new CoordPoint(p1.X - a, p1.Y - a);
        }
        public static CoordPoint operator +(CoordPoint p1, float a) {
            return new CoordPoint(p1.X + a, p1.Y + a);
        }
        public static CoordPoint operator -(CoordPoint p1, CoordPoint p2) {
            return new CoordPoint(p1.X - p2.X, p1.Y - p2.Y);
        }
        public static CoordPoint operator +(CoordPoint p1, CoordPoint p2) {
            if(p2 == null)
                return p1;
            if(p1 == null)
                return p2;
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

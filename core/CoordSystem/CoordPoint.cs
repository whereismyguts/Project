using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace GameCore {
    public static class Vector2Operations {

        public static Vector2 GetRotated(this Vector2 v, float angle) {
            if(angle == 0)
                return v;
            var newX = (float)(v.X * Math.Cos(angle) - v.Y * Math.Sin(angle));
            var newY = (float)(v.X * Math.Sin(angle) + v.Y * Math.Cos(angle));

            return new Vector2(newX, newY);
        }

        //internal static Vector2 Add(Vector2 size, float a) {
        //    return new Vector2(size.X + a, size.Y + a);
        //}

        //internal static Vector2 Minus(Vector2 p1, Vector2 p2) {
        //    return new Vector2(p1.X - p2.X, p1.Y - p2.Y);
        //}

        internal static float AngleTo(this Vector2 vector, Vector2 v2) {
            //return AngleTo(new CoordPoint(0, -1));

            var x1 = v2.X;//0;
            var y1 = v2.Y;//-1;
            var x2 = vector.X;
            var y2 = vector.Y;
            var dot = x1 * x2 + y1 * y2;
            var det = x1 * y2 - y1 * x2;
            var angle = Math.Atan2(det, dot);

            //var angle = (Math.Atan2(0 - X, Y - (-1)));
            return (float)angle;
        }

        public static Vector2 Clone(this Vector2 vector) {
            return new Vector2(vector.X, vector.Y);
        }

        public static Vector2 UnaryVector(this Vector2 vector) {
            return Vector2.Normalize(vector);
        }

        public static Vector2 Add( this Vector2 p1, Vector2 p2) {
            return Vector2.Add(p1, p2);
        }

        public static Vector2 Add(this Vector2 p1, float a) {
            return new Vector2(p1.X + a, p1.Y + a);
        }

        public static Vector2 Substract(this Vector2 p1, Vector2 p2) {
            return Vector2.Subtract(p1, p2);
        }
        public static Vector2 Substract(this Vector2 p1, float a) {
            return new Vector2(p1.X - a, p1.Y - a);
        }
        //public static Vector2 Multiply(this Vector2 p, float a) {
        //    return Vector2.Multiply(p, a);
        //}
        public static Vector2 Divide(this Vector2 p, float a) {
            return Vector2.Divide(p, a);
        }

        public static float Angle(this Vector2 v) {
            return v.AngleTo(new Vector2(0, -1));
        }
    }

    /*
    public class CoordPoint {
        internal CoordPoint UnaryVector {
            get {
                var length = (float)Math.Sqrt(X * X + Y * Y);
                return this / length;
            }
        }

        public float Angle { get { return AngleTo(new CoordPoint(0, -1)); } }
        public float Length {
            get { return Distance(new CoordPoint(), this); }
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

        public static CoordPoint operator -(CoordPoint p1, float a) {
            return new CoordPoint(p1.X - a, p1.Y - a);
        }
        public static CoordPoint operator -(CoordPoint p1, CoordPoint p2) {
            return new CoordPoint(p1.X - p2.X, p1.Y - p2.Y);
        }
        public static CoordPoint operator -(CoordPoint p) {
            return new CoordPoint(-p.X, -p.Y);
        }
        public static CoordPoint operator *(CoordPoint vector, float factor) {
            return new CoordPoint(vector.X * factor, vector.Y * factor);
        }

        internal CoordPoint Clone() {
            return new CoordPoint(X, Y);
        }

        public static CoordPoint operator *(float factor, CoordPoint vector) {
            return vector * factor;
        }
        public static CoordPoint operator /(CoordPoint vector, float factor) {
            return new CoordPoint(vector.X / factor, vector.Y / factor);
        }
        public static CoordPoint operator +(CoordPoint p1, float a) {
            return new CoordPoint(p1.X + a, p1.Y + a);
        }
        public static CoordPoint operator +(CoordPoint p1, CoordPoint p2) {
            if(p2 == null)
                return p1;
            if(p1 == null)
                return p2;
            return new CoordPoint(p1.X + p2.X, p1.Y + p2.Y);
        }

        internal CoordPoint GetRotated(float angle) {
            CoordPoint res = new CoordPoint(X, Y);
            res.Rotate(angle);
            return res;
        }

        internal void Rotate(float angle) {
            if(angle == 0)
                return;
            var newX = X * Math.Cos(angle) - Y * Math.Sin(angle);
            var newY = X * Math.Sin(angle) + Y * Math.Cos(angle);
            X = (float)newX;
            Y = (float)newY;
        }

        public float AngleTo(CoordPoint vector) {
            var x2 = vector.X;
            var y2 = vector.Y;
            var dot = X * x2 + Y * y2;
            var det = X * y2 - Y * x2;
            var angle = Math.Atan2(det, dot);

            //var angle = (Math.Atan2(0 - X, Y - (-1)));
            return -(float)angle;
        }

        public static float Distance(CoordPoint p1, CoordPoint p2) {
            return (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
        public override string ToString() {
            return string.Format("X:{0}, Y:{1}, Length:{2}", X, Y, Length);
        }
    }
    */
}

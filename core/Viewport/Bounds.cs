using System;

namespace Core {
    public class Bounds {
        public CoordPoint LeftTop;
        public CoordPoint RightBottom;

        public CoordPoint Center {
            get {
                return (LeftTop + RightBottom) / 2f;
            }
        }
        public float Height {
            get {
                return Math.Abs(RightBottom.Y - LeftTop.Y);
            }
        }
        public float Width {
            get {
                return Math.Abs(RightBottom.X - LeftTop.X);
            }
        }

        public Bounds(CoordPoint lt, CoordPoint rb) {
            LeftTop = lt;
            RightBottom = rb;
        }

        public  bool isIntersect(Bounds obj) {
            return (Math.Abs(LeftTop.X - obj.LeftTop.X) * 2 < (Width + obj.Width)) &&
            (Math.Abs(LeftTop.Y - obj.LeftTop.Y) * 2 < (Height + obj.Height));
        }
        public static Bounds operator /(Bounds p1, float k) {
            return new Bounds(p1.LeftTop / k, p1.RightBottom / k);
        }
        public static Bounds operator *(Bounds p1, float k) {
            return new Bounds(p1.LeftTop * k, p1.RightBottom * k);
        }
        public static Bounds operator -(Bounds p1, Bounds p2) {
            return new Bounds(p1.LeftTop - p2.LeftTop, p1.RightBottom - p2.RightBottom);
        }
        public static Bounds operator +(Bounds p1, Bounds p2) {
            return new Bounds(p1.LeftTop + p2.LeftTop, p1.RightBottom + p2.RightBottom);
        }
        public static Bounds operator -(Bounds p1, CoordPoint p2) {
            return new Bounds(p1.LeftTop - p2, p1.RightBottom - p2);
        }
        public static Bounds operator +(Bounds p1, CoordPoint p2) {
            return new Bounds(p1.LeftTop + p2, p1.RightBottom + p2);
        }
    }
}

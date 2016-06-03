using System;
using System.Collections.Generic;

namespace GameCore {
    public class Bounds {
        public CoordPoint Center { get { return (LeftTop + RightBottom) / 2f; } }
        public float Height { get { return Math.Abs(RightBottom.Y - LeftTop.Y); } }
        public CoordPoint Size { get { return new CoordPoint(Width, Height); } }
        public float Width { get { return Math.Abs(RightBottom.X - LeftTop.X); } }

        public Bounds() { }

        public Bounds(CoordPoint lt, CoordPoint rb) {
            LeftTop = lt;
            RightBottom = rb;
        }

        public Bounds(float x, float y, float w, float h) {
            LeftTop = new CoordPoint(x, y);
            RightBottom = new CoordPoint(x + w, y + h);
        }

        public static Bounds operator -(Bounds p1, Bounds p2) {
            return new Bounds(p1.LeftTop - p2.LeftTop, p1.RightBottom - p2.RightBottom);
        }
        public static Bounds operator -(Bounds p1, CoordPoint p2) {
            return new Bounds(p1.LeftTop - p2, p1.RightBottom - p2);
        }
        public static Bounds operator *(Bounds p1, float k) {
            return new Bounds(p1.LeftTop * k, p1.RightBottom * k);
        }
        public static Bounds operator /(Bounds p1, float k) {
            return new Bounds(p1.LeftTop / k, p1.RightBottom / k);
        }
        public static Bounds operator +(Bounds p1, Bounds p2) {
            return new Bounds(p1.LeftTop + p2.LeftTop, p1.RightBottom + p2.RightBottom);
        }
        public static Bounds operator +(Bounds p1, CoordPoint p2) {
            return new Bounds(p1.LeftTop + p2, p1.RightBottom + p2);
        }

        //public  bool isIntersect(Bounds obj) {
        //    return (Math.Abs(LeftTop.X - obj.LeftTop.X) * 2 < (Width + obj.Width)) &&
        //    (Math.Abs(LeftTop.Y - obj.LeftTop.Y) * 2 < (Height + obj.Height));
        //}
        public bool Contains(CoordPoint p) {
            return this.LeftTop.X <= p.X && LeftTop.Y <= p.Y && RightBottom.X >= p.X && RightBottom.Y > p.Y;
        }
        public CoordPoint[] GetPoints() {
            return new CoordPoint[] {
                LeftTop,
                LeftTop + new CoordPoint(Width, 0),
                RightBottom,
                RightBottom + new CoordPoint(0, Height) };
        }
        public bool Intersect(Bounds bounds) {
            var points = bounds.GetPoints();
            for(int i = 0; i < points.Length; i++)
                if(bounds.Contains(points[i]))
                    return true;
            return false;
        }
        public override string ToString() {
            return LeftTop + " : " + RightBottom;
        }
        public CoordPoint LeftTop = new CoordPoint();
        public CoordPoint RightBottom = new CoordPoint();

        public bool Contains(object bounds) {
            throw new NotImplementedException();
        }
    }
}

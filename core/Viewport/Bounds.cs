using System;

namespace Core {
    public class Bounds {
        public CoordPoint LeftTop;
        public CoordPoint RightBottom;
        public Bounds(CoordPoint lt, CoordPoint rb) {
            this.LeftTop = lt;
            this.RightBottom = rb;
        }
        public float X { get { return LeftTop.X; } }
        public float Y { get { return LeftTop.Y; } }
        public float Height { get { return Math.Abs(RightBottom.Y - LeftTop.Y); } }
        public float Width { get { return Math.Abs(RightBottom.X - LeftTop.X); } }

        public  bool isIntersect(Bounds obj) {
            return (Math.Abs(this.LeftTop.X - obj.LeftTop.X) * 2 < (this.Width + obj.Width)) &&
        (Math.Abs(this.LeftTop.Y - obj.LeftTop.Y) * 2 < (this.Height + obj.Height));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class CoordPoint {
        float x;
        float y;
        public static CoordPoint operator +(CoordPoint p1, CoordPoint p2) {
            return new CoordPoint(p1.X + p2.X, p1.Y + p2.Y);
        }
        public static CoordPoint operator -(CoordPoint p1, CoordPoint p2) {
            return new CoordPoint(p1.X - p2.X, p1.Y - p2.Y);
        }
        public float X {
            get { return x; }
            set { x = value; }
        }
        public float Y {
            get { return y; }
            set { y = value; }
        }
        public CoordPoint(float X, float Y) {
            this.x = X;
            this.y = Y;
        }
        public override string ToString() {
            return string.Format("({0}:{1})", x, y);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class CoordPoint {
        int x;
        int y;
        public static CoordPoint operator +(CoordPoint p1, CoordPoint p2) {
            return new CoordPoint(p1.X + p2.X, p1.Y + p2.Y);
        }
        public static CoordPoint operator -(CoordPoint p1, CoordPoint p2) {
            return new CoordPoint(p1.X - p2.X, p1.Y - p2.Y);
        }
        public int X { get { return x; } }
        public int Y { get { return y; } }
        public CoordPoint(int X, int Y) {
            this.x = X;
            this.y = Y;
        }
        public override string ToString() {
            return string.Format("({0}:{1})", x, y);
        }
    }
}

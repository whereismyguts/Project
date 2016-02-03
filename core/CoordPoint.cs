using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class CoordPoint {
        int x;
        int y;
        public int X { get { return x; } }
        public int Y { get { return y; } }
        public CoordPoint(int X, int Y) {
            this.x = X;
            this.y = Y;
        }
    }
}

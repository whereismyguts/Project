using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class Viewport {
        CoordPoint centerpoint;
        const int defaultHeight = 15;
        const int defaultWidth = 30;
        int height;
        int width;

        public int Width {
            get { return width; }
            set {
                width = value;
                centerpoint.X = LeftTop.X + width / 2;
            }
        }
        public int Height {
            get { return height; }
            set {
                height = value;
                centerpoint.Y = LeftTop.Y + height / 2;
            }
        }
        public CoordPoint Centerpoint
        {
            get { return centerpoint; }
            set { centerpoint = value; }
        }

        public CoordPoint RightBottom { get { return centerpoint + new CoordPoint(Width / 2, Height / 2); } }
        public CoordPoint LeftTop { get { return centerpoint - new CoordPoint(Width / 2, Height / 2); } }

        public Viewport(float x, float y) {
            this.centerpoint= new CoordPoint(x, y);
            this.width = defaultWidth;
            this.height = defaultHeight;
        }
        public override string ToString() {
            return string.Format("bounds: {0}:{1}|size: {2}x{3}", LeftTop, RightBottom, Width, Height);
        }

        public void Move(int x, int y) {
            centerpoint += new CoordPoint(x, y);
        }

        public bool IsIntersect(Bounds bounds) {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class Viewport {
        CoordPoint centerpoint;
        World world;
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
            set { SetCenterPoint(value); }
        }

        public CoordPoint RightBottom { get { return centerpoint + new CoordPoint(Width / 2, Height / 2); } }
        public CoordPoint LeftTop { get { return centerpoint - new CoordPoint(Width / 2, Height / 2); } }

        public Viewport(CoordPoint center, World world) {
            this.centerpoint = center;
            this.width = defaultWidth;
            this.height = defaultHeight;
            this.world = world;
            SetCenterPoint(center);
        }
        void SetCenterPoint(CoordPoint center) {
            if(center.X + Width / 2 > world.Width)
                return;
            if(center.X - Width / 2 < 0)
                return;
            if(center.Y + Height / 2 > world.Height)
                return;
            if(center.Y - Height / 2 > 0)
                return;
            this.centerpoint = center;
        }
        public override string ToString() {
            return string.Format("bounds: {0}:{1}|size: {2}x{3}", LeftTop, RightBottom, Width, Height);
        }
    }
}

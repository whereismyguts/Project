using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class Viewport {
        CoordPoint centerpoint;
        CoordPoint leftTop;
        CoordPoint rightBottom;
        World world;
        const int defaultHeight = 20;
        const int defaultWidth = 20;

        public int Width { get; set; }
        public int Height { get; set; }
        public CoordPoint Centerpoint
        {
            get { return centerpoint; }
            set { SetCenterPoint(value); }
        }

        public CoordPoint RightBottom { get { return rightBottom; } }
        public CoordPoint LeftTop { get { return leftTop; } }

        public Viewport(CoordPoint center, World world) {
            this.centerpoint = center;
            this.Width = defaultWidth;
            this.Height = defaultHeight;
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
    }
}

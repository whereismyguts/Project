using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
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
    public class Viewport {
        CoordPoint centerpoint;
        CoordPoint leftTop;
        CoordPoint rightBottom;
        World world;
        const int defaultHeight = 20;
        const int defaultWidth = 20;

        public int Width { get; set; }
        public int Height { get; set; }
        public CoordPoint Centerpoint {
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
    public class World {
        public Viewport Viewport { get; set; }
        public int Width { get; }
        public int Height { get; }
        public void SetViewport(int X,int Y) {
            SetViewport(new CoordPoint(X, Y));
        }
        public void SetViewport(CoordPoint center) {
            Viewport = new Viewport(center, this);
        }
        public World(int width, int height) {
            Width = width;
            Height = height;
            SetViewport(Width / 2, Height / 2);
        }
    }
    public class Character {
        
    }
    public class Game
    {
        Character Character { get; }
        public World World { get; } = new World(100, 100);
        public Viewport Viewport { get { return World.Viewport; } }
    }
}

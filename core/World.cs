using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class World {
        public Viewport Viewport { get; set; }
        public int Width { get; }
        public int Height { get; }
        public void SetViewport(int X, int Y) {
            SetViewport(new CoordPoint(X, Y));
        }
        public void SetViewport(CoordPoint center) {
            Viewport = new Viewport(center, this);
        }
        //public int this[int x, int y] {
        //    get {

        //    }
        //    set { }
        //}
        public World(int width, int height) {
            Width = width;
            Height = height;
            SetViewport(Width / 2, Height / 2);
        }
    }
}

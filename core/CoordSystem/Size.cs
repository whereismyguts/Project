using System;

namespace GameCore {
    public class Size {
        float height;
        float width;

        public float Height { get { return height; } }
        public float Width { get { return width; } }

        public Size(int width, int height) {
            this.width = width;
            this.height = height;
        }
    }
}

using System;

namespace Core {
    public class Viewport {
        CoordPoint centerpoint;
        float height;
        float width;
        float scale = 1f;
        public CoordPoint Centerpoint
        {
            get { return centerpoint; }
            set { centerpoint = value; }
        }

        public Bounds Bounds
        {
            get
            {
                return new Bounds(centerpoint - new CoordPoint(width / 2, height / 2), centerpoint + new CoordPoint(width / 2, height / 2));
            }
        }

        public float Scale { get { return scale; } }
        public Viewport(float x, float y, float w, float h) {
            this.centerpoint = new CoordPoint(x, y);
            this.width = w;
            this.height = h;
        }
        public float Width {
            get { return width; }
        }
        public float Height{
            get { return height; }
        }
        public override string ToString() {
            return string.Format("Bounds: {0}:{1} | Size: {2}x{3} | Centerpoint: {4}", Bounds.LeftTop, Bounds.RightBottom, width, height, centerpoint);
        }
        internal void SetScale(float scale) {
            this.scale = scale;
        }

        public void SetViewportSize(int width, int height) {
            this.width = width;
            this.height = height;
        }

        public void ZoomOut() {
            scale += .01f;
        }
        public void ZoomIn() {
            scale -= .01f;
        }
    }
}

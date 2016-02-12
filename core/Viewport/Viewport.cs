using System;

namespace Core {
    public class Viewport {
        float height;
        float scale = 1f;
        float width;

        public Bounds Bounds {
            get {
                return new Bounds(Centerpoint - new CoordPoint(width / 2, height / 2), Centerpoint + new CoordPoint(width / 2, height / 2));
            }
        }
        public CoordPoint Centerpoint { get; set; }
        public float Height {
            get {
                return height;
            }
        }
        public float Scale {
            get {
                return scale;
            }
        }
        public float Width {
            get {
                return width;
            }
        }

        public Viewport(float x, float y, float w, float h) {
            Centerpoint = new CoordPoint(x, y);
            width = w;
            height = h;
        }

        internal void SetScale(float scale) {
            this.scale = scale;
        }

        public void SetViewportSize(int width, int height) {
            this.width = width;
            this.height = height;
        }
        public override string ToString() {
            return string.Format("Bounds: {0}:{1} | Size: {2}x{3} | Centerpoint: {4}", Bounds.LeftTop, Bounds.RightBottom, width, height, Centerpoint);
        }
        public void ZoomIn() {
            scale += .01f;
        }
        public void ZoomOut() {
            scale -= .01f;
            if (scale < 0)
                scale = 0;
        }
    }
}

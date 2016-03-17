using System;

namespace Core {
    public class Viewport {
        float height;
        float scale = 30f;
        float width;
        int lockTime = 0;

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
        public float MiniMapScale {
            get {
                return 30f;
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


        public void SetViewportSize(int width, int height) {
            this.width = width;
            this.height = height;
        }
        public override string ToString() {
            return string.Format("Bounds: {0}:{1} | Size: {2}x{3} | Centerpoint: {4}", Bounds.LeftTop, Bounds.RightBottom, width, height, Centerpoint);
        }
        public void ZoomIn() {
            ChangeZoom(scale);
        }

        private void ChangeZoom(float delta) {
            if(lockTime == 0) {
                scale += delta;
                lockTime = 5;
            }
            else
                lockTime--;
        }

        public void ZoomOut() {
            ChangeZoom(-scale / 2);
            if (scale < 0)
                scale = 0;
        }
    }
}

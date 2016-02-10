namespace Core {
    public class Viewport {
        CoordPoint centerpoint;
        float height;
        float width;
        float scale =2f;
        public CoordPoint Centerpoint
        {
            get { return centerpoint; }
            set { centerpoint = value; }
        }

        public Bounds Bounds {
            get {
                return new Bounds(centerpoint - new CoordPoint(width / 2, height / 2), centerpoint + new CoordPoint(width / 2, height / 2));
            }
        }

        public float Scale { get { return scale; } }
        public Viewport(float x, float y, float w, float h) {
            this.centerpoint = new CoordPoint(x, y);
            this.width = w;
            this.height = h;
        }
        public void Move(float x, float y) {
            centerpoint += new CoordPoint(x, y);
        }
        public override string ToString() {
            return string.Format("Bounds: {0}:{1} | Size: {2}x{3} | Centerpoint: {4}", Bounds.LeftTop, Bounds.RightBottom, width, height, centerpoint);
        }
    }
}

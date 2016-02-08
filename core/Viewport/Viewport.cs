namespace Core {
    public class Viewport {
        CoordPoint centerpoint;
        const float defaultHeight = 15;
        const float defaultWidth = 30;
        float height;
        float width;
        public float Width
        {
            get { return width; }
            set { width = value; }
        }
        public float Height
        {
            get { return height; }
            set { height = value; }
        }
        public CoordPoint Centerpoint
        {
            get { return centerpoint; }
            set { centerpoint = value; }
        }
        public CoordPoint RightBottom { get { return centerpoint + new CoordPoint(Width / 2, Height / 2); } }
        public CoordPoint LeftTop { get { return centerpoint - new CoordPoint(Width / 2, Height / 2); } }
        public Viewport(float x, float y) {
            this.centerpoint = new CoordPoint(x, y);
            this.width = defaultWidth;
            this.height = defaultHeight;
        }
        public void Move(float x, float y) {
            centerpoint += new CoordPoint(x, y);
        }
        public bool IsIntersect(Bounds bounds) {
            return true;
        }
        public override string ToString() {
            return string.Format("bounds: {0}:{1}|size: {2}x{3}", LeftTop, RightBottom, Width, Height);
        }
    }
}

namespace Core {
    public class Bounds {
        public CoordPoint LeftTop;
        public CoordPoint RightBottom;
        public Bounds(CoordPoint lt, CoordPoint rb) {
            this.LeftTop = lt;
            this.RightBottom = rb;
        }
    }
}
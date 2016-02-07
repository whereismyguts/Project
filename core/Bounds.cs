namespace Core {
    public class Bounds {
        CoordPoint LeftTop;
        CoordPoint RightBottom;

        public Bounds(CoordPoint lt, CoordPoint rb) {
            this.LeftTop = lt;
            this.RightBottom = rb;
        }
    }
}
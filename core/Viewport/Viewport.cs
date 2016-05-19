using System;

namespace GameCore {
    public class Viewport {
        float pxlWidth;
        float pxlHeight;
        float scale = 2f;
        int lockTime = 0;

        public Bounds Bounds {
            get {
                return new Bounds(Centerpoint - new CoordPoint(pxlWidth * scale / 2, pxlHeight * scale / 2), Centerpoint + new CoordPoint(pxlWidth * scale / 2, pxlHeight * scale / 2));
            }
        }
        public CoordPoint Centerpoint { get; set; } = new CoordPoint();
        public float PxlWidth {
            get {
                return pxlWidth;
            }
        }
        public float PxlHeight {
            get {
                return pxlHeight;
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


        public Viewport(float x, float y, float w, float h) {
            Centerpoint = new CoordPoint(x, y);
            pxlWidth = w;
            pxlHeight = h;
        }
        void ChangeZoom(float delta) {
            if(lockTime == 0) {
                scale += delta;
                lockTime = 5;
            }
            else
                lockTime--;
        }
        public void SetViewportSize(int width, int height) {
            this.pxlWidth = width;
            this.pxlHeight = height;
        }
        public void ZoomIn() {
            ChangeZoom(scale);
        }
        public void ZoomOut() {
            ChangeZoom(-scale / 2);
            if(scale < 0)
                scale = 0;
        }
        public override string ToString() {
            return string.Format("Bounds: {0}:{1} | Size: {2}x{3} | Centerpoint: {4}", Bounds.LeftTop, Bounds.RightBottom, pxlWidth, pxlHeight, Centerpoint);
        }
        public CoordPoint Screen2WorldPoint(CoordPoint scrPoint) {
            double pixelFactorX = PxlWidth > 0 ? Bounds.Width / PxlWidth : 0;
            double pixelFactorY = PxlHeight > 0 ? Bounds.Width / PxlWidth : 0;
            return new CoordPoint(scrPoint.X * pixelFactorX + Bounds.LeftTop.X, scrPoint.Y * pixelFactorY + Bounds.LeftTop.Y);
        }
        public CoordPoint World2ScreenPoint(CoordPoint wrlPoint) {
            double unitFactorX = Bounds.Width > 0 ? PxlWidth / Bounds.Width : 0;
            double unitFactorY = Bounds.Height > 0 ? PxlHeight / Bounds.Height : 0;
            return new CoordPoint((wrlPoint.X - Bounds.LeftTop.X) * unitFactorX, (wrlPoint.Y - Bounds.LeftTop.Y) * unitFactorY);
        }
        public Bounds ScreenToWorldBounds(Bounds scrBounds) {
            return new Bounds(Screen2WorldPoint(scrBounds.LeftTop), Screen2WorldPoint(scrBounds.RightBottom));
        }
        public Bounds World2ScreenBounds(Bounds scrBounds) {
            return new Bounds(World2ScreenPoint(scrBounds.LeftTop), World2ScreenPoint(scrBounds.RightBottom));
        }
        public bool Contains(GameObject obj) {
            return Bounds.Intersect(obj.Bounds);
        }
    }
}

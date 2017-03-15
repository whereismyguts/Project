using System;

namespace GameCore {
    public class Viewport {
        float pxlHeight;
        float pxlWidth;

        public Bounds Bounds {
            get {
                return new Bounds(Centerpoint - new CoordPoint(pxlWidth * scale / 2, pxlHeight * scale / 2), Centerpoint + new CoordPoint(pxlWidth * scale / 2, pxlHeight * scale / 2));
            }
        }

        CoordPoint centerPoint = new CoordPoint();

        public CoordPoint Centerpoint {
            get {
                return centerPoint;
            }
            set {
                if((centerPoint - value).Length < 10) {
                    centerPoint = value;
                    return;
                }
                SmoothScroll(value);
            }
        }

        void SmoothScroll(CoordPoint value) {
            //  var step = (centerPoint - value).Length / 100f;
            centerPoint = (value - centerPoint).UnaryVector * (Math.Abs((value - centerPoint).Length) / 100 + 50) + centerPoint;
        }

        public float MiniMapScale {
            get {
                return 30f;
            }
        }
        public float PxlHeight {
            get {
                return pxlHeight;
            }
        }
        public float PxlWidth {
            get {
                return pxlWidth;
            }
        }
        public float Scale {
            get {
                return scale;
            }
            set {
                var d = value - scale;
                if(Math.Abs(d) > 4)
                    d /= 10;
                scale += d;
            }
        }

        public Viewport(float x, float y, float w, float h) {
            Centerpoint = new CoordPoint(x, y);
            pxlWidth = w;
            pxlHeight = h;
        }



        public CoordPoint Screen2WorldPoint(CoordPoint scrPoint) {
            double pixelFactorX = PxlWidth > 0 ? Bounds.Width / PxlWidth : 0;
            double pixelFactorY = PxlHeight > 0 ? Bounds.Height / pxlHeight : 0;
            return new CoordPoint(scrPoint.X * pixelFactorX + Bounds.LeftTop.X, scrPoint.Y * pixelFactorY + Bounds.LeftTop.Y);
        }
        public Bounds ScreenToWorldBounds(Bounds scrBounds) {
            return new Bounds(Screen2WorldPoint(scrBounds.LeftTop), Screen2WorldPoint(scrBounds.RightBottom));
        }
        public void SetViewportSize(int width, int height) {
            this.pxlWidth = width;
            this.pxlHeight = height;
        }
        public override string ToString() {
            return string.Format("Bounds: {0}:{1} | Size: {2}x{3} | Centerpoint: {4}", Bounds.LeftTop, Bounds.RightBottom, pxlWidth, pxlHeight, Centerpoint);
        }
        public Bounds World2ScreenBounds(Bounds scrBounds) {
            return new Bounds(World2ScreenPoint(scrBounds.LeftTop), World2ScreenPoint(scrBounds.RightBottom));
        }
        public CoordPoint World2ScreenPoint(CoordPoint wrlPoint) {
            double unitFactorX = Bounds.Width > 0 ? PxlWidth / Bounds.Width : 0;
            double unitFactorY = Bounds.Height > 0 ? PxlHeight / Bounds.Height : 0;
            return new CoordPoint((wrlPoint.X - Bounds.LeftTop.X) * unitFactorX, (wrlPoint.Y - Bounds.LeftTop.Y) * unitFactorY);
        }
        public void ZoomIn() {
            ChangeZoom(scale + scale / 10, scale / 50f);
        }
        public void ZoomOut() {
            ChangeZoom(scale - scale / 10, -scale / 50f);

        }

        public void Update() {

            //if(Math.Abs(scale - target) > 0.1f)
            //    scale += step;

            if(scale < 0)
                scale = 0;
        }
        float target = 128f;
        float step = 12.8f;
        void ChangeZoom(float target, float step) {
            this.target = target;
            this.step = Math.Sign(step) * Math.Max(0.1f, Math.Abs(step));
        }
        void ChangeZoom(float delta) {
            if(lockTime == 0) {
                scale += delta;
                lockTime = 5;
            }
            else
                lockTime--;
        }

        float scale = 128f;
        int lockTime = 0;
    }
}

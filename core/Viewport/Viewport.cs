using System;

namespace GameCore {
    public class Viewport {
        float pxlWidth;
        float pxlHeight;
        float scale = 2f;
        int lockTime = 0;

        public Bounds Bounds {
            get {

                return new Bounds(Centerpoint - new CoordPoint(pxlWidth* scale / 2, pxlHeight* scale / 2), Centerpoint + new CoordPoint(pxlWidth* scale / 2, pxlHeight* scale / 2));

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

        public CoordPoint Screen2World(CoordPoint scrPoint) {
            double pixelFactorX = PxlWidth > 0 ? Bounds.Width / PxlWidth : 0;
            double pixelFactorY = PxlHeight > 0 ? Bounds.Width / PxlWidth : 0;
            return new CoordPoint(scrPoint.X * pixelFactorX + Bounds.LeftTop.X, scrPoint.Y * pixelFactorY + Bounds.LeftTop.Y);
        }
        //public IMapUnit ScreenPointToMapUnit(MapPointCore point, MapRectCore viewport, MapSizeCore viewportInPixels) {
        //    double pixelFactorX = viewportInPixels.Width > 0 ? viewport.Width / viewportInPixels.Width : 0;
        //    double pixelFactorY = viewportInPixels.Height > 0 ? viewport.Height / viewportInPixels.Height : 0;
        //    return new MapUnitCore(point.X * pixelFactorX + viewport.Left, point.Y * pixelFactorY + viewport.Top);
        //}
        public CoordPoint World2Screen(CoordPoint wrlPoint) {
            double unitFactorX = Bounds.Width > 0 ? PxlWidth / Bounds.Width : 0;
            double unitFactorY = Bounds.Height > 0 ? PxlHeight / Bounds.Height : 0;
            return new CoordPoint((wrlPoint.X - Bounds.LeftTop.X) * unitFactorX, (wrlPoint.Y - Bounds.LeftTop.Y) * unitFactorY);
        }
        //public MapPointCore MapUnitToScreenPoint(IMapUnit mapUnit, MapRectCore viewport, MapSizeCore viewportInPixels) {
        //    double unitFactorX = viewport.Width > 0 ? viewportInPixels.Width / viewport.Width : 0;
        //    double unitFactorY = viewport.Height > 0 ? viewportInPixels.Height / viewport.Height : 0;
        //    return new MapPointCore((mapUnit.X - viewport.Left) * unitFactorX, (mapUnit.Y - viewport.Top) * unitFactorY);
        //}

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
    }
}

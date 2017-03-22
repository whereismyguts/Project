using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {

    public class Geometry {
        protected Viewport Viewport { get { return MainCore.Instance.Viewport; } }
        public CoordPoint ScreenPosition {
            get {
                return Viewport.World2ScreenPoint(Position);
            }
        }
        protected CoordPoint Position { get; set; }
        public ColorCore Color { get; protected set; }
    }
    public class InternalCircle: Geometry {

        private float radius;

        public InternalCircle(CoordPoint position, float radius, ColorCore color = new ColorCore()) {
            Position = position;
            Color = color;
            this.radius = radius;
        }

        public float ScreenRadius { get { return Viewport.World2ScreenBounds(new Bounds(Position - new CoordPoint(radius, radius), Position + new CoordPoint(radius, radius))).Width / 2; } }
        // public CoordPoint UpCenterPoint { get { return position + new CoordPoint(0, -radius); } }
    }
    public class InternalRectangle: Geometry {
        private CoordPoint Size { get; set; }
        public CoordPoint ScreenSize { get { return Viewport.World2ScreenBounds(new Bounds(Position - Size / 2f, Position + Size / 2f)).Size; } }

        public InternalRectangle(CoordPoint leftTop, CoordPoint size, ColorCore color) {
            this.Position = leftTop;
            this.Size = size;
            this.Color = color;
        }
    }
}

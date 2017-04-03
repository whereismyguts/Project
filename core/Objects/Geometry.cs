using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {

    public abstract class Renderable {


        protected CoordPoint Size { get; set; }
        public CoordPoint ScreenLocation { get { return RealSize ? Viewport.World2ScreenPoint(Location) : Location; } }
        public CoordPoint ScreenSize {
            get {
                return RealSize ?
                    Viewport.World2ScreenBounds(new Bounds(Location - Size / 2f, Location + Size / 2f)).Size :
                    new Bounds(Location - Size / 2f, Location + Size / 2f).Size;
            }
        }

        public virtual CoordPoint Location { get; set; }

        public bool RealSize { get; set; } = true;
        protected Viewport Viewport { get { return MainCore.Instance.Viewport; } }
    }

    public class Geometry: Renderable {
        public ColorCore Color { get; protected set; }
    }
    public class InternalCircle: Geometry {

        private float radius;

        public InternalCircle(CoordPoint position, float radius, ColorCore color = new ColorCore()) {
            Location = position;
            Color = color;
            Size = new CoordPoint(radius * 2, radius * 2);
            this.radius = radius;
        }

        public float ScreenRadius {
            get {
                return RealSize
? Viewport.World2ScreenBounds(new Bounds(Location - new CoordPoint(radius, radius), Location + new CoordPoint(radius, radius))).Width / 2
: radius;
            }
        }
        // public CoordPoint UpCenterPoint { get { return position + new CoordPoint(0, -radius); } }
    }
    public class InternalRectangle: Geometry {



        public InternalRectangle(CoordPoint leftTop, CoordPoint size, ColorCore color) {
            this.Location = leftTop;
            this.Size = size;
            this.Color = color;
        }
    }
}

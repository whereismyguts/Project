using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {

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
                return Viewport.World2ScreenBounds(new Bounds(Location - new CoordPoint(radius, radius), Location + new CoordPoint(radius, radius))).Width / 2;
            }
        }
    }
    public class InternalRectangle: Geometry {



        public InternalRectangle(CoordPoint leftTop, CoordPoint size, ColorCore color) {
            this.Location = leftTop;
            this.Size = size;
            this.Color = color;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {

    public abstract class Geometry: Renderable {
        //public ColorCore Color { get; protected set; } TODO add color
        public  Geometry(CoordPoint location, CoordPoint size, int padding=0) {
            Location = location;
            Size = size+2*padding;
            Origin = size / 2 + padding;

        }

        public int ZIndex { get; set; } = 1;
    }
    public class WorldGeometry: Geometry {
        public WorldGeometry(CoordPoint location, CoordPoint size) : base(location, size) {
        }
    }

    public class ScreenGeometry: Geometry {
        public ScreenGeometry(CoordPoint location, CoordPoint size, int padding=0) : base(location, size, padding) {
        }

        public override CoordPoint ScreenLocation {
            get {
                return Location;
            }
        }

        public override CoordPoint ScreenOrigin {
            get {
                return Origin;
            }
        }

        public override CoordPoint ScreenSize {
            get {
                return Size;
            }
        }

    }

}

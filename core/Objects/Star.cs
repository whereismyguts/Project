using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    internal class Star: GameObject {
        float t;

        protected internal override Bounds Bounds {
            get {
                return new Bounds(Location - 2, Location + 2);
            }
        }
        protected internal override string ContentString {
            get {
                return "planet3";
            }
        }

        protected internal Star(Viewport viewport, CoordPoint location)
            : base(viewport) {
            Location = location;
            t = 0;
        }

        protected internal override float GetRotation() {
            return t;
        }
        protected internal override void Move() {
            return;
        }
    }
}

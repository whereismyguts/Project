using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {

    public abstract class Geometry: Renderable {
        //public ColorCore Color { get; protected set; } TODO add color
        public Geometry(Vector2 location, Vector2 size, bool round) {
            Location = location;
            Size = size;
            Origin = size / 2;
            IsCircle = round;
        }
        public bool IsCircle;
        public int ZIndex { get; set; } = 1;
        public Color Color { get; set; } = Color.Black;
    }
    public class WorldGeometry: Geometry {
        public WorldGeometry(Vector2 location, Vector2 size, bool round = false) : base(location, size, round) {
        }
    }

    public class ScreenGeometry: Geometry {
        public ScreenGeometry(Vector2 location, Vector2 size, int padding = 0, bool round = false) : base(location, size, round) {
        }

        public override Vector2 ScreenLocation {
            get {
                return Location;
            }
        }

        public override Vector2 ScreenOrigin {
            get {
                return Origin;
            }
        }

        public override Vector2 ScreenSize {
            get {
                return Size;
            }
        }
    }

    public class Line: Geometry { 
        public Vector2 End { get; private set; }
        public Vector2 Start { get; private set; }
        public Line(Vector2 start, Vector2 end) : base(start, end - start, false) {
            Start = start;
            End = end;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core {
    public class StarSystem {
        List<SpaceBody> planets = new List<SpaceBody>();
        List<SpaceBody> stations = new List<SpaceBody>();
        SpaceBody star;
        public CoordPoint MapLocation { get; }
        public SpaceBody Star { get { return star; } }
        public List<SpaceBody> Objects {
            get {
                List<SpaceBody> objs = new List<SpaceBody>();
                objs.AddRange(planets);
                objs.AddRange(stations);
                objs.Add(star);
                return objs;
            }
        }
        public StarSystem(int planetsNumber) {
            planets = new List<SpaceBody>();
            //TODO Data Driven Factory
            star = new SpaceBody(new CoordPoint(0, 0), 5500, "planet1", this);
            planets.Add(new Planet(new CoordPoint(9600, 9600), 150, Core.GetRandomT(), "planet2", true, this));
            planets.Add(new Planet(new CoordPoint(8100, 8100), 100, Core.GetRandomT(), "planet3", false, this));
            planets.Add(new Planet(new CoordPoint(10000, 10000), 200, Core.GetRandomT(), "planet4", true, this));

        }

    }
}


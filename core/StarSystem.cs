using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core {
    public class StarSystem {
        List<AttractingObject> planets = new List<AttractingObject>();
        List<AttractingObject> stations = new List<AttractingObject>();
        AttractingObject star;
        public CoordPoint MapLocation { get; }
        public AttractingObject Star { get { return star; } }
        public List<AttractingObject> Objects {
            get {
                List<AttractingObject> objs = new List<AttractingObject>();
                objs.AddRange(planets);
                objs.AddRange(stations);
                objs.Add(star);
                return objs;
            }
        }
        public StarSystem(int planetsNumber) {
            planets = new List<AttractingObject>();
            //TODO Data Driven Factory
            star = new AttractingObject(new CoordPoint(0, 0), 5500, "planet1", this);
            planets.Add(new Planet(new CoordPoint(9600, 9600), 150, Core.GetRandomT(), "planet2", true, this));
            planets.Add(new Planet(new CoordPoint(8100, 8100), 100, Core.GetRandomT(), "planet3", false, this));
            planets.Add(new Planet(new CoordPoint(10000, 10000), 200, Core.GetRandomT(), "planet4", true, this));

        }

    }
}


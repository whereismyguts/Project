using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    public class StarSystem {
        List<Body> planets = new List<Body>();
        List<Body> stations = new List<Body>();
        Body star;
        public CoordPoint MapLocation { get; }
        public Body Star { get { return star; } }
        public List<Body> Objects {
            get {
                List<Body> objs = new List<Body>();
                objs.AddRange(planets);
                objs.AddRange(stations);
                objs.Add(star);
                return objs;
            }
        }
        public StarSystem(int planetsNumber) {
            planets = new List<Body>();
            //TODO Data Driven Factory
            star = new Body(new CoordPoint(0, 0), 55000, "planet1", this);
            planets.Add(new Planet(new CoordPoint(96000, 96000), 150, Core.GetRandomT(), "planet2", true, this));
            planets.Add(new Planet(new CoordPoint(81000, 81000), 100, Core.GetRandomT(), "planet3", false, this));
            planets.Add(new Planet(new CoordPoint(100000, 100000), 200, Core.GetRandomT(), "planet4", true, this));

        }

    }
}


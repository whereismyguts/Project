using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCore {
    public class StarSystem {
        Body star;

        public CoordPoint MapLocation { get; }
        public List<Body> Objects {
            get {
                List<Body> objs = new List<Body>();
                objs.AddRange(planets);
                objs.AddRange(stations);
                objs.Add(star);
                return objs;
            }
        }
        public Body Star { get { return star; } }

        public StarSystem(int planetsNumber) {
            planets = new List<Body>();
            //TODO Data Driven Factory
            star = new Body(new CoordPoint(0, 0), 4000,  this);
            planets.Add(new Planet(new CoordPoint(10600, 9600), 1400, RndService.GetPeriod(),  true, this));
            planets.Add(new Planet(new CoordPoint(12100, 8100), 1000, RndService.GetPeriod(),  false, this));
            planets.Add(new Planet(new CoordPoint(15000, 15000), 3000, RndService.GetPeriod(),  true, this));

        }
        List<Body> planets = new List<Body>();
        List<Body> stations = new List<Body>();
    }
}


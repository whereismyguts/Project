using System;
using System.Collections.Generic;
using System.Linq;
using Core;

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
            star = new Body(new CoordPoint(0, 0), 55000,  this);
            planets.Add(new Planet(new CoordPoint(96000, 96000), 15000, RndService.GetPeriod(),  true, this));
            planets.Add(new Planet(new CoordPoint(81000, 81000), 10000, RndService.GetPeriod(),  false, this));
            planets.Add(new Planet(new CoordPoint(100000, 100000), 20000, RndService.GetPeriod(),  true, this));

        }
        List<Body> planets = new List<Body>();
        List<Body> stations = new List<Body>();
    }
}


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

        public List<GameObject> Effects {
            get {
                return effects;
            }
        }

        public Body Star { get { return star; } }

        public StarSystem(int planetsNumber) {
            planets = new List<Body>();
            //TODO Data Driven Factory
            star = new Body(new CoordPoint(0, 0), 4000,  this);
            for(int i=0;i<5;i++)
            planets.Add(new Planet(Rnd.Get(5000, 20000), Rnd.Get(500, 3000), Rnd.GetPeriod(),  Rnd.Bool(), this));

        }
        List<Body> planets = new List<Body>();
        List<Body> stations = new List<Body>();
        List<GameObject> effects = new List<GameObject>();

        internal void Add(GameObject effect) {
            effects.Add(effect);
        }

        internal void CleanObjects() {
            planets.RemoveAll(p => p.ToRemove);
            stations.RemoveAll(p => p.ToRemove);
            effects.RemoveAll(p => p.ToRemove);
        }
    }
}


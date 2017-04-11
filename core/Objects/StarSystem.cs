using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCore {
    public class StarSystem {
        List<GameObject> objects = new List<GameObject>();

        public List<GameObject> Objects {
            get {
                return objects;
            }
        }



        public Body Star { get; internal set; }



        internal void Add(GameObject obj) {
            objects.Add(obj);
        }
        internal void CleanObjects() {
            objects.RemoveAll(p => p.ToRemove);
        }

        internal void CreatePlanets(int planetsNumber = 3) {

            //TODO Data Driven Factory
            Star = new Body(new CoordPoint(0, 0), 20000);
            for(int i = 0; i < planetsNumber; i++)
                new Planet(Rnd.Get(30000, 80000), Rnd.Get(7000, 15000), Rnd.GetPeriod(), Rnd.Bool());
        }
    }
}


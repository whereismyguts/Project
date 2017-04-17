using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
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



        public SpaceBody Star { get; internal set; }



        internal void Add(GameObject obj) {
            objects.Add(obj);
        }
        internal void CleanObjects() {
            objects.RemoveAll(p => p.ToRemove);
        }

        internal void CreatePlanets(World world, int planetsNumber = 3) {
            //TODO Data Driven Factory



            Star = new Star(100, world);
            for(int i = 0; i < planetsNumber; i++)
                new Planet(GameObject.GetNewLocation(null), Rnd.Get(10, 20), world);
        }
    }
}


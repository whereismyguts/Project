using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCore {
    public class MainCore {
        static MainCore instance;
        static Random rnd = new Random();
        List<Ship> ships = new List<Ship>();

        public static MainCore Instance {
            get {
                if(instance == null)
                    instance = new MainCore();
                return instance;
            }
        }
        public IEnumerable<VisualElement> VisualElements {
            get {
                foreach(GameObject obj in Objects)
                    if(Viewport.Contains(obj))
                        yield return new VisualElement(obj);
            }
        }
        public Viewport Viewport { get; set; }
        public List<Ship> Ships {
            get {
                return ships;
            }
        }
        List<StarSystem> StarSystems { get; set; } = new List<StarSystem>();
        IEnumerable<GameObject> GetAllObjects() {
            foreach(StarSystem sys in StarSystems)
                foreach(GameObject obj in sys.Objects)
                    yield return obj;
            foreach(Ship s in ships) {
                yield return s.Weapon;
                yield return s;
            }
        }
        List<GameObject> Objects {
            get { return GetAllObjects().ToList(); }
        }

        MainCore() {
            Viewport = new Viewport(300, 300, 0, 0);
            StarSystems.Add(new StarSystem(3));
            CreatePlayers();
        }

        public void Update() {
            foreach(GameObject obj in Objects)
                obj.Step();
            Viewport.Centerpoint = ships[0].Location;
        }
        void CreatePlayers() {
            ships.Add(new Ship(new CoordPoint(-10100, 10100), StarSystems[0].Objects[1], StarSystems[0])); // player controlled
            ships.Add(new Ship(new CoordPoint(10100, 10100), ships[0], StarSystems[0]));
            ships.Add(new Ship(new CoordPoint(-10100, 10100), ships[0], StarSystems[0]));
        }
    }
}

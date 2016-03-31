using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCore {
    public class Core {
        public static Core instance;
        static Random rnd = new Random();
        List<RenderObjectCore> renderObjects;
        List<Ship> ships = new List<Ship>();

        public static Core Instance {
            get {
                if(instance == null)
                    instance = new Core();
                return instance;
            }
        }
        public List<RenderObjectCore> RenderObjects {
            get {
                return renderObjects;
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
            foreach(Ship s in ships)
                yield return s;
        }
        List<GameObject> Objects {
            get { return GetAllObjects().ToList(); }
        }

        Core() {
            Viewport = new Viewport(300, 300, 0, 0);
            StarSystems.Add(new StarSystem(3));
            CreatePlayers();
        }

        public void Update() {
            MoveObjects();
            Viewport.Centerpoint = ships[0].Location;
            //Viewport.SetScale(5f / (objects.First() as Character).Speed);
            //System.Diagnostics.Debug.WriteLine((objects.First() as Character).Speed.ToString());
            UpdateRenderObjects();
        }
        static internal float GetRandomT() {
            return (float)(rnd.NextDouble() * Math.PI * 2);
        }
        void MoveObjects() {
            foreach(GameObject obj in Objects)
                obj.Step();

        }
        void UpdateRenderObjects() {
            renderObjects = new List<RenderObjectCore>();
            foreach(GameObject obj in Objects)
                if(obj.IsVisible)
                    renderObjects.Add(new RenderObjectCore(obj.GetScreenBounds(), obj.GetMiniMapBounds(), obj.ContentString, obj.GetRotation()));
        }
        void CreatePlayers() {
            ships.Add(new Ship(new CoordPoint(-10100, 10100), StarSystems[0].Objects[0], StarSystems[0])); // player controlled
            //ships.Add(new Ship(new CoordPoint(10100, 10100), ships[0], StarSystems[0]));
            //ships.Add(new Ship(new CoordPoint(-10100, 10100), ships[0], StarSystems[0]));
            
        }
    }
}

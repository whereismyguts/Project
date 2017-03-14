using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCore {
 
    public enum GameState { MainMenu, Space, Pause, Inventory, Landing, Station, CustomBattle };
    public class StateEventArgs: EventArgs {
        readonly GameState state;
        public GameState State { get { return state; } }

        internal StateEventArgs(GameState state) {
            this.state = state;
        }
    }
    public class MainCore {
        static MainCore instance;

        List<StarSystem> StarSystems { get; set; } = new List<StarSystem>();

        public InteractionController Controller { get; } = new InteractionController();

        public static MainCore Instance {
            get {
                if(instance == null)
                    instance = new MainCore();
                return instance;
            }
        }

        public List<GameObject> Objects {
            get { return GetAllObjects().ToList(); }
        }
        public List<Ship> Ships { get { return ships; } }
        GameState state = GameState.MainMenu;
        public GameState State {
            get { return state; }
            set {
                if(state == value)
                    return;
                state = value;
                if(StateChanged != null)
                    StateChanged(instance, new StateEventArgs(value));
            }
        }

        public delegate void StateEventHandler(object sender, StateEventArgs e);
        public event StateEventHandler StateChanged;

        public Viewport Viewport { get; set; }
        public static CoordPoint Cursor { get; set; }

        MainCore() {
            Viewport = new Viewport(300, 300, 0, 0);
            StarSystems.Add(new StarSystem(3));
            CreatePlayers();
        }

        void CreatePlayers() {
            ships.Add(new Ship(null, StarSystems[0])); // player controlled
            ships.Add(new Ship(ships[0], StarSystems[0]));
            ships.Add(new Ship(ships[0], StarSystems[0]));
            ships.Add(new Ship(ships[0], StarSystems[0]));
            ships.Add(new Ship(ships[0], StarSystems[0]));
            //   ships.Add(new Ship(new CoordPoint(-10100, 10100), ships[0], StarSystems[0]));
        }
        IEnumerable<GameObject> GetAllObjects() {

            foreach(StarSystem sys in StarSystems)
                foreach(GameObject obj in sys.Objects)
                    yield return obj;
            foreach(Ship s in ships) yield return s;
        }

        public void Update() {
            if(State == GameState.Space) {
                if(turnIsActive || !turnBasedGamplay) {
                    foreach(GameObject obj in Objects)
                        obj.Step();
                    

                    if(turnBasedGamplay) {
                        turnTime++;
                        if(turnTime == TurnLong) {
                            turnTime = 0;
                            turnIsActive = false;
                        }
                    }
                }

                UpdateViewport();
                
            }
        }

        void UpdateViewport() {

            //Viewport.Centerpoint = ships[0].Position;

            float left = float.MaxValue;
            float right = float.MinValue;
            float top = float.MinValue;
            float bottom = float.MaxValue;

            CoordPoint total = new CoordPoint();

            foreach(var shp in Ships) {
                total += shp.Position;
                if(shp.Position.X > right)
                    right = shp.Position.X;
                if(shp.Position.X < left)
                    left = shp.Position.X;
                if(shp.Position.Y > top)
                    top = shp.Position.Y;
                if(shp.Position.Y < bottom)
                    bottom = shp.Position.Y;
            }
             Cursor = total / ships.Count;
            var center = total / ships.Count;
            Viewport.Centerpoint = center;

            Viewport.Scale = (right - left) / 600;

            Viewport.Update();
        }

        static Random rnd = new Random();
        List<Ship> ships = new List<Ship>();

        static int pressCoolDown = 0;
        public static void Pressed(CoordPoint coordPoint) {
            if(pressCoolDown < 0)
                pressCoolDown++;
            else
               {
                pressCoolDown = -10;
               // var ship = new Ship(instance.ships[0], instance.StarSystems[0]);
                //ship.Position = coordPoint;
                //instance.ships.Add(ship);
            }
        }
        const int TurnLong = 100;
        int turnTime = 0;
        bool turnIsActive = false;
        bool turnBasedGamplay = false;
        public void NextTurn() {
            turnIsActive = true;
        }
    }
}

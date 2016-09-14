using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCore {
 
    public enum GameState { MainMenu, Space, Pause, Inventory, Landing, Station };
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
                if(active) {
                    foreach(GameObject obj in Objects)
                        obj.Step();
                    Viewport.Centerpoint = ships[0].Position;
                    turnTime++;
                    if(turnTime == TurnLong) {
                        turnTime = 0;
                        active = false;
                    }
                }
                Viewport.Update();
            }
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
        bool active = false;
        public void NextTurn() {
            active = true;
        }
    }
}

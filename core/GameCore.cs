using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCore {


    public class StateEventArgs: EventArgs {
        readonly UIState state;
        public UIState State { get { return state; } }

        internal StateEventArgs(UIState state) {
            this.state = state;
        }
    }
    public class MainCore {
        static MainCore instance;

        static List<UIState> states = new List<UIState>();
        static int state;

        List<StarSystem> StarSystems { get; set; } = new List<StarSystem>();
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

        public UIState CurrentState {
            get { return states[state]; }
        }

        internal static void SwitchState() {
            if(state < states.Count - 1)
                state++;
            else
                state = 0;
        }

        public static void AddControl(int state, IControl control, int actor) {
            states.First(st => st.Id == state).AddControl(control, actor);
        }

        //public int State {
        //    get { return state; }
        //    set {
        //        if(state == value)
        //            return;
        //        state = value;
        //        if(StateChanged != null)
        //            StateChanged(instance, new StateEventArgs(states[value]));
        //    }
        //}

        public delegate void StateEventHandler(object sender, StateEventArgs e);

        public static void AddStates(UIState[] newstates) {
            states.AddRange(newstates);
        }

        public event StateEventHandler StateChanged;
        public bool TurnBasedMode { get; private set; } = false;
        public Viewport Viewport { get; set; }
        public static CoordPoint Cursor { get; set; }

        MainCore() {
            Viewport = new Viewport(300, 300, 0, 0);
            StarSystems.Add(new StarSystem(2));
        }

        public Ship Player1 { get; private set; }
        public Ship Player2 { get; private set; }

        void CreatePlayers() {

            for(int i = 0; i < 6; i++) {
                var ship = new Ship(StarSystems[0]) { Fraction = i % 2 == 0 ? 1 : 0 };
                if(i == 0) {
                    Player1 = ship;
                    ShipController.Controllers.Add(new ManualControl(ship));
                }
                else
                    if(i == 6 / 2) {
                    Player2 = ship;
                    ShipController.Controllers.Add(new ManualControl(ship));
                }
                else
                    ShipController.Controllers.Add(new AutoControl(ship));
                ships.Add(ship);
            }
            //for(int i = 0; i < 3; i++)
            //    ships.Add(new Ship(StarSystems[0]));
        }
        IEnumerable<GameObject> GetAllObjects() {
            foreach(StarSystem sys in StarSystems) {
                foreach(GameObject obj in sys.Objects)
                    yield return obj;
                foreach(GameObject obj in sys.Effects)
                    yield return obj;
            }
            foreach(Ship s in ships) yield return s;
        }

        internal static void Console(string message) {
            messages.Add(message);
        }

        static List<string> messages = new List<string>();

        void CleanObjects() {
            foreach(StarSystem sys in StarSystems) {
                sys.CleanObjects();
            }
            ships.RemoveAll(s => s.ToRemove);
        }


        public void Update() {
            //&& Controller.Keys.ToList().Contains(32)
            if(CurrentState.InGame) {
                if(ships.Count == 0 && (ships.Count(s => s.Fraction == 0) == 0 || ships.Count(s => s.Fraction == 1) == 0)) {
                    foreach(Ship ship in ships)
                        ship.ToRemove = true;
                    ShipController.Controllers.Clear();
                    CreatePlayers();
                    messages.Clear();
                }
                CleanObjects();

                if(turnIsActive || !TurnBasedMode) {
                    ShipController.Step();

                    foreach(GameObject obj in Objects)
                        obj.Step();

                    if(TurnBasedMode) {
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
            float left = float.MaxValue;
            float right = float.MinValue;
            float top = float.MinValue;
            float bottom = float.MaxValue;

            CoordPoint total = new CoordPoint();

            foreach(var shp in Objects.Where(o => o is Planet || o is Ship)) {
                if(shp.Position.X > right)
                    right = shp.Position.X;
                if(shp.Position.X < left)
                    left = shp.Position.X;
                if(shp.Position.Y > top)
                    top = shp.Position.Y;
                if(shp.Position.Y < bottom)
                    bottom = shp.Position.Y;
            }
            //Cursor = total / Objects.Count;
            //var center = total / Objects.Count;
            // Viewport.Centerpoint = center;
            //Viewport.Centerpoint = new CoordPoint(left + (right - left) / 2, bottom + (top - bottom) / 2);

            Viewport.SetWorldBounds(left, top, right, bottom);
            //Viewport.Scale = Math.Max(right - left, top - bottom) / 300;
            Viewport.Update();
        }

        static Random rnd = new Random();
        List<Ship> ships = new List<Ship>();

        static int pressCoolDown = 0;
        //public static void Pressed(CoordPoint coordPoint) {
        //    if(pressCoolDown < 0)
        //        pressCoolDown++;
        //    else {
        //        pressCoolDown = -10;
        //        // var ship = new Ship(instance.ships[0], instance.StarSystems[0]);
        //        //ship.Position = coordPoint;
        //        //instance.ships.Add(ship);
        //    }
        //}
        const int TurnLong = 100;
        int turnTime = 0;
        bool turnIsActive = false;

        public void NextTurn() {
            turnIsActive = true;
        }

        public void Pause() {
            TurnBasedMode = !TurnBasedMode;
        }
    }
}

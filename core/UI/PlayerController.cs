using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace GameCore {
    public class PlayerController {
        static Player player;
        static CommonInterface commonInterface = new CommonInterface();

        public static Player Player {
            get {
                return player;
            }
            set {
                player = value;
                //Interface = new PlayerInterface(player);
            }
        }

        //public static PlayerInterface Interface { get; private set; }

        internal static void ExecuteKey (ActorKeyPair pair, bool clickedOnce) {
            Execute(pair.Action, clickedOnce);
        }
        internal static void ExecuteCursor (MouseActionInfo pair, bool clickedOnce) {
            foreach (IControl control in MainCore.Instance.CurrentState.Controls)
                if (control.Contains(pair.X, pair.Y)) {
                    if (control.Tag == PlayerAction.Up)
                        Player.Ship.Accselerate();
                }
        }

        

        //public static IEnumerable<IRenderableObject> GetInterfaceElements (Rectangle viewport) {
        //    commonInterface = new CommonInterface();

        //    List<IRenderableObject> res = new List<IRenderableObject>() { commonInterface };
        //    if (Player != null) {
        //        Interface.Update(viewport);
        //        res.Add(Interface);
        //    }

        //    return res;
        //}

        static void Execute (PlayerAction action, bool clickedOnce) {
        //    if (Interface.Focused) {
        //        if (clickedOnce)
        //            switch (action) {
        //                case PlayerAction.Down: Interface.SelectNext(); break;
        //                case PlayerAction.Up: Interface.SelectPrev(); break;
        //                case PlayerAction.Left: Interface.SelectPrev(); break;
        //                case PlayerAction.Right: Interface.SelectNext(); break;
        //                case PlayerAction.Yes: Interface.Select(); break;
        //                case PlayerAction.Tab: Interface.Focus(); break;
        //            }
        //    }
        //    else
        //        switch (action) {
        //            case PlayerAction.Down: //do nothing yet
        //                break;
        //            case PlayerAction.Up: player.Ship.Accselerate(); break;
        //            case PlayerAction.Left: player.Ship.RotateL(); break;
        //            case PlayerAction.Right: player.Ship.RotateR(); break;
        //            case PlayerAction.Yes: player.Ship.Fire(); break;
        //            case PlayerAction.Tab: if (clickedOnce) Interface.Focus(); break;
        //        }
        }


    }
    public class Player {
        Ship ship;

        public Player (Ship ship, int id) {
            this.ship = ship;
            Index = id;

        }

        public int Index { get; internal set; }
        //public PlayerInterface Interface { get; internal set; }

        public Ship Ship { get { return ship; } }
    }
    public class CommonInterface: IRenderableObject {
        public event RenderObjectChangedEventHandler Changed;

        List<Item> items = new List<Item>();
        List<Geometry> geometry = new List<Geometry>();

        public CommonInterface () {
            Update();
        }
        void Update () {
            items.Clear();
            geometry.Clear();
            //geometry.Add(new ScreenGeometry(new Microsoft.Xna.Framework.Vector2(100, 100), new Microsoft.Xna.Framework.Vector2(50, 50)) { Text = "TEST222" });
            //     Debugger.AddLine("common interafce NOT updated");
        }

        public IEnumerable<Item> GetItems () {
            return items;
        }

        public IEnumerable<Geometry> GetPrimitives () {
            return geometry;
        }
    }

    
}
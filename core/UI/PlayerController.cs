using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCore {
    public class PlayerController {
        public static List<Player> Players = new List<Player>();

        public static void AddPlayer(Player p) {
            Players.Add(p);
        }

        internal static void Execute(ActorKeyPair pair, bool clickedOnce) {
            Player p = Players.FirstOrDefault(pl => pl.Index == pair.Actor);
            if(p != null)
                p.Execute(pair.Action, clickedOnce);
        }

        static CommonInterface commonInterface = new CommonInterface();

        public static IEnumerable<IRenderableObject> GetInterfaceElements() {

            List<IRenderableObject> res = new List<IRenderableObject>();
            res.Add(commonInterface);
            res.AddRange(Players.Select(p => p.Interface));
            return res;
        }

        internal static void Clear() {
            Players.Clear();
        }
    }
    public class Player {
        Ship ship;

        public Player(Ship ship, int id) {
            this.ship = ship;
            Index = id;
            Interface = new PlayerInterface(this);
        }

        public int Index { get; internal set; }
        public PlayerInterface Interface { get; internal set; }

        public Ship Ship { get { return ship; } }



        internal void Execute(PlayerAction action, bool clickedOnce) {
            if(Interface.Focused) {
                if(clickedOnce)
                    switch(action) {
                        case PlayerAction.Down: Interface.SelectNext(); break;
                        case PlayerAction.Up: Interface.SelectPrev(); break;
                        case PlayerAction.Left: Interface.SelectPrev(); break;
                        case PlayerAction.Right: Interface.SelectNext(); break;
                        case PlayerAction.Yes: Interface.Select(); break;
                        case PlayerAction.Tab: Interface.Focus(); break;
                    }
            }
            else
                switch(action) {
                    case PlayerAction.Down: //do nothing yet
                        break;
                    case PlayerAction.Up: ship.Accselerate(); break;
                    case PlayerAction.Left: ship.RotateL(); break;
                    case PlayerAction.Right: ship.RotateR(); break;
                    case PlayerAction.Yes: ship.Fire(); break;
                    case PlayerAction.Tab: if(clickedOnce) Interface.Focus(); break;
                }
        }
    }
    public class CommonInterface: IRenderableObject {
        public event RenderObjectChangedEventHandler Changed;

        List<Item> items = new List<Item>();
        List<Geometry> geometry = new List<Geometry>();

        public CommonInterface() {
            Update();

            MainCore.Instance.Viewport.Changed += Viewport_Changed;
        }

        private void Viewport_Changed(ViewportChangedEventArgs args) {
            Update();
        }

        void Update() {
            //ScreenSpriteItem item = new ScreenSpriteItem(Align.FillBottom, new Vector2(150, 150), new Vector2(), new SpriteInfo("256tile.png") { ZIndex = -2 });
            items.Clear();
            geometry.Clear();
            //items.Add(item);
            //geometry.Add(new ScreenGeometry(item.ScreenLocation, item.ScreenSize) { ZIndex = -3, Origin = new Vector2() });
            Debugger.Lines.Add("common interafce NOT updated");
        }

        public IEnumerable<Item> GetItems() {
            return items;
        }

        public IEnumerable<Geometry> GetPrimitives() {
            return geometry;
        }
    }
}
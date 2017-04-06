using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCore {
    public class PlayerController {
        public static List<Player> Players = new List<Player>();

        public static void AddPlayer(Player p) {
            Players.Add(p);
        }

        internal static void Execute(ActorKeyPair pair) {
            Player p = Players.FirstOrDefault(pl => pl.Index == pair.Actor);
            if(p != null)
                p.Execute(pair.Action);
        }

        public static IEnumerable<IRenderableObject> GetInterfaceElements() {
            return Players.Select(p => p.Interface);
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
        public IRenderableObject Interface { get; internal set; }

        public Ship Ship { get { return ship; } }

        internal void Execute(PlayerAction action) {
            switch(action) {
                case PlayerAction.Down: //do nothing yet
                    break;
                case PlayerAction.Up: ship.Accselerate(); break;
                case PlayerAction.Left: ship.RotateL(); break;
                case PlayerAction.Right: ship.RotateR(); break;
                case PlayerAction.Yes: ship.Fire(); break;
            }
        }
    }
}
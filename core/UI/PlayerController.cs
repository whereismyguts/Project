using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCore {
    public class PlayerController {
        static Dictionary<int, Player> Players { get; set; } = new Dictionary<int, Player>();

        public static void AddPlayer(int id, Player p) {
            Players[id] = p;
        }

        internal static void Execute(ActorKeyPair pair) {
            if(Players.ContainsKey(pair.Actor))
                Players[pair.Actor].Execute(pair.Action);
        }

        public static IEnumerable<IRenderableObject> GetInterfaceElements() {
            return Players.Select(p => p.Value.Interface);
        }
    }
    public class Player {
        Ship ship;

        public Player(Ship ship) {
            this.ship = ship;
            Interface = new PlayerInterface(this);
        }

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
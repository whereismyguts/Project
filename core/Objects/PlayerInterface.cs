using GameCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    public class PlayerInterface: IRenderableObject {
        private Player player;

        public PlayerInterface(Player player) {
            this.player = player;
        }

        public IEnumerable<Item> GetItems() {
            return new  Item[]{ };
        }

        public IEnumerable<Geometry> GetPrimitives() {
            return new Geometry[] { new InternalRectangle(new CoordPoint(0, 0), new CoordPoint(100, 100), ColorCore.Green) { RealSize = false} };
        }
    }
}

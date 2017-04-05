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
            foreach(var item in player.Ship.GetItems()) {
                yield return new JustSpriteItem(player.Ship, new CoordPoint(300, 300), new CoordPoint(150, 150), item.Content) { IsWorldSize = false };
            }
        }

        public IEnumerable<Geometry> GetPrimitives() {
            return new Geometry[] { new InternalRectangle(new CoordPoint(0, 0), new CoordPoint(100, 100), ColorCore.Green) { IsWorldSize = false } };
        }
    }
}

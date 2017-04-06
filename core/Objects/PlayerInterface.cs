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
            UpdateItems();
            player.Ship.Inventory.Changed += Inventory_Changed;
        }

        void Inventory_Changed(InventoryChangedEventArgs args) {
            UpdateItems();
        }

        void UpdateItems() {
            ColorCore color = player.Index == 1 ? ColorCore.Blue : ColorCore.Red;
            CoordPoint offset = player.Index == 1 ? new CoordPoint() : new CoordPoint(300, 0);
            Align align = player.Index == 1 ? Align.LeftBottom : Align.RightBottom;
            ScreenSpriteItem hull = new ScreenSpriteItem(align, player.Ship.Hull.Size / 20, player.Ship.Hull.Origin / 20, player.Ship.Hull.SpriteInfo);

            items.Add(hull);
            foreach(AttachedItem item in player.Ship.GetItems().Where(s => !(s is ShipHull))) {
                items.Add(new ScreenSpriteItem(hull, item.Slot.RelativeLocation.GetRotated((float)Math.PI) / 20, item.Size / 20, item.Origin / 20, item.SpriteInfo));
            }
        }

        List<ScreenSpriteItem> items = new List<ScreenSpriteItem>();

        public IEnumerable<Item> GetItems() {
            return items;
        }

        public IEnumerable<Geometry> GetPrimitives() {
            ColorCore color = player.Index == 1 ? ColorCore.Blue : ColorCore.Red;
            CoordPoint offset = player.Index == 1 ? new CoordPoint() : new CoordPoint(300, 0);

            foreach(var item in player.Ship.GetItems()) {
                yield return new InternalRectangle(offset, item.Size / 20, color) { }; // isworld size!!

            }

        }
    }
}

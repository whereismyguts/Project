using GameCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    public class PlayerInterface: IRenderableObject {
        private Player player;
        int selectedIndex = 0;

        public PlayerInterface(Player player) {
            this.player = player;
            UpdateItems();
            player.Ship.Inventory.Changed += InterfaceChanged;
            MainCore.Instance.Viewport.Changed += InterfaceChanged;
        }

        void InterfaceChanged(EventArgs args) {
            RaiseChanged();
        }

        private void RaiseChanged() {
            UpdateItems();
            if(Changed != null)
                Changed(new RenderObjectChangedEventArgs(this));
        }

        void UpdateItems() {
            Align align = player.Index == 1 ? Align.LeftBottom : Align.RightBottom;
            ScreenSpriteItem hull = new ScreenSpriteItem(
                align, player.Ship.Hull.Size / 20,
                player.Ship.Hull.Origin / 20,
                player.Ship.Hull.SpriteInfo);
            items.Clear();
            items.Add(hull);

            var shipItems = player.Ship.GetItems().ToList();

            foreach(AttachedItem item in shipItems.Where(s => !(s is ShipHull))) {
                items.Add(new ScreenSpriteItem(hull, item.Slot.RelativeLocation.GetRotated((float)Math.PI) / 20, item.Size / 20, item.Origin / 20, item.SpriteInfo));
            }
            //ColorCore color = player.Index == 1 ? ColorCore.Blue : ColorCore.Red;
            //CoordPoint offset = player.Index == 1 ? new CoordPoint() : new CoordPoint(300, 0);

            geometry.Clear();
            geometry.Add(new ScreenGeometry(hull.ScreenLocation, hull.ScreenSize, 20) { ZIndex = -1 });
            if(Focused)
                geometry.Add(new ScreenGeometry(
                items[selectedIndex].ScreenLocation,
                items[selectedIndex].ScreenSize));
        }

        internal void SelectPrev() {
            if(selectedIndex > 0)
                selectedIndex--;
            else selectedIndex = items.Count - 1;
            RaiseChanged();
        }

        internal void SelectNext() {
            if(selectedIndex < items.Count - 1)
                selectedIndex++;
            else
                selectedIndex = 0;
            RaiseChanged();
        }

        internal void Select() {

        }

        internal void Focus() {
            Focused = !Focused;
            RaiseChanged();
        }

        List<ScreenSpriteItem> items = new List<ScreenSpriteItem>();
        List<ScreenGeometry> geometry = new List<ScreenGeometry>();

        public bool Focused { get; set; }

        public event RenderObjectChangedEventHandler Changed;

        public IEnumerable<Item> GetItems() {
            return items;
        }

        public IEnumerable<Geometry> GetPrimitives() {
            return geometry;
        }
    }
}

using GameCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GameCore {
    public class PlayerInterface: IRenderableObject {
        private Player player;
        int selectedIndex = 0;

        public PlayerInterface(Player player) {
            this.player = player;
            //Update();
            player.Ship.Inventory.Changed += InterfaceChanged;
            MainCore.Instance.Viewport.Changed += InterfaceChanged;
        }

        void InterfaceChanged(EventArgs args) {
            //RaiseChanged();
        }

        private void RaiseChanged() {
            //Update();
            if(Changed != null)
                Changed(new RenderObjectChangedEventArgs(this));
        }


        List<Item> ShipItems { get { return player.Ship.Inventory.Container; } }

        internal void Update(Rectangle viewport) {
            geometry.Clear();
            Vector2 size = new Vector2(180, 18);
            float x = player.Index == 1 ? 10 : viewport.Width - size.X - 10;
            float y = viewport.Height - 90;

            if(!Focused) {
                geometry.Add(new ScreenGeometry(new Vector2(x, y), new Vector2(size.X, 18 * 4)));
                return;
            }

            for(int j = minItemPresented; j <= maxItemPresented; j++) {
                AttachedItem item = ShipItems[j] as AttachedItem;
                if(item == null) { }
                var listItem = new ScreenGeometry(new Vector2(x, y), size) { Text = j + "." + item.Name };
                listItem.Color = j == selectedIndex ? Color.Red : Color.Black;
                listItem.TextColor = item.Slot == null ? Color.Black : Color.Green;
                geometry.Add(listItem);
                y += 18;
            }
        }

        void UpdatePresentedItems() {
            if(selectedIndex == 0) {
                minItemPresented = selectedIndex;
                maxItemPresented = selectedIndex + 3;
                return;
            }
            if(selectedIndex == ShipItems.Count - 1) {
                maxItemPresented = selectedIndex;
                minItemPresented = selectedIndex - 3;
                return;
            }
            if(selectedIndex < minItemPresented) {
                minItemPresented--;
                maxItemPresented--;
            }
            else
            if(selectedIndex > maxItemPresented) {
                minItemPresented++;
                maxItemPresented++;
            }
        }

        int minItemPresented = 0;
        int maxItemPresented = 3;

        internal void SelectPrev() {
            if(selectedIndex > 0)
                selectedIndex--;
            else selectedIndex = ShipItems.Count - 1;
            RaiseChanged();
            UpdatePresentedItems();
        }
        internal void SelectNext() {
            if(selectedIndex < ShipItems.Count - 1)
                selectedIndex++;
            else
                selectedIndex = 0;
            RaiseChanged();
            UpdatePresentedItems();
        }

        internal void Select() {
            var item = player.Ship.Inventory.Container[selectedIndex];
            if(item is AttachedItem)
                player.Ship.Inventory.Attach(item as AttachedItem);
            else
                item.Activate();
            //ShipItems[selectedIndex].Activate();
        }

        internal void Focus() {
            Focused = !Focused;
            RaiseChanged();
        }

        List<Item> items = new List<Item>();
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

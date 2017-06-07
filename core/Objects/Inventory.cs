using System;
using System.Collections.Generic;

namespace GameCore {
    public class InventoryChangedEventArgs: EventArgs {
        Inventory inv;
        public Inventory Inv { get { return inv; } }
        public InventoryChangedEventArgs(Inventory inv) {
            this.inv = inv;
        }
    }

    public class Inventory {
        ShipHull hull;

        public List<Item> Container { get; set; } = new List<Item>();
        public Inventory(ShipHull hull) {
            this.hull = hull;
            RaiseChanged();
        }
        public void Attach(AttachedItem item) {
            hull.Attach(item, null);
            RaiseChanged();
        }
        public void Attach(Slot slot, AttachedItem item) {
            hull.Attach(item, slot);
            RaiseChanged();
        }
        internal void Add(Item item) {
            Container.Add(item);
            RaiseChanged();
        }
        public delegate void InventoryChangedEventHandler(InventoryChangedEventArgs args);
        public event InventoryChangedEventHandler Changed;
        void RaiseChanged() {
            if(Changed != null)
                Changed(new InventoryChangedEventArgs(this));
        }
    }
}
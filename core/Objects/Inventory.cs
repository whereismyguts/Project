using System;
using System.Collections.Generic;

namespace GameCore {
    public class Inventory {
        ShipHull hull;
        int SummaryVolume {
            get {
                int sum = 0;
                foreach(Item item in Container)
                    sum += item.Volume;
                return sum;
            }
        }
        public List<Item> Container { get; set; } = new List<Item>();
        public Inventory(ShipHull hull) {
            
            this.hull = hull;
        }
        public void Attach(Slot slot, AttachedItem item) {
            hull.Attach(item, slot);
        }
        internal void Add(Item item) {
            if(SummaryVolume + item.Volume <= hull.Volume) {
                Container.Add(item);
            }
        }
        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    public enum SlotType { EngineSlot, WeaponSlot };
    public class Slot {
        public SlotType Type { get; }
        private CoordPoint relativeLocation;

        protected internal ShipHull Hull { get; set; }
        Item attachedItem;
        public Item AttachedItem { get { return attachedItem == null ? new EmptySlotItem(this) : attachedItem; } set { attachedItem = value; } }

        public bool IsEmpty { get { return attachedItem == null; } }

        public CoordPoint RelativePosition { get { return relativeLocation.GetRotated(Hull.Rotation); } }

        public Slot(CoordPoint relativeLocation, ShipHull hull, SlotType type) {
            this.relativeLocation = relativeLocation;
            Hull = hull;
            Type = type;
        }


        internal void Attach(AttachedItem item) {
            AttachedItem = item;
            item.Slot = this;
        }

        internal void Detach() {
            AttachedItem = null;
        }
    }
}

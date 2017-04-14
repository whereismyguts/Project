using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    public enum SlotType { EngineSlot, WeaponSlot };
    public class Slot {
        public SlotType Type { get; }
        public Vector2 RelativeLocation { get; }

        protected internal ShipHull Hull { get; set; }
        Item attachedItem;
        public Item AttachedItem { get { return attachedItem == null ? new EmptySlotItem(this) : attachedItem; } set { attachedItem = value; } }

        public bool IsEmpty { get { return attachedItem == null; } }

        public Vector2 RelativePosition {
            get {
                return RelativeLocation.GetRotated(Hull.Rotation);
            }
        }

        public Slot(Vector2 relativeLocation, ShipHull hull, SlotType type) {
            this.RelativeLocation = relativeLocation;
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

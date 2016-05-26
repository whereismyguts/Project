using System;
using System.Collections.Generic;

namespace GameCore {
    public abstract class Item {
        Viewport Viewport { get { return MainCore.Instance.Viewport; } }

        protected internal CoordPoint Origin { get; }
        protected internal abstract CoordPoint Position { get; }
        protected internal CoordPoint Size { get; }

        public virtual string ContentString { get { return string.Empty; } }
        public CoordPoint PositionScreen { get { return Viewport.World2ScreenPoint(Position); } }

        public abstract float Rotation { get; }
        public CoordPoint ScreenOrigin { get { return Viewport.World2ScreenBounds(new Bounds(0, 0, Origin.X, Origin.Y)).Size; } }
        public CoordPoint ScreenSize { get { return Viewport.World2ScreenBounds(new Bounds(0, 0, Size.X, Size.Y)).Size; } }

        public Item(CoordPoint size, CoordPoint origin) { //TODO implement in children
            Size = size;
            Origin = origin;
        }
    }

    public class Slot {
        private CoordPoint relativeLocation;

        protected internal ShipHull Hull { get; set; }

        public Item AttachedItem { get; internal set; }

        public bool IsEmpty { get { return AttachedItem == null; } }

        public CoordPoint RelativePosition { get { return relativeLocation.GetRotated(Hull.Rotation); } }

        public Slot(CoordPoint relativeLocation, ShipHull hull) {
            this.relativeLocation = relativeLocation;
            Hull = hull;
        }

        internal void Attach(AttachedItem item) {
            AttachedItem = item;
            item.Slot = this;
        }

        internal void Detach() {
            AttachedItem = null;
        }
    }
    public class SpaceBodyItem: Item {
        private Body body;

        protected internal override CoordPoint Position { get { return body.Position; } }

        public override float Rotation { get { return body.Rotation; } }

        public SpaceBodyItem(Body body) : base(new CoordPoint(body.Radius * 2f, body.Radius * 2f), new CoordPoint(body.Radius, body.Radius)) {
            this.body = body;
        }
    }
    public class ShipHull: Item {
        protected internal Ship Owner { get; set; }
        protected internal override CoordPoint Position { get { return Owner.Position; } }

        public override string ContentString {
            get {
                return "256tile.png";
            }
        }
        public override float Rotation { get { return Owner.Rotation; } }

        public List<Slot> Slots { get { return slots; } }

        public ShipHull() : base(new CoordPoint(100, 100), new CoordPoint(50, 50)) {
            slots.Add(new Slot(new CoordPoint(25, -50), this));
            slots.Add(new Slot(new CoordPoint(-25, -50), this));
        }

        public void Attach(AttachedItem item, Slot slot) {
            if(!slot.IsEmpty)
                slot.Detach();
            slot.Attach(item);

        }
        List<Slot> slots = new List<Slot>();
    }

    public class AttachedItem: Item {

        protected internal override CoordPoint Position {
            get {
                return Slot.Hull.Position + Slot.RelativePosition;
            }
        }

        protected internal Slot Slot { get; set; }

        public override float Rotation {
            get {
                return 0 + Slot.Hull.Rotation; // TODO set rotation external
            }
        }

        public AttachedItem() : base(new CoordPoint(20, 40), new CoordPoint(10, 40)) {

        }
    }
}
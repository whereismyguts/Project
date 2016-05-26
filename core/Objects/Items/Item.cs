using System;
using System.Collections.Generic;

namespace GameCore {
    public abstract class Item {
        
        public string ContentString { get; protected set; }
        protected internal virtual Bounds Bounds { get; }
        protected internal CoordPoint Size { get; protected set; }
        protected internal virtual CoordPoint Origin { get; set; }

        protected Viewport Viewport { get { return MainCore.Instance.Viewport; } }

        public abstract float Rotation { get; }


        protected internal abstract Bounds GetScreenBounds();
        public Item(CoordPoint size, CoordPoint origin) { //TODO implement in children
            ContentString = "256tile.png";
            Size = size;
            Origin = origin;
        }
    }

    public class Slot {
        private CoordPoint relativeLocation;

        public CoordPoint RelativeLocation { get { return relativeLocation.GetRotated(Hull.Rotation); } }
        public Item AttachedItem { get; internal set; }
        protected internal ShipHull Hull { get; set; }
        public Slot(CoordPoint relativeLocation, ShipHull hull) {
            this.relativeLocation = relativeLocation;
            Hull = hull;
        }

        public bool IsEmpty { get { return AttachedItem == null; } }

        internal void Detach() {
            AttachedItem = null;
        }

        internal void Attach(Attached item) {
            AttachedItem = item;
            //item.Origin = Location;
            item.Slot = this;
        }
    }
    public class ShipHull: Item {
        List<Slot> slots = new List<Slot>();
        public List<Slot> Slots { get { return slots; } }
        protected internal Ship Owner { get; set; }
        protected internal override Bounds Bounds {
            get {
                return new Bounds(Owner.Location - Origin, Owner.Location - Origin + Size);
            }
        }
        public ShipHull() : base(new CoordPoint(100, 100), new CoordPoint(50, 50)) {
            slots.Add(new Slot(new CoordPoint(25, 50),this));
            slots.Add(new Slot(new CoordPoint(75, 50),this));
        }
        protected internal override Bounds GetScreenBounds() {
            return Viewport.World2ScreenBounds(Owner.Bounds);
        }
        public override float Rotation {
            get {
                return Owner.Rotation;
            }
        }



        public void Attach(Attached item, Slot slot) {
            if(!slot.IsEmpty)
                slot.Detach();
            slot.Attach(item);
            
        }
    }

    public class Attached: Item {
        
        protected internal Slot Slot { get; set; }
        public override float Rotation {
            get {
                return Slot.Hull.Rotation;
            }
        }
        protected internal override CoordPoint Origin {
            get {
                return Slot.Hull.Origin;
            }
        }
        protected internal override Bounds Bounds {
            get {
                CoordPoint lefttop = Slot.Hull.Bounds.LeftTop + Slot.RelativeLocation - Origin;
                return new Bounds(lefttop, lefttop + Size); }
                 
        }
        
        public Attached() : base(new CoordPoint(10, 50), new CoordPoint(5, 5)) {
            
        }
        protected internal override Bounds GetScreenBounds() {
            return Viewport.World2ScreenBounds(Bounds);
            //return new Bounds( Hull.GetScreenBounds().LeftTop+Origin-Size/2f, Hull.GetScreenBounds().LeftTop + Origin - (Size -Origin))
            //CoordPoint l = Owner.Location + Owner.Direction.GetRotated(45) * 5;
            //return new Bounds(l, l + new CoordPoint(5, 5));
            //TODO slot bounds on the hull
        }
    }
}
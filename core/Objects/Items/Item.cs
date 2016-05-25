using System;

namespace GameCore {
    public abstract class Item {
        protected Ship Owner { get; set; }
        public string ContentString { get; protected set; } 
        //protected internal abstract Bounds Bounds { get; }
        protected internal CoordPoint Size { get;protected  set; }
        protected internal CoordPoint Origin { get; protected set; }

        protected Viewport Viewport { get { return MainCore.Instance.Viewport; } }

        public abstract float Rotation { get;  }

        protected internal abstract Bounds GetScreenBounds();
        public Item(Ship ship, CoordPoint size, CoordPoint origin) { //TODO implement in children
            ContentString = "256tile.png";
            Owner = ship;
            Size = size;
            Origin = origin;
        }
    }
    public class ShipHull: Item {
        //TODO add slots
        public ShipHull(Ship ship) : base(ship, new CoordPoint(100, 100), new CoordPoint(50, 50)) {
        }
        protected internal override Bounds GetScreenBounds() {
            return Viewport.World2ScreenBounds(Owner.Bounds);
        }
        public override float Rotation {
            get {
                return Owner.Rotation;
            }
        }
    }
    
    public class Cannon: Item {
        float r = 0;
        public override float Rotation {
            get {
                return r=+0.1f;
            }
        }
        public Cannon(Ship ship):base(ship, new CoordPoint(10,10), new CoordPoint(5, 5)) { }
        protected internal override Bounds GetScreenBounds() {
            CoordPoint l = Owner.Location + Owner.Direction.GetRotated(45)*5;
            return new Bounds(l, l + new CoordPoint(5, 5));
            //TODO slot bounds on the hull
        }
    }
}
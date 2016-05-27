using System;
using System.Collections.Generic;

namespace GameCore {
    public abstract class Item {
        Viewport Viewport { get { return MainCore.Instance.Viewport; } }

        protected internal CoordPoint Origin { get; }
        protected internal abstract CoordPoint Position { get; }
        protected internal CoordPoint Size { get; }

        public abstract SpriteInfo SpriteInfo { get; }
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

        public override SpriteInfo SpriteInfo {
            get {
                return new SpriteInfo("planet.png", 1);
            }
        }

        public SpaceBodyItem(Body body) : base(new CoordPoint(body.Radius * 2f, body.Radius * 2f), new CoordPoint(body.Radius, body.Radius)) {
            this.body = body;
        }
    }
    public class ShipHull: Item {
        protected internal Ship Owner { get; set; }
        protected internal override CoordPoint Position { get { return Owner.Position; } }

        public override SpriteInfo SpriteInfo {
            get {
                return new SpriteInfo("player_1_straight_idle.gif", 1);
            }
        }
        public override float Rotation { get { return Owner.Rotation; } }

        public List<Slot> Slots { get { return slots; } }

        public ShipHull() : base(new CoordPoint(60, 100), new CoordPoint(30, 15)) {
            slots.Add(new Slot(new CoordPoint(-15, 30), this));
            slots.Add(new Slot(new CoordPoint(15, 30), this));
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
                return (float)(-Math.PI / 2f) + Slot.Hull.Rotation; // TODO set rotation external
            }
        }

        public AttachedItem() : base(new CoordPoint(50, 50), new CoordPoint(25, 25)) {

        }
        public override SpriteInfo SpriteInfo {
            get {
                return new SpriteInfo( "flame_sprite.png", 6);
            }
        }
    }
    public struct SpriteInfo {
        public SpriteInfo(string c, int f) {
            Content = c;
            Framecount = f;
        }
        public string Content;
        public int Framecount;
    }
}
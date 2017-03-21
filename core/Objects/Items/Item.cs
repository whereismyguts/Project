using System;
using System.Collections.Generic;

namespace GameCore {
    public abstract class Item {

        Viewport Viewport { get { return MainCore.Instance.Viewport; } }
        public virtual void Step() {

        }
        protected internal CoordPoint Origin { get; }
        protected internal abstract CoordPoint Position { get; }
        protected internal CoordPoint Size { get; }
        public virtual string Name { get; }
        public virtual int Volume { get {return 10; } }

        public abstract SpriteInfo SpriteInfo { get; }
        public CoordPoint PositionScreen { get { return Viewport.World2ScreenPoint(Position); } }

        public abstract float Rotation { get; }
        public CoordPoint ScreenOrigin { get { return Viewport.World2ScreenBounds(new Bounds(0, 0, Origin.X, Origin.Y)).Size; } }
        public CoordPoint ScreenSize { get { return Viewport.World2ScreenBounds(new Bounds(0, 0, Size.X, Size.Y)).Size; } }



        public Item(CoordPoint size, CoordPoint origin) { //TODO implement in children
            Size = size;
            Origin = origin;
        }

        public abstract void Activate();
        public abstract void Deactivate();
        public override string ToString() {
            return Name;
        }
    }


    //public class SpaceBodyItem: Item {
    //    private Body body;

    //    protected internal override CoordPoint Position { get { return body.Position; } }

    //    public override float Rotation { get { return body.Rotation; } }

    //    public override SpriteInfo SpriteInfo {
    //        get {
    //            return body.SpriteInfo;
    //        }
    //    }

    //    public SpaceBodyItem(Body body) : base(new CoordPoint(body.Radius * 2f, body.Radius * 2f), new CoordPoint(body.Radius, body.Radius)) {
    //        this.body = body;
    //    }

    //    public override void Activate() { }
    //    public override void Deactivate() { }
    //}
    public class ShipHull: Item {
        protected internal Ship Owner { get; set; }
        protected internal override CoordPoint Position { get { return Owner.Position; } }
        int capacity = 100;
        public override int Volume { get { return capacity; } }
        public override SpriteInfo SpriteInfo {
            get {
                return new SpriteInfo("player_1_straight_idle.gif", 1);
            }
        }
        public override float Rotation { get { return Owner.Rotation; } }

        public List<Slot> Slots { get { return slots; } }


        public ShipHull() : base(new CoordPoint(900, 1500), new CoordPoint(450, 450)) {
            slots.Add(new Slot(new CoordPoint(-150, 150), this, SlotType.EngineSlot));
            slots.Add(new Slot(new CoordPoint(150, 150), this, SlotType.EngineSlot));
            slots.Add(new Slot(new CoordPoint(0, -200), this, SlotType.WeaponSlot));
        }
        public override void Activate() { }
        public override void Deactivate() { }
        public void Attach(AttachedItem item, Slot slot) {
            if(!slot.IsEmpty)
                slot.Detach();
            slot.Attach(item);

        }
        List<Slot> slots = new List<Slot>();

        internal IEnumerable<DefaultEngine> GetEngines() {
            for(int i =0;i<slots.Count;i++)
                if(slots[i].AttachedItem is DefaultEngine)
                    yield return slots[i].AttachedItem as DefaultEngine;
        }
    }

    //public abstract class StorableItem {
    //    public abstract string Name { get; }
    //    public abstract int Volume { get; }
    //}

    public abstract class AttachedItem: Item {

        protected internal override CoordPoint Position {
            get {
                return Slot.Hull.Position + Slot.RelativePosition;
            }
        }

        protected internal virtual Slot Slot { get; set; }

        public override float Rotation {
            get {
                return /*(float)(-Math.PI / 2f) + */Slot.Hull.Rotation; // TODO set rotation external
            }
        }

        public AttachedItem(CoordPoint size, CoordPoint origin) : base(size, origin) {

        }
        //public abstract SpriteInfo SpriteInfo {
        //    get {
        //        return new SpriteInfo("flame_sprite.png", 6);
        //    }
        //}
    }
    public class SpriteInfo {
        public SpriteInfo(string c, int f) {
            Content = c;
            Framecount = f;
        }
        public SpriteInfo() { }
        public string Content { get; } = string.Empty;
        public int Framecount { get; } = 1;
    }
    public class EmptySlotItem: AttachedItem {

        public override SpriteInfo SpriteInfo {
            get {
                return new SpriteInfo("emptyslot.png", 1);
            }
        }
        public EmptySlotItem(Slot slot) : base(new CoordPoint(10, 10), new CoordPoint(5, 5)) {
            Slot = slot;

        }
        public override void Activate() { }
        public override void Deactivate() { }
    }
}
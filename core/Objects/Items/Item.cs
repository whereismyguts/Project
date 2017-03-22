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
        public virtual int Volume { get { return 10; } }

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
                return new SpriteInfo("spaceship.png", 4, 2);
            }
        }
        public override float Rotation { get { return Owner.Rotation; } }

        public List<Slot> Slots { get { return slots; } }

        public int Health { get; internal set; } = 10;

        public ShipHull(int diameter) : base(new CoordPoint(diameter, diameter), new CoordPoint(diameter / 2, diameter / 2)) {
            slots.Add(new Slot(new CoordPoint(-150, 150), this, SlotType.EngineSlot));
            slots.Add(new Slot(new CoordPoint(150, 150), this, SlotType.EngineSlot));
            slots.Add(new Slot(new CoordPoint(1000, 200), this, SlotType.WeaponSlot));
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
            for(int i = 0; i < slots.Count; i++)
                if(slots[i].AttachedItem is DefaultEngine)
                    yield return slots[i].AttachedItem as DefaultEngine;
        }

        internal IEnumerable<DefaultWeapon> GetWeapons() {
            for(int i = 0; i < slots.Count; i++)
                if(slots[i].AttachedItem is DefaultWeapon)
                    yield return slots[i].AttachedItem as DefaultWeapon;
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
        public int ZIndex = 0;
        public SpriteInfo(string content, int framesX = 1, int framesY = 1, int zIndex = 0) {
            Content = content;
            FramesX = framesX;
            FramesY = framesY;
            ZIndex = zIndex;
        }
        public SpriteInfo() { }
        public string Content { get; } = string.Empty;
        public int FramesX { get; } = 1;
        public int FramesY { get; } = 1;
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
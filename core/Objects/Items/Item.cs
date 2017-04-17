using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GameCore {
    public abstract class Item: Renderable {

        public virtual void Step() {

        }
        public virtual string Name { get { return "no-name item"; } }

        public abstract SpriteInfo SpriteInfo { get; }
        // public CoordPoint ScreenPosition { get { return Viewport.World2ScreenPoint(Location); } }

        public abstract float Rotation { get; }
        public string Content { get { return SpriteInfo.Content; } }

        public Item(Vector2 size, Vector2 origin) { //TODO implement in children
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


        public override Vector2 Location { get { return Owner.Location; } set { } }


        public override SpriteInfo SpriteInfo {
            get {
                return new SpriteInfo("hull.png", 1, 1);
            }
        }
        public override float Rotation { get { return Owner.Rotation; } }

        public List<Slot> Slots { get { return slots; } }

        public int Health { get; internal set; } = 10;

        public ShipHull(float diameter) : base(new Vector2(diameter, diameter), new Vector2(diameter / 2, diameter / 2)) {
            slots.Add(new Slot(new Vector2(-12, 7), this, SlotType.EngineSlot));
            slots.Add(new Slot(new Vector2(12, 7), this, SlotType.EngineSlot));
            slots.Add(new Slot(new Vector2(25, -20), this, SlotType.WeaponSlot));
            slots.Add(new Slot(new Vector2(-25, -20), this, SlotType.WeaponSlot));
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

        public override Vector2 Location {
            get {
                return Slot.Hull.Location + Slot.RelativePosition;
            }
        }



        //protected internal override CoordPoint Location {
        //    
        //}

        protected internal virtual Slot Slot { get; set; }

        public override float Rotation {
            get {
                return /*(float)(-Math.PI / 2f) + */Slot.Hull.Rotation; // TODO set rotation external
            }
        }

        public AttachedItem(Vector2 size, Vector2 origin) : base(size, origin) {

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
                return new SpriteInfo("256tile.png", 1, 1, 1);
            }
        }
        public EmptySlotItem(Slot slot) : base(new Vector2(200, 200), new Vector2(100, 100)) {
            Slot = slot;
        }
        public override void Activate() { }
        public override void Deactivate() { }
    }
}
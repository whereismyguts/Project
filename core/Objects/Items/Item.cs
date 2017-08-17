using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCore {
    public abstract class Item: Renderable {
        public virtual void Step() {

        }
        public abstract string Name { get; }

        public abstract SpriteInfo SpriteInfo { get; }
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

    public class ShipHull: Item {
        List<Slot> slots = new List<Slot>();

        protected internal Ship Owner { get; set; }

        public override Vector2 Location { get { return Owner.Location; } set { } }
        public override SpriteInfo SpriteInfo {
            get {
                return new SpriteInfo("hull.png", 1, 1);
            }
        }
        public override float Rotation { get { return Owner.Rotation; } }

        public List<Slot> Slots { get { return slots; } }
        public int Health { get; internal set; } = 100;

        public override string Name {
            get {
                return "hull";
            }
        }
        public ShipHull(float diameter) : base(new Vector2(diameter, diameter), new Vector2(diameter / 2, diameter / 2)) {
            slots.Add(new Slot(new Vector2(-2f, 1.5f), this, SlotType.EngineSlot));
            slots.Add(new Slot(new Vector2(2f, 1.5f), this, SlotType.EngineSlot));
            slots.Add(new Slot(new Vector2(-3f, -3f), this, SlotType.WeaponSlot));
            slots.Add(new Slot(new Vector2(3f, -3f), this, SlotType.WeaponSlot));
        }

        public override void Activate() { }
        public override void Deactivate() { }
        public void Attach(AttachedItem item, Slot slot) {
            if(item.Slot != null) {
                item.Detach();
                return;
            }
            var newSlot = slots.FirstOrDefault(s => s.IsEmpty && s.Type == item.Type);
            if(newSlot != null)
                newSlot.Attach(item);
        }

        internal IEnumerable<DefaultEngine> GetEngines() {
            for(int i = 0; i < slots.Count; i++)
                if(slots[i].AttachedItem is DefaultEngine)
                    yield return slots[i].AttachedItem as DefaultEngine;
        }
        internal IEnumerable<WeaponBase> GetWeapons() {
            for(int i = 0; i < slots.Count; i++)
                if(slots[i].AttachedItem is WeaponBase)
                    yield return slots[i].AttachedItem as WeaponBase;
        }
    }

    public abstract class AttachedItem: Item {
        public override Vector2 Location {
            get {
                return Slot.Hull.Location + Slot.RelativePosition;
            }
        }
        public override float Rotation {
            get {
                return /*(float)(-Math.PI / 2f) + */Slot.Hull.Rotation; // TODO set rotation external
            }
        }

        protected internal virtual Slot Slot { get; set; }
        public abstract  SlotType  Type { get; }

        public AttachedItem(Vector2 size, Vector2 origin) : base(size, origin) {

        }
        internal void Detach() {
            Slot.Detach();
            Slot = null;
        }
    }

    public class SpriteInfo {
        public int ZIndex = 0;
        public string Content { get; } = string.Empty;
        public int FramesX { get; } = 1;
        public int FramesY { get; } = 1;

        public SpriteInfo(string content, int framesX = 1, int framesY = 1, int zIndex = 0) {
            Content = content;
            FramesX = framesX;
            FramesY = framesY;
            ZIndex = zIndex;
        }
        public SpriteInfo() { }
    }

    public class EmptySlotItem: AttachedItem {
        public override SpriteInfo SpriteInfo {
            get {
                return new SpriteInfo("256tile.png", 1, 1, 1); //TODO return nothing
            }
        }
        public EmptySlotItem(Slot slot) : base(new Vector2(2, 2), new Vector2(1, 1)) {
            Slot = slot;
        }
        public override SlotType Type {
            get {
                throw new NotImplementedException();
            }
        }
        public override string Name {
            get {
                return "empty slot";
            }
        }
        public override void Activate() { }
        public override void Deactivate() { }
    }
}
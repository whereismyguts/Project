using System;
using System.Collections.Generic;

namespace GameCore {
    public abstract class GameObject : IRenderableObject{
        protected string Image { get; set; }
        protected Viewport Viewport { get { return MainCore.Instance.Viewport; } }

        protected internal float Mass { get; set; }
        protected internal abstract float Rotation { get; }

        internal abstract bool IsMinimapVisible { get; }

        public StarSystem CurrentSystem { get; }
        public bool ToRemove { get; set; } = false;
        public virtual string Name { get { return string.Empty; } }

        public abstract Bounds ObjectBounds { get; }
        public virtual CoordPoint Location { get; set; }

        public GameObject(StarSystem system) {
            CurrentSystem = system;
        }
        protected internal virtual void Step() {
            Location += Velosity;

            if(noClipTimer < 10 )
                noClipTimer++;

        }
        /// <summary>foreach in all iternal items (weapons, effects, clouds, engines)</summary>
        public virtual IEnumerable<Item> GetItems() {
            return new Item[] { };
        }
        public abstract IEnumerable<Geometry> GetPrimitives();
        public CoordPoint Velosity { get; set; }

        int noClipTimer=10;

        public bool TemporaryNoclip { get {
                return noClipTimer < 10;
            } set {
                noClipTimer = value? 0 : 10;
            } }

        public Bounds GetScreenBounds() {
            return Viewport.World2ScreenBounds(ObjectBounds);
        }
    }
}

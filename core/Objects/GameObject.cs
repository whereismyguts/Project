using System;
using System.Collections.Generic;

namespace GameCore {
    public abstract class GameObject: IRenderableObject {
        protected string Image { get; set; }
        protected Viewport Viewport { get { return MainCore.Instance.Viewport; } }

        protected internal float Mass { get; set; } = 0;
        protected internal abstract float Rotation { get; }

        public static StarSystem CurrentSystem { get { return MainCore.Instance.System; } }
        public bool ToRemove { get; set; } = false;
        public virtual string Name { get { return string.Empty; } }

        public float Radius { get; protected set; }
        public override string ToString() {
            return GetName();
        }

        protected abstract string GetName();

        public abstract Bounds ObjectBounds { get; }
        public virtual CoordPoint Location { get; set; }

        public GameObject() {
            MainCore.Instance.System.Add(this);
        }
        protected internal virtual void Step() {
            Location += Velosity;

            if(noClipTimer < 10)
                noClipTimer++;

        }
        /// <summary>foreach in all iternal items (weapons, effects, clouds, engines)</summary>
        public virtual IEnumerable<Item> GetItems() {
            return new Item[] { };
        }
        public virtual IEnumerable<Geometry> GetPrimitives() {
            return new Geometry[] { };
        }
        public CoordPoint Velosity { get; set; }

        int noClipTimer = 10;

        public event RenderObjectChangedEventHandler Changed;

        public bool TemporaryNoclip {
            get {
                return noClipTimer < 10;
            }
            set {
                noClipTimer = value ? 0 : 10;
            }
        }

        public Bounds GetScreenBounds() {
            return Viewport.World2ScreenBounds(ObjectBounds);
        }
    }
}

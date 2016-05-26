using System;
using System.Collections.Generic;

namespace GameCore {
    public abstract class GameObject {
        protected string Image { get; set; }
        protected Viewport Viewport { get { return MainCore.Instance.Viewport; } }

        protected internal float Mass { get; set; }
        protected internal abstract float Rotation { get; }

        internal abstract bool IsMinimapVisible { get; }

        public StarSystem CurrentSystem { get; }

        public virtual string Name { get { return string.Empty; } }

        public abstract Bounds ObjectBounds { get; }
        public virtual CoordPoint Position { get; set; }

        public GameObject(StarSystem system) {
            CurrentSystem = system;
        }

        protected internal abstract void Step();

        public abstract IEnumerable<Item> GetItems();    //TODO foreach in all iternal items (weapons, effects, clouds, engines) 

        public Bounds GetScreenBounds() {
            return Viewport.World2ScreenBounds(ObjectBounds);
        }
    }
}

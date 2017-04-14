using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCore {
    public abstract class GameObject: IRenderableObject {
        static IEnumerable<Item> itemsEmpty = new Item[] { };
        static IEnumerable<Geometry> geometryEmpty = new Geometry[] { };
        int noClipTimer = 10;

        public event RenderObjectChangedEventHandler Changed;

        public CoordPoint Velosity { get; set; }

        protected string Image { get; set; }
        protected Viewport Viewport { get { return MainCore.Instance.Viewport; } }
        protected internal float Mass { get; set; } = 0;
        protected internal abstract float Rotation { get; }

        public static StarSystem CurrentSystem { get { return MainCore.Instance.System; } }

        public bool ToRemove { get; set; } = false;
        public bool TemporaryNoclip {
            get {
                return noClipTimer < 10;
            }
            set {
                noClipTimer = value ? 0 : 10;
            }
        }
        public virtual float Radius { get; protected set; }
        public virtual string Name { get { return string.Empty; } }
        public virtual bool IsDynamic { get; } = false;
        public virtual CoordPoint Location { get; set; }
        public abstract Bounds ObjectBounds { get; }

        public GameObject() {
            MainCore.Instance.System.Add(this);
        }

        protected internal virtual void Step() {
            if(IsDynamic)
                Velosity = Velosity * 0.9999f + PhysicsHelper.GetSummaryAttractingForce(CurrentSystem.Objects, this) * 5;


            if(noClipTimer < 10)
                noClipTimer++;

            //foreach(GameObject obj in MainCore.Instance.Objects)
            //    if(obj != this) {
            //        var nextPos = Location + Velosity;
            //        if(CoordPoint.Distance(obj.Location, nextPos) <= Radius + obj.Radius) {
            //            Location -= Velosity.UnaryVector;
            //            return;
            //        }
            //    }

            //if(MainCore.Instance.Objects.FirstOrDefault(ob => CoordPoint.Distance(ob.Location, Location) < Radius + ob.Radius && ob == this) != null)
            Location += Velosity;


        }
        protected abstract string GetName();

        public override string ToString() {
            return GetName();
        }
        /// <summary>foreach in all iternal items (weapons, effects, clouds, engines)</summary>
        public virtual IEnumerable<Item> GetItems() {
            return itemsEmpty;
        }
        public virtual IEnumerable<Geometry> GetPrimitives() {
            return geometryEmpty;
        }

        public Bounds GetScreenBounds() {
            return Viewport.World2ScreenBounds(ObjectBounds);
        }
    }
}

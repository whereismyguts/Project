using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    public abstract class Renderable {

        public CoordPoint Size { get; set; }
        public CoordPoint Origin { get; set; }

        public virtual CoordPoint ScreenOrigin { get { return Viewport.World2ScreenBounds(new Bounds(0, 0, Origin.X, Origin.Y)).Size; } }
        public virtual CoordPoint ScreenLocation { get { return Viewport.World2ScreenPoint(Location); } }
        public virtual CoordPoint ScreenSize {
            get {
                //return IsWorldSize ?
                return Viewport.World2ScreenBounds(new Bounds(Location - Size / 2f, Location + Size / 2f)).Size;
                //                    new Bounds(Location - Size / 2f, Location + Size / 2f).Size;
            }
        }

        public virtual CoordPoint Location { get; set; }

        //public bool IsWorldSize { get; set; } = true;
        protected Viewport Viewport { get { return MainCore.Instance.Viewport; } }
    }

    public enum Align { LeftBottom, RightBottom };

    public class ScreenSpriteItem: Item {
        SpriteInfo info;
        ScreenSpriteItem owner;
        Align align;

        bool HasOwner {
            get { return owner != null; }
        }
        public override string Name {
            get {
                return SpriteInfo.Content + " Screen Sprite Item";
            }
        }
        public override CoordPoint ScreenLocation {
            get {
                return Location;
            }
        }
        public override CoordPoint ScreenOrigin {
            get {
                return Origin;
            }
        }
        public override CoordPoint ScreenSize {
            get {
                return new Bounds(Location - Size / 2f, Location + Size / 2f).Size;
            }
        }
        public override float Rotation {
            get {
                return 0; // yet
            }
        }
        public override SpriteInfo SpriteInfo {
            get { return info; }
        }

        public ScreenSpriteItem(Align align, CoordPoint size, CoordPoint origin, SpriteInfo info) : base(size, origin) {
            this.info = info;
            this.align = align;
            Location = CalcArrangeLocation(align, new CoordPoint(), Viewport.World2ScreenBounds(Viewport.Bounds).Size, Size) + Origin;
        }

        public ScreenSpriteItem(ScreenSpriteItem owner, CoordPoint relativeLocation, CoordPoint size, CoordPoint origin, SpriteInfo info) : base(size, origin) {
            this.info = info;
            this.owner = owner;
            Location = owner.Location - relativeLocation;// CalcArrangeLocation(align, owner.Location, owner.Size, Size);
        }

        static CoordPoint CalcArrangeLocation(Align objAlign, CoordPoint ownerLocation, CoordPoint ownerSize, CoordPoint objSize) {
            switch(objAlign) {
                case Align.LeftBottom:
                    return ownerLocation + new CoordPoint(0, ownerSize.Y - objSize.Y);
                case Align.RightBottom:
                    return ownerLocation + ownerSize - objSize;
            }
            return null;
        }
        public override void Activate() {
        }
        public override void Deactivate() {
        }
    }

    public class WordSpriteItem: Item {
        SpriteInfo info;

        public override float Rotation {
            get { return Owner.Rotation; }
        }

        public override SpriteInfo SpriteInfo {
            get { return info; }
        }

        public override CoordPoint Location {
            get { return Owner.Location; }
            set { throw new Exception("it didt suppose to happen!"); }
        }

        public GameObject Owner { get; private set; }

        public WordSpriteItem(GameObject owner, CoordPoint size, CoordPoint origin, string content, int framesX = 1, int framesY = 1) : base(size, origin) {
            info = new SpriteInfo(content, framesX, framesY);
            Owner = owner;
        }

        public override void Activate() {
        }

        public override void Deactivate() {
        }
    }
}

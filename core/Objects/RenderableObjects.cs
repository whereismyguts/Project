using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    public abstract class Renderable {

        public Vector2 Size { get; set; }
        public Vector2 Origin { get; set; }

        public virtual Vector2 ScreenOrigin { get { return Viewport.World2ScreenBounds(new Bounds(0, 0, Origin.X, Origin.Y)).Size; } }
        public virtual Vector2 ScreenLocation { get { return Viewport.World2ScreenPoint(Location); } }
        public virtual Vector2 ScreenSize {
            get {
                //return IsWorldSize ?
                return Viewport.World2ScreenBounds(new Bounds(Location - Size / 2f, Location + Size / 2f)).Size;
                //                    new Bounds(Location - Size / 2f, Location + Size / 2f).Size;
            }
        }
        
        public virtual Vector2 Location { get; set; }

        //public bool IsWorldSize { get; set; } = true;
        protected Viewport Viewport { get { return MainCore.Instance.Viewport; } }
    }

    public enum Align { LeftBottom, RightBottom, FillBottom };
    /*
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
        public override Vector2 ScreenLocation {
            get {
                return Location;
            }
        }
        public override Vector2 ScreenOrigin {
            get {
                return Origin;
            }
        }
        public override Vector2 ScreenSize {
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

        public ScreenSpriteItem(Align align, Vector2 size, Vector2 origin, SpriteInfo info, int padding = 0) : base(size, origin) {
            this.info = info;
            this.align = align;
            var viewportSize = Viewport.World2ScreenBounds(Viewport.Bounds).Size;
            Location = CalcArrangeLocation(align, new Vector2(), viewportSize, Size) + Origin;
            Size = CalcArrangeSize(align, viewportSize, Size);
        }

        static Vector2 CalcArrangeSize(Align align, Vector2 ownerSize, Vector2 objSize) {
            if(align == Align.FillBottom)
                return new Vector2(ownerSize.X, objSize.Y);
            return objSize;
        }

        public ScreenSpriteItem(ScreenSpriteItem owner, Vector2 relativeLocation, Vector2 size, Vector2 origin, SpriteInfo info) : base(size, origin) {
            this.info = info;
            this.owner = owner;
            Location = owner.Location - relativeLocation;// CalcArrangeLocation(align, owner.Location, owner.Size, Size);
        }

        public ScreenSpriteItem(Vector2 location, Vector2 size, Vector2 origin, SpriteInfo info) : base(size, origin, 0) {
            Location = location;
            this.info = info;
        }

        static Vector2 CalcArrangeLocation(Align objAlign, Vector2 ownerLocation, Vector2 ownerSize, Vector2 objSize) {
            switch(objAlign) {
                case Align.LeftBottom:
                    return ownerLocation + new Vector2(0, ownerSize.Y - objSize.Y);
                case Align.RightBottom:
                    return ownerLocation + ownerSize - objSize;
                case Align.FillBottom:
                    return ownerLocation + new Vector2(0, ownerSize.Y - objSize.Y);
                    //                        new CoordPoint(ownerSize.X / 2 - objSize.X / 2, ownerSize.Y - objSize.Y);
            }
            return null;
        }
        public override void Activate() {
        }
        public override void Deactivate() {
        }
    }
    */
    public class WordSpriteItem: Item {
        SpriteInfo info;

        public override float Rotation {
            get { return Owner.Rotation; }
        }

        public override SpriteInfo SpriteInfo {
            get { return info; }
        }

        public override Vector2 Location {
            get { return Owner.Location; }
            set { throw new Exception("it didt suppose to happen!"); }
        }

        public GameObject Owner { get; private set; }

        public WordSpriteItem(GameObject owner, Vector2 size, Vector2 origin, string content, int framesX = 1, int framesY = 1) : base(size, origin) {
            info = new SpriteInfo(content, framesX, framesY);
            Owner = owner;
        }

        public override void Activate() {
        }

        public override void Deactivate() {
        }
    }
}

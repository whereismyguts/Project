
using System;

namespace GameCore {
    public abstract class WeaponBase: GameObject {
        protected virtual Size Size { get; set; }
        protected virtual CoordPoint Origin { get; set; }

        protected internal override Bounds Bounds {
            get {
                return new Bounds(Owner.Location - Origin, Owner.Location - Origin + new CoordPoint(Size.Width, Size.Height));
            }
        }
        float rotation = 0f;
        protected Ship Owner { get; set; }
        protected internal override float Rotation { get { return rotation + Owner.Rotation; } }
        protected internal override string ContentString { get { return "weapon"; } }

        public WeaponBase(Ship owner) : base(owner.CurrentSystem) {
            Owner = owner;
            Size = new Size(10, 20);
            Origin = new CoordPoint(0, 0);
        }

        protected internal override void Step() { }
    }
    public class DefaultCannon: WeaponBase {
        public DefaultCannon(Ship owner) : base(owner) {
        }
    }
}
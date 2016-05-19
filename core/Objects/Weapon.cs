
namespace GameCore {
    public abstract class WeaponBase : GameObject {
        protected virtual Size Size { get; set; }
        protected virtual CoordPoint Origin { get; set; }

        protected internal override Bounds Bounds {
            get {
                return new Bounds(Owner.Location - Origin, Owner.Location - Origin + new CoordPoint(Size.Width, Size.Height));
            }
        }
        float rotation = 0f;
        protected Ship Owner { get; set; }
        public WeaponBase(Ship owner) : base(owner.CurrentSystem) {
            Owner = owner;
            Size = new Size(10, 20);
            Origin = new CoordPoint(0, 0);
        }
        protected internal override string ContentString {
            get {
                return "weapon";
            }
        }
        
        protected internal override float GetRotation() {
            return rotation + Owner.GetRotation();
        }

        protected internal override void Step() {
            
        }
    }
    public class DefaultCannon: WeaponBase {
        public DefaultCannon(Ship owner) : base(owner) {
        }
    }
}
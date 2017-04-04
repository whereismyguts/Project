using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    class Bullet: GameObject {
        public Bullet(CoordPoint position, CoordPoint direction, Ship owner) : base(owner.CurrentSystem) {

            Position = position;
           
            this.owner = owner;
            Velosity = direction * 420;
        }

        Ship owner;

        public Ship Owner {
            get { return owner; }
        }

        public override Bounds ObjectBounds {
            get {
                return new Bounds(Position.X - 800, Position.Y - 800, 1600, 1600);
            }
        }

        protected internal override float Rotation {
            get {
                return Velosity.Angle;
            }
        }

        internal override bool IsMinimapVisible {
            get {
                return false;
            }
        }

        public override IEnumerable<Item> GetItems() {
            return new Item[] { new JustSpriteItem(this, ObjectBounds.Size, ObjectBounds.Size / 2, "slime.png", 4, 2) };
        }

        public override IEnumerable<Geometry> GetPrimitives() {
            return new Geometry[] { new InternalCircle(Position, 20) };
        }

        internal void Impact() {
            CurrentSystem.Add(new Explosion(CurrentSystem, Position, 1200));
        }

        int liveTime = 0;
        protected internal override void Step() {
            if(liveTime > 100)
                ToRemove = true;
            

            liveTime++;

            base.Step();
        }
    }
}

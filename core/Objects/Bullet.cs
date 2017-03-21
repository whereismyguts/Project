using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    class Bullet: GameObject {
        public Bullet(Ship owner) : base(owner.CurrentSystem) {

            Position = owner.Position;
            direction = owner.Direction;
            this.owner = owner;
            Velosity = direction * 420;
        }

        Ship owner;
        CoordPoint direction;

        public override Bounds ObjectBounds {
            get {
                return new Bounds(Position.X - 20, Position.Y - 20, 40, 40);
            }
        }

        protected internal override float Rotation {
            get {
                return direction.Angle;
            }
        }

        internal override bool IsMinimapVisible {
            get {
                return false;
            }
        }

        public override IEnumerable<Item> GetItems() {
            return new Item[] { };
        }

        public override IEnumerable<Geometry> GetPrimitives() {
            return new Geometry[] { new Circle(Position, 20) };
        }
        int liveTime = 0;
        protected internal override void Step() {
            if(liveTime > 100)
                ToRemove = true;
            foreach(Ship ship in MainCore.Instance.Ships.Where(s => s != owner)) {
                if(CoordPoint.Distance(ship.Position, Position) <= ship.ObjectBounds.Width / 2) {
                    CurrentSystem.Add(new Explosion(CurrentSystem, Position));
                    ship.GetDamage(1, owner);
                    ToRemove = true;
                    return;
                }
            }

            liveTime++;

            base.Step();
        }
    }
}

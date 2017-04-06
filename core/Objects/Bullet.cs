using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    public class ProjectileBase: GameObject {
        public ProjectileBase(CoordPoint position, CoordPoint direction, Ship owner) : base(owner.CurrentSystem) {

            Location = position;

            this.owner = owner;
            Velosity = direction * 420;
        }

        Ship owner;

        protected virtual int Speed { get { return 420; } }

        public Ship Owner {
            get { return owner; }
        }

        public override Bounds ObjectBounds {
            get {
                return new Bounds(Location.X - 800, Location.Y - 800, 1600, 1600);
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

        public virtual int Damage {
            get { return 1; }
        }

        protected virtual int MaxLiveTime {
            get { return 100; }
        }

        public override IEnumerable<Item> GetItems() {
            return new Item[] { new WordSpriteItem(this, ObjectBounds.Size, ObjectBounds.Size / 2, "slime.png", 4, 2) };
        }

        public override IEnumerable<Geometry> GetPrimitives() {
            return new Geometry[] { new InternalCircle(Location, 200) };
        }

        internal bool Impact() {
            if(!ToRemove) {
                CurrentSystem.Add(new Explosion(CurrentSystem, Location, 1200));
                ToRemove = true;
                return true;
            }

            return false;
        }

        int liveTime = 0;
        protected internal override void Step() {
            if(liveTime > MaxLiveTime)
                ToRemove = true;


            liveTime++;

            base.Step();
        }
    }

    public class Rocket: ProjectileBase {
        public Rocket(CoordPoint position, CoordPoint direction, Ship owner) : base(position, direction, owner) {
        }

        protected override int Speed { get { return 200; } }
        protected override int MaxLiveTime { get { return 300; } }
        public override int Damage { get { return 3; } }

        public override IEnumerable<Item> GetItems() {
            return new Item[] { new WordSpriteItem(this, ObjectBounds.Size, ObjectBounds.Size / 2, "flame_sprite.png", 6, 1) };
        }
    }
}

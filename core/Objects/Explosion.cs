using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    class Explosion: GameObject {
        public override Bounds ObjectBounds {
            get {
                return new Bounds(Position + new CoordPoint(-100, -100), Position + new CoordPoint(100, 100));
            }
        }

        protected internal override float Rotation {
            get {
                return 0;
            }
        }

        int lifeTime = 0;

        internal override bool IsMinimapVisible {
            get {
                return false;
            }
        }

        protected internal override void Step() {
            lifeTime++;

            if(lifeTime>10)
                ToRemove = true;
            base.Step();
        }

        public Explosion(StarSystem system, CoordPoint position):base(system) {
            this.Position = position;
        }

        public override IEnumerable<Item> GetItems() {
            return new Item[] {  };
        }

        public override IEnumerable<Geometry> GetPrimitives() {
            Circle c = new Circle(Position, lifeTime * lifeTime*lifeTime);
            return new Geometry[] {  c};
        }
    }
}

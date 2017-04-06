using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    class Explosion: GameObject {
        float rotation;
        int radius = 3000;
        public override Bounds ObjectBounds {
            get {
                return new Bounds(Location + new CoordPoint(-radius, -radius), Location + new CoordPoint(radius, radius));
            }
        }

        protected internal override float Rotation {
            get {
                return rotation;
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

            if(lifeTime > 45)
                ToRemove = true;
            base.Step();
        }

        public Explosion(StarSystem system, CoordPoint position, int radius = 3000) : base(system) {
            this.Location = position;
            this.radius = radius;
            rotation = (float)Rnd.Get(-Math.PI, Math.PI);
        }

        public override IEnumerable<Item> GetItems() {
            //return new Item[] { };
            return new Item[] { new WordSpriteItem(this, ObjectBounds.Size, ObjectBounds.Size / 2, "exp2.png", 4, 4) };

            //return new Item[] { new JustSpriteItem(this, new CoordPoint(200,200), new CoordPoint(100,100), "exp.png", 10) };
        }

        public override IEnumerable<Geometry> GetPrimitives() {
            //Circle c = new Circle(Position, lifeTime * lifeTime*lifeTime, ColorCore.Red);
            return new Geometry[] { };
        }
    }
}

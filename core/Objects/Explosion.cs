using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    class Explosion: GameObject {
        float rotation;
        public override Bounds ObjectBounds {
            get {
                return new Bounds(Position + new CoordPoint(-3000, -3000), Position + new CoordPoint(3000, 3000));
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

            if(lifeTime>45)
                ToRemove = true;
            base.Step();
        }

        public Explosion(StarSystem system, CoordPoint position):base(system) {
            this.Position = position;
            rotation = (float)Rnd.Get(-Math.PI, Math.PI);
        }

        public override IEnumerable<Item> GetItems() {
            //return new Item[] { };
            return new Item[] { new JustSpriteItem(this, ObjectBounds.Size, ObjectBounds.Size / 2, "exp2.png", 4, 4) };
            
            //return new Item[] { new JustSpriteItem(this, new CoordPoint(200,200), new CoordPoint(100,100), "exp.png", 10) };
        }

        public override IEnumerable<Geometry> GetPrimitives() {
            //Circle c = new Circle(Position, lifeTime * lifeTime*lifeTime, ColorCore.Red);
            return new Geometry[] {  };
        }
    }

    public class JustSpriteItem : Item {
        public override float Rotation {
            get {
                return Owner.Rotation;
            }
        }
        SpriteInfo info;
        public override SpriteInfo SpriteInfo {
            get {
                return info;
            }
        }

        public override CoordPoint Location {
            get {
                return Owner.Position;
            }

            set {
                throw new Exception("it didt suppose to happen!");
            }
        }

        

        public GameObject Owner { get; private set; }

        public JustSpriteItem(GameObject owner, CoordPoint size, CoordPoint origin, string content, int framesX=1, int framesY=1) : base(size, origin) {
            info = new SpriteInfo(content, framesX, framesY);
            Owner = owner;
        }

        public override void Activate() {
        }

        public override void Deactivate() {
        }
    }
}

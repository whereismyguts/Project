using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {/*
    class Asteroid: ProjectileBase {
        public override Bounds ObjectBounds {
            get {
                return new Bounds(Location - size / 2, Location + size / 2);
            }
        }

        float rotation = 0;
        Vector2 size;
        Item[] items;

        protected internal override float Rotation {
            get {
                return rotation += 0.5f;
            }
        }

        public override IEnumerable<Item> GetItems() {
            return items;
        }

        public Asteroid():base(new Vector2(0,30000).GetRotated(Rnd.GetPeriod()), new Vector2(0, 500).GetRotated(Rnd.GetPeriod()), null) {
            float side = Rnd.Get(1500, 2500);
            size = new Vector2(side, side);
            Mass = side;
            items = new Item[] { new WordSpriteItem(this, size, size / 2, "emptyslot.png") };
        }
    }*/
}

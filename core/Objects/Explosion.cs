using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    class Explosion: GameObject {
        float rotation;
        int radius = 3000;
        int lifeTime = 0;

        protected internal override void Step() {
            lifeTime++;

            if(lifeTime > 45)
                ToRemove = true;
            base.Step();
        }

        public Explosion(World world, Vector2 position, int radius = 3000) : base(world, position, radius) {

            //rotation = (float)Rnd.Get(-Math.PI, Math.PI);
        }

        public override IEnumerable<Item> GetItems() {
            //return new Item[] { };
            return new Item[] { new WordSpriteItem(this, ObjectBounds.Size, ObjectBounds.Size / 2, "exp2.png", 4, 4) };

            //return new Item[] { new JustSpriteItem(this, new CoordPoint(200,200), new CoordPoint(100,100), "exp.png", 10) };
        }

        protected override string GetName() {
            return "EXPLOSION #" + rotation;
        }
    }
}

using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    class Explosion: GameObject {
        int lifeTime = 0;
        Vector2 location;
        float rotation = Rnd.GetPeriod();

        protected internal override float Rotation {
            get {
                return rotation;
            }
        }
        public override Vector2 Location {
            get {
                return location;
            }
        }

        public Explosion(World world, Vector2 position, int radius = 30) : base(world, position, radius) {
            location = position;
            //rotation = (float)Rnd.Get(-Math.PI, Math.PI);
        }

        protected internal override void Step() {
            lifeTime++;
            if(lifeTime > 45)
                ToRemove = true;
            if(lifeTime < 15)
                foreach(var obj in MainCore.Instance.Objects)
                    if((obj is Ship || obj is ProjectileBase) && Vector2.Distance(obj.Location, Location) <= Radius) {
                        obj.ApplyLinearImpulse((obj.Location - Location).UnaryVector() * Radius * 10000000);
                        obj.GetDamage(1);
                    }
        }
        public override IEnumerable<Geometry> GetPrimitives() {
            yield return new WorldGeometry(Location, ObjectBounds.Size, true);
        }
        public override IEnumerable<Item> GetItems() {
            //return new Item[] { };
            return new Item[] { new WordSpriteItem(this, ObjectBounds.Size, ObjectBounds.Size / 2, "exp2.png", 4, 4) };
            //return new Item[] { new JustSpriteItem(this, new CoordPoint(200,200), new CoordPoint(100,100), "exp.png", 10) };
        }
        protected override void CreateBody(float radius, Vector2 location) {
            // do nothing, because its not a physic body; perhaps later
        }
        protected override string GetName() {
            return "EXPLOSION";
        }
    }
}

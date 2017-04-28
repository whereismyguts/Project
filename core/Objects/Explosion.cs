using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {

    class SlimeExplosion: Explosion {
        public SlimeExplosion(World world, Vector2 position, float rotation, int radius) : base(world, position, rotation, radius) {

        }

        public override IEnumerable<Item> GetItems() {
            return new Item[] { new WordSpriteItem(this,Vector2.Zero, (float)(-Math.PI/2),
                 ObjectBounds.Size, new Vector2(ObjectBounds.Size.X/1.3f, ObjectBounds.Size.Y / 2) , "blob.png", 10, 1) };
        }
    }

    class FireExplosion: Explosion {
        public FireExplosion(World world, Vector2 position, int radius) : base(world, position, Rnd.GetPeriod(), radius) {

        }

        public override IEnumerable<Item> GetItems() {
            //return new Item[] { new WordSpriteItem(this, ObjectBounds.Size, ObjectBounds.Size / 2, "explosion-sprite.png", 5, 3) };
            return new Item[] { new WordSpriteItem(this, ObjectBounds.Size, ObjectBounds.Size / 2, "explosion_generated.png", 15, 1) };
        }
    }


    class Explosion: GameObject {
        int lifeTime = 0;
        Vector2 location;
        float rotation;

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

        public Explosion(World world, Vector2 position, float rotation, int radius) : base(world, position, radius) {
            this.rotation = rotation;
            location = position;
            //rotation = (float)Rnd.Get(-Math.PI, Math.PI);
        }

        protected internal override void Step() {
            lifeTime++;
            if(lifeTime >= 13)
                ToRemove = true;
            if(lifeTime < 15)
                foreach(var obj in MainCore.Instance.Objects)
                    if((obj is Ship || obj is ProjectileBase) && Vector2.Distance(obj.Location, Location) <= Radius) {
                        obj.ApplyLinearImpulse((obj.Location - Location).UnaryVector() * Radius * Radius);
                        obj.GetDamage(1);
                    }
        }
        public override IEnumerable<Geometry> GetPrimitives() {
            yield return new WorldGeometry(Location, ObjectBounds.Size, true);
        }

        protected override void CreateBody(float radius, Vector2 location) {
            // do nothing, because its not a physic body; perhaps later
        }
        protected override string GetName() {
            return "EXPLOSION";
        }
    }
}

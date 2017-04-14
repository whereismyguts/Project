using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    public class ProjectileBase: GameObject {
        public ProjectileBase(Vector2 position, Vector2 direction, Ship owner) : base(owner.World, position, 2000) {
            this.owner = owner;
            ApplyForce(direction * 400);
        }



        Ship owner;

        protected virtual int Speed { get { return 420; } }

        public Ship Owner {
            get { return owner; }
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
            return new Geometry[] { new WorldGeometry(Location, new Vector2(400, 400)) };
        }

        internal bool Impact() {
            if(!ToRemove) {
                new Explosion(World, Location, 1200);
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

        protected override string GetName() {
            return "PROJECTILE: " + Owner.Name;
        }
    }

    public class Rocket: ProjectileBase {
        public Rocket(Vector2 position, Vector2 direction, Ship owner) : base(position, direction, owner) {

        }

        protected override int Speed { get { return 200; } }
        protected override int MaxLiveTime { get { return 300; } }
        public override int Damage { get { return 3; } }

        public override IEnumerable<Item> GetItems() {
            return new Item[] { new WordSpriteItem(this, ObjectBounds.Size, ObjectBounds.Size / 2, "flame_sprite.png", 6, 1) };
        }

        protected override string GetName() {
            return "ROCKET: " + Owner.Name;
        }
    }
}

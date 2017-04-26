using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    public class ProjectileBase: GameObject {
        Ship owner;
        int liveTime = 0;
        public ProjectileBase(Vector2 position, Vector2 direction, Ship owner) : base(owner.World, position, 3) {
            this.owner = owner;
            Body.Restitution = 1f;
            Body.OnCollision += CollideProcessing.OnCollision;
            Body.Rotation = direction.Angle();
        }
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

        internal bool Impact() {
            if(!ToRemove) {
                new Explosion(World, Location, 30);
                ToRemove = true;
                return true;
            }
            return false;
        }
        protected internal override void GetDamage(int d) {
            if(!ToRemove) {
                new Explosion(World, Location, 30);
                ToRemove = true;
            }
        }
        protected internal override void Step() {
            if(liveTime > MaxLiveTime && !ToRemove) {
                new Explosion(World, Location, Damage*10);
                ToRemove = true;
            }
            liveTime++;
            base.Step();
        }
        protected override string GetName() {
            return "PROJECTILE: " + Owner.Name;
        }
    }

    public class Rocket: ProjectileBase {
        public Rocket(Vector2 position, Vector2 direction, Ship owner) : base(position, direction, owner) {
            Body.Mass *= 2;
            Body.Restitution = 0.3f;
        }
        protected override void CreateBody(float radius, Vector2 location) {
            Body = BodyFactory.CreateEllipse(World, radius * 0.5f, radius * 1f, 32, 1.2f, location);// CreateRectangle(World, radius , radius * 2, 1.6f, location, this);
            Body.BodyType = BodyType.Dynamic;
            //base.CreateBody(radius, location);
        }
        protected override int Speed { get { return 200; } }
        protected override int MaxLiveTime { get { return 1000; } }
        public override int Damage { get { return 3; } }

        public override IEnumerable<Item> GetItems() {
            return new Item[] {
                new WordSpriteItem(this,  new Vector2(0, Radius*1.1f).GetRotated(Rotation),- (float)Math.PI/2, new Vector2(Radius*2,Radius*2), new Vector2(Radius, Radius), "flame_sprite.png", 6,1 ),
                 new WordSpriteItem(this, ObjectBounds.Size, ObjectBounds.Size / 2, "rocket.png")
            };
        }

        public override IEnumerable<Geometry> GetPrimitives() {
            var list = base.GetPrimitives().ToList();
            //     list.Add(new Line(Location + new Vector2(0, -100).GetRotated(Rotation), Location + new Vector2(0, Radius*1.1f).GetRotated(Rotation)){ Color =  InternalColor.Green});
            return list;
        }
        protected override string GetName() {
            return "ROCKET: " + Owner.Name;
        }

        protected internal override void Step() {
            Body.ApplyForce(new Vector2(0, -100000).GetRotated(Rotation), Location + new Vector2(0, Radius * 1.1f).GetRotated(Rotation /*+ Rnd.Get(-1.1f, 1.1f)*/));
            base.Step();
        }
    }
}

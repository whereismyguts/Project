using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    public abstract class ProjectileBase: GameObject {
        Ship owner;
        int liveTime = 0;
        public ProjectileBase(Vector2 position, Vector2 direction, Ship owner) : base(owner.World, position, 3) {
            this.owner = owner;
            Body.Restitution = 1f;
            Body.OnCollision += CollideProcessing.OnCollision;
            Body.Rotation = direction.Angle();
        }
        

        public Ship Owner {
            get { return owner; }
        }

        public virtual int Resistance { get { return 0; } } // 0 minds explodes everytime touching someting;

        public virtual int Damage {
            get { return 1; }
        }

        protected virtual int MaxLiveTime {
            get { return 100; }
        }
        protected internal override void GetDamage(int d) {
            if(!ToRemove) {
                new FireExplosion(World, Location,  30);
                ToRemove = true;
            }
        }
        protected internal override void Step() {
            if(liveTime > MaxLiveTime && !ToRemove) {
                CreateExplosion(Direction.Angle(), Damage * 10);
                ToRemove = true;
            }
            liveTime++;
            base.Step();
        }

        internal abstract void CreateExplosion(float rotation, int radius);
    }

    public class Slime: ProjectileBase {
     //   Vector2 direction = Vector2.Zero;
     //   Vector2 location;
        Vector2 startPos;


        //public override Vector2 Location {
        //    get {
        //        return location;
        //    }
        //}

        //protected internal override float Rotation {
        //    get {
        //        return direction.Angle();
        //    }
        //}



        public Slime(Vector2 position, Vector2 direction, Ship owner) : base(position, direction, owner) {

       //     Body = null;
      //      location = position;
        //    this.direction = direction;
            startPos = position;

            ApplyLinearImpulse(direction * 10000);
        }

        public override IEnumerable<Geometry> GetPrimitives() {
            var list = base.GetPrimitives().ToList();

        //    list.Add(new Line(startPos, Location) { Color = Color.Blue });
            return list;
        }
        protected override string GetName() {
            return "SLIME: " + Owner.Name;
        }
        public override IEnumerable<Item> GetItems() {
            return new Item[] { new WordSpriteItem(this, Vector2.Zero, -(float)(Math.PI/2), ObjectBounds.Size*2, ObjectBounds.Size , "slime.png", 10, 1) };
        }
        protected internal override void Step() {

            Body.Position += Direction*5;
//            location += direction*30;
            base.Step();
            //Body.LinearVelocity = direction * 9999999999;
        }

        internal override void CreateExplosion(float rotation, int raduis) {
            new SlimeExplosion(World, Location, rotation, raduis);
        }
    }

    public class Rocket: ProjectileBase {
        float force;

        public Rocket(Vector2 position, Vector2 direction, Ship owner) : base(position, direction, owner) {
            Body.Mass *= 2;
            Body.Restitution = 0.3f;
            force = Rnd.Get(5000, 10000);
        }
        
        public override int Resistance {
            get {
                return 80;
            }
        }
        protected override void CreateBody(float radius, Vector2 location) {
            Body = BodyFactory.CreateEllipse(World, radius * 0.5f, radius * 1f, 32, 1.2f, location);// CreateRectangle(World, radius , radius * 2, 1.6f, location, this);
            Body.BodyType = BodyType.Dynamic;
            //base.CreateBody(radius, location);
        }
        
        protected override int MaxLiveTime { get { return 1000; } }
        public override int Damage { get { return 3; } }

        public override IEnumerable<Item> GetItems() {
            return new Item[] {
                new WordSpriteItem(this,  new Vector2(0, Radius*1.1f).GetRotated(Rotation),- (float)Math.PI/2, new Vector2(Radius*2,Radius*2), new Vector2(Radius, Radius), "flame_sprite.png", 6,1 ),
                 new WordSpriteItem(this, ObjectBounds.Size, ObjectBounds.Size / 2, "rocket.png")
            };
        }

        internal override void CreateExplosion(float rotation, int radius) {
            new FireExplosion(World, Location, radius);
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
            Body.ApplyForce(new Vector2(0, -force).GetRotated(Rotation), Location + new Vector2(0, Radius * 1.1f).GetRotated(Rotation /*+ Rnd.Get(-1.1f, 1.1f)*/));
            base.Step();
        }
    }
}

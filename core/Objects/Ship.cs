using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCore {
    public class Ship: GameObject {
        public event EventHandler OnDead;
        string name;
        int fraction;
        float angleSpeed = 0;

        public int Fraction {
            get { return fraction; }
            set {
                fraction = value;
                name = NameGenerator.Generate(fraction);
            }
        }
        public int Health { get { return Hull.Health; } }
        public Color Color { get; }
        #region inventory
        public Inventory Inventory { get; set; }
        protected internal ShipHull Hull { get; set; }
        #endregion

        public override string Name {
            get {
                return name + ", Fraction" + Fraction;
            }
        }
        public override Bounds ObjectBounds {
            get {
                return new Bounds(Location - Hull.Size / 2f, Location + Hull.Size / 2f); //TODO use origin, because ship posiotion may be no in center of the ship
            }
        }

        public Ship(World world, Vector2 location, int fraction = 1) : base(world, location, 5) {
            Fraction = fraction;
            Hull = new ShipHull(10) { Owner = this };
            Inventory = new Inventory(Hull);
            var e1 = new DefaultEngine();
            var e2 = new DefaultEngine();

            for(int i = 0; i < 5; i++) {
                var w1 = Rnd.Bool() ? (WeaponBase)new SlimeGun() : new RocketLauncher();
                Inventory.Add(w1);
            }

            Inventory.Add(e1);
            Inventory.Add(e2);
            //Hull.Attach(new AttachedItem(new CoordPoint(20, 20), new CoordPoint(10,10)), Hull.Slots[0]);
            //Hull.Attach(new AttachedItem(new CoordPoint(20, 20), new CoordPoint(10, 10)), Hull.Slots[1]);

            Inventory.Attach(Hull.Slots[0], e1);
            Inventory.Attach(Hull.Slots[1], e2);

            Inventory.Attach(Hull.Slots[2], Inventory.Container[0] as AttachedItem);
            Inventory.Attach(Hull.Slots[3], Inventory.Container[1] as AttachedItem);

            //Inventory.Attach(Hull.Slots[2], w1);

            Color = Rnd.GetColor();

            Body.OnCollision += CollideProcessing.OnCollision;
        }




        internal void Accselerate() {
            foreach(Slot slot in Hull.Slots)
                if(slot.Type == SlotType.EngineSlot && !slot.IsEmpty)
                    slot.AttachedItem.Activate();
        }
        internal void Dead() {
            if(OnDead != null)
                OnDead(this, EventArgs.Empty);
            //       new Explosion(World, Location);
            ToRemove = true;
        }
        internal void Fire() {
            IEnumerable<WeaponBase> weapons = Hull.GetWeapons();
            foreach(WeaponBase W in weapons)
                W.Fire();
        }
        public void RotateL() {
            angleSpeed -= .01f;
        }
        public void RotateR() {
            angleSpeed += .01f;
        }

        protected override string GetName() {
            return "SHIP " + Name;
        }
        protected internal override void GetDamage(int d) {
            Hull.Health -= d;
            if(Hull.Health <= 0)
                Dead();
        }
        protected override void CreateBody(float radius, Vector2 location) {
            Body = BodyFactory.CreateRectangle(World, radius * 2, radius * 2, 0.1f, location);
            Body.BodyType = BodyType.Dynamic;
        }
        protected internal override void Step() {
            foreach(Item item in Inventory.Container)
                item.Step();
            //var acc = GetAcceleration() * 0.5f;
            //Circle.ApplyLinearImpulse(acc);
            if(Math.Abs( angleSpeed ) > 0.001) {
                Body.AngularVelocity = 0;
            }
            Body.Rotation += angleSpeed;
            Body.Position += Body.LinearVelocity/100f;
            angleSpeed *= .9f;
            base.Step();
        }
        public override IEnumerable<Item> GetItems() {
            for(int i = 0; i < Hull.Slots.Count; i++)
                yield return Hull.Slots[i].AttachedItem;
            yield return Hull;
        }
        public override IEnumerable<Geometry> GetPrimitives() {
            float colorFactor = Hull.Health / 100f;
            Color color = new Color(1 - colorFactor, colorFactor, 0);
            float height = Math.Max(.5f, Math.Min(5, ScreenBounds.Height));
            float width = ScreenBounds.RightBottom.X - ScreenBounds.LeftTop.X;

            if(width < 2 || height < 1 || Math.Abs(height-width) < 1)
                return new Geometry[] { };
            return new Geometry[]{
                new ScreenGeometry(
                    new Vector2(ScreenBounds.LeftTop.X, ScreenBounds.LeftTop.Y-height*2.3f),
                    new Vector2(width, height)) { Color  = Fraction == 1 ? Color.Red : Color.Blue},

                new ScreenGeometry(
                    new Vector2(ScreenBounds.LeftTop.X, ScreenBounds.LeftTop.Y-height),
                    new Vector2(width*colorFactor, height)) { Color  = color}
            //,            new ScreenGeometry(ScreenBounds.LeftTop,Vector2.Zero) { Text = Hull.Health.ToString() }
            };
        }
        //Vector2 GetAcceleration() {
        //    IEnumerable<DefaultEngine> engines = Hull.GetEngines();
        //    Vector2 sum = new Vector2();
        //    foreach(DefaultEngine engine in engines)
        //        sum += new Vector2(0, -1).GetRotated(engine.Rotation) * engine.GetAcceleration();
        //    return sum;
        //}
    }
}

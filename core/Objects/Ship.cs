using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCore {
    public class Ship: GameObject {

        //  AIController controller;
        public event EventHandler OnDead;

        int fraction;
        public int Fraction {
            get { return fraction; }
            set {
                fraction = value;
                name = NameGenerator.Generate(fraction);
            }
        }
        public int Health { get { return Hull.Health; } }
        public InternalColor Color { get; } // TODO remove
        string name;
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
        //public CoordPoint Reactive { get { return -(direction * acceleration) * 50; } }

        //public CoordPoint Velosity { get { return velosity; } }

        public Ship(World world, Vector2 location, int fraction = 1) : base(world, location, 2) {
            Fraction = fraction;
            Hull = new ShipHull(2) { Owner = this };
            Inventory = new Inventory(Hull);
            var e1 = new DefaultEngine();
            var e2 = new DefaultEngine();
            //var w1 = Rnd.Bool() ? new DefaultWeapon() : new RocketLauncher();
            var w2 = new RocketLauncher();
            Inventory.Add(e1);
            Inventory.Add(e2);
            //Inventory.Add(w1);
            Inventory.Add(w2);
            //Hull.Attach(new AttachedItem(new CoordPoint(20, 20), new CoordPoint(10,10)), Hull.Slots[0]);
            //Hull.Attach(new AttachedItem(new CoordPoint(20, 20), new CoordPoint(10, 10)), Hull.Slots[1]);

            Inventory.Attach(Hull.Slots[0], e1);
            Inventory.Attach(Hull.Slots[1], e2);
            //Inventory.Attach(Hull.Slots[2], w1);
            Inventory.Attach(Hull.Slots[3], w2);

            Color = Rnd.GetColor();



          //Reborn();
            //if(target != null)
            //    controller = new AIController(this, target, TaskType.Peersuit);
        }

        internal void GetDamage(int d, string text) {
            Hull.Health -= d;
            if(Hull.Health <= 0)
                Dead("from " + text);
        }

        internal void Dead(string cause) {

            if(OnDead != null)
                OnDead(this, EventArgs.Empty);

            new Explosion(World, Location);

            ToRemove = true;

            MainCore.Console(Name + " is dead cause of " + cause);
        }


        public void Accselerate() {
            foreach(Slot slot in Hull.Slots)
                if(slot.Type == SlotType.EngineSlot && !slot.IsEmpty)
                    slot.AttachedItem.Activate();
        }

        protected override string GetName() {
            return "SHIP " + Name;
        }

        //public TrajectoryCalculator Calculator { get; set; }
   

     
        

        protected internal override void Step() {

            //if(CoordPoint.Distance(CurrentSystem.Star.Position, Position) > 25000) {
            //    Dead("lost in the Void");
            //}

            //if(IsBot) {
            //    List<Action> actions = controller.Step();
            //    foreach(Action a in actions)
            //        a();
            //}

            foreach(Item item in Inventory.Container)
                item.Step();
            //var acc = GetAcceleration() * 0.5f;
            //Circle.ApplyLinearImpulse(acc);
            Circle.AngularVelocity = 0;
            Circle.Rotation += angleSpeed;
            angleSpeed *= .9f;

            base.Step();
        }



        //Vector2 GetAcceleration() {
        //    IEnumerable<DefaultEngine> engines = Hull.GetEngines();
        //    Vector2 sum = new Vector2();
        //    foreach(DefaultEngine engine in engines)
        //        sum += new Vector2(0, -1).GetRotated(engine.Rotation) * engine.GetAcceleration();
        //    return sum;
        //}

        internal void Fire() {
            IEnumerable<DefaultWeapon> weapons = Hull.GetWeapons();
            foreach(DefaultWeapon W in weapons)
                W.Fire();
        }

        public override IEnumerable<Item> GetItems() {
            //if(GetScreenBounds().Size.X < 10) {
            //    yield return new ScreenSpriteItem(Viewport.World2ScreenPoint(Location), new CoordPoint(20, 20), new CoordPoint(10, 10), new SpriteInfo("256tile.png"));
            //}
            //else
            for(int i = 0; i < Hull.Slots.Count; i++)
                yield return Hull.Slots[i].AttachedItem;
            yield return Hull;
        }

        float angleSpeed = 0;

        public void RotateL() {
            angleSpeed -= .01f;
        }
        public void RotateR() {
            angleSpeed += .01f;
        }

        //public void SwitchAI() {
        //    if(controller != null)
        //        controller = null;
        //    else controller = new AIController(this, CurrentSystem.Objects[2], TaskType.Peersuit);
        //}

        #region inventory
        //protected List<Item> ActiveItems { get { return new Item[](Hull.Slots.Select(p => p.AttachedItem).Where(i => i != null)) { Hull }; } }
        public Inventory Inventory { get; set; }
        protected internal ShipHull Hull { get; set; }
        #endregion
    }

    public struct InternalColor {

        public int b;
        public int g;
        public int r;

        public InternalColor(int r, int g, int b) {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public static InternalColor Black = new InternalColor() { r = 0, g = 0, b = 0 };
        public static InternalColor Blue = new InternalColor() { r = 55, g = 0, b = 255 };
        public static InternalColor Red = new InternalColor() { r = 255, g = 55, b = 0 };
        public static InternalColor Green = new InternalColor() { r = 55, g = 255, b = 0 };
        public static InternalColor Yellow = new InternalColor() { r = 255, g = 255, b = 0 };

    }
}

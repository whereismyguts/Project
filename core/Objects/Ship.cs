using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCore {
    public class Ship: GameObject {

        AIController controller;

        protected internal override float Rotation { get { return (Direction.Angle); } }

        internal override bool IsMinimapVisible { get { return true; } }

        public ColorCore Color { get; } // TODO remove
        public CoordPoint Direction { get { return direction; } }
        public bool IsBot { get { return controller != null; } }

        public override string Name {
            get {
                return "Ship";
            }
        }
        public override Bounds ObjectBounds {
            get {
                return new Bounds(Position - Hull.Size / 2f, Position + Hull.Size / 2f); //TODO use origin, because ship posiotion may be no in center of the ship
            }
        }
        //public CoordPoint Reactive { get { return -(direction * acceleration) * 50; } }

        //public CoordPoint Velosity { get { return velosity; } }

        public Ship( GameObject target, StarSystem system) : base(system) {

            Hull = new ShipHull() { Owner = this };
            Inventory = new Inventory(Hull);
            var e1 = new DefaultEngine();
                var e2 = new DefaultEngine();
                var w1 = new DefaultWeapon();
            Inventory.Add(e1);
            Inventory.Add(e2);
            Inventory.Add(w1);
            //Hull.Attach(new AttachedItem(new CoordPoint(20, 20), new CoordPoint(10,10)), Hull.Slots[0]);
            //Hull.Attach(new AttachedItem(new CoordPoint(20, 20), new CoordPoint(10, 10)), Hull.Slots[1]);

            Inventory.Attach(Hull.Slots[0], e1);
            Inventory.Attach(Hull.Slots[1], e2);
            Inventory.Attach(Hull.Slots[2], w1);

            Mass = 1;
            Color = RndService.GetColor();
            Reborn();
            if(target!=null)
               controller = new AIController(this, target, TaskType.Peersuit);

        }

        public void Accselerate() {
            foreach(Slot slot in Hull.Slots)
                if(slot.Type == SlotType.EngineSlot && !slot.IsEmpty)
                    slot.AttachedItem.Activate();
        }
        public override CoordPoint Position {
            get {
                return base.Position;
            }

            set {
                base.Position = value;
              //  if(Calculator!=null)
                //Calculator.Update();
            }
        }
        //public TrajectoryCalculator Calculator { get; set; }
        void Reborn() {
            Position =  new CoordPoint(RndService.Get(-12100, 12100), RndService.Get(-12100, 12100));
            //acceleration = 0;
            direction = new CoordPoint(1, 0);
            Velosity = new CoordPoint(-50,-50);
            //Calculator= new TrajectoryCalculator(this);
        }


        

        protected internal override void Step() {
            if(CoordPoint.Distance(CurrentSystem.Star.Position, Position) > 120000)
                Reborn();
            foreach(Body obj in CurrentSystem.Objects)
                if(CoordPoint.Distance(obj.Position, Position) <= obj.Radius)
                    Reborn();
            if(IsBot) {
                List<Action> actions = controller.Step();
                foreach(Action a in actions)
                    a();
            }
         
            foreach(Item item in Inventory.Container)
                item.Step();

            Velosity += Direction * GetAcceleration() + PhysicsHelper.GetSummaryAttractingForce(CurrentSystem.Objects, this);
            
            direction.Rotate(angleSpeed);
            angleSpeed *= PhysicsHelper.RotationInertia;

            base.Step();
        }

        float GetAcceleration() {
            IEnumerable<DefaultEngine> engines = Hull.GetEngines();
            float sum = 0;
            foreach(DefaultEngine engine in engines)
                sum+=engine.GetAcceleration();
            return sum;
        }

        public override IEnumerable<Item> GetItems() {
            for(int i = 0; i < Hull.Slots.Count; i++)
                yield return Hull.Slots[i].AttachedItem;
            yield return Hull;
        }

        public void RotateL() {
            angleSpeed -= .01f;
        }
        public void RotateR() {
            angleSpeed += .01f;
        }

        public void SwitchAI() {
            if(controller != null)
                controller = null;
            else controller = new AIController(this, CurrentSystem.Objects[2], TaskType.Peersuit);
        }

        public override IEnumerable<Geometry> GetPrimitives() {
            return new Geometry[] { };
        }

        #region inventory
        //protected List<Item> ActiveItems { get { return new Item[](Hull.Slots.Select(p => p.AttachedItem).Where(i => i != null)) { Hull }; } }
        public Inventory Inventory { get; set; }
        protected internal ShipHull Hull { get; set; }
        #endregion

        float angleSpeed = 0;
        //public CoordPoint velosity = new CoordPoint();
        CoordPoint direction = new CoordPoint(1, 0);
    }

    public struct ColorCore {
        public int b;
        public int g;
        public int r;

        public ColorCore(int r, int g, int b) {
            this.r = r;
            this.g = g;
            this.b = b;
        }
    }
}

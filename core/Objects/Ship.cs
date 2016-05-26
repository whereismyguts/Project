using System;
using System.Collections.Generic;
using System.Linq;
using Core;

namespace GameCore {
    public class Ship: GameObject {
        float accselerationDown;
        float accselerationUp;
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
        public CoordPoint Reactive { get { return -(direction * acceleration) * 50; } }

        public CoordPoint Velosity { get { return velosity; } }

        public Ship(CoordPoint location, GameObject target, StarSystem system) : base(system) {
            Hull = new ShipHull() { Owner = this };
            Hull.Attach(new AttachedItem(), Hull.Slots[0]);
            Hull.Attach(new AttachedItem(), Hull.Slots[1]);

            Position = location;
            Mass = 1;
            Color = RndService.GetColor();
            //   controller = new AIController(this, target, TaskType.Peersuit);
            accselerationUp = .1f;
            accselerationDown = accselerationUp / 3f;
        }

        void Death() {
            Position = new CoordPoint(-101000, 101000);
            acceleration = 0;
            direction = new CoordPoint(1, 0);
            velosity = new CoordPoint();
        }


        CoordPoint GetSummaryAttractingForce() {
            var vector = new CoordPoint();
            foreach(var obj in CurrentSystem.Objects)
                vector += PhysicsHelper.GravitationForceVector(this, obj);
            return vector;
        }

        protected internal override void Step() {
            foreach(Body obj in CurrentSystem.Objects)
                if(CoordPoint.Distance(obj.Position, Position) <= obj.Radius)
                    Death();
            if(IsBot) {
                List<Action> actions = controller.Step();
                foreach(Action a in actions)
                    a();
            }

            velosity += Direction * acceleration + GetSummaryAttractingForce();
            Position += velosity;
            direction.Rotate(angleSpeed);
            angleSpeed *= PhysicsHelper.RotationInertia;

            LowEngine();
        }

        public void AccselerateEngine() {
            if(acceleration + accselerationUp <= accelerationMax)
                acceleration = acceleration + accselerationUp;
            else
                acceleration = accelerationMax;
        }

        public override IEnumerable<Item> GetItems() {
            foreach(Item item in ActiveItems)
                yield return item;
        }
        public void LowEngine() {
            if(acceleration - accselerationDown >= 0)
                acceleration = acceleration - accselerationDown;
            else
                acceleration = 0;
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

        #region inventory
        protected List<Item> ActiveItems { get { return new List<Item>(Hull.Slots.Select(p => p.AttachedItem).Where(i => i != null).ToList()) { Hull }; } }
        protected internal ShipHull Hull { get; set; }
        #endregion

        float acceleration = 0;
        float accelerationMax = 0.7f;
        float angleSpeed = 0;
        CoordPoint velosity = new CoordPoint();
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

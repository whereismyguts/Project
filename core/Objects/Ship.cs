using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCore {
    public class Ship: GameObject {
        float acceleration = 0;
        float angleSpeed = 0;
        GameObject targetObject;
        CoordPoint velosity = new CoordPoint();
        CoordPoint direction = new CoordPoint(1, 0);
        static Random r = new Random();
        WeaponBase weapon;
        AIController controller;

        public WeaponBase Weapon { get { return weapon; } }
        public ColorCore Color { get; }
        public bool IsBot { get { return controller != null; } }
        public GameObject TargetObject { get { return IsBot ? targetObject : this; } }
        public CoordPoint Direction { get { return direction; } }
        public CoordPoint Velosity { get { return velosity; } }

        protected internal override Bounds Bounds {
            get {
                return new Bounds(Location - new CoordPoint(5, 5), Location + new CoordPoint(5, 5));
            }
        }
        protected internal override string ContentString {
            get {
                return "ship1";
            }
        }

        public Ship(CoordPoint location, GameObject target, StarSystem system) : base(system) {
            Location = location;
            Mass = 10;
            Color = new ColorCore(r.Next(100, 255), r.Next(100, 255), r.Next(100, 255));
            this.targetObject = target;
            weapon = new DefaultCannon();
            controller = new AIController(this, target, TaskType.Peersuit);
        }

        public void AccselerateEngine() {
            acceleration = acceleration + .1f <= 1f ? acceleration + .1f : acceleration;
        }
        public void StopEngine() {
            acceleration = 0;
        }
        public void RotateL() {
            angleSpeed -= .01f;
        }
        public void RotateR() {
            angleSpeed += .01f;
        }

        CoordPoint GetSummaryAttractingForce() {
            var vector = new CoordPoint();
            foreach(var obj in CurrentSystem.Objects)
                if(!Bounds.isIntersect(obj.Bounds))
                    vector += PhysicsHelper.GravitationForceVector(this, obj);

            return vector;
        }
        protected internal override float GetRotation() {
            return (float)(Direction.Angle);
        }
        protected internal override void Step() {
            if(IsBot)
                controller.Step()();


            velosity += Direction * acceleration + GetSummaryAttractingForce();
            Location += velosity;
            direction.Rotate(angleSpeed);
            angleSpeed *= PhysicsHelper.RotationInertia;
        }
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

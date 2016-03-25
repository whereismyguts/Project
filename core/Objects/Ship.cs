using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class Ship: GameObject {
        float enginePower = 0;
        float angleSpeed = 0;
        GameObject targetObject;
        CoordPoint totalSpeedVector = new CoordPoint();
        CoordPoint direction = new CoordPoint(1, 0);
        static Random r = new Random();
        WeaponBase weapon;

        public WeaponBase Weapon { get { return weapon; } }
        public ColorCore Color { get; set; }
        public bool IsBot {
            get { return targetObject != null; }
        }
        public GameObject TargetObject { get { return IsBot ? targetObject : this; } }
        public CoordPoint Direction { get { return direction; } }

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

        List<SpaceBody> AttractingObjects {
            get { return CurrentSystem.Objects; }
        }

        public Ship(CoordPoint location, GameObject target, StarSystem system) : base(system) {

            Location = location;
            Mass = 10;
            Color = new ColorCore(r.Next(100, 255), r.Next(100, 255), r.Next(100, 255));
            this.targetObject = target;// GetRandomTarget();
            weapon = new DefaultCannon();
        }

        public void AccselerateEngine() {
            enginePower = 1f;
        }
        public void StopEngine() {
            enginePower = 0;
        }
        public void RotateL() {
            angleSpeed -= .01f;
        }
        public void RotateR() {
            angleSpeed += .01f;
        }

        protected internal override float GetRotation() {
            return (float)(Direction.Angle);
        }
        protected internal override void Move() {

            totalSpeedVector += Direction * enginePower + GetSummaryAttractingForce();
            totalSpeedVector *= PhysicsHelper.MovingInertia;
            Location += totalSpeedVector;
            direction.Rotate(angleSpeed);
            angleSpeed *= PhysicsHelper.RotationInertia;

            if(IsBot)
                MakeAIMove();
        }
        
        CoordPoint GetSummaryAttractingForce() {
            var vector = new CoordPoint();
            foreach(var obj in AttractingObjects)
                if(!Bounds.isIntersect(obj.Bounds))
                    vector += PhysicsHelper.GravitationForceVector(this, obj);

            return vector;
        }
        int CheckWayToTarget() {
            float angle = direction.AngleTo(targetObject.Location - Location);

            if(angle <= Math.PI / 16 && angle > -Math.PI / 16)
                return 0;
            return angle > 0 ? 1 : -1;
        }
        void StraitToTarget() {
            switch(CheckWayToTarget()) {
                case 0:
                    AccselerateEngine();
                    break;
                case 1:
                    RotateL();
                    break;
                case -1:
                    RotateR();
                    break;
            }
        }
        bool InStarDeathZone {
            get {

                CurrentSystem.Star.Location
                return false;
            }
        }
        void LeaveDeathZone() {

        }
        void MoveToTarget(CoordPoint location) {
            if(InStarDeathZone)
                LeaveDeathZone();
            else
                StraitToTarget();
        }
        void MakeAIMove() {
            MoveToTarget(targetObject.Location);
        }
        Planet GetRandomTarget() {
            while(true) {
                int i = r.Next(0, AttractingObjects.Count - 1);
                return (Planet)AttractingObjects[i];
            }
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

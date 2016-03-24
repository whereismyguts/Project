using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class Character: GameObject {
        List<AttractingObject> AttractingObjects;
        float enginePower = 0;
        CoordPoint totalSpeedVector = new CoordPoint();
        float angleSpeed = 0;
        CoordPoint direction = new CoordPoint(1, 0);
        GameObject target;

        static Random r = new Random();
        public ColorCore Color { get; set; }
        public bool IsBot {
            get { return target != null; } }
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

        public GameObject Target { get { return IsBot ? target : this; } }

        public Character(Viewport viewport, List<AttractingObject> objects, CoordPoint location, GameObject target)
            : base(viewport) {
            AttractingObjects = objects;
            Location = location;
            Mass = 10;
            Color = new ColorCore(r.Next(100, 255), r.Next(100, 255), r.Next(100, 255));
            this.target = target;// GetRandomTarget();
        }

        CoordPoint GetSummaryAttractingForce() {
            var vector = new CoordPoint();
            foreach(var obj in AttractingObjects)
                if(!Bounds.isIntersect(obj.Bounds))
                    vector += PhysicsHelper.GravitationForceVector(this, obj);

            return vector;
        }
        protected internal override float GetRotation() {
            return (float)(Direction.Angle);
        }
        int CheckWayToTarget() {
            float angle = direction.AngleTo(target.Location - Location);

            System.Diagnostics.Debug.WriteLine(angle.ToString());
            if(angle <= Math.PI / 16 && angle > -Math.PI / 16)
                return 0;
            return angle > 0 ? 1 : -1;
        }

        void MakeDecision() {
            switch(CheckWayToTarget()) {
                case 0:
                    AccselerateEngine();
                    break;
                case 1: RotateL();
                    break;
                case -1:
                    RotateR();
                    break;
            }            
            
        }
        protected internal override void Move() {
            
            totalSpeedVector += Direction * enginePower + GetSummaryAttractingForce();
            totalSpeedVector *= PhysicsHelper.MovingInertia;
            Location += totalSpeedVector;
            direction.Rotate(angleSpeed);
            angleSpeed *= PhysicsHelper.RotationInertia;

            if(IsBot)
            MakeDecision();
        }
        public void AccselerateEngine() {
            enginePower = 5;
            return;
            if(enginePower + .1f <= 20f)
                enginePower += .1f;
        }
        public void LowDownEngine() {
            enginePower = 0;
            return;
            if(enginePower - .5f > 0)
                enginePower -= .5f;
        }

        public void RotateL() {
            angleSpeed -= .01f;
        }
        public void RotateR() {
            angleSpeed += .01f;
        }
        public void StopEngine() {
            enginePower = 0;
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

using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class Character: GameObject {
        List<AttractingObject> AttractingObjects;
        float enginePower = 0;
        CoordPoint totalSpeedVector;

        float angleSpeed = 0;
        CoordPoint direction = new CoordPoint(1, 0);

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

        public Character(Viewport viewport, List<AttractingObject> objects, CoordPoint location)
            : base(viewport) {
            AttractingObjects = objects;
            Location = location;
            Mass = 10;
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
        protected internal override void Move() {
            totalSpeedVector += Direction * enginePower + GetSummaryAttractingForce();
            Location += totalSpeedVector * PhysicsHelper.MovingInertia;
            direction.Rotate(angleSpeed);
            angleSpeed *= PhysicsHelper.RotationInertia;
        }
        public void Accselerate() {
            enginePower = .1f;
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
    }
}

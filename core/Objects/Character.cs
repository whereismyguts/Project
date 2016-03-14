using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class Character: GameObject {
        List<AttractingObject> AttractingObjects;
        float enginePower = 0;
        CoordPoint totalSpeedVector;

        public CoordPoint direction = new CoordPoint(1, 0);

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
            foreach (var obj in AttractingObjects)
                if (!Bounds.isIntersect(obj.Bounds)) 
                    vector += PhysicsHelper.GravitationForceVector(this, obj);
                
            return vector;
        }

        


        protected internal override float GetRotation() {
            return (float)(direction.Angle);
        }
        protected internal override void Move() {
            totalSpeedVector += direction * enginePower + GetSummaryAttractingForce();
            Location += totalSpeedVector * PhysicsHelper.Inertia;
        }

        public void AccselerateF() {
            enginePower =.1f;
        }
        public void RotateL() {
            direction.Rotate(-.1f);
        }
        public void RotateR() {
            direction.Rotate(.1f);
        }
        public void Stop() {
            enginePower = 0;
        }
    }
}

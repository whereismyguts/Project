using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class Character : GameObject {
        const float gravitationConstant = .6f;
        const float inertiaFactor = .5f;

        List<AttractingObject> AttractingObjects;
        CoordPoint currentTotalSpeedVector;
        float enginePower;
        CoordPoint engineVector;

        protected internal override Bounds Bounds {
            get {
                return new Bounds(Location - new CoordPoint(10, 10), Location + new CoordPoint(10, 10));
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
            engineVector = new CoordPoint(0, -1);
            enginePower = 0;
            Location = location;
            Mass = 10;
            currentTotalSpeedVector = new CoordPoint();
        }

        float AttractForce(GameObject obj) {
            var dist = CoordPoint.Distance(obj.Location, Location);
            return Mass * obj.Mass / (dist * dist) * gravitationConstant;
        }
        CoordPoint CalcSummaryForceVector() {
            var vector = new CoordPoint(engineVector).UnaryVector * enginePower;
            foreach (var obj in AttractingObjects) {
                var unary = (obj.Location - Location).UnaryVector;
                var force = AttractForce(obj);
                vector += unary * force;
            }
            return vector;
        }
        bool IsCollideSomething() {
            foreach (var obj in AttractingObjects)
                if (obj.Bounds.isIntersect(Bounds))
                    return true;
            return false;
        }

        protected internal override float GetRotation() {
            return 0;
        }
        protected internal override void Move() {
            if (!IsCollideSomething()) {
                currentTotalSpeedVector += CalcSummaryForceVector();
                Location += currentTotalSpeedVector;
            }
            currentTotalSpeedVector *= inertiaFactor;

            enginePower -= .1f;
            if (enginePower < 0)
                enginePower = 0;
        }

        public void Accselerate() {
            enginePower += 1;
        }
    }
}

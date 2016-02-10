using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class Character : GameObject {
        const float gravitationConstant = 0.6f;
        const float inertiaFactor = 0.5f;
        CoordPoint engineSpeed;
        CoordPoint currentSpeedVector;
        CoordPoint CalcSummaryForceVector() {
            CoordPoint vector = new CoordPoint(engineSpeed);
            foreach(var obj in AttractingObjects) {
                var unary = (obj.Location - Location).UnaryVector;
                var force = AttractForce(obj);
                vector += unary * force;
            }
            return vector;
        }
        float AttractForce(GameObject obj) {
            float dist = CoordPoint.Distance(obj.Location, Location);
            return this.Mass * obj.Mass / (dist * dist) * gravitationConstant;
        }
        List<AttractingObject> AttractingObjects;
        protected internal override Bounds Bounds
        {
            get { return new Bounds(Location - new CoordPoint(10, 10), Location + new CoordPoint(10, 10)); }
        }
        protected internal override string ContentString { get { return "ship1"; } }
        public float Speed {
            get { return CalcSummaryForceVector().Length; }
        }
        public Character(Viewport viewport, List<AttractingObject> objects, CoordPoint location) : base(viewport) {
            AttractingObjects = objects;
            engineSpeed = new CoordPoint(-.1f, 0);
            Location = location;
            Mass = 10;
            currentSpeedVector = new CoordPoint();
        }
        protected internal override float GetRotation() {
            return 0;
        }
        protected internal override void Move() {
            if(!IsCollideSomething()) {
                currentSpeedVector += CalcSummaryForceVector();
                this.Location += currentSpeedVector;
            }
            currentSpeedVector *= inertiaFactor;
        }
        bool IsCollideSomething() {
            foreach(var obj in AttractingObjects)
                if(obj.Bounds.isIntersect(Bounds))
                    return true;
            return false;
        }
    }
}

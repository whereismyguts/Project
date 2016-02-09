using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class Character : GameObjectBase {
        const float gravitationConstant = 0.6f;
        const float inertiaFactor = 0.05f;

        private CoordPoint CalcSummaryForceVector() {
            CoordPoint vector = new CoordPoint(EngineSpeed);
            foreach(var obj in AttractingObjects) {
                var unary = (obj.Location - Location).UnaryVector;
                var force = AttractForce(obj);
                vector += unary * force;
            }
            return vector;
        }

        float AttractForce(GameObjectBase obj) {
            float dist = CoordPoint.Distance(obj.Location, Location);
            return this.Mass*obj.Mass/(dist*dist)*gravitationConstant;
        }

        public CoordPoint EngineSpeed { get; }
        List<AttractingObject> AttractingObjects;
        CoordPoint currentSpeedVector;

        public override Bounds Bounds
        {
            get { return new Bounds(Location - new CoordPoint(30, 30), Location + new CoordPoint(30, 30)); }
        }
        public Character(Viewport viewport, List<AttractingObject> objects, CoordPoint location) :base(viewport) {
            AttractingObjects = objects;
            EngineSpeed = new CoordPoint(0, 0);
            Location = location;
            Mass = 100;
            currentSpeedVector = new CoordPoint();
        }
        public override void Move() {
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

using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class Character: GameObject {
        const float gravitationConstant = .015f;
        const float inertiaFactor = .99f;

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

        CoordPoint GetSummaryAttractingForcesVector() {
            var vector = new CoordPoint();
            foreach (var obj in AttractingObjects)
                if (!Bounds.isIntersect(obj.Bounds)) {
                    var force = gravitationConstant * obj.Mass * Mass / CoordPoint.Distance(obj.Location, Location);
                    var direction = (obj.Location - Location).UnaryVector;
                    vector += direction * force;
                }
            return vector;
        }

        protected internal override float GetRotation() {
            return (float)(direction.Angle);
        }
        protected internal override void Move() {
            totalSpeedVector += direction * enginePower + GetSummaryAttractingForcesVector();
            Location += totalSpeedVector * inertiaFactor;
        }

        public void AccselerateF() {
            enginePower +=.5f;
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

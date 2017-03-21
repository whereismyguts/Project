using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCore {
    public static class PhysicsHelper {
        public const float Gravitation = .35f;
        /// <summary>0 - stop moving immediately, 1.0 -moving never stops</summary>
        //public const float MovingInertia = .9f;
        public const float RotationInertia = .9f;

        public static float CalcGravitationForce(GameObject obj1, GameObject obj2) {
            return Gravitation * obj1.Mass * obj2.Mass / CoordPoint.Distance(obj1.Position, obj2.Position);
        }
        public static CoordPoint GravitationForceVector(GameObject obj1, GameObject obj2) {
            float force = CalcGravitationForce(obj1, obj2);
            var direction = (obj2.Position - obj1.Position).UnaryVector;
            return direction * force;
        }
        public static CoordPoint GetSummaryAttractingForce(List<Body> objects, GameObject subject) {
            var vector = new CoordPoint();
            foreach(var obj in objects)
                vector += GravitationForceVector(subject, obj);
            return vector;
        }
    }
}

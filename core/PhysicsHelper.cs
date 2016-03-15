using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core {
    public static class PhysicsHelper {
        public const float Gravitation = .015f;
        /// <summary>0 - stop moving immediately, 1.0 -moving never stops</summary>
        public const float MovingInertia = .7f;
        public const float RotationInertia = .9f;

        public static float CalcGravitationForce(GameObject obj1, GameObject obj2) {
            return Gravitation * obj1.Mass * obj2.Mass / CoordPoint.Distance(obj1.Location, obj2.Location);
        }
        public static CoordPoint GravitationForceVector(GameObject obj1, GameObject obj2) {
            float force = CalcGravitationForce(obj1, obj2);
            var direction = (obj2.Location - obj1.Location).UnaryVector;
            return direction * force;
        }
    }
}

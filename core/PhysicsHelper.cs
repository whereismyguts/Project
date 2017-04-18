using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCore {
    public static class PhysicsHelper {
        public const float Gravitation = .35f;
        /// <summary>0 - stop moving immediately, 1.0 -moving never stops</summary>
        //public const float MovingInertia = .9f;
        public const float RotationInertia = .85f;

        public static float CalcGravitationForce(GameObject obj1, GameObject obj2) {
            return Gravitation * obj1.Mass * obj2.Mass / Vector2.Distance(obj1.Location, obj2.Location);
        }
        public static Vector2 GravitationForceVector(GameObject obj1, GameObject obj2) {
            float force = CalcGravitationForce(obj1, obj2);
            var direction = obj1.Location.Substract(obj2.Location).UnaryVector();
            return direction * force;
        }
        //public static Vector2 GetSummaryAttractingForce(List<GameObject> objects, GameObject subject) {
        //    var vector = new Vector2();
        //    foreach(var obj in objects)
        //        if(obj.Mass > 0 && obj != subject)
        //            vector += GravitationForceVector(subject, obj);
        //    return vector;
        //}
    }
}

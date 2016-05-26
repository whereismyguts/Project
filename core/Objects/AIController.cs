using System;
using System.Collections.Generic;

namespace GameCore {
    public class AIController {
        Ship owner;
        CoordPoint targetLocation;
        GameObject targetObject;
        TaskType task;

        public AIController(Ship ship, GameObject target, TaskType taskType) {
            owner = ship;
            targetObject = target;
            task = taskType;
        }

        int CheckWayToTarget() {
            float angle = owner.Direction.AngleTo(targetLocation - owner.Position);
            if(angle <= Math.PI / 16 && angle > -Math.PI / 16)
                return 0;
            return angle > 0 ? 1 : -1;
        }
        Body GetDangerZone() {
            foreach(Body obj in owner.CurrentSystem.Objects)
                if(CoordPoint.Distance(obj.Position, owner.Position) <= 1.5 * obj.Radius)
                    return obj;
            return null;
        }
        List<Action> GetPeersuitActions() {
            Body danger = GetDangerZone();
            if(danger != null)
                TaskLeaveDeathZone(danger);
            else {
                float dist = CoordPoint.Distance(targetObject.Position, owner.Position);
                if(dist < owner.Velosity.Length)
                    TaskDecreaseSpeed();
                else
                    TaskGoToTarget();
                if(dist <= targetObject.ObjectBounds.Width && owner.Velosity.Length <= 20)
                    return new List<Action>();
            }



            switch(CheckWayToTarget()) {
                case 0:
                    return new List<Action>() { new Action(owner.AccselerateEngine) };
                case 1:
                    return new List<Action>() { new Action(owner.RotateL) };
                case -1:
                    return new List<Action>() { new Action(owner.RotateR) };
            }
            throw new Exception("cant decide");
        }
        void TaskDecreaseSpeed() {

            targetLocation = owner.Position - owner.Velosity.UnaryVector;
        }
        void TaskGoToTarget() {
            targetLocation = targetObject.Position;
        }
        void TaskLeaveDeathZone(GameObject obj) {
            // set target location as end of vector, normal to strait vector from center of danger obj
            CoordPoint toTarget = targetObject.Position - owner.Position;
            CoordPoint leaveVector = (owner.Position - obj.Position) * 2;

            if(Math.Abs(leaveVector.AngleTo(toTarget)) < Math.PI / 8)
                targetLocation = targetObject.Position;
            else {

                CoordPoint v1 = new CoordPoint(-leaveVector.Y, leaveVector.X);
                CoordPoint v2 = new CoordPoint(leaveVector.Y, -leaveVector.X);

                float a1 = Math.Abs(v1.AngleTo(toTarget));
                float a2 = Math.Abs(v2.AngleTo(toTarget));

                CoordPoint normalVector = a1 < a2 ? v1 : v2;


                targetLocation = leaveVector + normalVector + owner.Position;
            }
        }

        public List<Action> Step() {
            switch(task) {
                case TaskType.Peersuit:
                    return GetPeersuitActions();
            }
            return null;
        }
    }
    public enum TaskType { Peersuit, Kill, Away };
}

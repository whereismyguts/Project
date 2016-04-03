using System;
using System.Collections.Generic;

namespace GameCore {
    public class AIController {
        CoordPoint targetLocation;
        Ship owner;
        GameObject targetObject;
        TaskType task;

        public AIController(Ship ship, GameObject target, TaskType taskType) {
            owner = ship;
            targetObject = target;
            task = taskType;
        }
        int CheckWayToTarget() {
            float angle = owner.Direction.AngleTo(targetLocation - owner.Location);
            if(angle <= Math.PI / 16 && angle > -Math.PI / 16)
                return 0;
            return angle > 0 ? 1 : -1;
        }
        void TaskDecreaseSpeed() {
            
            targetLocation = owner.Location - owner.Velosity.UnaryVector;
        }
        Body GetDangerZone() {
            foreach(Body obj in owner.CurrentSystem.Objects)
                if(CoordPoint.Distance(obj.Location, owner.Location) <= 1.5 * obj.Radius)
                    return obj;
            return null;
        }
        void TaskLeaveDeathZone(GameObject obj) {
            // set target location as end of vector, normal to strait vector from center of danger obj
            CoordPoint toTarget = targetObject.Location - owner.Location;
            CoordPoint leaveVector = (owner.Location - obj.Location) * 2;

            if(Math.Abs(leaveVector.AngleTo(toTarget)) < Math.PI / 8)
                targetLocation = targetObject.Location;
            else {

                CoordPoint v1 = new CoordPoint(-leaveVector.Y, leaveVector.X);
                CoordPoint v2 = new CoordPoint(leaveVector.Y, -leaveVector.X);

                float a1 = Math.Abs(v1.AngleTo(toTarget));
                float a2 = Math.Abs(v2.AngleTo(toTarget));

                CoordPoint normalVector = a1 < a2 ? v1 : v2;


                targetLocation = leaveVector + normalVector + owner.Location;
            }
        }
        void TaskGoToTarget() {
            targetLocation = targetObject.Location;
        }
        List<Action> GetPeersuitActions() {
            Body danger = GetDangerZone();
            if(danger != null)
                TaskLeaveDeathZone(danger);
            else {
                float dist = CoordPoint.Distance(targetObject.Location, owner.Location);
                if(dist < owner.Velosity.Length)
                    TaskDecreaseSpeed();
                else
                    TaskGoToTarget();
                if(dist <= targetObject.Bounds.Width && owner.Velosity.Length <= 20)
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
        public List<Action> Step() {
            switch (task){
                case TaskType.Peersuit: 
                    return GetPeersuitActions();
            }
            return null;
        }
    }
    public enum TaskType { Peersuit, Kill, Away };
}

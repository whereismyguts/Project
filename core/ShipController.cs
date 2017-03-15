using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    public static class ShipController {
        public static List<BindingController> Controllers { get; } = new List<BindingController>();
        public static void Step() {
            foreach(BindingController controller in Controllers) {
                controller.Step();
            }
        }
    }
    public abstract class BindingController {
        public enum ShipAction { Accelerate, Left, Right }
        public BindingController(Ship ship) {
            owner = ship;
        }

        protected Ship owner;
        protected Action GetShipAction(ShipAction action) {
            switch(action) {
                case ShipAction.Accelerate: return owner.Accselerate;
                case ShipAction.Left: return owner.RotateL;
                case ShipAction.Right: return owner.RotateR;
                default: return null;
            }
        }
        internal abstract void Step();
    }
    public class ManualControl: BindingController {
        Dictionary<int, ShipAction> Actions = new Dictionary<int, ShipAction>();
        protected static int[] PressedKeys { get { return MainCore.Instance.Controller.Keys; } }

        public ManualControl(Ship ship) : base(ship) {
            SetKey(ShipAction.Accelerate, 38);
            SetKey(ShipAction.Left, 37);
            SetKey(ShipAction.Right, 39);
        }

        public void SetKey(ShipAction action, int key) {
            Actions[key] = action;
        }

        internal override void Step() {
            foreach(int key in Actions.Keys) {
                if(PressedKeys.Contains(key))
                    GetShipAction(Actions[key])();
            }
        }
    }
    class AutoControl: BindingController {
        CoordPoint targetLocation;

        public AutoControl(Ship ship) : base(ship) { }

        Body GetDangerZone() {
            for(int i = 0; i < owner.CurrentSystem.Objects.Count; i++)
                if(CoordPoint.Distance(owner.CurrentSystem.Objects[i].Position, owner.Position) <= 1.5 * owner.CurrentSystem.Objects[i].Radius)
                    return owner.CurrentSystem.Objects[i];
            return null;
        }
        void TaskLeaveDeathZone(GameObject obj) {
            // set target location as end of vector, normal to strait vector from center of danger obj

            CoordPoint leaveVector = (owner.Position - obj.Position) * 2;

            CoordPoint v1 = new CoordPoint(-leaveVector.Y, leaveVector.X);
            CoordPoint v2 = new CoordPoint(leaveVector.Y, -leaveVector.X);

            //float a1 = Math.Abs(v1.AngleTo(toTarget));
            //float a2 = Math.Abs(v2.AngleTo(toTarget));

            //CoordPoint normalVector = a1 < a2 ? v1 : v2;

            CoordPoint normalVector = v1;

            targetLocation = leaveVector + normalVector + owner.Position;
        }
        ShipAction CheckWayToTarget() {
            float angle = owner.Direction.AngleTo(targetLocation - owner.Position);
            if(angle <= Math.PI / 8 && angle > -Math.PI / 8)
                return ShipAction.Accelerate;
            return angle > 0 ? ShipAction.Left : ShipAction.Right;
        }

        internal override void Step() {
            Body danger = GetDangerZone();
            if(danger != null) {
                TaskLeaveDeathZone(danger);
                GetShipAction(CheckWayToTarget())();
            }
        }
    }
}

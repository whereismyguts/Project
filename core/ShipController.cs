using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    public static class AIShipsController {
        public static List<AIController> Controllers { get; } = new List<AIController>();
        public static void Step() {
            Controllers.RemoveAll(c => c.Owner == null || c.Owner.ToRemove);
            foreach(AIController controller in Controllers) {
                controller.Step();
            }
        }
        internal static void AddController(DefaultAutoControl controller) {
            Controllers.Add(controller);
        }
    }
    public abstract class AIController {
        //public enum ShipAction { Accelerate, Left, Right, Idle, Fire }
        public AIController(Ship ship) {
            Owner = ship;
        }

        public Ship Owner { get; set; }

        protected Action GetShipAction(PlayerAction action) {
            switch(action) {
                case PlayerAction.Up: return Owner.Accselerate;
                case PlayerAction.Left: return Owner.RotateL;
                case PlayerAction.Right: return Owner.RotateR;
                case PlayerAction.Yes: return Owner.Fire;
                default: return DoNothing;
            }
        }
        internal abstract void Step();
        public void DoNothing() { } //TODO rewrite
    }
    //public class ManualControl: BindingController {
    //    Dictionary<int, ShipAction> Actions = new Dictionary<int, ShipAction>();
    //    protected static List<int> PressedKeys { get { return InteractionController.KeysPressed; } }

    //    public ManualControl(Ship ship) : base(ship) {
    //        SetKey(ShipAction.Accelerate, 38);
    //        SetKey(ShipAction.Left, 37);
    //        SetKey(ShipAction.Right, 39);
    //        SetKey(ShipAction.Fire, 90);
    //    }

    //    public void SetKey(ShipAction action, int key) {
    //        Actions[key] = action;
    //    }

    //    internal override void Step() {
    //        foreach(int key in Actions.Keys) {
    //            if(PressedKeys != null && PressedKeys.Contains(key))
    //                GetShipAction(Actions[key])();
    //        }
    //    }
    //}
    public class DefaultAutoControl: AIController {
        public CoordPoint TargetLocation { get; private set; }

        public Ship ToKill { get; set; }

        double acceptableAngle = 0.1;
        double dangerZoneMultiplier = 2.4;

        public DefaultAutoControl(Ship ship) : base(ship) {

        }

        GameObject GetDangerZone() {
            for(int i = 0; i < Owner.CurrentSystem.Objects.Count; i++)
                if(CoordPoint.Distance(Owner.CurrentSystem.Objects[i].Position, Owner.Position) <= dangerZoneMultiplier * Owner.CurrentSystem.Objects[i].Radius)
                    return Owner.CurrentSystem.Objects[i];



            foreach(Ship s in MainCore.Instance.Ships)
                if(s != Owner && CoordPoint.Distance(Owner.Position, s.Position) < dangerZoneMultiplier * s.ObjectBounds.Width)
                    return s;
            return null;
        }
        void TaskLeaveDeathZone(GameObject obj) {
            // set target location as end of vector, normal to strait vector from center of danger obj

            CoordPoint toTarget = TargetLocation == null ? new CoordPoint() : TargetLocation - Owner.Position;
            CoordPoint leaveVector = (Owner.Position - obj.Position) * 2;

            //CoordPoint v1 = new CoordPoint(-leaveVector.Y, leaveVector.X);
            //CoordPoint v2 = new CoordPoint(leaveVector.Y, -leaveVector.X);

            //float a1 = Math.Abs(v1.AngleTo(toTarget));
            //float a2 = Math.Abs(v2.AngleTo(toTarget));

            //CoordPoint normalVector = a1 < a2 ? v1 : v2;


            //TargetLocation = leaveVector + normalVector + Owner.Position;

            TargetLocation = Owner.Position + leaveVector;
        }
        PlayerAction CheckWayToTarget() {
            if(TargetLocation == null)
                return PlayerAction.None;
            float angle = Owner.Direction.AngleTo(TargetLocation - Owner.Position);
            if(angle <= acceptableAngle && angle > -acceptableAngle)
                return PlayerAction.Up;
            return angle > 0 ? PlayerAction.Left : PlayerAction.Right;
        }

        internal override void Step() {
            GameObject danger = GetDangerZone();

            if(ToKill == null || ToKill.ToRemove)
                ToKill = FindEnemy();
            if(Owner.Health <= 5 && ToKill != null)
                TaskLeaveDeathZone(ToKill);

            if(Owner.Velosity.Length > 100)
                TaskDecreaseSpeed();
            else
                if(danger != null)
                TaskLeaveDeathZone(danger);
            else
                TaskGoToGoal();

            if(ToKill != null) {
                FireIfCan();
            }

            GetShipAction(CheckWayToTarget())();
        }

        private Ship FindEnemy() {
            var e = MainCore.Instance.Ships.Where(s => s.Fraction != Owner.Fraction && !s.ToRemove).OrderBy(s => CoordPoint.Distance(s.Position, Owner.Position)).ToList();
            return (e != null && e.Count > 0) ? e[Rnd.Get(0, e.Count - 1)] : null;
        }

        private void FireIfCan() {
            if(IsLookingTo(Owner, ToKill, Math.PI / 8) && !IsIntersectSomething(ToKill.Position, Owner.Position))
                Owner.Fire();
        }

        private bool IsLookingTo(Ship owner, Ship target, double a) {
            float angle = owner.Direction.AngleTo(target.Position - owner.Position);
            return angle <= a / 2f && angle >= -a / 2f;
        }

        private bool IsIntersectSomething(CoordPoint p1, CoordPoint p2) {
            foreach(var body in Owner.CurrentSystem.Objects)
                if(CommonSectionCircle(p1.X, p1.Y, p2.X, p2.Y, body.Position.X, body.Position.Y, body.Radius))
                    return true;
            return false;
        }

        bool CommonSectionCircle(double x1, double y1, double x2, double y2, double xC, double yC, double R) {
            x1 -= xC;
            y1 -= yC;
            x2 -= xC;
            y2 -= yC;

            double dx = x2 - x1;
            double dy = y2 - y1;

            //составляем коэффициенты квадратного уравнения на пересечение прямой и окружности.
            //если на отрезке [0..1] есть отрицательные значения, значит отрезок пересекает окружность
            double a = dx * dx + dy * dy;
            double b = 2 * (x1 * dx + y1 * dy);
            double c = x1 * x1 + y1 * y1 - R * R;

            //а теперь проверяем, есть ли на отрезке [0..1] решения
            if(-b < 0)
                return (c < 0);
            if(-b < (2 * a))
                return ((4 * a * c - b * b) < 0);

            return (a + b + c < 0);
        }

        private void TaskGoToGoal() {
            if(ToKill != null && CoordPoint.Distance(ToKill.Position, Owner.Position) > 3000)
                TargetLocation = ToKill.Position;
        }

        private void TaskDecreaseSpeed() {
            TargetLocation = Owner.Position - Owner.Velosity.UnaryVector * 10;
        }

        //private void SetRandomLocationGoal() {
        //    bool correctPosition = false;

        //    while(!correctPosition) {
        //        goal = new CoordPoint(RndService.Get(-20000, 20000), RndService.Get(-20000, 20000));
        //        correctPosition = true;
        //        foreach(Body body in Owner.CurrentSystem.Objects) {
        //            if(body.ObjectBounds.Contains(goal)) {
        //                correctPosition = false;
        //                break;
        //            }

        //        }
        //    }
        //}
    }
}

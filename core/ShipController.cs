using Microsoft.Xna.Framework;
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
        public Vector2 TargetLocation { get; private set; }

        public Ship ToKill { get; set; }

        double acceptableAngle = 0.1;
        double dangerZoneMultiplier = 2.4;

        public DefaultAutoControl(Ship ship) : base(ship) {

        }

        StarSystem System {
            get { return MainCore.Instance.System; }
        }

        GameObject GetDangerZone() {
            foreach(var obj in System.Objects)

                if(obj != Owner && Vector2.Distance(obj.Location, Owner.Location) <= dangerZoneMultiplier * obj.Radius)
                    return obj;

            //foreach(Ship s in MainCore.Instance.Ships)
            //    if(s != Owner && CoordPoint.Distance(Owner.Location, s.Location) < dangerZoneMultiplier * s.ObjectBounds.Width)
            //        return s;

            return null;
        }
        void TaskLeaveDeathZone(GameObject obj) {
            // set target location as end of vector, normal to strait vector from center of danger obj

            Vector2 toTarget = TargetLocation == null ? new Vector2() : TargetLocation - Owner.Location;
            Vector2 leaveVector = (Owner.Location - obj.Location) * 2;

            //CoordPoint v1 = new CoordPoint(-leaveVector.Y, leaveVector.X);
            //CoordPoint v2 = new CoordPoint(leaveVector.Y, -leaveVector.X);

            //float a1 = Math.Abs(v1.AngleTo(toTarget));
            //float a2 = Math.Abs(v2.AngleTo(toTarget));

            //CoordPoint normalVector = a1 < a2 ? v1 : v2;


            //TargetLocation = leaveVector + normalVector + Owner.Position;

            TargetLocation = Owner.Location + leaveVector;
        }
        PlayerAction CheckWayToTarget() {
            if(TargetLocation == null)
                return PlayerAction.None;
            float angle = Owner.Direction.AngleTo(TargetLocation - Owner.Location);
            if(angle <= acceptableAngle && angle > -acceptableAngle)
                return PlayerAction.Up;
            return angle > 0 ? PlayerAction.Left : PlayerAction.Right;
        }

        internal override void Step() {
            GameObject danger = GetDangerZone();

            if(ToKill == null || ToKill.ToRemove)
                ToKill = FindEnemy();
            if(Owner.Health <= 5 && ToKill != null && Vector2.Distance(Owner.Location, ToKill.Location) < ToKill.Radius*2)
                TaskLeaveDeathZone(ToKill);

            if(Owner.Velosity.Length() > Owner.Radius*10)
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
            var e = MainCore.Instance.Ships.Where(s => s.Fraction != Owner.Fraction && !s.ToRemove).OrderBy(s => Vector2.Distance(s.Location, Owner.Location)).ToList();
            return (e != null && e.Count > 0) ? e[Rnd.Get(0, e.Count - 1)] : null;
        }

        private void FireIfCan() {
            if(IsLookingTo(Owner, ToKill, Math.PI / 8) && !IsIntersectSomething(ToKill.Location, Owner.Location))
                Owner.Fire();
        }

        private bool IsLookingTo(Ship owner, Ship target, double a) {
            float angle = owner.Direction.AngleTo(target.Location - owner.Location);
            return angle <= a / 2f && angle >= -a / 2f;
        }

        private bool IsIntersectSomething(Vector2 p1, Vector2 p2) {
            foreach(var body in System.Objects)
                if(CommonSectionCircle(p1.X, p1.Y, p2.X, p2.Y, body.Location.X, body.Location.Y, body.Radius))
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
            if(ToKill != null && Vector2.Distance(ToKill.Location, Owner.Location) > (ToKill.Radius+Owner.Radius)*2)
                TargetLocation = ToKill.Location;
        }

        private void TaskDecreaseSpeed() {
            TargetLocation = Owner.Location - Owner.Velosity.UnaryVector() * 10;
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

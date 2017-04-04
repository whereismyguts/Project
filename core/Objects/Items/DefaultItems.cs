using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    public class DefaultWeapon: AttachedItem {
        int fireCoolDownMax;
        int fireCoolDown = 0;
        public override string Name {
            get {
                return "standard weapon";
            }
        }
        public override SpriteInfo SpriteInfo {
            get {
                return fireCoolDown < 10 ? new SpriteInfo("retrogunfire.png", 6, 1, 1) : new SpriteInfo("retrogun.png", 1, 1, 1); ;
            }
        }

        public DefaultWeapon() : base(new CoordPoint(1200, 1200), new CoordPoint(600, 600)) {
            fireCoolDownMax = Rnd.Get(100, 150);
        }

        public override float Rotation {
            get {
                return base.Rotation;
            }
        }

        public override void Activate() {
        }
        public override void Deactivate() {
        }

        public override void Step() {
            if(fireCoolDown < fireCoolDownMax)
                fireCoolDown++;
            base.Step();
        }
        internal void Fire() {
            if(fireCoolDown == fireCoolDownMax) {
                //var direction = Direction.GetRotated(Rnd.Get(-.1f, .1f));
                Slot.Hull.Owner.CurrentSystem.Add(CreateProjectile(Location + Origin, Slot.Hull.Owner.Direction, Slot.Hull.Owner));
                fireCoolDown = 0;
            }
        }

        protected virtual ProjectileBase CreateProjectile(CoordPoint location, CoordPoint direction, Ship owner) {
            return new ProjectileBase(location, direction, owner);
        }
    }
    public class DefaultEngine: AttachedItem {
        float acceleration = 0;
        float accelerationMax = 0.7f;
        float accselerationDown;
        float accselerationUp;
        public override float Rotation {
            get {
                return base.Rotation;
            }
        }
        public override string Name {
            get {
                return "standard engine";
            }
        }
        public DefaultEngine() : base(new CoordPoint(1200, 1200), new CoordPoint(600, 400)) {
            accselerationUp = .1f;
            accselerationDown = accselerationUp / 5f;
        }
        bool active = false;
        public override SpriteInfo SpriteInfo {
            get {
                return active ? new SpriteInfo("eng_active.png", 4, 1, 1) : new SpriteInfo("eng.png", 1, 1, 1);
            }
        }
        public override void Activate() {
            active = true; //TODO check fuel
            activetimer = 5;
        }
        public override void Deactivate() {
            active = false;
        }
        void AccselerateEngine() {
            if(acceleration + accselerationUp <= accelerationMax)
                acceleration = acceleration + accselerationUp;
            else
                acceleration = accelerationMax;

        }
        void LowEngine() {
            if(acceleration - accselerationDown >= 0)
                acceleration = acceleration - accselerationDown;
            else
                acceleration = 0;

        }
        int activetimer = 0;
        public override void Step() {

            if(active)
                AccselerateEngine();
            if(activetimer == 0)
                Deactivate();
            LowEngine();
            activetimer--;
        }

        internal float GetAcceleration() {
            return acceleration;
        }
    }



    public class RocketLauncher: DefaultWeapon {
        public override SpriteInfo SpriteInfo {
            get {
                return new SpriteInfo("gun.png");
            }
        }
        protected override ProjectileBase CreateProjectile(CoordPoint location, CoordPoint direction, Ship owner) {
            return new Rocket(location, direction, owner);
        }

    }
}

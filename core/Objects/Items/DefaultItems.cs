using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    public class DefaultWeapon: AttachedItem {
        int fireCoolDown = 0;
        public override string Name {
            get {
                return "standard weapon";
            }
        }
        public override SpriteInfo SpriteInfo {
            get {
                return new SpriteInfo("gun.png", 1, 1, 1);
            }
        }

        public DefaultWeapon() : base(new CoordPoint(1500, 1500), new CoordPoint(750, 750)) {
        }

        public override float Rotation {
            get {
                return base.Rotation;// + (float)(Math.PI / 2.0);
            }
        }

        public override void Activate() {
        }
        public override void Deactivate() {
        }

        public override void Step() {
            if(fireCoolDown < 120)
                fireCoolDown++;
            base.Step();
        }
        internal void Fire() {
            if(fireCoolDown == 120) {
                //var direction = Direction.GetRotated(Rnd.Get(-.1f, .1f));
                Slot.Hull.Owner.CurrentSystem.Add(new Bullet(Position + Origin, Position.GetRotated(-Rotation - (float)(Math.PI / 2.0)).UnaryVector, Slot.Hull.Owner));
                fireCoolDown = 0;
            }
        }
    }
    public class DefaultEngine: AttachedItem {
        float acceleration = 0;
        float accelerationMax = 0.7f;
        float accselerationDown;
        float accselerationUp;
        public override float Rotation {
            get {
                return base.Rotation - (float)(Math.PI / 2.0);
            }
        }
        public override string Name {
            get {
                return "standard engine";
            }
        }
        public DefaultEngine() : base(new CoordPoint(80, 40), new CoordPoint(40, 20)) {
            accselerationUp = .1f;
            accselerationDown = accselerationUp / 5f;
        }
        bool active = false;
        public override SpriteInfo SpriteInfo {
            get {
                return active ? new SpriteInfo("flame_sprite.png", 6) : new SpriteInfo("engine.png", 1);
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
}

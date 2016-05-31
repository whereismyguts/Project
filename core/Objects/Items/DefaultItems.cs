using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    public class DefaultWeapon: AttachedItem {
        public DefaultWeapon() : base(new CoordPoint(20,50), new CoordPoint(10,45)) {

        }

        public override SpriteInfo SpriteInfo {
            get {
                return new SpriteInfo("",1);
            }
        }
        public override void Activate() {
        }
        public override void Deactivate() {
        }
    }
    public class DefaultEngine : AttachedItem {
        float acceleration = 0;
        float accelerationMax = 0.7f;
        float accselerationDown;
        float accselerationUp;
        public override float Rotation {
            get {
                return active? base.Rotation: base.Rotation-(float)(Math.PI/2.0);
            }
        }
        public DefaultEngine() : base(new CoordPoint(80, 40), new CoordPoint(40, 20)) {
            accselerationUp = .1f;
            accselerationDown = accselerationUp / 3f;
        }
        bool active = false;
        public override SpriteInfo SpriteInfo {
            get {
                return new SpriteInfo("engine.png", 1);
            }
        }
        public override void Activate() {
            active = true; //TODO check fuel
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
        public override void Step() {
            if(active)
                AccselerateEngine();
            LowEngine();
        }

        internal float GetAcceleration() {
            return acceleration;
        }
    }
}

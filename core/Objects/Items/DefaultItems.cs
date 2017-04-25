using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    public class DefaultWeapon: AttachedItem {
        int fireCoolDownMax;
        int fireCoolDown = 10;

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
        public override float Rotation {
            get {
                return base.Rotation;
            }
        }

        public DefaultWeapon() : base(new Vector2(.5f, .5f), new Vector2(.25f, .25f)) {
            fireCoolDownMax = Rnd.Get(100, 150);
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
            if(fireCoolDown >= fireCoolDownMax) {
                //var direction = Direction.GetRotated(Rnd.Get(-.1f, .1f));
                CreateProjectile(Location.Add(Origin) + Slot.Hull.Owner.Direction * 10, Slot.Hull.Owner.Direction, Slot.Hull.Owner);
                fireCoolDown = 0;
            }
        }

        protected virtual ProjectileBase CreateProjectile(Vector2 location, Vector2 direction, Ship owner) {
            return new ProjectileBase(location, direction, owner);
        }
    }

    public class DefaultEngine: AttachedItem {
        //float acceleration = 0;
        //float accelerationMax = 0.7f;
        //float accselerationDown;
        //float accselerationUp;
        int activetimer = 0;
        bool active = false;

        public override SpriteInfo SpriteInfo {
            get {
                return active ? new SpriteInfo("eng_active.png", 4, 1, 1) : new SpriteInfo("eng.png", 1, 1, 1);
            }
        }
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

        public DefaultEngine() : base(new Vector2(.5f, .5f), new Vector2(0.25f, 0.25f)) {

        }

        void AccselerateEngine() {
            Slot.Hull.Owner.ApplyForce(Slot.Hull.Owner.Direction * 550);
        }
        public override void Activate() {
            active = true; //TODO check fuel
            activetimer = 5;
        }
        public override void Deactivate() {
            active = false;
        }
        public override void Step() {
            if(active)
                AccselerateEngine();
            if(activetimer == 0)
                Deactivate();
            activetimer--;
        }
    }

    public class RocketLauncher: DefaultWeapon {
        public override SpriteInfo SpriteInfo {
            get {
                return new SpriteInfo("gun.png");
            }
        }
        protected override ProjectileBase CreateProjectile(Vector2 location, Vector2 direction, Ship owner) {
            return new Rocket(location, direction, owner);
        }
    }
}

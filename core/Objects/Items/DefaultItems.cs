using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    public abstract class WeaponBase: AttachedItem {
        int fireCoolDownMax;
        int fireCoolDown = 10;

        public override SlotType Type {
            get {
                return SlotType.WeaponSlot;
            }
        }
        public override float Rotation {
            get {
                return base.Rotation;
            }
        }
        public WeaponBase() : base(new Vector2(3, 3), new Vector2(1.5f, 1.5f)) {
            fireCoolDownMax = Rnd.Get(100, 150); //TODO: customizesize, origine & cooldowntime
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

        protected abstract ProjectileBase CreateProjectile(Vector2 location, Vector2 direction, Ship owner);
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

        public override SlotType Type {
            get {
                return SlotType.EngineSlot;
            }
        }

        public DefaultEngine() : base(new Vector2(4f, 4f), new Vector2(2f, 2f)) {

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

    public class SlimeGun: WeaponBase {
        public override SpriteInfo SpriteInfo {
            get {
                return new SpriteInfo("retrogunfire.png", 6, 1, 1);// : new SpriteInfo("retrogun.png", 1, 1, 1); ;
                // TODO: return animation when fireing;
            }
        }

        protected override ProjectileBase CreateProjectile(Vector2 location, Vector2 direction, Ship owner) {
            return new Slime(location, direction, owner);
        }
        public override string Name {
            get {
                return "slime gun";
            }
        }
    }

    public class RocketLauncher: WeaponBase {
        public override SpriteInfo SpriteInfo {
            get {
                return new SpriteInfo("gun.png");
            }
        }
        protected override ProjectileBase CreateProjectile(Vector2 location, Vector2 direction, Ship owner) {
            return new Rocket(location, direction, owner);
        }

        public override string Name {
            get {
                return "rocket launcher";
            }
        }
    }
}

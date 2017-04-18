using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GameCore {
    public abstract class SpaceBody: GameObject {
        List<Geometry> sat = new List<Geometry>();
        protected float SelfRotation { get; set; }

        // public SpriteInfo SpriteInfo { get; } = new SpriteInfo("", 1);
        public SpaceBody(Vector2 location, float radius, World world) : base(world, location, radius) {
        }

        public override IEnumerable<Item> GetItems() {
            return new Item[] { new WordSpriteItem(this, ObjectBounds.Size, ObjectBounds.Size / 2, "earth.png") };
            // return new Item[] { new JustSpriteItem(this, ObjectBounds.Size, ObjectBounds.Size/2, "exp2.png", 4, 4) };
            //return new Item[] { };
        }
        public override IEnumerable<Geometry> GetPrimitives() {

            /*
            List<Geometry> geometry = new List<Geometry>(base.GetPrimitives());

            geometry.AddRange(sat);
            return geometry;
            */
            return base.GetPrimitives();
            
        }
        protected internal override void Step() {
            UpdateSatellite();
            base.Step();
        }
        private void UpdateSatellite() {

            double theta = 0;  // angle that will be increased each loop
                               //    double step = .01;  // amount to add to theta each time (degrees)
                               //   if(radius < 1)
                               //        radius = 1;

            float satRaduis = Radius * 2.2f;
            //  CoordPoint satSize = new CoordPoint(2000, 2000);

            double c = 2 * Math.PI * satRaduis;
            double step = Math.PI / c * 50;

            sat = new List<Geometry>();
            bool down = false;

            while(theta < Math.PI * 2) {
                //    if(theta+Math.PI/2 < Math.PI / 4f || theta + Math.PI / 2 > Math.PI * 3f / 4f) {

                float sx = (float)(satRaduis * Math.Cos(theta) * 0.15f);
                float sy = (float)(satRaduis * Math.Sin(theta));

                if((theta < Math.PI / 2 + step / 2 && theta > Math.PI / 2 - step / 2) ||
                    (theta < 3 * Math.PI / 2 + step / 2 && theta > 3 * Math.PI / 2 - step / 2))
                    down = !down;

                var p = new Vector2(sx, sy).GetRotated(SelfRotation);

                //if(satRadius < Radius * 0.95 && p.X < Location.X) { }
                //else {
                //sat.Add(new WorldGeometry(new CoordPoint(sx, sy), satSize));

                p = p + Location;

                Vector2 p2 = p + (Location - p).UnaryVector() * Math.Max(Location.Substract(p).Length() / 10, 200);

                byte color1 = (byte)Math.Abs(255 - theta / (Math.PI * 2) * 255 * 2);

                sat.Add(new Line(p, p2) { ZIndex = down ? -10 : 10, Color = new InternalColor(color1, color1, color1) });

                theta += step;
            }
        }
    }

    public class Star: SpaceBody {
        public Star(float radius, World world) : base(Vector2.Zero, radius, world) {
            Circle.BodyType = BodyType.Static;
            Circle.Mass *= 50;
        }

        protected override string GetName() {
            return "STAR" + ", mass = " + Mass; ;
        }
    }

    public class Planet: SpaceBody {
        string name;
        float DistanceToSun { get { return Vector2.Distance(RotateCenter.Location, Location); } }
        static SpaceBody RotateCenter { get { return CurrentSystem.Star; } }
        public override string Name { get { return name; } }

        public Planet(Vector2 location, float radius, World world)
            : base(location, radius, world) {

            SetOrbitaVelosity();
        }

        void SetOrbitaVelosity() {
            Circle.LinearVelocity = Vector2.Zero;

            var dir = Location.GetRotated((Rnd.Bool() ? 1 : -1) * (float)Math.PI / 2);
            var speed = (float)(Mass * Rnd.Get(0.01, 500));
            Circle.ApplyLinearImpulse(dir * speed);
            Circle.ApplyAngularImpulse(Mass*2);

            name = speed.ToString() + ", " + NameGenerator.Generate(Rnd.Get(0, 3)); ;

        }
        protected override string GetName() {
            return name + "mass = " + Mass;
        }
        protected internal override void Step() {
            if(DistanceToSun > 1000 || DistanceToSun < 180) {
                Circle.Position = GetNewLocation(this);
                SetOrbitaVelosity();
            }

            base.Step();
        }
        //protected internal override void Step() {
        //    if(starRotation >= 2 * Math.PI)
        //        starRotation = 0;
        //    Location = new Vector2((float)(DistanceToSun * Math.Cos(starRotation) + RotateCenter.Location.X), (float)(DistanceToSun * Math.Sin(starRotation) + RotateCenter.Location.Y));

        //    starRotation += clockwise ? rotationSpeed : -rotationSpeed;
        //    SelfRotation += .005f;

        //}
        //public override IEnumerable<Item> GetItems() {
        //    return new Item[] { new JustSpriteItem(this, ObjectBounds.Size, ObjectBounds.Size/2, "planet1.png", 5,4) };
        //}
    }

    static class NameGenerator {
        static List<string> baseparts = new List<string> { "za", "ke", "bre", "tho", "hu", "me", "ni", "jo", "cu", "az", "li", "eh", "unt", "ua", "hi", "che", "shi", "om", "slu" };
        static List<List<string>> bioms = new List<List<string>>() {
            new List<string> { "mue", "flo", "ph", "ble", "loo", "parr", "lio", "khai", "q'lee", "oo", "rash", "kroo", "o-i", "w'e", "aml", "faul", "hua", "hue", "hui", "eh", "oh", "ah" },
            new List<string> { "wlan", "xor", "tty", "stack", "izm", "tox", "vox", "pex", "mex", "sky", "zex", "row", "buff", "ling", "synt", "tic" },
            new List<string> { "grog", "agrh", "gerr", "urg", "krag", "ghar", "rog", "kog", "zorg", "gnar" },
            new List<string> { "plok", "plu", "qwa", "kue", "wle", "kle", "blow", "blob", "plee" }
        };
        static Random rnd = new Random();

        static string InternalGenerate(List<string> biom) {
            int n = rnd.Next(2, 4);
            string name = string.Empty;
            List<string> parts = new List<string>();
            parts.AddRange(biom);
            parts.AddRange(biom);
            parts.AddRange(baseparts);


            for(int i = 0; i < n; i++)
                name += parts[rnd.Next(0, parts.Count - 1)];
            return char.ToUpper(name[0]) + name.Substring(1);
        }
        public static string Generate(int i) {
            return InternalGenerate(bioms[i]);// + " / " + internalGenerate(biom2) + " / " +internalGenerate(biom3) + " / " + internalGenerate(biom4); 
        }
    }
}

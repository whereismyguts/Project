using System;
using System.Collections.Generic;

namespace GameCore {
    public class Body: GameObject {
        float radius;
        string ImageName;

        public float Diameter {
            get { return radius * 2; }
        }
        protected float SelfRotation { get; set; }
        protected internal override Bounds Bounds {
            get {
                return new Bounds(Location - new CoordPoint(radius, radius), Location + new CoordPoint(radius, radius));
            }
        }
        protected internal override string ContentString {
            get {
                return ImageName;
            }
        }

        public Body(CoordPoint location, float diameter, string imageName, StarSystem system) : base(system) {
            radius = diameter / 2.0f;
            Location = location;
            Mass = diameter;
            ImageName = imageName;
        }

        protected internal override float GetRotation() {
            return SelfRotation;
        }
        protected internal override void Step() {
            SelfRotation += .001f;
        }
    }

    public class Planet: Body {
        bool clockwise;
        static Random rnd = new Random();
        float starRotation = 0;
        public override string Name { get; } = NameGenerator.Generate();
        Body RotateCenter { get { return CurrentSystem.Star; } }
        float DistanceToSun {
            get {
                return CoordPoint.Distance(RotateCenter.Location, Location);
            }
        }

        public Planet(CoordPoint location, float diameter, float rotation, string imageName, bool clockwise, StarSystem system)
            : base(location, diameter, imageName, system) {
            this.starRotation = rotation;

            this.clockwise = clockwise;

            for(int i = 0; i < 100; i++)
                System.Diagnostics.Debug.WriteLine(NameGenerator.Generate());

        }

        protected internal override void Step() {
            if(starRotation >= 2 * Math.PI)
                starRotation = 0;
            Location = new CoordPoint((float)(DistanceToSun * Math.Cos(starRotation) + RotateCenter.Location.X), (float)(DistanceToSun * Math.Sin(starRotation) + RotateCenter.Location.Y));

            starRotation += clockwise ? .0001f : -.0001f;
            SelfRotation += .005f;

        }
    }

    static class NameGenerator {
        static List<string> baseparts = new List<string> { "za", "ke", "bre", "tho", "hu", "me", "ni", "jo", "cu", "az", "li", "eh", "unt", "ua", "hi", "che", "shi", "om", "slu" };
        static List<string> biom1 = new List<string> { "mue", "flo", "ph", "ble", "loo", "parr", "lio", "khai", "q'lee", "oo", "rash", "kroo", "o-i", "w'e", "aml", "faul", "hua", "hue", "hui", "eh", "oh", "ah" };
        static List<string> biom2 = new List<string> { "wlan", "xor", "tty", "stack", "izm", "tox", "vox", "pex", "mex", "sky", "zex", "row", "buff","ling","synt","tic" };
        static List<string> biom3 = new List<string> { "grog", "agrh", "gerr", "urg", "krag", "ghar", "rog", "kog", "zorg", "gnar" };
        static List<string> biom4 = new List<string> { "plok", "plu", "qwa", "kue", "wle", "kle", "blow", "blob", "plee" };
        static Random rnd = new Random();
        static string internalGenerate(List<string> biom) {
            int n = rnd.Next(2, 4);
            string name = "";
            List<string> parts = new List<string>();
            parts.AddRange(biom);
            parts.AddRange(biom);
            parts.AddRange(baseparts);


            for(int i = 0; i < n; i++)
                name += parts[rnd.Next(0, parts.Count - 1)];
            return char.ToUpper(name[0]) + name.Substring(1);
        }
        public static string Generate() {
            return internalGenerate(biom1);// + " / " + internalGenerate(biom2) + " / " +internalGenerate(biom3) + " / " + internalGenerate(biom4); 
        }
    }
}

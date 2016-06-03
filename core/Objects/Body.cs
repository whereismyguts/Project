using System;
using System.Collections.Generic;

namespace GameCore {
    public class Body: GameObject {
        float radius;
        float selfRotation;

        protected internal override float Rotation { get { return selfRotation; } }

        internal override bool IsMinimapVisible { get { return true; } }

        public override Bounds ObjectBounds {
            get {
                return new Bounds(Position - new CoordPoint(radius, radius), Position + new CoordPoint(radius, radius));
            }
        }

        public float Radius { get { return radius; } }

        public SpriteInfo SpriteInfo { get; } = new SpriteInfo("", 1);

        public Body(CoordPoint location, float radius, StarSystem system) : base(system) {
            this.radius = radius;
            Position = location;
            Mass = radius * 2;
        }

        protected internal override void Step() {
            selfRotation += .001f;
        }

        public override IEnumerable<Item> GetItems() {
            return new Item[] { };
        }
        public override IEnumerable<Geometry> GetPrimitives() {
            return new Geometry[] { new Circle(Position, Radius) };
        }
    }

    public class Planet: Body {
        bool clockwise;
        float selfRotation;

        float DistanceToSun { get { return CoordPoint.Distance(RotateCenter.Position, Position); } }
        Body RotateCenter { get { return CurrentSystem.Star; } }

        protected internal override float Rotation { get { return selfRotation; } }

        public override string Name { get; } = NameGenerator.Generate();

        public Planet(float distance, float diameter, float rotation, bool clockwise, StarSystem system)
            : base(new CoordPoint(system.Star.Position+new CoordPoint(distance, 0)), diameter, system) {
            starRotation = rotation;
            this.clockwise = clockwise;
            Mass *= 2;
        }

        protected internal override void Step() {
            if(starRotation >= 2 * Math.PI)
                starRotation = 0;
            Position = new CoordPoint((float)(DistanceToSun * Math.Cos(starRotation) + RotateCenter.Position.X), (float)(DistanceToSun * Math.Sin(starRotation) + RotateCenter.Position.Y));

            starRotation += clockwise ? .0001f : -.0001f;
            selfRotation += .005f;

        }
        float starRotation = 0;
    }

    static class NameGenerator {
        static string internalGenerate(List<string> biom) {
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

        public static string Generate() {
            return internalGenerate(biom1);// + " / " + internalGenerate(biom2) + " / " +internalGenerate(biom3) + " / " + internalGenerate(biom4); 
        }
        static List<string> baseparts = new List<string> { "za", "ke", "bre", "tho", "hu", "me", "ni", "jo", "cu", "az", "li", "eh", "unt", "ua", "hi", "che", "shi", "om", "slu" };
        static List<string> biom1 = new List<string> { "mue", "flo", "ph", "ble", "loo", "parr", "lio", "khai", "q'lee", "oo", "rash", "kroo", "o-i", "w'e", "aml", "faul", "hua", "hue", "hui", "eh", "oh", "ah" };
        static List<string> biom2 = new List<string> { "wlan", "xor", "tty", "stack", "izm", "tox", "vox", "pex", "mex", "sky", "zex", "row", "buff", "ling", "synt", "tic" };
        static List<string> biom3 = new List<string> { "grog", "agrh", "gerr", "urg", "krag", "ghar", "rog", "kog", "zorg", "gnar" };
        static List<string> biom4 = new List<string> { "plok", "plu", "qwa", "kue", "wle", "kle", "blow", "blob", "plee" };
        static Random rnd = new Random();
    }
}

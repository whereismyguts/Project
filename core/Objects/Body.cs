﻿using System;
using System.Collections.Generic;

namespace GameCore {
    public class Body: GameObject {
        float selfRotation;

        protected internal override float Rotation { get { return selfRotation; } }

        public override Bounds ObjectBounds {
            get {
                return new Bounds(Location - new CoordPoint(Radius, Radius), Location + new CoordPoint(Radius, Radius));
            }
        }

        // public SpriteInfo SpriteInfo { get; } = new SpriteInfo("", 1);
        public Body(CoordPoint location, float radius) {
            this.Radius = radius;
            Location = location;
            Mass = radius;
        }

        protected internal override void Step() {
            selfRotation += .001f;
        }

        public override IEnumerable<Item> GetItems() {
            return new Item[] { new WordSpriteItem(this, ObjectBounds.Size, ObjectBounds.Size / 2, "planet.png", 19, 1) };
            // return new Item[] { new JustSpriteItem(this, ObjectBounds.Size, ObjectBounds.Size/2, "exp2.png", 4, 4) };
            //return new Item[] { };
        }
        public override IEnumerable<Geometry> GetPrimitives() {
            return new Geometry[] { new WorldGeometry(Location, new CoordPoint(Radius * 2, Radius * 2), true) };
        }

        protected override string GetName() {
            return "STAR" + ", mass = " + Mass; ;
        }
    }

    public class Planet: Body {
        bool clockwise;
        float selfRotation;

        float DistanceToSun { get { return CoordPoint.Distance(RotateCenter.Location, Location); } }
        static Body RotateCenter { get { return CurrentSystem.Star; } }

        protected internal override float Rotation { get { return selfRotation; } }

        public override string Name { get; } = NameGenerator.Generate(Rnd.Get(0, 3));

        public Planet(float distance, float diameter, float rotation, bool clockwise)
            : base(new CoordPoint(RotateCenter.Location + new CoordPoint(distance, 0)), diameter / 2) {
            starRotation = rotation;
            this.clockwise = clockwise;
            rotationSpeed = Rnd.Get(.0005f, .0015f);
        }

        protected override string GetName() {
            return "PLANET " + Name + ", mass = " + Mass;
        }

        float rotationSpeed = 0;

        protected internal override void Step() {
            if(starRotation >= 2 * Math.PI)
                starRotation = 0;
            Location = new CoordPoint((float)(DistanceToSun * Math.Cos(starRotation) + RotateCenter.Location.X), (float)(DistanceToSun * Math.Sin(starRotation) + RotateCenter.Location.Y));

            starRotation += clockwise ? rotationSpeed : -rotationSpeed;
            selfRotation += .005f;

        }
        //public override IEnumerable<Item> GetItems() {
        //    return new Item[] { new JustSpriteItem(this, ObjectBounds.Size, ObjectBounds.Size/2, "planet1.png", 5,4) };
        //}

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

        public static string Generate(int i) {
            return internalGenerate(bioms[i]);// + " / " + internalGenerate(biom2) + " / " +internalGenerate(biom3) + " / " + internalGenerate(biom4); 
        }
        static List<string> baseparts = new List<string> { "za", "ke", "bre", "tho", "hu", "me", "ni", "jo", "cu", "az", "li", "eh", "unt", "ua", "hi", "che", "shi", "om", "slu" };

        static List<List<string>> bioms = new List<List<string>>() {
            new List<string> { "mue", "flo", "ph", "ble", "loo", "parr", "lio", "khai", "q'lee", "oo", "rash", "kroo", "o-i", "w'e", "aml", "faul", "hua", "hue", "hui", "eh", "oh", "ah" },
            new List<string> { "wlan", "xor", "tty", "stack", "izm", "tox", "vox", "pex", "mex", "sky", "zex", "row", "buff", "ling", "synt", "tic" },
            new List<string> { "grog", "agrh", "gerr", "urg", "krag", "ghar", "rog", "kog", "zorg", "gnar" },
            new List<string> { "plok", "plu", "qwa", "kue", "wle", "kle", "blow", "blob", "plee" }
        };
        static Random rnd = new Random();


    }
}

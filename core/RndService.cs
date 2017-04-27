using System;
using System.Linq;
using GameCore;
using Microsoft.Xna.Framework;

namespace GameCore {
    public static class Rnd {
        static Random rnd = new Random();

        static internal InternalColor GetColor() {
            return new InternalColor(rnd.Next(100, 255), rnd.Next(100, 255), rnd.Next(100, 255));
        }

        static public float GetPeriod() {
            return (float)(rnd.NextDouble() * Math.PI * 2);
        }

        public static int Get(int v1, int v2) {
            return rnd.Next(v1, v2 + 1);
        }

        public static bool Bool() {
            return rnd.Next(0, 2) == 0;
        }

        public static float Get(float v1, float v2) {
            var d = rnd.NextDouble() * (v2 - v1);
            return (float)(v1 + d);
        }
        public static double Get(double v1, double v2) {
            var d = rnd.NextDouble() * (v2 - v1);
            return v1 + d;
        }
        public static Vector2 Vector2(float v1, float v2, float d = 50) {
            return new Microsoft.Xna.Framework.Vector2(Get(v1 - d, v1 + d), Get(v2 - d, v2 + d));
        }

    }
}

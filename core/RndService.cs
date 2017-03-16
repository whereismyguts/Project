using System;
using System.Linq;
using GameCore;

namespace GameCore {
    public static class Rnd {
        static Random rnd = new Random();

        static internal ColorCore GetColor() {
            return new ColorCore(rnd.Next(100, 255), rnd.Next(100, 255), rnd.Next(100, 255));
        }

        static internal float GetPeriod() {
            return (float)(rnd.NextDouble() * Math.PI * 2);
        }

        internal static int Get(int v1, int v2) {
            return rnd.Next(v1, v2+1);
        }

        internal static bool Bool() {
            return rnd.Next(0, 1) == 0;
        }

        internal static float Get(float v1, float v2) {
            var d = rnd.NextDouble() * (v2 - v1);
            return (float)(v1 + d);
        }
        internal static double Get(double v1, double v2) {
            var d = rnd.NextDouble() * (v2 - v1);
            return v1 + d;
        }
    }
}

using System;
using System.Linq;
using GameCore;

namespace GameCore {
    public static class RndService {
        static Random rnd = new Random();

        static internal ColorCore GetColor() {
            return new ColorCore(rnd.Next(100, 255), rnd.Next(100, 255), rnd.Next(100, 255));
        }

        static internal float GetPeriod() {
            return (float)(rnd.NextDouble() * Math.PI * 2);
        }

        internal static int Get(int v1, int v2) {
            return rnd.Next(v1, v2);
        }

        internal static bool Bool() {
            return rnd.Next(0, 1) == 0;
        }
    }
}

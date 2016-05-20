using System;
using System.Linq;
using GameCore;

namespace Core {
    public static class RndService {
        static internal ColorCore GetColor() {
            return new ColorCore(rnd.Next(100, 255), rnd.Next(100, 255), rnd.Next(100, 255));
        }
        static internal float GetPeriod() {
            return (float)(rnd.NextDouble() * Math.PI * 2);
        }
        static Random rnd = new Random();
    }
}

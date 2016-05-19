using GameCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core {
    public static class RndService {
        static Random rnd = new Random();
        static internal float GetPeriod() {
            return (float)(rnd.NextDouble() * Math.PI * 2);
        }
        static internal ColorCore GetColor() {
            return new ColorCore(rnd.Next(100, 255), rnd.Next(100, 255), rnd.Next(100, 255));
        }
    }
}

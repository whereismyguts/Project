using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    public static class GeneticAnalizer {

        public static List<float[]> Results = new List<float[]>();

        internal static void Add(float[] moveCoeffs, int liveTime) {
            //    throw new NotImplementedException();


            if(liveTime > 1) {
                Results.Add(new float[] { moveCoeffs[0], moveCoeffs[1], moveCoeffs[2], liveTime });
            }
        }

        public static float[] GetNewCoeffs() {

            if(Results.Count >= 100) {

                var best5 = Results.OrderBy(r => r[3]).Reverse().Take(5).ToList();

                var best = best5[0];

                var newCoefs =  new float[] {
                    RndService.Get(best[0] - 0.1f, best[0] + 0.1f),
                    RndService.Get(best[1] - 0.1f, best[1] + 0.1f),
                    RndService.Get(best[2] - 0.1f, best[2] + 0.1f)
                };

                return newCoefs;
            }
            return new float[] { RndService.Get(0.5f, 1f), RndService.Get(0.5f, 1.5f), RndService.Get(0.5f, 1.5f) };
        }
    }
}

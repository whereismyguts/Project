using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore {
    public static class GeneticAnalizer {

      

        static int value = 0;

        static double bestA = 0;
        static double bestD = 0;
        static double bestS = 0;

        internal static void NewGeneration(int liveTime, ref double acceptableAngle, ref double dangerZoneMultiplier, ref double speed) {
            //if(liveTime > prevLiveTime) {
            //    prevLiveTime = liveTime;
            //    bestA = acceptableAngle;
            //    bestD = dangerZoneMultiplier;

            //}
            acceptableAngle = Rnd.Get(bestA - 0.05, bestA + 0.05);
            dangerZoneMultiplier = Rnd.Get(bestD - 0.05, bestD + 0.05);
            speed = Rnd.Get(bestS - 10, bestS + 10);
        }
        internal static void AnalizeStep(int goals, int liveTime, double acceptableAngle, double dangerZoneMultiplier, double maxSpeed) {
            if(liveTime+ goals*1000 > value ) {
                value = liveTime+ goals * 1000;
                bestA = acceptableAngle;
                bestD = dangerZoneMultiplier;
                bestS = maxSpeed;
            }

        }
    }
}

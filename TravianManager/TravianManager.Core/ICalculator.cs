using System;
using System.Collections.Generic;
using System.Text;
using TravianManager.Core.Data;

namespace TravianManager.Core
{
    public interface ICalculator
    {
        TimeSpan CalculateDistance(Coordinate coordOrigin, Coordinate coordTarget, int speed, int arena);

        long FindMinDifference(List<long> list);

        void RefreshDataPerAttacker(int AttackerID);
    }
}

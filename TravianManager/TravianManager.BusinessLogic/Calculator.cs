using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TravianManager.Core;
using TravianManager.Core.Data;
using TravianManager.Core.DataProvider;

namespace TravianManager.BusinessLogic
{
    public class Calculator : ICalculator
    {
        private readonly ITemplateDataProvider _templateDataProvider;

        private const int limitTime = 60;
        public Calculator(ITemplateDataProvider templateDataProvider)
        {
            _templateDataProvider = templateDataProvider;
        }
        public TimeSpan CalculateDistance(Coordinate coordOrigin, Coordinate coordTarget, int speed, int arena)
        {
            var overBorderX = 0;
            var overBorderY = 0;
            if (coordTarget.XCoordinate > 0)
            {
                overBorderX = 200 - Math.Abs(coordOrigin.XCoordinate) + 200 - Math.Abs(coordTarget.XCoordinate) + 1;
                overBorderY = coordOrigin.YCoordinate + (-1 * coordTarget.YCoordinate);
            }
            else
            {
                overBorderX = 200 - Math.Abs(coordOrigin.YCoordinate) + 200 - Math.Abs(coordTarget.YCoordinate) + 1;
                overBorderY = coordTarget.XCoordinate + (-1 * coordOrigin.XCoordinate);
            }

            var overBorder = Math.Sqrt(overBorderX * overBorderX + overBorderY * overBorderY);

            var fields = Math.Sqrt((coordOrigin.XCoordinate - coordTarget.XCoordinate) * (coordOrigin.XCoordinate - coordTarget.XCoordinate) +
                (coordOrigin.YCoordinate - coordTarget.YCoordinate) * (coordOrigin.YCoordinate - coordTarget.YCoordinate));

            if (overBorder < fields)
                fields = overBorder;

            var timeDistance = 0.0;

            if (fields > 20 && arena > 0)
            {
                var first20fields = (20.00 * 3600) / speed;

                var bonusSpeed = 100;
                for (int i = 0; i < arena; i++)
                {
                    bonusSpeed += 20;
                }
                var actualSpeed = (double)(speed * bonusSpeed) / 100;

                var arenaFields = (fields - 20) * 3600 / actualSpeed;

                timeDistance = Math.Round(first20fields + arenaFields);
            }
            else
            {
                timeDistance = Math.Round((fields * 3600) / speed);
            }

            return TimeSpan.FromSeconds(timeDistance);
        }

        public void RefreshDataPerAttacker(int AttackerID)
        {
            var attacker = _templateDataProvider.GetAttacker(AttackerID);

            var attackerCoordinates = new Coordinate
            {
                XCoordinate = Convert.ToInt32(attacker.Account.XCoord),
                YCoordinate = Convert.ToInt32(attacker.Account.YCoord)
            };

            foreach(var defender in attacker.Defender.OrderByDescending(x => x.DefenderID))
            {
                var defenderCoordinates = new Coordinate
                {
                    XCoordinate = Convert.ToInt32(defender.Account.XCoord),
                    YCoordinate = Convert.ToInt32(defender.Account.YCoord)
                };

                var distance = CalculateDistance(attackerCoordinates, defenderCoordinates, attacker.TroopSpeed, attacker.TournamentSquare);

                var date = Convert.ToDateTime(defender.ArrivingTime).AddDays(10) - Convert.ToDateTime(distance.ToString());
                defender.AttackingTime = date.Hours.ToString("00") + ":" + date.Minutes.ToString("00") + ":" + date.Seconds.ToString("00");

                var otherAttacks = attacker.Defender.Where(x => !x.DefenderID.Equals(defender.DefenderID)).Select(x => DateTimeOffset.Parse(x.AttackingTime).ToUnixTimeSeconds()).ToList();

                var notBeforeTime = String.IsNullOrEmpty(attacker.NotBeforeTime) ? 0 : DateTimeOffset.Parse(attacker.NotBeforeTime).ToUnixTimeSeconds();
                var attTime = DateTimeOffset.Parse(defender.AttackingTime).ToUnixTimeSeconds();

                if (notBeforeTime > attTime && notBeforeTime > 0)
                {
                    defender.AttackType = 0;
                }
                else
                {
                    var difference = FindMinDifference(DateTimeOffset.Parse(defender.AttackingTime).ToUnixTimeSeconds(), otherAttacks);
                    defender.AttackType = difference < 60 ? 0 : 1;
                }

                _templateDataProvider.UpdateDefender(defender);
            }

        }
        public long FindMinDifference(List<long> list)
        {
            list.Sort();

            long diff = long.MaxValue;

            for (int i = 0; i < list.Count - 1; i++)
                if (list[i + 1] - list[i] < diff)
                    diff = list[i + 1] - list[i];

            return diff;
        }

        public long FindMinDifference(long attack, List<long> otherAttacks)
        {
            otherAttacks.Sort();

            var diffValue = default(long);
            foreach(var item in otherAttacks)
            {
                diffValue = item >= attack ? item - attack : attack - item;

                if (diffValue < limitTime)
                    return diffValue;
            }

            return diffValue;
        }
    }
}

/*
 * PIURED-ENGINE
 * Copyright (C) 2023 PIURED
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System.Collections.Generic;

namespace Courel
{
    using Loader.GimmickSpecs;

    public class Q : PiecewiseFunction
    {
        List<GimmickPair> _warps;
        List<GimmickPair> _WPrime = new List<GimmickPair>();
        private PiecewiseFunction _if;
        List<double> _sumWjs = new List<double>();

        public Q(List<GimmickPair> warps, PiecewiseFunction iF)
        {
            _warps = warps;
            _if = iF;

            if (warps.Count < 1)
            {
                SetUpEmptyGimmick();
            }
            else
            {
                ConfigureGimmicks();
                PrepareSumWjs();
                SetUpFunctions();
            }
        }

        // TODO: improve performance
        public bool IsBeatWarpedOver(double beat)
        {
            foreach (GimmickPair warp in _warps)
            {
                // Debug.Log($"beat={beat}, warp.Beat={warp.Beat}, wart.End={warp.Beat + warp.Value}");
                if (beat >= warp.Beat && beat < warp.Beat + warp.Value)
                {
                    return true;
                }
            }
            return false;
        }

        void SetUpEmptyGimmick()
        {
            Add(
                new Step(
                    new TwoSidedCondition(
                        NegativeInfinity,
                        PositiveInfinity,
                        TwoSidedConditionInterval.ClosedLeftClosedRight
                    ),
                    new Identity()
                )
            );
        }

        void SetUpFunctions()
        {
            SetUpFirstStep();
            for (int i = 0; i < _WPrime.Count - 1; i++)
            {
                var currentWElement = _WPrime[i];
                var nextWElement = _WPrime[i + 1];
                var wjSumi = GetWjSum(i);
                var wjSumiminus1 = GetWjSum(i - 1);
                Add(
                    new Step(
                        new TwoSidedCondition(
                            currentWElement.Beat - wjSumiminus1,
                            nextWElement.Beat - wjSumi,
                            TwoSidedConditionInterval.OpenLeftClosedRight
                        ),
                        new F1(wjSumi)
                    )
                );
            }
        }

        void SetUpFirstStep()
        {
            Add(
                new Step(
                    new TwoSidedCondition(
                        NegativeInfinity,
                        // Beat is in seconds here
                        _WPrime[0].Beat,
                        TwoSidedConditionInterval.OpenLeftClosedRight
                    ),
                    new Identity()
                )
            );
        }

        void PrepareSumWjs()
        {
            double sum = 0;
            for (int j = 0; j < _WPrime.Count; j++)
            {
                sum += _WPrime[j].Value;
                _sumWjs.Add(sum);
            }
        }

        double GetWjSum(int index)
        {
            if (index < 0)
            {
                return 0;
            }
            else
            {
                return _sumWjs[index];
            }
        }

        void ConfigureGimmicks()
        {
            ConvertToWPrime();
            AddLastInfinityInterval();
        }

        void ConvertToWPrime()
        {
            foreach (var warp in _warps)
            {
                var bprime = _if.Eval(warp.Beat);
                var z = _if.Eval(warp.Beat + warp.Value);
                var wprime = z - bprime;
                _WPrime.Add(new GimmickPair(bprime, wprime));
            }
        }

        void AddLastInfinityInterval()
        {
            _WPrime.Add(new GimmickPair(PositiveInfinity, 0));
        }

        public class F1 : Function
        {
            double _sumWj;

            public F1(double sumWj)
            {
                _sumWj = sumWj;
            }

            public double Eval(double x)
            {
                return x + _sumWj;
            }
        }
    }
}

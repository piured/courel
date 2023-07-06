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

using UnityEngine;
using System.Collections.Generic;

namespace Courel
{
    using Loader;

    public class IF : PiecewiseFunction
    {
        List<GimmickPair> _bpss;

        public IF(List<GimmickPair> bpss)
        {
            if (bpss.Count < 1)
            {
                throw new System.Exception("BPMS must have at least one definition.");
            }
            _bpss = bpss;
            ConfigureGimmicks();
            SetUpStepFunctions();
        }

        void SetUpStepFunctions()
        {
            SetUpFirstStep();
            for (int i = 1; i < _bpss.Count - 1; i++)
            {
                var currentBps = _bpss[i];
                var nextBps = _bpss[i + 1];
                var function = new F2(Eval(currentBps.Beat), currentBps.Beat, currentBps.Value);
                Add(
                    new Step(
                        new TwoSidedCondition(
                            currentBps.Beat,
                            nextBps.Beat,
                            TwoSidedConditionInterval.OpenLeftClosedRight
                        ),
                        function
                    )
                );
            }
        }

        void SetUpFirstStep()
        {
            var firstStepFunction = new F1(_bpss[0].Value);
            var firstStep = new Step(
                new TwoSidedCondition(
                    NegativeInfinity,
                    _bpss[1].Beat,
                    TwoSidedConditionInterval.ClosedLeftClosedRight
                ),
                firstStepFunction
            );
            Add(firstStep);
        }

        void ConfigureGimmicks()
        {
            AddLastInfinityInterval();
        }

        void AddLastInfinityInterval()
        {
            _bpss.Add(new GimmickPair(PositiveInfinity, 0));
        }

        public class F1 : Function
        {
            double _v1;

            public F1(double v1)
            {
                _v1 = v1;
            }

            public double Eval(double x)
            {
                return x / _v1;
            }
        }

        public class F2 : Function
        {
            double _ifbi;
            double _bi;
            double _vi;

            public F2(double ifbi, double bi, double vi)
            {
                _ifbi = ifbi;
                _bi = bi;
                _vi = vi;
            }

            public double Eval(double x)
            {
                return _ifbi + (x - _bi) / _vi;
            }
        }
    }
}

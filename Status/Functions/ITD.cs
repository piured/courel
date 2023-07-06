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
    using Loader;

    public class ITD : PiecewiseFunction
    {
        private PiecewiseFunction _if;
        List<GimmickPair> _stops;
        private PiecewiseFunction _its;
        private PiecewiseFunction _iq;
        List<GimmickPair> _TPrime = new List<GimmickPair>();

        List<double> _sumRjs = new List<double>();

        public ITD(
            List<GimmickPair> stops,
            PiecewiseFunction iF,
            PiecewiseFunction iTS,
            PiecewiseFunction iQ
        )
        {
            _if = iF;
            _stops = stops;
            _its = iTS;
            _iq = iQ;

            if (stops.Count < 1)
            {
                SetUpEmptyGimmick();
            }
            else
            {
                ConfigureGimmicks();
                PrepareSumRjs();
                SetUpFunctions();
            }
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
            for (int i = 0; i < _TPrime.Count - 1; i++)
            {
                SetUpFirstStep();
                var currentTElement = _TPrime[i];
                var nextTElement = _TPrime[i + 1];
                var rjSumi = GetRjSum(i);
                Add(
                    new Step(
                        new TwoSidedCondition(
                            // Beat is in seconds here.
                            currentTElement.Beat,
                            nextTElement.Beat,
                            TwoSidedConditionInterval.ClosedLeftOpenRight
                        ),
                        new F1(rjSumi)
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
                        _TPrime[0].Beat,
                        TwoSidedConditionInterval.ClosedLeftOpenRight
                    ),
                    new Identity()
                )
            );
        }

        void PrepareSumRjs()
        {
            double sum = 0;
            for (int j = 0; j < _TPrime.Count; j++)
            {
                sum += _TPrime[j].Value;
                _sumRjs.Add(sum);
            }
        }

        double GetRjSum(int index)
        {
            return _sumRjs[index];
        }

        void ConfigureGimmicks()
        {
            ConvertToTPrime();
            AddLastInfinityInterval();
        }

        void ConvertToTPrime()
        {
            foreach (var stop in _stops)
            {
                _TPrime.Add(new GimmickPair(_its.Eval(_iq.Eval(_if.Eval(stop.Beat))), stop.Value));
            }
        }

        void AddLastInfinityInterval()
        {
            _TPrime.Add(new GimmickPair(PositiveInfinity, 0));
        }

        public class F1 : Function
        {
            double _sumRj;

            public F1(double sumRj)
            {
                _sumRj = sumRj;
            }

            public double Eval(double x)
            {
                return x + _sumRj;
            }
        }
    }
}

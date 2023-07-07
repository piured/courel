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

    public class TS : PiecewiseFunction
    {
        private PiecewiseFunction _if;
        private PiecewiseFunction _iq;
        List<GimmickPair> _stops;
        List<GimmickPair> _TPrime = new List<GimmickPair>();

        List<double> _sumRjs = new List<double>();

        public TS(List<GimmickPair> stops, PiecewiseFunction iF, PiecewiseFunction iQ)
        {
            _if = iF;
            _iq = iQ;
            _stops = stops;

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
            for (int i = 1; i < _TPrime.Count; i++)
            {
                var priorTElement = _TPrime[i - 1];
                var currentTElement = _TPrime[i];
                var rjSumiMinus1 = GetRjSum(i - 1);
                var rjSumi = GetRjSum(i);
                Add(
                    new Step(
                        new TwoSidedCondition(
                            // Beat is in seconds here.
                            priorTElement.Beat + rjSumiMinus1,
                            currentTElement.Beat + rjSumiMinus1,
                            TwoSidedConditionInterval.OpenLeftClosedRight
                        ),
                        new F1(rjSumiMinus1)
                    )
                );

                Add(
                    new Step(
                        new TwoSidedCondition(
                            // Beat is in seconds here.
                            currentTElement.Beat + rjSumiMinus1,
                            currentTElement.Beat + rjSumi,
                            TwoSidedConditionInterval.OpenLeftClosedRight
                        ),
                        new F2(currentTElement.Beat)
                    )
                );
                SetUpLastStep();
            }
        }

        void SetUpLastStep()
        {
            var lastTelement = _TPrime[_TPrime.Count - 1];
            var rjSumn = GetRjSum(_TPrime.Count - 1);
            Add(
                new Step(
                    new TwoSidedCondition(
                        // Beat is in seconds here.
                        lastTelement.Beat + rjSumn,
                        PositiveInfinity,
                        TwoSidedConditionInterval.OpenLeftClosedRight
                    ),
                    new F1(rjSumn)
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
            AddFirstInfinityInterval();
        }

        void ConvertToTPrime()
        {
            foreach (var stop in _stops)
            {
                _TPrime.Add(new GimmickPair(_iq.Eval(_if.Eval(stop.Beat)), stop.Value));
            }
        }

        void AddFirstInfinityInterval()
        {
            _TPrime.Insert(0, new GimmickPair(NegativeInfinity, 0));
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
                return x - _sumRj;
            }
        }

        public class F2 : Function
        {
            double _ci;

            public F2(double ci)
            {
                _ci = ci;
            }

            public double Eval(double x)
            {
                return _ci;
            }
        }
    }
}

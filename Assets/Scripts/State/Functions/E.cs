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

namespace Courel.State.Functions
{
    using Loader.GimmickSpecs;
    using Piecewise;

    public class E : PiecewiseFunction
    {
        private List<Speed> _speeds;
        private List<Speed> _EPrime = new List<Speed>();
        private PiecewiseFunction _if;
        private PiecewiseFunction _its;
        private PiecewiseFunction _itd;
        private PiecewiseFunction _iq;

        public E(
            List<Speed> speeds,
            PiecewiseFunction iF,
            PiecewiseFunction its,
            PiecewiseFunction itd,
            PiecewiseFunction iq
        )
        {
            _speeds = speeds;
            _if = iF;
            _its = its;
            _itd = itd;
            _iq = iq;

            if (speeds.Count < 1)
            {
                throw new System.Exception("SPEEDS need at lest 1 signature");
            }
            else
            {
                ConfigureGimmicks();
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
            SetUpFirstStep();
            for (int i = 1; i < _EPrime.Count - 1; i++)
            {
                var previuosEElement = _EPrime[i - 1];
                var currentEElement = _EPrime[i];
                var nextEElement = _EPrime[i + 1];
                var currentSpeedElement = _speeds[i - 1];

                if (currentSpeedElement.SpanTime != 0)
                {
                    Add(
                        new Step(
                            new TwoSidedCondition(
                                currentEElement.Beat,
                                currentEElement.Beat + currentEElement.SpanTime,
                                TwoSidedConditionInterval.OpenLeftClosedRight
                            ),
                            new F2(
                                currentEElement.Value,
                                previuosEElement.Value,
                                currentEElement.SpanTime,
                                currentEElement.Beat
                            )
                        )
                    );
                }
                Add(
                    new Step(
                        new TwoSidedCondition(
                            currentEElement.Beat + currentEElement.SpanTime,
                            nextEElement.Beat,
                            TwoSidedConditionInterval.OpenLeftClosedRight
                        ),
                        new F1(currentEElement.Value)
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
                        _EPrime[0].Beat,
                        TwoSidedConditionInterval.OpenLeftClosedRight
                    ),
                    new F1(_EPrime[0].Value)
                )
            );
        }

        void ConfigureGimmicks()
        {
            ConvertToEPrime();
            AddFirstAndLastInfinityInterval();
        }

        void ConvertToEPrime()
        {
            foreach (var speed in _speeds)
            {
                if (speed.SpanTimeType == SpanTimeType.Seconds)
                {
                    var s = new Speed();
                    s.Beat = H(speed.Beat); // in seconds here
                    s.Value = speed.Value;
                    s.SpanTime = speed.SpanTime;
                    _EPrime.Add(s);
                }
                else // SpanTimeType.Beats
                {
                    var s = new Speed();
                    s.Beat = H(speed.Beat); // in seconds here
                    s.Value = speed.Value;
                    s.SpanTime = H(speed.Beat + speed.SpanTime) - s.Beat;
                    _EPrime.Add(s);
                }
            }
        }

        double H(double b)
        {
            return _itd.Eval(_its.Eval(_iq.Eval(_if.Eval(b))));
        }

        void AddFirstAndLastInfinityInterval()
        {
            var s = new Speed();
            s.Beat = PositiveInfinity;
            _EPrime.Add(s);
            s = new Speed();
            s.Value = _EPrime[0].Value;
            _EPrime.Insert(0, s);
        }

        public class F1 : Function
        {
            double _s;

            public F1(double s)
            {
                _s = s;
            }

            public double Eval(double x)
            {
                return _s;
            }
        }

        public class F2 : Function
        {
            double _siminus1;
            double _b;

            double _k; // ((_si - _siminus1)/_pprime);

            public F2(double si, double siminus1, double pprime, double b)
            {
                _siminus1 = siminus1;
                _b = b;
                _k = ((si - _siminus1) / pprime);
            }

            public double Eval(double x)
            {
                return _k * (x - _b) + _siminus1;
            }
        }
    }
}

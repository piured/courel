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

namespace Courel.State.Functions
{
    using Loader.GimmickSpecs;
    using Piecewise;

    public class P : PiecewiseFunction
    {
        List<GimmickPair> _scrolls;

        public P(List<GimmickPair> scrolls)
        {
            if (scrolls.Count < 1)
            {
                throw new System.Exception("SCROLLS must have at least one definition.");
            }
            _scrolls = scrolls;
            ConfigureGimmicks();
            setUpStepFunctions();
        }

        void setUpStepFunctions()
        {
            SetUpFirstStep();
            for (int i = 1; i < _scrolls.Count - 1; i++)
            {
                var currentScroll = _scrolls[i];
                var nextScroll = _scrolls[i + 1];
                var function = new F2(
                    Eval(currentScroll.Beat),
                    currentScroll.Beat,
                    currentScroll.Value
                );
                Add(
                    new Step(
                        new TwoSidedCondition(
                            currentScroll.Beat,
                            nextScroll.Beat,
                            TwoSidedConditionInterval.OpenLeftClosedRight
                        ),
                        function
                    )
                );
            }
        }

        void SetUpFirstStep()
        {
            var firstStepFunction = new F1(_scrolls[0].Value);
            var firstStep = new Step(
                new TwoSidedCondition(
                    NegativeInfinity,
                    _scrolls[1].Beat,
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
            _scrolls.Add(new GimmickPair(PositiveInfinity, 0));
        }

        public class F1 : Function
        {
            double _s1;

            public F1(double s1)
            {
                _s1 = s1;
            }

            public double Eval(double x)
            {
                return x * _s1;
            }
        }

        public class F2 : Function
        {
            double _pbi;
            double _bi;
            double _si;

            public F2(double pbi, double bi, double si)
            {
                _pbi = pbi;
                _bi = bi;
                _si = si;
            }

            public double Eval(double x)
            {
                return (x - _bi) * _si + _pbi;
            }
        }
    }
}

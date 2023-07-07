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

using System;
using System.Collections.Generic;

namespace Courel.State.Functions.Piecewise
{
    public class PiecewiseFunction
    {
        // These should be enough large numbers for any song
        public double NegativeInfinity = Double.MinValue / 2;
        public double PositiveInfinity = Double.MaxValue / 2;
        public List<Step> Steps = new List<Step>();
        private int _lastStepIndex = 0;

        //TODO
        public void ResetIndex()
        {
            _lastStepIndex = 0;
        }

        public double Eval(double x)
        {
            for (int i = _lastStepIndex; i < _lastStepIndex + Steps.Count; i++)
            {
                int index = i % Steps.Count;
                var step = Steps[index];
                if (checkCondition(x, step.Condition))
                {
                    _lastStepIndex = index;
                    return step.Function.Eval(x);
                }
            }
            throw new Exception("FATAL ERROR: Not defined step in PiecewiseFunction");
        }

        public void Add(Step step)
        {
            Steps.Add(step);
        }

        private bool checkCondition(double x, TwoSidedCondition condition)
        {
            if (condition.Interval == TwoSidedConditionInterval.OpenLeftOpenRight)
            {
                return condition.LeftValue < x && x < condition.RightValue;
            }
            else if (condition.Interval == TwoSidedConditionInterval.OpenLeftClosedRight)
            {
                return condition.LeftValue < x && x <= condition.RightValue;
            }
            else if (condition.Interval == TwoSidedConditionInterval.ClosedLeftOpenRight)
            {
                return condition.LeftValue <= x && x < condition.RightValue;
            }
            else
            {
                return condition.LeftValue <= x && x <= condition.RightValue;
            }
        }

        public class Identity : Function
        {
            public double Eval(double x)
            {
                return x;
            }
        }
    }
}

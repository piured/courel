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

namespace Courel.State.Functions.Piecewise
{
    public enum TwoSidedConditionInterval
    {
        OpenLeftOpenRight,
        OpenLeftClosedRight,
        ClosedLeftOpenRight,
        ClosedLeftClosedRight
    }

    public class TwoSidedCondition
    {
        public double LeftValue;
        public double RightValue;
        public TwoSidedConditionInterval Interval;

        public TwoSidedCondition(
            double leftValue,
            double rightValue,
            TwoSidedConditionInterval interval
        )
        {
            LeftValue = leftValue;
            RightValue = rightValue;
            Interval = interval;
        }
    }
}
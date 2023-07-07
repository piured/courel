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
    using Loader.GimmickSpecs;

    public class Combos : PiecewiseFunction
    {
        public Combos(List<GimmickPair> combos)
        {
            if (combos.Count < 1)
            {
                // default is 0=1
                throw new System.Exception("COMBOS must have at least one definition.");
            }
            setUpStepFunctions(combos);
        }

        void setUpStepFunctions(List<GimmickPair> combos)
        {
            SetUpFirstStep(combos);
            for (int i = 0; i < combos.Count - 1; i++)
            {
                var currentCombo = combos[i];
                var nextCombo = combos[i + 1];
                var function = new ComboFunction((int)currentCombo.Value);
                Add(
                    new Step(
                        new TwoSidedCondition(
                            currentCombo.Beat,
                            nextCombo.Beat,
                            TwoSidedConditionInterval.OpenLeftClosedRight
                        ),
                        function
                    )
                );
            }
            SetUpLastStep(combos);
        }

        void SetUpFirstStep(List<GimmickPair> combos)
        {
            var firstStepFunction = new ComboFunction((int)combos[0].Value);
            var firstStep = new Step(
                new TwoSidedCondition(
                    NegativeInfinity,
                    combos[0].Beat,
                    TwoSidedConditionInterval.ClosedLeftClosedRight
                ),
                firstStepFunction
            );
            Add(firstStep);
        }

        void SetUpLastStep(List<GimmickPair> combos)
        {
            var lastCombo = combos[combos.Count - 1];
            var lastStepFunction = new ComboFunction((int)lastCombo.Value);
            var lastStep = new Step(
                new TwoSidedCondition(
                    lastCombo.Beat,
                    PositiveInfinity,
                    TwoSidedConditionInterval.OpenLeftClosedRight
                ),
                lastStepFunction
            );
            Add(lastStep);
        }

        private class ComboFunction : Function
        {
            private int _comboValue;

            public ComboFunction(int comboValue)
            {
                _comboValue = comboValue;
            }

            public double Eval(double x)
            {
                return _comboValue;
            }
        }
    }
}

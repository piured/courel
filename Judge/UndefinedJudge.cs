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

using Courel.Loader;

namespace Courel.Judge
{
    class UndefinedJudge : IJudge
    {
        Judgment _premature = new Judgment(false, true);

        public Judgment EvalBoundary(float delta, Note note)
        {
            return _premature;
        }

        public Judgment EvalEndHoldEvent(float delta, Hold hold)
        {
            return _premature;
        }

        public Judgment EvalHoldEvent(float delta, Note note)
        {
            return _premature;
        }

        public Judgment EvalLiftEvent(float delta, Note note)
        {
            return _premature;
        }

        public Judgment EvalTapEvent(float delta, Note note)
        {
            return _premature;
        }

        public bool ShouldHoldBeActive(Hold hold)
        {
            return true;
        }
    }
}

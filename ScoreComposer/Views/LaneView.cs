/*
 * COUREL
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

namespace Courel.ScoreComposer.Views
{
    using System;
    using Loader;

    public class LaneView : IndexBasedView
    {
        private Lane _lane;

        public LaneView(Lane lane, int length)
            : base(length)
        {
            _lane = lane;
        }

        public int GetNoteCount()
        {
            return _lane.GetAll().Count;
        }

        public LaneItem GetFirst()
        {
            // FIXME: what happens if there are no more notes. Maybe throw
            return HasReachedEnd() ? null : _lane.GetAll()[Index];
        }

        public LaneItem GetIthFromCurrentIndex(int i)
        {
            int j = GetNthFromIndex(i);
            return HasReachedEnd(j) ? null : _lane.GetAll()[j];
        }

        public LaneItem GetPrior()
        {
            return HasReachedBeginning(Index - 1) ? null : _lane.GetAll()[Index - 1];
        }
    }
}

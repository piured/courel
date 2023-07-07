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

namespace Courel.ScoreComposer
{
    using Loader;
    using Views;

    public class Lane
    {
        int _laneId;
        List<LaneItem> _notes = new List<LaneItem>();

        public Lane(int laneId)
        {
            _laneId = laneId;
        }

        public void Add(LaneItem laneItem)
        {
            _notes.Add(laneItem);
        }

        public int GetNoteCount()
        {
            return _notes.Count;
        }

        public LaneItem GetFirst()
        {
            // FIXME: what happens if there are no more notes. Maybe throw
            return _notes[0];
        }

        public void RemoveFirst()
        {
            _notes.RemoveAt(0);
        }

        public LaneView CreateView()
        {
            return new LaneView(this, _notes.Count);
        }

        public List<LaneItem> GetAll()
        {
            return _notes;
        }

        public int GetId()
        {
            return _laneId;
        }
    }

    public class LaneItem
    {
        public Note Note;
        public Row Row;

        private int _id;

        public LaneItem(Note note, Row row, int id)
        {
            Note = note;
            Row = row;
            _id = id;
        }

        public int GetId()
        {
            return _id;
        }

        public void SetRow(Row row)
        {
            Row = row;
        }
    }
}

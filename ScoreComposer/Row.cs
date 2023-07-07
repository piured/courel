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
    using Loader.Notes;

    public class Row
    {
        private List<RowItem> _rowItems = new List<RowItem>();
        private int _id;

        public Row(int id)
        {
            _id = id;
        }

        public int GetId()
        {
            return _id;
        }

        public void Add(RowItem rowItem)
        {
            _rowItems.Add(rowItem);
        }

        public List<RowItem> GetAll()
        {
            return _rowItems;
        }

        public bool AreAllNotesJudgedAndNotified()
        {
            bool flag = true;
            foreach (var rowItem in _rowItems)
            {
                flag &= rowItem.Note.HasBeenJudged();
                flag &= rowItem.Note.HasBeenNotified();
            }
            return flag;
        }

        public bool AreAllNotesOfType<T>()
        {
            bool flag = true;
            foreach (var rowItem in _rowItems)
            {
                flag &= rowItem.Note is T;
            }
            return flag;
        }

        public bool AreAnyNotesOfType<T>()
        {
            bool flag = false;
            foreach (var rowItem in _rowItems)
            {
                flag |= rowItem.Note is T;
            }
            return flag;
        }

        public void ResetNotes()
        {
            foreach (var rowItem in _rowItems)
            {
                rowItem.Note.ResetNote();
            }
        }

        // public bool AreAllHoldsHeld()
        // {
        //     bool flag = true;
        //     foreach(var rowItem in _rowItems)
        //     {
        //         if (rowItem.Note is Hold h)
        //         {
        //             flag &= h.IsHeld();
        //         }
        //     }
        //     return flag;
        // }

        public RowItem GetFirst()
        {
            return _rowItems[0];
        }
    }

    public class RowItem
    {
        public Note Note;
        public Lane Lane;

        public LaneItem LaneItem;

        public RowItem(Note note, Lane lane, LaneItem laneItem)
        {
            Note = note;
            Lane = lane;
            LaneItem = laneItem;
        }

        public void SetLane(Lane lane)
        {
            Lane = lane;
        }
    }
}

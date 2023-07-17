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
    using Views;

    public class Score
    {
        public Rows Rows = new Rows();
        public Lanes Lanes;

        public int NumberOfLanes;

        public Score(int numberOfLanes)
        {
            Lanes = new Lanes(numberOfLanes);
            NumberOfLanes = numberOfLanes;
        }

        public Row GetFirstRow()
        {
            return Rows.GetFirst();
        }

        public Row RemoveFirstRow()
        {
            Row row = Rows.GetFirst();
            //1. update lanes
            foreach (RowItem rowItem in row.GetAll())
            {
                rowItem.Lane.RemoveFirst();
            }
            //2. remove row
            Rows.RemoveFirst();
            return row;
        }

        public List<Row> GetAllRows()
        {
            return Rows.GetAll();
        }

        public Note GetFirstNoteAtLane(int laneId)
        {
            Lane lane = Lanes.GetLane(laneId);
            LaneItem laneItem = lane.GetFirst();
            return laneItem.Note;
        }

        public ScoreView CreateView()
        {
            return new ScoreView(this);
        }

        public LanesView CreateLanesView()
        {
            return Lanes.CreateView();
        }
    }
}

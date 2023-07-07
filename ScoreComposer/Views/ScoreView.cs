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

namespace Courel.ScoreComposer.Views
{
    using System;
    using Loader;

    public class ScoreView
    {
        private RowsView _rowsView;
        private LanesView _lanesView;

        private Score _score;

        public ScoreView(Score score)
        {
            _score = score;
            _rowsView = score.Rows.CreateView();
            _lanesView = score.Lanes.CreateView();
        }

        public void RollBackOne()
        {
            Row priorRow = _rowsView.GetPrior();
            _rowsView.RollBackOne();
            foreach (RowItem rowItem in priorRow.GetAll())
            {
                LaneView laneView = GetLaneView(rowItem.Lane);
                laneView.RollBackOne();
            }
        }

        public Row RemoveFirstRow()
        {
            Row row = _rowsView.GetFirst();
            //1. update lanes
            foreach (RowItem rowItem in row.GetAll())
            {
                LaneView laneView = GetLaneView(rowItem.Lane);
                laneView.RemoveFirst();
            }
            //2. remove row
            _rowsView.RemoveFirst();
            return row;
        }

        public void RemoveRowAndtLaneItems(Row row)
        {
            //1. update lanes
            foreach (RowItem rowItem in row.GetAll())
            {
                LaneView laneView = GetLaneView(rowItem.Lane);
                laneView.RemoveNth(rowItem.LaneItem.GetId());
            }
            //2. remove row
            _rowsView.RemoveNth(row.GetId());
        }

        public Row GetFirstRow()
        {
            return _rowsView.GetFirst();
        }

        private LaneView GetLaneView(Lane lane)
        {
            int laneId = lane.GetId();
            return _lanesView.GetLane(laneId);
        }

        public Row GetRowOfFirstNoteAtLane(int laneId)
        {
            LaneView lane = _lanesView.GetLane(laneId);
            LaneItem laneItem = lane.GetFirst();
            return laneItem == null ? null : laneItem.Row;
        }

        public Note GetFirstNoteAtLane(int laneId)
        {
            LaneView lane = _lanesView.GetLane(laneId);
            LaneItem laneItem = lane.GetFirst();
            return laneItem == null ? null : laneItem.Note;
        }

        public Note GetIthNoteAtLane(int laneId, int i)
        {
            LaneView lane = _lanesView.GetLane(laneId);
            LaneItem laneItem = lane.GetIthFromCurrentIndex(i);
            return laneItem == null ? null : laneItem.Note;
        }

        public Row GetPriorRow()
        {
            return _rowsView.GetPrior();
        }
    }
}

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

using System.Collections.Generic;

namespace Courel.ScoreComposer.Views
{
    public class LanesView
    {
        List<LaneView> _lanesViews = new List<LaneView>();

        public LanesView(List<Lane> lanes)
        {
            foreach (var lane in lanes)
            {
                _lanesViews.Add(lane.CreateView());
            }
        }

        public LaneView GetLane(int i)
        {
            return _lanesViews[i];
        }

        public List<LaneView> GetAll()
        {
            return _lanesViews;
        }
    }
}

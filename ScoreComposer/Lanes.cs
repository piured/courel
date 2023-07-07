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
    using Views;

    public class Lanes
    {
        private List<Lane> _lanes = new List<Lane>();

        public Lanes(int numberOfLanes)
        {
            for (int i = 0; i < numberOfLanes; i++)
            {
                _lanes.Add(new Lane(i));
            }
        }

        public Lane GetLane(int i)
        {
            return _lanes[i];
        }

        public List<Lane> GetAll()
        {
            return _lanes;
        }

        public LanesView CreateView()
        {
            return new LanesView(_lanes);
        }
    }
}

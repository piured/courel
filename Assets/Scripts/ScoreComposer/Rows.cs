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

    public class Rows
    {
        private List<Row> _rows = new List<Row>();

        public void Add(Row row)
        {
            _rows.Add(row);
        }

        public Row GetFirst()
        {
            if (_rows.Count > 0)
            {
                return _rows[0];
            }
            else
            {
                // faster than throwing
                return null;
            }
        }

        public void RemoveFirst()
        {
            _rows.RemoveAt(0);
        }

        public List<Row> GetAll()
        {
            return _rows;
        }

        public RowsView CreateView()
        {
            return new RowsView(this, _rows.Count);
        }
    }
}

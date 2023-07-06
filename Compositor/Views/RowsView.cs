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
using System;
using System.Collections.Generic;

namespace Courel
{
    public class RowsView : IndexBasedView
    {
        private Rows _rows;

        public RowsView(Rows rows, int length)
            : base(length)
        {
            _rows = rows;
        }

        public Row GetFirst()
        {
            return HasReachedEnd() ? null : _rows.GetAll()[Index];
        }

        public Row GetPrior()
        {
            return HasReachedBeginning(Index - 1) ? null : _rows.GetAll()[Index - 1];
        }
    }
}

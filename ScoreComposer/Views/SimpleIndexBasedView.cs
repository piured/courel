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
    public class SimpleIndexBasedView
    {
        public int Index;
        protected int _length;

        public SimpleIndexBasedView(int length)
        {
            Index = length == 0 ? -1 : 0;
            _length = length;
        }

        public void RemoveFirst()
        {
            Index++;
            if (Index >= _length)
            {
                Index = -1;
            }
        }
    }
}

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

namespace Courel
{
    public class IndexBasedView
    {
        public int Index;
        private bool[] _skips;
        protected int _length;

        public IndexBasedView(int length)
        {
            _skips = new bool[length];
            _length = length;
        }

        public void RemoveFirst()
        {
            // Index ++;
            IncrementIndex(1);
        }

        public void RemoveNth(int n)
        {
            if (n == Index)
            {
                IncrementIndex(1);
            }
            else
            {
                _skips[n] = true;
            }
        }

        private void IncrementIndex(int increment)
        {
            Index = GetNthFromIndex(increment);
        }

        public int GetNthFromIndex(int n)
        {
            int auxIndex = Index;
            for (int i = auxIndex; i < n + Index; i++)
            {
                if (i + 1 < _length && _skips[i + 1])
                {
                    n++;
                }
            }
            auxIndex += n;
            return auxIndex;
        }

        public bool HasReachedEnd()
        {
            return Index >= _length;
        }

        public bool HasReachedEnd(int newIndex)
        {
            return newIndex >= _length;
        }

        public bool HasReachedBeginning(int newIndex)
        {
            return newIndex < 0;
        }

        public void RollBackOne()
        {
            if (HasReachedBeginning(Index - 1))
            {
                return;
            }
            else
            {
                Index--;
                _skips[Index] = false;
            }
        }
    }
}

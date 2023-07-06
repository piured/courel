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
using UnityEngine;

namespace Courel
{
    public class TestInput : IHoldInput
    {
        Action<int> _onKeyDown;
        private Action<int> _onKeyUp;

        public TestInput(Action<int> onKeyDown, Action<int> onKeyUp)
        {
            _onKeyDown = onKeyDown;
            _onKeyUp = onKeyUp;
        }

        private bool[] _holdLanes = new bool[10];

        public void UpdateStatus()
        {
            CheckKeyDown();
            CheckKeyUp();
        }

        private void CheckKeyDown()
        {
            if (Input.GetKeyDown(KeyCode.Z) == true)
            {
                _onKeyDown(0);
                _holdLanes[0] = true;
            }
            if (Input.GetKeyDown(KeyCode.Q) == true)
            {
                _onKeyDown(1);
                _holdLanes[1] = true;
            }
            if (Input.GetKeyDown(KeyCode.S) == true)
            {
                _onKeyDown(2);
                _holdLanes[2] = true;
            }
            if (Input.GetKeyDown(KeyCode.E) == true)
            {
                _onKeyDown(3);
                _holdLanes[3] = true;
            }
            if (Input.GetKeyDown(KeyCode.C) == true)
            {
                _onKeyDown(4);
                _holdLanes[4] = true;
            }

            if (Input.GetKeyDown(KeyCode.V) == true)
            {
                _onKeyDown(5);
                _holdLanes[5] = true;
            }
            if (Input.GetKeyDown(KeyCode.R) == true)
            {
                _onKeyDown(6);
                _holdLanes[6] = true;
            }
            if (Input.GetKeyDown(KeyCode.G) == true)
            {
                _onKeyDown(7);
                _holdLanes[7] = true;
            }
            if (Input.GetKeyDown(KeyCode.Y) == true)
            {
                _onKeyDown(8);
                _holdLanes[8] = true;
            }
            if (Input.GetKeyDown(KeyCode.N) == true)
            {
                _onKeyDown(9);
                _holdLanes[9] = true;
            }
        }

        private void CheckKeyUp()
        {
            if (Input.GetKeyUp(KeyCode.Z) == true)
            {
                _holdLanes[0] = false;
                _onKeyUp(0);
            }
            if (Input.GetKeyUp(KeyCode.Q) == true)
            {
                _holdLanes[1] = false;
                _onKeyUp(1);
            }
            if (Input.GetKeyUp(KeyCode.S) == true)
            {
                _holdLanes[2] = false;
                _onKeyUp(2);
            }
            if (Input.GetKeyUp(KeyCode.E) == true)
            {
                _holdLanes[3] = false;
                _onKeyUp(3);
            }
            if (Input.GetKeyUp(KeyCode.C) == true)
            {
                _holdLanes[4] = false;
                _onKeyUp(4);
            }

            if (Input.GetKeyUp(KeyCode.V) == true)
            {
                _holdLanes[5] = false;
                _onKeyUp(5);
            }
            if (Input.GetKeyUp(KeyCode.R) == true)
            {
                _holdLanes[6] = false;
                _onKeyUp(6);
            }
            if (Input.GetKeyUp(KeyCode.G) == true)
            {
                _holdLanes[7] = false;
                _onKeyUp(7);
            }
            if (Input.GetKeyUp(KeyCode.Y) == true)
            {
                _holdLanes[8] = false;
                _onKeyUp(8);
            }
            if (Input.GetKeyUp(KeyCode.N) == true)
            {
                _holdLanes[9] = false;
                _onKeyUp(9);
            }
        }

        public bool IsHeld(int lane)
        {
            return _holdLanes[lane];
        }
    }
}

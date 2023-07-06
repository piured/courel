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


using UnityEngine;

namespace Courel
{
    namespace Loader
    {
        public class DdrStyleHold : Hold
        {
            private TapNote _head;
            private double _lastVActive;

            private double _elapsedTimeInactive;

            // when a hold is active, it is allowed to be notified
            private bool _isActive;

            public DdrStyleHold(double beginBeat, double endBeat, int lane, Visibility visibility)
                : base(beginBeat, endBeat, lane, visibility)
            {
                _elapsedTimeInactive = 0.0;
                _isActive = true;
            }

            public override void ResetNote()
            {
                base.ResetNote();
                _elapsedTimeInactive = 0.0;
                _isActive = true;
            }

            public override bool ReactsTo(InputEvent inputEvent)
            {
                return false;
            }

            public void SetHiddenHead(TapNote invisibleTapNote)
            {
                _head = invisibleTapNote;
            }

            public override void SetVBegin(double vBegin)
            {
                base.SetVBegin(vBegin);
                _lastVActive = vBegin;
            }

            public override void SetHeld(bool held, double currentSongTime)
            {
                if (currentSongTime >= _lastVActive)
                {
                    if (held)
                    {
                        _elapsedTimeInactive = 0.0;
                        _lastVActive = currentSongTime;
                    }
                    else
                    {
                        _elapsedTimeInactive = currentSongTime - _lastVActive;
                    }
                }
            }

            public double GetElapsedTimeInactive()
            {
                return _elapsedTimeInactive;
            }

            public TapNote GetInvisibleHead()
            {
                return _head;
            }

            public override bool IsActive()
            {
                return _isActive;
            }

            public override void SetActive(bool active)
            {
                _isActive = active;
            }
        }
    }
}

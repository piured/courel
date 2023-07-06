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


namespace Courel
{
    namespace Loader
    {
        public class DdrStyleRollHold : DdrStyleHold
        {
            private double _lastVInactive;

            private double _elapsedTimeActive;

            public DdrStyleRollHold(
                double beginBeat,
                double endBeat,
                int lane,
                Visibility visibility
            )
                : base(beginBeat, endBeat, lane, visibility)
            {
                _elapsedTimeActive = 0.0;
            }

            public override void ResetNote()
            {
                base.ResetNote();
                _elapsedTimeActive = 0.0;
            }

            public override bool ReactsTo(InputEvent inputEvent)
            {
                return false;
            }

            public override void SetVBegin(double vBegin)
            {
                base.SetVBegin(vBegin);
                _lastVInactive = vBegin;
            }

            public override void SetHeld(bool held, double currentSongTime)
            {
                base.SetHeld(held, currentSongTime);
                if (currentSongTime >= _lastVInactive)
                {
                    if (!held)
                    {
                        _elapsedTimeActive = 0.0;
                        _lastVInactive = currentSongTime;
                    }
                    else
                    {
                        _elapsedTimeActive = currentSongTime - _lastVInactive;
                    }
                }
            }

            public double GetElapsedTimeActive()
            {
                return _elapsedTimeActive;
            }
        }
    }
}

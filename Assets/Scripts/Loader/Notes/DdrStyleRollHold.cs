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


namespace Courel.Loader.Notes
{
    using Input;

    /// <summary>
    ///  A note that must be tapped when it crosses the judgment row, then tapped repeatly it until the end.
    ///  <see cref="Courel.Loader.Notes.DdrStyleRollHold"/> generate both
    /// a  <see cref="Courel.Loader.Notes.TapNote"/> with <see cref="Courel.Loader.Notes.Visibility.Normal"/> visibility at the beginning beat of the hold (Head) and
    /// a <see cref="Courel.Loader.Notes.HoldNote"/> with <see cref="Courel.Loader.Notes.Visibility.Hidden"/> visibility
    /// at the ending beat of the hold.
    /// </summary>
    public class DdrStyleRollHold : DdrStyleHold
    {
        private double _lastVInactive;

        private double _elapsedTimeActive;

        /// <summary>
        /// Initializes a new instance of the <see cref="Courel.Loader.Notes.DdrStyleRollHold"/> class.
        /// </summary>
        /// <param name="beginBeat"> Beat at which the rollHold should be tapped.</param>
        /// <param name="endBeat"> Beat at which the rollHold runs out of scope.</param>
        /// <param name="lane"> Lane index the rollHold belongs to.</param>
        /// <param name="visibility"> RollHold visibility.</param>
        public DdrStyleRollHold(double beginBeat, double endBeat, int lane, Visibility visibility)
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

        /// <summary>
        ///  Gets the time elapsed since the rollHold was last tapped.
        /// </summary>
        /// <returns> Elapsed time in seconds.</returns>
        public double GetElapsedTimeActive()
        {
            return _elapsedTimeActive;
        }
    }
}

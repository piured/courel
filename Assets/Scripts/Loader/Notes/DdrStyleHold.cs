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

namespace Courel.Loader.Notes
{
    using Input;

    /// <summary>
    /// A note that must be tapped when it crosses the judgment row, then held until the end crosses the judgment row.
    /// <see cref="Courel.Loader.Notes.DdrStyleHold"/> generate both
    /// a  <see cref="Courel.Loader.Notes.TapNote"/> with <see cref="Courel.Loader.Notes.Visibility.Normal"/> visibility at the beginning beat of the hold (Head) and
    /// a <see cref="Courel.Loader.Notes.HoldNote"/> with <see cref="Courel.Loader.Notes.Visibility.Hidden"/> visibility
    /// at the ending beat of the hold.
    /// </summary>
    public class DdrStyleHold : Hold
    {
        private TapNote _head;
        private double _lastVActive;

        private double _elapsedTimeInactive;

        // when a hold is active, it is allowed to be notified
        private bool _isActive;

        /// <summary>
        /// Initializes a new instance of the <see cref="Courel.Loader.Notes.DdrStyleHold"/> class.
        /// </summary>
        /// <param name="beginBeat"> Beat at which the hold should be tapped.</param>
        /// <param name="endBeat"> Beat at which the hold can be unheld.</param>
        /// <param name="lane"> Lane index the hold belongs to.</param>
        /// <param name="visibility"> Hold visibility.</param>
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

        /// <summary>
        /// Sets the head <see cref="Courel.Loader.Notes.TapNote"/> of the hold.
        /// </summary>
        /// <param name="invisibleTapNote"></param>
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

        /// <summary>
        /// Gets the time elapsed since the hold was last held.
        /// </summary>
        /// <returns> Elapsed time in seconds.</returns>
        public double GetElapsedTimeInactive()
        {
            return _elapsedTimeInactive;
        }

        /// <summary>
        /// Gets the head <see cref="Courel.Loader.Notes.TapNote"/> of the hold.
        /// </summary>
        /// <returns></returns>
        public TapNote GetHead()
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

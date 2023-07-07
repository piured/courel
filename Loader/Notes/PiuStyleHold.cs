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
    /// A hold that must be tapped or held when it crosses the judgment row, then held  until the end crosses the judgment row.
    /// <see cref="Courel.Loader.Notes.PiuStyleHold"/> generate <see cref="Courel.Loader.Notes.HoldNote"/>s with <see cref="Courel.Loader.Notes.Visibility.Hidden"/> visibility
    /// according to the hold's length and tickcounts gimmick.
    /// </summary>
    public class PiuStyleHold : Hold
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Courel.Loader.Notes.PiuStyleHold"/> class.
        /// </summary>
        /// <param name="beginBeat"> Beat at which the hold should be tapped or held.</param>
        /// <param name="endBeat"> Beat at which the hold can be unheld.</param>
        /// <param name="lane"> Lane index the hold belongs to.</param>
        /// <param name="visibility"> Hold visibility.</param>
        public PiuStyleHold(double beginBeat, double endBeat, int lane, Visibility visibility)
            : base(beginBeat, endBeat, lane, visibility) { }

        public override bool IsActive()
        {
            return true;
        }

        public override bool ReactsTo(InputEvent inputEvent)
        {
            return false;
        }

        public override void SetActive(bool active) { }
    }
}

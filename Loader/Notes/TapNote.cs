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
    /// A single note that reacts to <see cref="Courel.Input.InputEvent.Tap"/>.
    /// </summary>
    public class TapNote : SingleNote
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Courel.Loader.Notes.TapNote"/> class.
        /// </summary>
        /// <param name="beat">Beat at which the note should be tapped.</param>
        /// <param name="lane">Lane index the note belongs to.</param>
        /// <param name="visibility">Note visibility.</param>
        public TapNote(double beat, int lane, Visibility visibility)
            : base(beat, lane, visibility) { }

        /// <summary>
        /// Checks if the note reacts to given <see cref="Courel.Input.InputEvent"/>.
        /// </summary>
        /// <param name="inputEvent"> Input event to check.</param>
        /// <returns> True if <paramref name="inputEvent"/> is <see cref="Courel.Input.InputEvent.Tap"/>, false otherwise.</returns>
        public override bool ReactsTo(InputEvent inputEvent)
        {
            return inputEvent == InputEvent.Tap;
        }
    }
}

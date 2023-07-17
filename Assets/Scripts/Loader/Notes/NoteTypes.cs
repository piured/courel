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
    /// <summary>
    /// Visibility type of a note.
    /// </summary>
    public enum Visibility
    {
        /// A note with Hidden visibility will not be included in <see cref="Courel.ScoreComposer.Composer.GetDrawableNotes"/>, but it will be judged normally.
        Hidden,

        /// A note with Fake visibility will not be included in <see cref="Courel.ScoreComposer.Composer.GetDrawableNotes"/> and will not be judged.
        Fake,

        /// A note with Normal visibility will be included in <see cref="Courel.ScoreComposer.Composer.GetDrawableNotes"/> and will be judged normally.
        Normal,
    }
}

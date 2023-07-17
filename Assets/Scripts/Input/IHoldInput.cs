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

namespace Courel.Input
{
    /// <summary>
    /// An interface to retrieve the hold state of the input.
    /// </summary>
    public interface IHoldInput
    {
        /// <summary>
        /// Check if the input of a lane is held.
        /// </summary>
        /// <param name="lane">Target lane</param>
        /// <returns> True if the input of the lane is held, false otherwise.</returns>
        public bool IsHeld(int lane);
    }
}

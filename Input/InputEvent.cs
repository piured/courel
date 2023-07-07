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
    /// Input events that notes can react to.
    /// </summary>
    public enum InputEvent
    {
        /// A note reacting to a Tap event will be judged after <see cref="Courel.Sequencer.Tap"/> is called.
        Tap,

        /// A note reacting to a Hold event will be judged according to the <see cref="Courel.IHoldInput"/> state.
        Hold,

        /// A note reacting to a Lift event will be judged after <see cref="Courel.Sequencer.Lift"/> is called.
        Lift,
    }
}

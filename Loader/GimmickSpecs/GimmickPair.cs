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

namespace Courel.Loader.GimmickSpecs
{
    /// <summary>
    /// A beat and value pair for a gimmick.
    /// </summary>
    public class GimmickPair
    {
        /// <summary>
        /// The beat at which the gimmick is triggered.
        /// </summary>
        public double Beat;

        /// <summary>
        /// The value of the gimmick at the beat. Values are gimmick-specific.
        /// </summary>
        public double Value;

        /// <summary>
        /// Creates a new gimmick pair.
        /// </summary>
        /// <param name="beat"></param>
        /// <param name="value"></param>
        public GimmickPair(double beat, double value)
        {
            Beat = beat;
            Value = value;
        }
    }
}

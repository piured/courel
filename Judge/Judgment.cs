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


namespace Courel.Judge
{
    /// <summary>
    /// Base class for judgments.
    /// </summary>
    public class Judgment
    {
        /// <summary>
        /// True if note is missed, false otherwise.
        /// </summary>
        public bool Miss;

        /// <summary>
        /// True if note is premature, false otherwise.
        /// </summary>
        public bool Premature;

        /// <summary>
        /// Initializes a new instance of the <see cref="Courel.Judge.Judgment"/> class.
        /// </summary>
        /// <param name="miss">See property of class</param>
        /// <param name="premature">See property of class</param>
        public Judgment(bool miss, bool premature)
        {
            Miss = miss;
            Premature = premature;
        }
    }
}

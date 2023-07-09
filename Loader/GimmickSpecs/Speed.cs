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
    /// Span of time for a speed gimmick
    /// </summary>
    public enum SpanTimeType
    {
        /// The span of time is in seconds.
        Seconds,

        /// The span of time is in beats.
        Beats,
    }

    /// <summary>
    /// A speed gimmick.
    /// </summary>
    public class Speed
    {
        /// <summary>
        /// The beat at which the speed gimmick is triggered.
        /// </summary>
        public double Beat;

        ///  <summary>
        /// New speed value
        /// </summary>
        public double Value;

        /// <summary>
        /// The span of time for the speed transition. Only positive values are allowed, including 0.
        /// </summary>
        public double SpanTime;

        /// <summary>
        /// The type of span time. See <see cref="Courel.Loader.GimmickSpecs.SpanTimeType"/>.
        /// </summary>
        public SpanTimeType SpanTimeType;
    }
}

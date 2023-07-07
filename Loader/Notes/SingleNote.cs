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
    /// A generic single note. Single notes are notes that are actioned and judged just once.
    /// </summary>
    public abstract class SingleNote : Note
    {
        // beat when the note should be hit w.r.t. the first beat.
        private double _beat;

        // when should it be actioned. Single notes only have 1 V value.
        private double _v;

        // relative position. Single notes only have 1 W value.
        private double _w;

        // combo contribution (from COMBOS segments)
        private int _combo;

        /// <summary>
        /// Initializes a new instance of the <see cref="Courel.Loader.Notes.SingleNote"/> class.
        /// </summary>
        /// <param name="beat"> Beat at which the note should be actioned.</param>
        /// <param name="lane"> Lane index the note belongs to.</param>
        /// <param name="visibility"> Note visibility.</param>
        protected SingleNote(double beat, int lane, Visibility visibility)
            : base(lane, visibility)
        {
            _beat = beat;
        }

        public override double BeginBeat()
        {
            return _beat;
        }

        public override double EndBeat()
        {
            return _beat;
        }

        public override double WBegin()
        {
            return _w;
        }

        public override double WEnd()
        {
            return _w;
        }

        public override double VBegin()
        {
            return _v;
        }

        public override double VEnd()
        {
            return _v;
        }

        /// <summary>
        /// Sets the beat at which the note should be actioned.
        /// </summary>
        /// <param name="beat"> Beat at which the note should be actioned.</param>
        public void SetBeat(double beat)
        {
            _beat = beat;
        }

        /// <summary>
        /// Sets the V timeStamp at which the note should be actioned.
        /// </summary>
        /// <param name="v"></param>
        public void SetV(double v)
        {
            _v = v;
        }

        /// <summary>
        /// Sets the W position at which the note should be actioned.
        /// </summary>
        /// <param name="w"></param>
        public void SetW(double w)
        {
            _w = w;
        }

        /// <summary>
        /// Sets the combo contribution of the note.
        /// </summary>
        /// <param name="combo"> Combo contribution.</param>
        public void SetCombo(int combo)
        {
            _combo = combo;
        }

        /// <summary>
        /// Gets the combo contribution of the note.
        /// </summary>
        /// <returns></returns>
        public int GetCombo()
        {
            return _combo;
        }
    }
}

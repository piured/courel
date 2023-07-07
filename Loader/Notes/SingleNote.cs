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
    public abstract class SingleNote : Note
    {
        // beat when the note should be hit w.r.t. the first beat.
        private double _beat;

        // when should it be actioned
        private double _v;

        // relative position
        private double _w;

        // combo contribution (from COMBOS segments)
        private int _combo;

        public SingleNote(double beat, int lane, Visibility visibility)
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

        public void SetBeat(double beat)
        {
            _beat = beat;
        }

        public void SetV(double v)
        {
            _v = v;
        }

        public void SetW(double w)
        {
            _w = w;
        }

        public void SetCombo(int combo)
        {
            _combo = combo;
        }

        public int GetCombo()
        {
            return _combo;
        }
    }
}

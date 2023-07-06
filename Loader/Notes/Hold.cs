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


namespace Courel
{
    namespace Loader
    {
        public abstract class Hold : Note
        {
            // beat when the note should be hit w.r.t. the first beat.
            private double _beginBeat;

            // beat when a hold (Ddr-style, Piu-style or Roll) can be released.
            private double _endBeat;

            // when should it be tapped
            private double _vBegin;
            private double _vEnd;

            // relative position
            // this wBegin might be changed during runtime (e.g. when the hold is held)
            private double _wBegin;
            private double _wEnd;

            // original back-up wBegin for rollingback
            private double _originalWBegin;

            private bool _isHeld;

            public Hold(double beginBeat, double endBeat, int lane, Visibility visibility)
                : base(lane, visibility)
            {
                _beginBeat = beginBeat;
                _endBeat = endBeat;
            }

            public override void ResetNote()
            {
                base.ResetNote();
                _isHeld = false;
                _wBegin = _originalWBegin;
            }

            public override double BeginBeat()
            {
                return _beginBeat;
            }

            public bool IsHeld()
            {
                return _isHeld;
            }

            public virtual void SetHeld(bool held, double currentSongTime)
            {
                _isHeld = held;
            }

            public override double EndBeat()
            {
                return _endBeat;
            }

            public override double VBegin()
            {
                return _vBegin;
            }

            public override double VEnd()
            {
                return _vEnd;
            }

            public override double WBegin()
            {
                return _wBegin;
            }

            public override double WEnd()
            {
                return _wEnd;
            }

            public void SetBeginBeat(double beginBeat)
            {
                _beginBeat = beginBeat;
            }

            public void SetEndBeat(double endBeat)
            {
                _endBeat = endBeat;
            }

            public virtual void SetVBegin(double v)
            {
                _vBegin = v;
            }

            public void SetVEnd(double v)
            {
                _vEnd = v;
            }

            public void SetWBegin(double w)
            {
                _wBegin = w;
            }

            public void SetOriginalWBegin(double w)
            {
                _originalWBegin = w;
            }

            public void SetWEnd(double w)
            {
                _wEnd = w;
            }

            public abstract bool IsActive();
            public abstract void SetActive(bool active);
        }
    }
}

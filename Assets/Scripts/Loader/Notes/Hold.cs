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
    /// A note that is actioned during a span of beats.
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="Courel.Loader.Notes.Hold"/> class.
        /// </summary>
        /// <param name="beginBeat"> Beat at which the hold starts.</param>
        /// <param name="endBeat"> Beat at which the hold ends.</param>
        /// <param name="lane"> Lane index the hold belongs to.</param>
        /// <param name="visibility"> Hold visibility.</param>
        protected Hold(double beginBeat, double endBeat, int lane, Visibility visibility)
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

        /// <summary>
        /// Checks if the hold is currently held.
        /// </summary>
        /// <returns></returns>
        public bool IsHeld()
        {
            return _isHeld;
        }

        /// <summary>
        /// Sets the hold to be held or not.
        /// </summary>
        /// <param name="held"> True if the hold is held, false otherwise.</param>
        /// <param name="currentSongTime"> Current song time.</param>
        public virtual void SetHeld(bool held, double currentSongTime)
        {
            _isHeld = held;
        }

        /// <summary>
        /// Gets the beat at which the hold ends.
        /// </summary>
        /// <returns></returns>
        public override double EndBeat()
        {
            return _endBeat;
        }

        /// <summary>
        /// Gets the beat at which the hold starts.
        /// </summary>
        /// <returns></returns>
        public override double VBegin()
        {
            return _vBegin;
        }

        /// <summary>
        /// Gets the V timeStamp at which the hold ends.
        /// </summary>
        /// <returns></returns>
        public override double VEnd()
        {
            return _vEnd;
        }

        /// <summary>
        /// Gets the W position at which the hold starts.
        /// </summary>
        /// <returns></returns>
        public override double WBegin()
        {
            return _wBegin;
        }

        /// <summary>
        /// Gets the W position at which the hold ends.
        /// </summary>
        /// <returns></returns>
        public override double WEnd()
        {
            return _wEnd;
        }

        /// <summary>
        /// Gets the beat at which the hold starts.
        /// </summary>
        /// <param name="beginBeat"></param>
        public void SetBeginBeat(double beginBeat)
        {
            _beginBeat = beginBeat;
        }

        /// <summary>
        /// Sets the beat at which the hold ends.
        /// </summary>
        /// <param name="endBeat"></param>
        public void SetEndBeat(double endBeat)
        {
            _endBeat = endBeat;
        }

        /// <summary>
        /// Sets the V timeStamp at which the hold starts.
        /// </summary>
        /// <param name="v"></param>
        public virtual void SetVBegin(double v)
        {
            _vBegin = v;
        }

        /// <summary>
        ///  Sets the V timeStamp at which the hold ends.
        /// </summary>
        /// <param name="v"></param>
        public void SetVEnd(double v)
        {
            _vEnd = v;
        }

        /// <summary>
        /// Sets the W position at which the hold starts.
        /// </summary>
        /// <param name="w"></param>
        public void SetWBegin(double w)
        {
            _wBegin = w;
        }

        /// <summary>
        /// Sets the original W position at which the hold starts.
        /// </summary>
        /// <param name="w"></param>
        public void SetOriginalWBegin(double w)
        {
            _originalWBegin = w;
        }

        /// <summary>
        /// Sets the W position at which the hold ends.
        /// </summary>
        /// <param name="w"></param>
        public void SetWEnd(double w)
        {
            _wEnd = w;
        }

        /// <summary>
        /// Checks if the hold is active. A note will be judged or notified only if it is active.
        /// </summary>
        /// <returns></returns>
        public abstract bool IsActive();

        /// <summary>
        /// Sets the hold to be active or not.
        /// </summary>
        /// <param name="active"> True if the hold is active, false otherwise.</param>
        public abstract void SetActive(bool active);
    }
}

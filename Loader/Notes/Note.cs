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


namespace Courel.Loader
{
    using Input;
    using Judge;

    public abstract class Note
    {
        // lane where the note belongs to
        private int _lane;
        private Judgment _judgment;
        private bool _notified;
        private Visibility _visibility;

        public Note(int lane, Visibility visibility)
        {
            _lane = lane;
            _visibility = visibility;
            ResetNote();
        }

        public virtual void ResetNote()
        {
            _judgment = null;
            _notified = false;
        }

        public int Lane()
        {
            return _lane;
        }

        public void SetJudgment(Judgment judgment)
        {
            _judgment = judgment;
        }

        public Judgment Judgment()
        {
            return _judgment;
        }

        public bool HasBeenJudged()
        {
            return _judgment != null;
        }

        public bool HasBeenNotified()
        {
            return _notified;
        }

        public void SetNotified(bool notified)
        {
            _notified = notified;
        }

        public Visibility GetVisibility()
        {
            return _visibility;
        }

        public abstract double BeginBeat();
        public abstract double EndBeat();
        public abstract double WBegin();
        public abstract double WEnd();
        public abstract double VBegin();
        public abstract double VEnd();

        public abstract bool ReactsTo(InputEvent inputEvent);
    }
}

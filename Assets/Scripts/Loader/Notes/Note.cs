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
    using Input;
    using Judge;

    /// <summary>
    /// A generic note that can be placed in a <see cref="Courel.ScoreComposer.Score"/>.
    /// </summary>
    public abstract class Note
    {
        // lane at which the note belongs to
        private int _lane;

        // judgment if the note has been judged
        private Judgment _judgment;

        // whether the note has been notified to subscribers
        private bool _notified;

        // visibility of the note
        private Visibility _visibility;

        /// <summary>
        /// Initializes a new instance of the <see cref="Courel.Loader.Notes.Note"/> class.
        /// </summary>
        /// <param name="lane">Lane index the note belongs to.</param>
        /// <param name="visibility">Note visibility.</param>
        public Note(int lane, Visibility visibility)
        {
            _lane = lane;
            _visibility = visibility;
            ResetNote();
        }

        /// <summary>
        /// Resets the note to default values, e.g. when rolling back notes.
        /// </summary>
        public virtual void ResetNote()
        {
            _judgment = null;
            _notified = false;
        }

        /// <summary>
        /// Gets the lane index the note belongs to.
        /// </summary>
        /// <returns></returns>
        public int Lane()
        {
            return _lane;
        }

        /// <summary>
        /// Sets the judgment of the note.
        /// </summary>
        /// <param name="judgment"> Judgment to set.</param>
        public void SetJudgment(Judgment judgment)
        {
            _judgment = judgment;
        }

        /// <summary>
        /// Gets the judgment of the note.
        /// </summary>
        /// <returns>Returns the judgment of the note. If the note has not been judged, returns null.</returns>
        public Judgment Judgment()
        {
            return _judgment;
        }

        /// <summary>
        /// Checks if the note has been judged.
        /// </summary>
        /// <returns></returns>
        public bool HasBeenJudged()
        {
            return _judgment != null;
        }

        /// <summary>
        /// Checks if the note has been notified to subscribers.
        /// </summary>
        /// <returns></returns>
        public bool HasBeenNotified()
        {
            return _notified;
        }

        /// <summary>
        /// Sets the notified state of the note.
        /// </summary>
        /// <param name="notified"></param>
        public void SetNotified(bool notified)
        {
            _notified = notified;
        }

        /// <summary>
        /// Gets the visibility of the note.
        /// </summary>
        /// <returns></returns>
        public Visibility GetVisibility()
        {
            return _visibility;
        }

        /// <summary>
        /// Gets the beat at which the note begins.
        /// </summary>
        /// <returns></returns>
        public abstract double BeginBeat();

        /// <summary>
        /// Gets the beat at which the note ends.
        /// </summary>
        /// <returns></returns>
        public abstract double EndBeat();

        /// <summary>
        /// Gets the beginning W position of the note.
        /// </summary>
        /// <returns></returns>
        public abstract double WBegin();

        /// <summary>
        /// Gets the ending W position of the note.
        /// </summary>
        /// <returns></returns>
        public abstract double WEnd();

        /// <summary>
        /// Gets the beginning V timeStamp of the note.
        /// </summary>
        /// <returns></returns>
        public abstract double VBegin();

        /// <summary>
        /// Gets the ending V timeStamp of the note.
        /// </summary>
        /// <returns></returns>
        public abstract double VEnd();

        /// <summary>
        /// Checks if the note reacts to <paramref name="inputEvent"/>.
        /// </summary>
        /// <param name="inputEvent"> Input event to check.</param>
        /// <returns></returns>
        public abstract bool ReactsTo(InputEvent inputEvent);
    }
}

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



using Courel.Loader;

namespace Courel.Subscription
{
    using ScoreComposer;
    using Loader.Notes;

    /// <summary>
    /// An interface to subscribe to events produced <see cref="Courel.Sequencer"/> during runtime.
    /// </summary>
    public interface ISubscriber
    {
        /// <summary>
        /// This method is called when all notes in a row are missed.
        /// </summary>
        /// <param name="row">Row containing <see cref="Courel.Loader.Notes.SingleNote"/>s that have been judged as miss.</param>
        public void OnMissedSingleNotesOnRow(Row row);

        /// <summary>
        /// This method is called when the action time of notes in a row is equal to the song time. In other words,
        /// when the notes are expected to be actioned, this method is called.
        /// </summary>
        /// <param name="row"> Row containing <see cref="Courel.Loader.Notes.SingleNote"/>s that are expected to be actioned.</param>
        public void OnHoveringReceptorSingleNotes(Row row);

        /// <summary>
        /// This method is called when a <see cref="Courel.Loader.Notes.Hold"/> is active and in action range.
        /// </summary>
        /// <param name="note"></param>
        /// <param name="held"></param>
        public void OnActiveHold(Hold note, bool held);

        public void OnHoldEnded(Hold note, bool held);

        public void OnJudgedSingleNoteOnRow(SingleNote note, Row row);

        public void OnHoldInactive(Hold note);
        public void OnHoldEndJudged(Hold note);
        public void OnRolledBackSingleNoteRow(Row row);
        public void OnHoldIsPartiallyRolledBack(Hold note);
        public void OnHoldIsRolledBack(Hold note);
    }
}

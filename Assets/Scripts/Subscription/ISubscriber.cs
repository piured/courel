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
    /// An interface to subscribe to events produced <see cref="Courel.Sequencer"/> during runtime. See the wiki for more information.
    /// </summary>
    public interface ISubscriber
    {
        /// <summary>
        /// This method is called when at least one note in a row is judged as miss.
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
        /// This method is called at every update when a <see cref="Courel.Loader.Notes.Hold"/> is active and in action range.
        /// </summary>
        /// <param name="note">Target active hold note and in action range.</param>
        /// <param name="held">True if the hold is held, false otherwise.</param>
        public void OnActiveHold(Hold note, bool held);

        /// <summary>
        /// This method is called when a <see cref="Courel.Loader.Notes.Hold"/> is active and has reached its end.
        /// </summary>
        /// <param name="note">Targe active hold.</param>
        /// <param name="held">True if the hold is held at the end of its life, false otherwise.</param>
        public void OnHoldEnded(Hold note, bool held);

        /// <summary>
        /// This method is called when a <see cref="Courel.Loader.Notes.SingleNote"/> is judged due to an input event.
        /// </summary>
        /// <param name="note">Judged note.</param>
        /// <param name="row">Row containing the note that has been judged.</param>
        public void OnJudgedSingleNoteOnRow(SingleNote note, Row row);

        /// <summary>
        /// This method is called when a <see cref="Courel.Loader.Notes.Hold"/> becomes inactive.
        /// </summary>
        /// <param name="note"> Target inactive hold.</param>
        public void OnHoldInactive(Hold note);

        /// <summary>
        /// This method is called when an active <see cref="Courel.Loader.Notes.Hold"/> is judged at the end of its life.
        /// </summary>
        /// <param name="note">Judged active hold.</param>
        public void OnHoldEndJudged(Hold note);

        /// <summary>
        /// This method is called when a row of <see cref="Courel.Loader.Notes.SingleNote"/> is rolled back.
        /// </summary>
        /// <param name="row">Rolled back row.</param>
        public void OnRolledBackSingleNotesOnRow(Row row);

        /// <summary>
        /// This method is called when a <see cref="Courel.Loader.Notes.Hold"/> is partially rolled back, becoming active and in action range.
        /// </summary>
        /// <param name="note">Partially rolled back hold.</param>
        public void OnHoldIsPartiallyRolledBack(Hold note);

        /// <summary>
        /// This method is called when a <see cref="Courel.Loader.Notes.Hold"/> is rolled back completely, becoming active and not in action range.
        /// </summary>
        /// <param name="note"></param>
        public void OnHoldIsRolledBack(Hold note);
    }
}

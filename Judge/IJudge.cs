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
    using Loader.Notes;

    /// <summary>
    /// This interface is used by <see cref="Courel.Sequencer"/> to judge notes.
    /// </summary>
    public interface IJudge
    {
        // Evaluates only if note is miss or premature

        /// <summary>
        /// Check if note is miss.
        /// </summary>
        /// <param name="delta">Delta time (in seconds) between the note's action time $v$ and the current song time $t$.</param>
        /// <param name="note">Note to be judged.</param>
        /// <returns>
        /// If note is miss, return new instance of <see cref="Courel.Judge.Judgment"/> with <see cref="Courel.Judge.Judgment.Miss"/> set to true.
        /// </returns>
        public Judgment IsMiss(float delta, SingleNote note);

        // Evaluates note of tap event (Tap method of RunTimeResolver) (only notes that react to taps are passed as argument) w.r.t. delta. Return Judgment.Premature if premature.

        /// <summary>
        /// Judges <see cref="Courel.Loader.Notes.SingleNote"/> reacting to Tap events.
        /// </summary>
        /// <param name="delta">Delta time (in seconds) between the note's action time $v$ and the song time when the tap event was produced.</param>
        /// <param name="note">Note to be judged.</param>
        /// <returns>
        /// If note is premature, return new instance of <see cref="Courel.Judge.Judgment"/> with <see cref="Courel.Judge.Judgment.Premature"/> set to true.
        /// If note is miss, return new instance of <see cref="Courel.Judge.Judgment"/> with <see cref="Courel.Judge.Judgment.Miss"/> set to true.
        /// If note is not premature nor miss, return new instance of <see cref="Courel.Judge.Judgment"/> with both <see cref="Courel.Judge.Judgment.Miss"/> and <see cref="Courel.Judge.Judgment.Premature"/> set to false. The sequencer
        /// will assume that the note was hit.
        /// Return null if you want to ignore the judgment of this tap event. The sequencer will ask this note to be judged again for the next tap event if note is not missed.
        /// </returns>
        public Judgment EvalTapEvent(float delta, Note note);

        /// <summary>
        /// Judges <see cref="Courel.Loader.Notes.SingleNote"/> reacting to Hold events.
        /// </summary>
        /// <param name="delta">Delta time (in seconds) between the note's action time $v$ and the song time when the tap event was produced.</param>
        /// <param name="note">Note to be judged.</param>
        /// <returns>
        /// If note is premature, return new instance of <see cref="Courel.Judge.Judgment"/> with <see cref="Courel.Judge.Judgment.Premature"/> set to true.
        /// If note is miss, return new instance of <see cref="Courel.Judge.Judgment"/> with <see cref="Courel.Judge.Judgment.Miss"/> set to true.
        /// If note is not premature nor miss, return new instance of <see cref="Courel.Judge.Judgment"/> with both <see cref="Courel.Judge.Judgment.Miss"/> and <see cref="Courel.Judge.Judgment.Premature"/> set to false. The sequencer
        /// will assume that the note was hit.
        /// Return null if you want to ignore the judgment of this tap event. The sequencer will ask this note to be judged again for the next tap event if note is not missed.
        /// </returns>
        public Judgment EvalHoldEvent(float delta, Note note);
        public Judgment EvalLiftEvent(float delta, Note note);

        public bool ShouldHoldBeActive(Hold hold);

        public Judgment EvalEndHoldEvent(float delta, Hold hold);
    }
}

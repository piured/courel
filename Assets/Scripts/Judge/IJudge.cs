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
        /// Check if <see cref="Courel.Loader.Notes.SingleNote"/> is miss.
        /// </summary>
        /// <param name="delta">Delta time (in seconds) between the note's action time $v$ and the current song time $t$.</param>
        /// <param name="note">Note to be judged.</param>
        /// <returns>
        /// If note is miss, return new instance of <see cref="Courel.Judge.Judgment"/> with <see cref="Courel.Judge.Judgment.Miss"/> set to true.
        /// </returns>
        public Judgment IsMiss(float delta, SingleNote note);

        // Evaluates note of tap event (Tap method of RunTimeResolver) (only notes that react to taps are passed as argument) w.r.t. delta. Return Judgment.Premature if premature.

        /// <summary>
        /// Judges <see cref="Courel.Loader.Notes.TapNote"/>
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
        public Judgment EvalTapEvent(float delta, TapNote note);

        /// <summary>
        /// Judges <see cref="Courel.Loader.Notes.HoldNote"/>.
        /// </summary>
        /// <param name="delta">Delta time (in seconds) between the note's action time $v$ and the song time when the hold event was produced.</param>
        /// <param name="note">Note to be judged.</param>
        /// <returns>
        /// If note is premature, return new instance of <see cref="Courel.Judge.Judgment"/> with <see cref="Courel.Judge.Judgment.Premature"/> set to true.
        /// If note is miss, return new instance of <see cref="Courel.Judge.Judgment"/> with <see cref="Courel.Judge.Judgment.Miss"/> set to true.
        /// If note is not premature nor miss, return new instance of <see cref="Courel.Judge.Judgment"/> with both <see cref="Courel.Judge.Judgment.Miss"/> and <see cref="Courel.Judge.Judgment.Premature"/> set to false. The sequencer
        /// will assume that the note was hit.
        /// Return null if you want to ignore the judgment of this hold event. The sequencer will ask this note to be judged again for the next hold event if note is not missed.
        /// </returns>
        public Judgment EvalHoldEvent(float delta, HoldNote note);

        /// <summary>
        /// Judges <see cref="Courel.Loader.Notes.LiftNote"/>.
        /// </summary>
        /// <param name="delta">Delta time (in seconds) between the note's action time $v$ and the song time when the lift event was produced.</param>
        /// <param name="note">Note to be judged.</param>
        /// <returns>
        /// If note is premature, return new instance of <see cref="Courel.Judge.Judgment"/> with <see cref="Courel.Judge.Judgment.Premature"/> set to true.
        /// If note is miss, return new instance of <see cref="Courel.Judge.Judgment"/> with <see cref="Courel.Judge.Judgment.Miss"/> set to true.
        /// If note is not premature nor miss, return new instance of <see cref="Courel.Judge.Judgment"/> with both <see cref="Courel.Judge.Judgment.Miss"/> and <see cref="Courel.Judge.Judgment.Premature"/> set to false. The sequencer
        /// will assume that the note was hit.
        /// Return null if you want to ignore the judgment of this lift event. The sequencer will ask this note to be judged again for the next lift event if note is not missed.
        /// </returns>
        public Judgment EvalLiftEvent(float delta, LiftNote note);

        /// <summary>
        /// Evaluates if a hold should be active.
        /// </summary>
        /// <param name="hold">Hold to be evaluated</param>
        /// <returns>Return true if hold should be active, false otherwise.</returns>
        public bool ShouldHoldBeActive(Hold hold);

        /// <summary>
        /// Judges the end of a <see cref="Courel.Loader.Notes.Hold"/>.
        /// </summary>
        /// <param name="delta">Delta time (in seconds) between the end holds's action time $v$ and the song time when the end hold event was produced.</param>
        /// <param name="hold">Hold to be judged.</param>
        /// <returns>
        /// If note is premature, return new instance of <see cref="Courel.Judge.Judgment"/> with <see cref="Courel.Judge.Judgment.Premature"/> set to true.
        /// If note is miss, return new instance of <see cref="Courel.Judge.Judgment"/> with <see cref="Courel.Judge.Judgment.Miss"/> set to true.
        /// If note is not premature nor miss, return new instance of <see cref="Courel.Judge.Judgment"/> with both <see cref="Courel.Judge.Judgment.Miss"/> and <see cref="Courel.Judge.Judgment.Premature"/> set to false. The sequencer
        /// will assume that the hold end was hit.
        /// You are not allowed to return null.
        public Judgment EvalEndHoldEvent(float delta, Hold hold);
    }
}

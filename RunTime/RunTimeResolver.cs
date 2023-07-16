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

using UnityEngine;
using System.Collections.Generic;

namespace Courel.RunTime
{
    using System;
    using Loader.Notes;
    using ScoreComposer;
    using ScoreComposer.Views;
    using Input;
    using Judge;
    using Subscription;

    public class RunTimeResolver
    {
        private IJudge _judge;
        private Score _singleNoteScore;
        private Score _holdScore;
        private LanesView _activeHoldsView;
        private ScoreView _judgingNotesView;
        private ScoreView _hoveringNotesView;
        private Notifier _notifier;

        IHoldInput _holdInput;
        IHoldInput _autoPlayInput = new AutoPlayInput();

        SongTimeDirection songTimeDirection = new SongTimeDirection();
        private bool _autoPlay;

        // this bool array is used to check whether a hold has been rolled back partially.
        // We have one per lane.
        private bool[] _shouldHoldPartiallyRollBack;

        public RunTimeResolver(
            IJudge iJudge,
            Score singleNoteScore,
            Score holdScore,
            Notifier notifier,
            IHoldInput holdInput
        )
        {
            _judge = iJudge;
            _holdInput = holdInput;
            _singleNoteScore = singleNoteScore;
            _holdScore = holdScore;
            _judgingNotesView = singleNoteScore.CreateView();
            _hoveringNotesView = singleNoteScore.CreateView();
            _activeHoldsView = _holdScore.CreateLanesView();
            _notifier = notifier;
            _shouldHoldPartiallyRollBack = new bool[_holdScore.NumberOfLanes];
        }

        public void ResolveAndNotify(float currentSongTime)
        {
            Direction direction = songTimeDirection.GetDirection(currentSongTime);
            if (direction == Direction.Forward)
            {
                ResolveAndNotifyForward(currentSongTime);
            }
            else
            {
                ResolveAndNotifyBackward(currentSongTime);
            }
        }

        private void ResolveAndNotifyBackward(float currentSongTime)
        {
            RollBackActiveHoldsView(currentSongTime);
            RollBackScoreView(_judgingNotesView, currentSongTime, true);
            RollBackScoreView(_hoveringNotesView, currentSongTime, false);
        }

        private void RollBackActiveHoldsView(float currentSongTime)
        {
            int numberOfLanes = _holdScore.NumberOfLanes;
            for (int i = 0; i < numberOfLanes; i++)
            {
                LaneView laneView = _activeHoldsView.GetLane(i);
                LaneItem laneItem;
                if (!_shouldHoldPartiallyRollBack[i])
                {
                    laneItem = laneView.GetPrior();
                }
                else
                {
                    laneItem = laneView.GetFirst();
                }
                // LaneItem laneItem = laneView.GetPrior();
                while (laneItem != null)
                {
                    double vend = laneItem.Note.VEnd();
                    double vbegin = laneItem.Note.VBegin();
                    double vendDelta = currentSongTime - vend;
                    double vbeginDelta = currentSongTime - vbegin;
                    if (vendDelta < 0.0f && vbeginDelta >= 0.0f)
                    {
                        // just one time until roll has been rolled back completely
                        if (!_shouldHoldPartiallyRollBack[i])
                        {
                            laneView.RollBackOne();
                            _shouldHoldPartiallyRollBack[i] = true;
                        }
                        _notifier.NotifyHoldIsPartiallyRolledBack((Hold)laneItem.Note);
                        break;
                    }
                    else if (vbeginDelta < 0.0f)
                    {
                        if (!_shouldHoldPartiallyRollBack[i])
                        {
                            laneView.RollBackOne();
                        }
                        _shouldHoldPartiallyRollBack[i] = false;
                        _notifier.NotifyHoldIsRolledBack((Hold)laneItem.Note);
                        laneItem.Note.ResetNote();
                        laneItem = laneView.GetPrior();
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private void RollBackScoreView(
            ScoreView scoreView,
            float currentSongTime,
            bool shouldNotify
        )
        {
            Row row = scoreView.GetPriorRow();
            while (row != null)
            {
                RowItem rowItem = row.GetFirst();
                double vend = rowItem.Note.VEnd();
                double delta = currentSongTime - vend;
                if (delta < 0.0f)
                {
                    row.ResetNotes();
                    scoreView.RollBackOne();
                    if (shouldNotify)
                    {
                        _notifier.NotifyRolledBackSingleNoteRow(row);
                    }
                    row = scoreView.GetPriorRow();
                }
                else
                {
                    break;
                }
            }
        }

        private void ResolveAndNotifyForward(float currentSongTime)
        {
            CheckForActiveHolds(currentSongTime);
            JudgeHoldSingleNotes(currentSongTime);
            CheckForJudgedNotesAndRows();
            CheckMissedSingleNotes(currentSongTime);
            CheckHoveringReceptorNotes(currentSongTime);
        }

        private void CheckMissedSingleNotes(float songTime)
        {
            Row row = _judgingNotesView.GetFirstRow();
            while (row != null)
            {
                RowItem rowItem = row.GetFirst();
                double vbegin = rowItem.Note.VBegin();
                Judgment judgment = _judge.IsMiss(
                    songTime - (float)vbegin,
                    (SingleNote)rowItem.Note
                );
                if (judgment.Miss)
                {
                    AssignJudgmentToRow(row, judgment);
                    _notifier.NotifyMissedSingleNotes(row);
                    _judgingNotesView.RemoveFirstRow();
                    row = _judgingNotesView.GetFirstRow();
                }
                else
                {
                    break;
                }
            }
        }

        private void AssignJudgmentToRow(Row row, Judgment judgment)
        {
            foreach (var rowItem in row.GetAll())
            {
                rowItem.Note.SetJudgment(judgment);
            }
        }

        private void CheckHoveringReceptorNotes(float songTime)
        {
            Row row = _hoveringNotesView.GetFirstRow();
            ;
            while (row != null)
            {
                RowItem rowItem = row.GetFirst();
                double vbegin = rowItem.Note.VBegin();
                bool hovering = songTime - (float)vbegin > 0.0f;
                if (hovering)
                {
                    _notifier.NotifyHoveringReceptorSingleNotes(row);
                    _hoveringNotesView.RemoveFirstRow();
                    if (_autoPlay)
                    {
                        TapAllNotesInRowAutoPlay(row);
                    }
                    row = _hoveringNotesView.GetFirstRow();
                }
                else
                {
                    break;
                }
            }
        }

        private void TapAllNotesInRowAutoPlay(Row row)
        {
            foreach (var rowItem in row.GetAll())
            {
                Judgment judgment = null;
                if (rowItem.Note is TapNote tapNote)
                {
                    judgment = _judge.EvalTapEvent(0.0f, tapNote);
                }
                else if (rowItem.Note is LiftNote liftNote)
                {
                    judgment = _judge.EvalLiftEvent(0.0f, liftNote);
                }
                else
                {
                    judgment = _judge.EvalHoldEvent(0.0f, (HoldNote)rowItem.Note);
                }
                rowItem.Note.SetJudgment(judgment);
            }
        }

        private void CheckForJudgedNotesAndRows()
        {
            int numberOfLanes = _singleNoteScore.NumberOfLanes;
            for (int i = 0; i < numberOfLanes; i++)
            {
                bool flag;
                do
                {
                    Row row = _judgingNotesView.GetRowOfFirstNoteAtLane(i);
                    flag = false;
                    if (row != null)
                    {
                        flag = CheckForJudgedNotesAndRowAtRow(row);
                    }
                } while (flag);
            }
        }

        // Returns true if a row is removed, otherwise false.
        private bool CheckForJudgedNotesAndRowAtRow(Row row)
        {
            List<RowItem> rowItems = row.GetAll();
            int countNoteJudged = 0;
            foreach (var rowItem in rowItems)
            {
                var note = rowItem.Note;
                if (note.HasBeenJudged())
                {
                    countNoteJudged++;
                    if (!note.HasBeenNotified())
                    {
                        note.SetNotified(true);
                        _notifier.NotifyJudgedSingleNoteOnRow((SingleNote)note, row);
                    }
                }
            }
            if (countNoteJudged == rowItems.Count)
            {
                _judgingNotesView.RemoveRowAndtLaneItems(row);
                return true;
            }
            return false;
        }

        public void Tap(int lane, float currentSongTime)
        {
            JudgeTapNote(lane, currentSongTime);
        }

        private void JudgeHoldSingleNotes(float currentSongTime)
        {
            int numberOfLanes = _singleNoteScore.NumberOfLanes;
            for (int i = 0; i < numberOfLanes; i++)
            {
                if (_holdInput.IsHeld(i))
                {
                    Judgment judgment = null;
                    // For really high tickcounts/low fps
                    int j = 0;
                    while (true)
                    {
                        var note = _judgingNotesView.GetIthNoteAtLane(i, j);
                        judgment = null;
                        if (note != null)
                        {
                            judgment = JudgeNote<HoldNote>(
                                currentSongTime,
                                note,
                                InputEvent.Hold,
                                _judge.EvalHoldEvent
                            );
                        }
                        if (judgment == null || judgment.Premature)
                        {
                            break;
                        }
                        else
                        {
                            j++;
                        }
                    }
                }
            }
        }

        private void CheckForActiveHolds(float currentSongTime)
        {
            int numberOfLanes = _holdScore.NumberOfLanes;
            for (int i = 0; i < numberOfLanes; i++)
            {
                LaneView laneView = _activeHoldsView.GetLane(i);
                LaneItem laneItem = laneView.GetFirst();
                if (laneItem != null)
                {
                    var note = (Hold)laneItem.Note;
                    if (note.IsActive())
                    {
                        HandleActiveHold(currentSongTime, i, laneView, note);
                    }
                }
            }
        }

        private void HandleActiveHold(float currentSongTime, int lane, LaneView laneView, Hold note)
        {
            double vbegin = note.VBegin();
            double vend = note.VEnd();
            if (currentSongTime >= vbegin)
            {
                if (currentSongTime <= vend)
                {
                    _shouldHoldPartiallyRollBack[lane] = true;
                    bool shouldBeActive = _judge.ShouldHoldBeActive(note);
                    if (shouldBeActive)
                    {
                        note.SetHeld(_holdInput.IsHeld(lane), currentSongTime);
                        _notifier.NotifyActiveHold(note, _holdInput.IsHeld(lane));
                    }
                    else
                    {
                        note.SetActive(false);
                        _notifier.NotifyHoldIsInactive(note);
                        laneView.RemoveFirst();
                    }
                }
                else
                {
                    _shouldHoldPartiallyRollBack[lane] = false;
                    _notifier.NotifyHoldEnded(note, _holdInput.IsHeld(lane));
                    Judgment judgment = _judge.EvalEndHoldEvent(
                        currentSongTime - (float)vend,
                        note
                    );
                    note.SetJudgment(judgment);
                    if (!judgment.Premature && _holdInput.IsHeld(lane))
                    {
                        _notifier.NotifyHoldEndJudged(note);
                        laneView.RemoveFirst();
                        // next hold
                    }
                    else if (judgment.Miss)
                    {
                        _notifier.NotifyHoldEndJudged(note);
                        laneView.RemoveFirst();
                        // next hold
                    }
                }
            }
        }

        private Judgment JudgeNote<T>(
            float currentSongTime,
            Note note,
            InputEvent inputEvent,
            Func<float, T, Judgment> eval
        )
            where T : Note
        {
            if (!note.HasBeenJudged() && note.ReactsTo(inputEvent))
            {
                double vbegin = note.VBegin();
                Judgment judgment = eval(currentSongTime - (float)vbegin, (T)note);
                if (!judgment.Premature)
                {
                    note.SetJudgment(judgment);
                }
                return judgment;
            }
            return null;
        }

        private void JudgeTapNote(int lane, float currentSongTime)
        {
            var note = _judgingNotesView.GetFirstNoteAtLane(lane);
            if (note != null)
            {
                JudgeNote<TapNote>(currentSongTime, note, InputEvent.Tap, _judge.EvalTapEvent);
            }
        }

        internal void Lift(int lane, float currentSongTime)
        {
            var note = _judgingNotesView.GetFirstNoteAtLane(lane);
            if (note != null)
            {
                JudgeNote<LiftNote>(currentSongTime, note, InputEvent.Lift, _judge.EvalLiftEvent);
            }
        }

        // TODO: This autoplay is not ready for ROLLS, LIFTNOTES; ETC.
        public void SetAutoPlay(bool flag)
        {
            if (flag != _autoPlay)
            {
                IHoldInput aux;
                aux = _autoPlayInput;
                _autoPlayInput = _holdInput;
                _holdInput = aux;
            }
            _autoPlay = flag;
        }

        internal void ChangeInput(IHoldInput holdInput)
        {
            _holdInput = holdInput;
        }

        internal void ChangeJudge(IJudge judge)
        {
            _judge = judge;
        }
    }
}

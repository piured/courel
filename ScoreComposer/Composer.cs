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

using UnityEngine;
using System.Collections.Generic;

namespace Courel.ScoreComposer
{
    using System;
    using Loader;
    using Loader.GimmickSpecs;

    public class Composer
    {
        private List<Note> _notes;
        private List<Note> _drawableNotes = new List<Note>();
        int _numberOfLanes;
        Score _singleNoteScore;
        Score _holdScore;
        private double _skipUntilSecond;
        StatusResolver _statusResolver;

        public Composer(ILoader iLoader, StatusResolver statusResolver)
        {
            _skipUntilSecond = iLoader.SkipUntilSecond();
            _statusResolver = statusResolver;
            _notes = iLoader.GetNotes();
            _numberOfLanes = iLoader.GetNumberOfLanes();
            _singleNoteScore = new Score(_numberOfLanes);
            _holdScore = new Score(_numberOfLanes);

            SortNotesByBeat();
            GenerateInvisibleHoldNotesForPiuStyleHolds();
            GenerateInvisibleHeadForDdrStyleHolds();
            GenerateInvisibleHeadForDdrStyleRolls();
            SortNotesByBeat();
            CreateScores();

            // SetUpNotesRelativePositionAndTapTime(_singleNoteScore);
            // SetUpNotesRelativePositionAndTapTime(_holdScore);

            Debug.Log(_singleNoteScore.Lanes.GetLane(0).GetNoteCount());
        }

        public int GetTotalNumberOfSingleNotes()
        {
            int aggregate = 0;
            for (int i = 0; i < _numberOfLanes; i++)
            {
                aggregate += _singleNoteScore.Lanes.GetLane(i).GetNoteCount();
            }
            return aggregate;
        }

        public int GetTotalNumberOfRows()
        {
            return _singleNoteScore.Rows.GetAll().Count;
        }

        private void GenerateInvisibleHeadForDdrStyleHolds()
        {
            GenerateInvisibleHeadForDdrStyleHold(
                (Note note) =>
                {
                    return note is DdrStyleHold && note is not DdrStyleRollHold;
                }
            );
        }

        private void GenerateInvisibleHeadForDdrStyleRolls()
        {
            GenerateInvisibleHeadForDdrStyleHold(
                (Note note) =>
                {
                    return note is DdrStyleRollHold;
                }
            );
        }

        List<GimmickPair> ConvertBpmsToBpss(List<GimmickPair> bpms)
        {
            List<GimmickPair> bpss = new List<GimmickPair>();
            foreach (var bpm in bpms)
            {
                bpss.Add(new GimmickPair(bpm.Beat, bpm.Value / 60.0));
            }
            return bpss;
        }

        private void GenerateInvisibleHeadForDdrStyleHold(Func<Note, bool> condition)
        {
            List<Note> invisibleHeadFromDdrStyleHolds = new List<Note>();
            foreach (var note in _notes)
            {
                if (condition(note) && note.GetVisibility() != Visibility.Fake)
                {
                    DdrStyleHold ddrStyleHold = (DdrStyleHold)note;
                    var hiddenTapNote = new TapNote(
                        note.BeginBeat(),
                        note.Lane(),
                        Visibility.Hidden
                    );
                    ddrStyleHold.SetHiddenHead(hiddenTapNote);
                    invisibleHeadFromDdrStyleHolds.Add(hiddenTapNote);
                }
            }
            _notes.AddRange(invisibleHeadFromDdrStyleHolds);
        }

        private void GenerateInvisibleHoldNotesForPiuStyleHolds()
        {
            List<Note> holdNotesFromPiuStyleHolds = new List<Note>();
            PiuStyleHiddenHoldNotesAligner piuStyleHiddenHoldNotesAligner =
                new PiuStyleHiddenHoldNotesAligner(_notes);
            foreach (var note in _notes)
            {
                if (note is PiuStyleHold piuStyleHold && note.GetVisibility() != Visibility.Fake)
                {
                    var holdBeats = _statusResolver.GetHoldTapBeats(
                        piuStyleHold.BeginBeat(),
                        note.EndBeat()
                    );

                    List<double> holdBeatsFromUnalignedNotesInHold =
                        piuStyleHiddenHoldNotesAligner.GetHoldBeatsFromUnalignedNotesInHold(
                            piuStyleHold,
                            holdBeats
                        );
                    CreateHiddenHoldNotesFromHoldBeats(
                        holdNotesFromPiuStyleHolds,
                        piuStyleHold,
                        holdBeats
                    );
                    CreateHiddenHoldNotesFromHoldBeats(
                        holdNotesFromPiuStyleHolds,
                        piuStyleHold,
                        holdBeatsFromUnalignedNotesInHold
                    );
                }
            }
            _notes.AddRange(holdNotesFromPiuStyleHolds);
        }

        private static void CreateHiddenHoldNotesFromHoldBeats(
            List<Note> holdNotesFromPiuStyleHolds,
            PiuStyleHold piuStyleHold,
            List<double> holdBeats
        )
        {
            foreach (double holdBeat in holdBeats)
            {
                holdNotesFromPiuStyleHolds.Add(
                    new HoldNote(holdBeat, piuStyleHold.Lane(), Visibility.Hidden)
                );
            }
        }

        private void CreateScores()
        {
            while (_notes.Count > 0)
            {
                List<Note> rowNotes = PullNotesWithSameBeatSignature();
                SetUpRowNotesRelativePositionAndTapTime(rowNotes);
                if (AreRowNotesInSkipInterval(rowNotes))
                {
                    continue;
                }
                if (!IsRowWarpedOver(rowNotes) && !IsRowFaked(rowNotes))
                {
                    PushBackRowIntoScore<SingleNote>(rowNotes, _singleNoteScore);
                }
                PushBackRowIntoScore<Hold>(rowNotes, _holdScore);
            }
        }

        private bool AreRowNotesInSkipInterval(List<Note> rowNotes)
        {
            return rowNotes[0].VEnd() < _skipUntilSecond;
        }

        private bool IsRowWarpedOver(List<Note> stepRow)
        {
            return _statusResolver.IsBeatWarpedOver(stepRow[0].BeginBeat());
        }

        private bool IsRowFaked(List<Note> stepRow)
        {
            return _statusResolver.IsBeatFaked(stepRow[0].BeginBeat());
        }

        private void PushBackRowIntoScore<T>(List<Note> stepRow, Score score)
        {
            Row regularRow = new Row(score.Rows.GetAll().Count);
            bool flag = false;
            foreach (var step in stepRow)
            {
                if (step is T && step.GetVisibility() != Visibility.Fake)
                {
                    PushBackStepIntoScore(regularRow, step, score);
                    flag = true;
                }
            }

            if (flag)
            {
                score.Rows.Add(regularRow);
            }
        }

        private void PushBackStepIntoScore(Row row, Note step, Score score)
        {
            Lane stepLane = score.Lanes.GetLane(step.Lane());
            LaneItem laneItem = new LaneItem(step, row, stepLane.GetAll().Count);
            // new row
            row.Add(new RowItem(step, score.Lanes.GetLane(step.Lane()), laneItem));

            // add to existing lanes
            stepLane.Add(laneItem);
        }

        private List<Note> PullNotesWithSameBeatSignature()
        {
            List<Note> stepRow = new List<Note>();

            double beatInRow = _notes[0].BeginBeat();
            int numberOfStepsRemoved = 0;
            foreach (var note in _notes)
            {
                if (note.BeginBeat() == beatInRow)
                {
                    stepRow.Add(note);
                    numberOfStepsRemoved++;
                    if (note.GetVisibility() != Visibility.Hidden)
                    {
                        _drawableNotes.Add(note);
                    }
                }
                else
                {
                    break;
                }
            }
            // remove from _notes list the notes pulled out.
            _notes.RemoveRange(0, numberOfStepsRemoved);
            return stepRow;
        }

        private void SortNotesByBeat()
        {
            _notes.Sort((x, y) => x.BeginBeat().CompareTo(y.BeginBeat()));
        }

        // private void SetUpNotesRelativePositionAndTapTime(Score score)
        // {
        //     foreach (var row in score.Rows.GetAll())
        //     {
        //         // All notes in the same row have the same beat. THAT IS NOT TRUE!! HOLDS DO NOT HAVE THE SAME EndBEAT!!!!
        //         var rowNote = row.GetAll()[0].Note;
        //         double VBegin = _statusResolver.GetVFromBeat(rowNote.BeginBeat());
        //         double WBegin = _statusResolver.GetWFromBeat(rowNote.BeginBeat());

        //         foreach (var rowItem in row.GetAll())
        //         {
        //             Note note = rowItem.Note;
        //             // HOLDS do not have to have the same EndBeat
        //             double VEnd = _statusResolver.GetVFromBeat(note.EndBeat());
        //             double WEnd = _statusResolver.GetWFromBeat(note.EndBeat());

        //             if(note is Hold)
        //             {
        //                 ((Hold)note).SetVBegin(VBegin);
        //                 ((Hold)note).SetWBegin(WBegin);
        //                 ((Hold)note).SetVEnd(VEnd);
        //                 ((Hold)note).SetWEnd(WEnd);
        //             }
        //             else
        //             {
        //                 ((SingleNote)note).SetV(VBegin);
        //                 ((SingleNote)note).SetW(WBegin);
        //             }
        //         }
        //     }
        // }
        private void SetUpRowNotesRelativePositionAndTapTime(List<Note> rowNotes)
        {
            // All notes in the same row have the same beat. THAT IS NOT TRUE!! HOLDS DO NOT HAVE THE SAME EndBEAT!!!!
            var rowNote = rowNotes[0];
            double VBegin = _statusResolver.GetVFromBeat(rowNote.BeginBeat());
            double WBegin = _statusResolver.GetWFromBeat(rowNote.BeginBeat());

            foreach (var note in rowNotes)
            {
                if (note is Hold)
                {
                    // HOLDS do not have to have the same EndBeat
                    double VEnd = _statusResolver.GetVFromBeat(note.EndBeat());
                    double WEnd = _statusResolver.GetWFromBeat(note.EndBeat());
                    ((Hold)note).SetVBegin(VBegin);
                    ((Hold)note).SetWBegin(WBegin);
                    ((Hold)note).SetOriginalWBegin(WBegin);
                    ((Hold)note).SetVEnd(VEnd);
                    ((Hold)note).SetWEnd(WEnd);
                }
                else
                {
                    ((SingleNote)note).SetV(VBegin);
                    ((SingleNote)note).SetW(WBegin);
                    ((SingleNote)note).SetCombo(_statusResolver.GetComboFromBeat(note.BeginBeat()));
                }
            }
        }

        public List<Note> GetDrawableNotes()
        {
            return _drawableNotes;
        }

        public Score GetSingleNoteScore()
        {
            return _singleNoteScore;
        }

        public Score GetHoldScore()
        {
            return _holdScore;
        }

        public int GetTotalNumberOfSingleNotesAccountingForCombo()
        {
            int aggregate = 0;
            foreach (var row in _singleNoteScore.Rows.GetAll())
            {
                aggregate += ((SingleNote)row.GetFirst().Note).GetCombo();
            }
            return aggregate;
        }
    }
}

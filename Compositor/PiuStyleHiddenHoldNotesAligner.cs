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


using System.Collections.Generic;
using Courel.Loader;

namespace Courel
{
    public class PiuStyleHiddenHoldNotesAligner
    {
        private List<Note> _notes;
        private int _cachedIndex;

        public PiuStyleHiddenHoldNotesAligner(List<Note> notes)
        {
            _notes = notes;
            _cachedIndex = 0;
        }

        // startFrom: not to iterate notes from the very beginning in every call
        public List<double> GetHoldBeatsFromUnalignedNotesInHold(
            PiuStyleHold piuStyleHold,
            List<double> holdBeats
        )
        {
            var holdBeginBeat = piuStyleHold.BeginBeat();
            var holdEndBeat = piuStyleHold.EndBeat();
            List<double> beats = new List<double>();

            for (int i = _cachedIndex; i < _notes.Count; i++)
            {
                var note = _notes[i];
                if (
                    note is SingleNote
                    && note.BeginBeat() > holdBeginBeat
                    && note.BeginBeat() < holdEndBeat
                )
                {
                    if (!holdBeats.Contains(note.BeginBeat()))
                    {
                        beats.Add(note.BeginBeat());
                    }
                }
                if (note.BeginBeat() > holdEndBeat)
                {
                    _cachedIndex = i;
                    return beats;
                }
            }
            return beats;
        }
    }
}

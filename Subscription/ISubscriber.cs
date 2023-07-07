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

namespace Courel
{
    using ScoreComposer;
    using Loader.Notes;

    public interface ISubscriber
    {
        public void OnMissedSingleNotesOnRow(Row row);

        public void OnHoveringReceptorSingleNotes(Row row);

        public void OnActiveHold(Note note, bool held);

        public void OnHoldEnded(Note note, bool held);

        public void OnJudgedSingleNoteOnRow(Note note, Row row);

        public void OnHoldInactive(Note note);
        public void OnHoldEndJudged(Hold note);
        public void OnRolledBackSingleNoteRow(Row row);
        public void OnHoldIsPartiallyRolledBack(Hold note);
        public void OnHoldIsRolledBack(Hold note);
    }
}

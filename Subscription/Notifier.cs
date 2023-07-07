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


using System;
using System.Collections.Generic;
using Courel.Loader;

namespace Courel
{
    using ScoreComposer;
    using Loader.Notes;

    public class Notifier
    {
        // Sequencer event subscriber
        private List<ISubscriber> _subscribers = new List<ISubscriber>();

        public void AddSubscriber(ISubscriber subscriber)
        {
            _subscribers.Add(subscriber);
        }

        public void NotifyMissedSingleNotes(Row row)
        {
            foreach (ISubscriber subscriber in _subscribers)
            {
                subscriber.OnMissedSingleNotesOnRow(row);
            }
        }

        public void NotifyHoveringReceptorSingleNotes(Row row)
        {
            foreach (ISubscriber subscriber in _subscribers)
            {
                subscriber.OnHoveringReceptorSingleNotes(row);
            }
        }

        public void NotifyJudgedSingleNoteOnRow(Note note, Row row)
        {
            foreach (ISubscriber subscriber in _subscribers)
            {
                subscriber.OnJudgedSingleNoteOnRow(note, row);
            }
        }

        internal void NotifyActiveHold(Note note, bool held)
        {
            foreach (ISubscriber subscriber in _subscribers)
            {
                subscriber.OnActiveHold(note, held);
            }
        }

        internal void NotifyHoldEnded(Note note, bool held)
        {
            foreach (ISubscriber subscriber in _subscribers)
            {
                subscriber.OnHoldEnded(note, held);
            }
        }

        internal void NotifyHoldIsInactive(Note note)
        {
            foreach (ISubscriber subscriber in _subscribers)
            {
                subscriber.OnHoldInactive(note);
            }
        }

        internal void NotifyHoldEndJudged(Hold note)
        {
            foreach (ISubscriber subscriber in _subscribers)
            {
                subscriber.OnHoldEndJudged(note);
            }
        }

        internal void NotifyRolledBackSingleNoteRow(Row row)
        {
            foreach (ISubscriber subscriber in _subscribers)
            {
                subscriber.OnRolledBackSingleNoteRow(row);
            }
        }

        internal void NotifyHoldIsPartiallyRolledBack(Hold note)
        {
            foreach (ISubscriber subscriber in _subscribers)
            {
                subscriber.OnHoldIsPartiallyRolledBack(note);
            }
        }

        internal void NotifyHoldIsRolledBack(Hold note)
        {
            foreach (ISubscriber subscriber in _subscribers)
            {
                subscriber.OnHoldIsRolledBack(note);
            }
        }
    }
}

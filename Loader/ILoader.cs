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

namespace Courel.Loader
{
    using GimmickSpecs;
    using Notes;

    /// <summary>
    /// This interface is used by <see cref="Courel.Sequencer"/> to load up everything that is needed
    /// to set up a chart.
    /// </summary>
    public interface ILoader
    {
        /// <summary>
        /// Number of lanes in the chart. This method must return a positive value. This method cannot return null.
        /// </summary>
        public int GetNumberOfLanes();

        /// <summary>
        /// Time offset (in seconds) from the beginning of the song (song time) until the first beat. This method cannot return null. Both negative and positive values are allowed.
        /// The song time during runtime is retrieved by <see cref="Courel.Song.ISong.GetSongTime"/>.
        /// </summary>
        public float GetOffset();

        /// <summary>
        /// The BPMs definition for the chart.
        /// </summary>
        /// <returns>
        /// Each <see cref="Courel.Loader.GimmickSpecs.GimmickPair"/> in the returning list is a definition of a BPM change,
        /// where <see cref="Courel.Loader.GimmickSpecs.GimmickPair.Beat"/> is the beat at which the BPM change occurs,
        /// and <see cref="Courel.Loader.GimmickSpecs.GimmickPair.Value"/> is the new BPM value.
        /// Charts without BPM changes (that is, only one BPM definition) must return a list with only one element,
        /// with <see cref="Courel.Loader.GimmickSpecs.GimmickPair.Beat"/> set to 0, and <see cref="Courel.Loader.GimmickSpecs.GimmickPair.Value"/> set to the BPM value.
        /// This method must not return null.
        /// </returns>
        public List<GimmickPair> GetBpms();

        /// <summary>
        /// The Stops definition for the chart. The stops gimmick artificially pauses the song time for a given amount of time just before the beat is placed at. For further details, see the wiki.
        /// </summary>
        /// <returns>
        /// Each <see cref="Courel.Loader.GimmickSpecs.GimmickPair"/> in the returning list is a definition of a stop gimmick,
        /// where <see cref="Courel.Loader.GimmickSpecs.GimmickPair.Beat"/> is the beat at which the stop gimmick occurs,
        /// and <see cref="Courel.Loader.GimmickSpecs.GimmickPair.Value"/> is the duration of the stop gimmick in seconds.
        /// This method must not return null. If there are no stop gimmicks in the chart, return an empty list.
        /// </returns>
        public List<GimmickPair> GetStops();

        /// <summary>
        /// The Delays definition for the chart. The delays gimmick artificially pauses the song time for a given amount of time just after the beat is placed at. For further details, see the wiki.
        /// </summary>
        /// <returns>
        /// Each <see cref="Courel.Loader.GimmickSpecs.GimmickPair"/> in the returning list is a definition of a delay gimmick,
        /// where <see cref="Courel.Loader.GimmickSpecs.GimmickPair.Beat"/> is the beat at which the delay gimmick occurs,
        /// and <see cref="Courel.Loader.GimmickSpecs.GimmickPair.Value"/> is the duration of the delay gimmick in seconds.
        /// This method must not return null. If there are no delay gimmicks in the chart, return an empty list.
        /// </returns>
        public List<GimmickPair> GetDelays();

        /// <summary>
        /// The Scrolls definition for the chart. The scroll gimmick changes the effective BPM at a given beat by a rate defined by the scroll value. For further details, see the wiki.
        /// </summary>
        /// <returns>
        /// Each <see cref="Courel.Loader.GimmickSpecs.GimmickPair"/> in the returning list is a definition of a scroll gimmick,
        /// where <see cref="Courel.Loader.GimmickSpecs.GimmickPair.Beat"/> is the beat at which the scroll gimmick occurs,
        /// and <see cref="Courel.Loader.GimmickSpecs.GimmickPair.Value"/> is the scroll value.
        /// Charts without scroll gimmicks must return a list with only one element,
        /// with <see cref="Courel.Loader.GimmickSpecs.GimmickPair.Beat"/> set to 0, and <see cref="Courel.Loader.GimmickSpecs.GimmickPair.Value"/> set to 1.
        /// This method must not return null.
        /// </returns>
        public List<GimmickPair> GetScrolls();

        /// <summary>
        /// The Warps definition for the chart. The warp gimmick warps over (skips) a given amount of beats at a given beat. Notes placed in the interval
        /// [<see cref="Courel.Loader.GimmickSpecs.GimmickPair.Beat"/> to <see cref="Courel.Loader.GimmickSpecs.GimmickPair.Beat"/> + <see cref="Courel.Loader.GimmickSpecs.GimmickPair.Value"/>] become <see cref="Courel.Loader.Notes.Visibility.Fake"/> notes.
        /// For further details, see the wiki.
        /// </summary>
        /// <returns>
        /// Each <see cref="Courel.Loader.GimmickSpecs.GimmickPair"/> in the returning list is a definition of a warp gimmick,
        /// where <see cref="Courel.Loader.GimmickSpecs.GimmickPair.Beat"/> is the beat at which the warp gimmick occurs,
        /// and <see cref="Courel.Loader.GimmickSpecs.GimmickPair.Value"/> is the number of beats to warp over.
        /// This method must not return null. If there are no warp gimmicks in the chart, return an empty list.
        /// </returns>
        public List<GimmickPair> GetWarps();

        /// <summary>
        /// The Fake sections definition for the chart. The fake gimmick makes all notes placed in the interval
        /// [<see cref="Courel.Loader.GimmickSpecs.GimmickPair.Beat"/> to <see cref="Courel.Loader.GimmickSpecs.GimmickPair.Beat"/> + <see cref="Courel.Loader.GimmickSpecs.GimmickPair.Value"/>] become <see cref="Courel.Loader.Notes.Visibility.Fake"/> notes.
        /// for further details, see the wiki.
        /// </summary>
        /// <returns>
        /// Each <see cref="Courel.Loader.GimmickSpecs.GimmickPair"/> in the returning list is a definition of a fake section,
        /// where <see cref="Courel.Loader.GimmickSpecs.GimmickPair.Beat"/> is the beat at which the fake section begins,
        /// and <see cref="Courel.Loader.GimmickSpecs.GimmickPair.Value"/> is the span of beats of the fake section.
        /// This method must not return null. If there are no fake sections in the chart, return an empty list.
        /// </returns>
        public List<GimmickPair> GetFakes();

        /// <summary>
        /// The Speeds definition for the chart. The speeds gimmick changes the effective scrolling speed at a given beat by a rate defined by the speed value. The speed changes linearly
        /// according to the span of time defined by the speed gimmick. For further details, see the wiki.
        /// </summary>
        /// <returns>
        /// Each <see cref="Courel.Loader.GimmickSpecs.Speed"/> in the returning list is a definition of a speed gimmick,
        /// where <see cref="Courel.Loader.GimmickSpecs.Speed.Beat"/> is the beat at which the speed gimmick occurs,
        /// <see cref="Courel.Loader.GimmickSpecs.Speed.Value"/> is the new speed value,
        /// <see cref="Courel.Loader.GimmickSpecs.Speed.SpanTime"/> is the span of time for the speed transition from the previous speed to the new speed,
        /// and <see cref="Courel.Loader.GimmickSpecs.Speed.SpanTimeType"/> is the type of span time.
        /// Charts without speed gimmicks must return a list with only one element,
        /// with <see cref="Courel.Loader.GimmickSpecs.Speed.Beat"/> set to 0, <see cref="Courel.Loader.GimmickSpecs.Speed.Value"/> set to 1,
        /// <see cref="Courel.Loader.GimmickSpecs.Speed.SpanTime"/> set to 0, and <see cref="Courel.Loader.GimmickSpecs.Speed.SpanTimeType"/> set to <see cref="Courel.Loader.GimmickSpecs.SpanTimeType.Beats"/>.
        /// This method must not return null.
        /// </returns>
        public List<Speed> GetSpeeds();

        /// <summary>
        /// The Tick Counts definition for the chart. The tick count gimmick changes the amount of hidden hold notes generated for <see cref="Courel.Loader.Notes.PiuStyleHold"/> per beat. For further details, see the wiki.
        /// </summary>
        /// <returns>
        /// Each <see cref="Courel.Loader.GimmickSpecs.GimmickPair"/> in the returning list is a definition of a tick count gimmick,
        /// where <see cref="Courel.Loader.GimmickSpecs.GimmickPair.Beat"/> is the beat at which the tick count gimmick occurs,
        /// and <see cref="Courel.Loader.GimmickSpecs.GimmickPair.Value"/> is the new tick count value.
        /// This method must not return null.
        /// </returns>
        public List<GimmickPair> GetTickCounts();

        /// <summary>
        /// The Combos gimmick for the chart. The combo gimmick changes the combo contribution of notes placed inbetween the combo section beats to the value. For further details, see the wiki.
        /// </summary>
        /// <returns>
        /// Each <see cref="Courel.Loader.GimmickSpecs.GimmickPair"/> in the returning list is a definition of a combo section,
        /// where <see cref="Courel.Loader.GimmickSpecs.GimmickPair.Beat"/> is the beat at which the combo section begins,
        /// and <see cref="Courel.Loader.GimmickSpecs.GimmickPair.Value"/> is the combo contribution.
        /// This method must not return null.
        /// </returns>
        public List<GimmickPair> GetCombos();

        /// <summary>
        /// All the notes the char consists of. See <see cref="Courel.Loader.Notes.Note"/> to check all available notes.
        /// </summary>
        /// <returns></returns>
        public List<Note> GetNotes();

        /// <summary>
        /// Skips all notes until the first note appearing after the provided second. All notes that would be actioned prior to the given second (inclusive), won't be processed by the sequencer, and won't be returned by
        /// <see cref="Courel.Sequencer.GetDrawableNotes"/>.
        /// </summary>
        /// <returns></returns>
        public double SkipUntilSecond();
    }
}

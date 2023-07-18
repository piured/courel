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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Courel
{
    using Loader;
    using ScoreComposer;
    using Input;
    using Judge;
    using Loader.Notes;
    using RunTime;
    using Song;
    using State;
    using Subscription;

    /// <summary>
    /// Courel's main sequencer abstraction
    /// </summary>
    public class Sequencer : MonoBehaviour
    {
        [SerializeField]
        private double _songTime = 0;

        [SerializeField]
        private double _warpedTime = 0;

        [SerializeField]
        private double _delayedTime = 0;

        [SerializeField]
        private double _stoppedTime = 0;

        [SerializeField]
        private double _position = 0;

        [SerializeField]
        private double _beat = 0;

        [SerializeField]
        private double _speed = 0;

        [SerializeField]
        private double _userSpeed = 3;

        [SerializeField]
        private double _offset = 0.0f;

        [SerializeField]
        private double _lag = -0.07f;

        [SerializeField]
        private int _combos = 0;

        private double _percentageOfBeat;
        private FStates _state;
        private StateResolver _statusResolver;
        private Composer _composer;
        private RunTimeResolver _runtimeResolver;

        private Notifier _notifier = new Notifier();
        private ISong _song = new UndefinedSong();
        private IHoldInput _holdInput = new UndefinedHoldInput();
        private IJudge _judge = new UndefinedJudge();

        /// <summary>
        /// Sets the HoldInput object to query the hold input state of each lane.
        /// </summary>
        /// <param name="iHoldInput">Object that implements the <see cref="Courel.Input.IHoldInput"/> interface.</param>
        public void SetIHoldInput(IHoldInput iHoldInput)
        {
            _holdInput = iHoldInput;
            if (_runtimeResolver != null)
                _runtimeResolver.ChangeInput(iHoldInput);
        }

        /// <summary>
        /// Sets the your custom judge object to judge the notes.
        /// </summary>
        /// <param name="judge"> Object that implements the <see cref="Courel.Judge.IJudge"/> interface.</param>
        public void SetIJudge(IJudge judge)
        {
            _judge = judge;
            if (_runtimeResolver != null)
                _runtimeResolver.ChangeJudge(judge);
        }

        /// <summary>
        /// Sets the song object to query the song time.
        /// </summary>
        /// <param name="song"> Object that implements the <see cref="Courel.Song.ISong"/> interface.</param>
        public void SetISong(ISong song)
        {
            _song = song;
        }

        /// <summary>
        /// Loads a chart (steps and gimmicks) into the sequencer.
        /// </summary>
        /// <param name="loader"> Object that implements the <see cref="Courel.Loader.IChart"/> interface.</param>
        public void LoadChart(IChart loader)
        {
            _statusResolver = new StateResolver(loader);
            _composer = new Composer(loader, _statusResolver);
            _runtimeResolver = new RunTimeResolver(
                _judge,
                _composer.GetSingleNoteScore(),
                _composer.GetHoldScore(),
                _notifier,
                _holdInput
            );
            _offset = loader.GetOffset();
        }

        /// <summary>
        /// Sets the user speed value.
        /// </summary>
        public void SetUserSpeed(double speed)
        {
            _userSpeed = speed;
        }

        /// <summary>
        /// Sets the input lag value.
        /// </summary>
        public void SetLag(double lag)
        {
            _lag = lag;
        }

        /// <summary>
        /// Get user speed value.
        /// </summary>
        public double GetUserSpeed()
        {
            return _userSpeed;
        }

        /// <summary>
        /// Get input lag value.
        /// </summary>
        public double GetLag()
        {
            return _lag;
        }

        void Update()
        {
            if (_statusResolver != null && _runtimeResolver != null)
            {
                UpdateSequencerStatus();
                _runtimeResolver.ResolveAndNotify((float)_songTime);
            }
        }

        private void UpdateSequencerStatus()
        {
            _songTime = _song.GetSongTime() + _offset + _lag;
            _statusResolver.UpdateStatus(_songTime);
            var state = _statusResolver.GetStatus();

            _warpedTime = state.WarpedTime;
            _delayedTime = state.DelayedTime;
            _stoppedTime = state.StoppedTime;
            _beat = state.Beat;
            _position = state.Position;
            _speed = state.Speed;
            _combos = state.Combos;

            _speed *= _userSpeed;
            _percentageOfBeat = 1 - (_beat % 1);
            _state = state;
        }

        /// <summary>
        /// Gets the current state of the sequencer.
        /// </summary>
        /// <returns></returns>
        public FStates GetState()
        {
            return _state;
        }

        /// <summary>
        /// Retrieves the notes that are visible on the screen.
        /// </summary>
        /// <returns> A list of <see cref="Courel.Loader.Notes.Note"/> objects.</returns>
        public List<Note> GetDrawableNotes()
        {
            return _composer.GetDrawableNotes();
        }

        /// <summary>
        /// Retrieves the percentage of the beat that the sequencer is currently in. The percentage
        /// of the beat is a number in the range [0, 1] that represents the current position of the
        /// sequencer in the current beat. Mostly used for visual effects.
        /// </summary>
        /// <returns> A number in the range [0, 1].</returns>
        public double GetPercentageOfBeat()
        {
            return _percentageOfBeat;
        }

        /// <summary>
        /// Retrieves the current speed value.
        /// </summary>
        /// <returns></returns>
        public double GetSpeed()
        {
            return _speed;
        }

        /// <summary>
        /// Returns the current scroll value.
        /// </summary>
        /// <returns></returns>
        public double GetScroll()
        {
            return _position;
        }

        /// <summary>
        /// Adds an event subscriber to the sequencer.
        /// </summary>
        /// <param name="subscriber"> Object that implements the <see cref="Courel.Subscription.ISubscriber"/> interface.</param>
        public void AddSubscriber(ISubscriber subscriber)
        {
            _notifier.AddSubscriber(subscriber);
        }

        /// <summary>
        /// Retrieves the total number of rows in the score.
        /// </summary>
        /// <returns></returns>
        public int GetTotalNumberOfRows()
        {
            return _composer.GetTotalNumberOfRows();
        }

        /// <summary>
        /// Retrieves the total number of single notes in the score.
        /// </summary>
        /// <returns></returns>
        public int GetTotalNumberOfSingleNotes()
        {
            return _composer.GetTotalNumberOfSingleNotes();
        }

        /// <summary>
        /// Retrieves the total number of single notes in the score accounting for combo.
        /// </summary>
        /// <returns></returns>
        public int GetTotalNumberOfSingleNotesAccountingForCombo()
        {
            return _composer.GetTotalNumberOfSingleNotesAccountingForCombo();
        }

        /// <summary>
        /// Informs the sequencer that a tap event has occurred on a lane.
        /// </summary>
        /// <param name="lane"> lane index at which the tap event occurred.</param>
        public void Tap(int lane)
        {
            _runtimeResolver.Tap(lane, (float)_songTime);
        }

        /// <summary>
        /// Informs the sequencer that a lift event has occurred on a lane.
        /// </summary>
        /// <param name="lane"> lane index at which the lift event occurred.</param>
        public void Lift(int lane)
        {
            _runtimeResolver.Lift(lane, (float)_songTime);
        }

        /// <summary>
        /// Sets the sequencer to autoplay mode.
        /// </summary>
        /// <param name="flag"> True to enable autoplay, false to disable.</param>
        public void SetAutoPlay(bool flag)
        {
            _runtimeResolver.SetAutoPlay(flag);
        }
    }
}

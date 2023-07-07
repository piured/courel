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
    using System;
    using Loader;
    using ScoreComposer;
    using Input;
    using Judge;

    /// <summary>
    /// Hello, world
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

        private StatusResolver _statusResolver;
        private Composer _composer;
        private RunTimeResolver _runtimeResolver;

        private Notifier _notifier = new Notifier();
        private ISong _song = new UndefinedSong();
        private IHoldInput _holdInput = new UndefinedHoldInput();
        private IJudge _judge = new UndefinedJudge();

        public void SetIHoldInput(IHoldInput iHoldInput)
        {
            _holdInput = iHoldInput;
            if (_runtimeResolver != null)
                _runtimeResolver.ChangeInput(iHoldInput);
        }

        public void SetIJudge(IJudge judge)
        {
            _judge = judge;
            if (_runtimeResolver != null)
                _runtimeResolver.ChangeJudge(judge);
        }

        public void SetISong(ISong song)
        {
            _song = song;
        }

        public void LoadLevel(ILoader loader)
        {
            _statusResolver = new StatusResolver(loader);
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
            var status = _statusResolver.GetStatus();

            _warpedTime = status.WarpedTime;
            _delayedTime = status.DelayedTime;
            _stoppedTime = status.StoppedTime;
            _beat = status.Beat;
            _position = status.Position;
            _speed = status.Speed;
            _combos = status.Combos;

            _speed *= _userSpeed;
            _percentageOfBeat = 1 - (_beat % 1);
        }

        public List<Note> GetDrawableNotes()
        {
            return _composer.GetDrawableNotes();
        }

        public double GetPercentageOfBeat()
        {
            return _percentageOfBeat;
        }

        public double GetSpeed()
        {
            return _speed;
        }

        public double GetPosition()
        {
            return _position;
        }

        public void AddSubscriber(ISubscriber subscriber)
        {
            _notifier.AddSubscriber(subscriber);
        }

        public int GetTotalNumberOfRows()
        {
            return _composer.GetTotalNumberOfRows();
        }

        public int GetTotalNumberOfSingleNotes()
        {
            return _composer.GetTotalNumberOfSingleNotes();
        }

        public int GetTotalNumberOfSingleNotesAccountingForCombo()
        {
            return _composer.GetTotalNumberOfSingleNotesAccountingForCombo();
        }

        /// <summary>
        /// TAl
        /// </summary>
        /// <param name="lane">Tal</param>
        public void Tap(int lane)
        {
            _runtimeResolver.Tap(lane, (float)_songTime);
        }

        public void Lift(int lane)
        {
            _runtimeResolver.Lift(lane, (float)_songTime);
        }

        public void SetAutoPlay(bool flag)
        {
            _runtimeResolver.SetAutoPlay(flag);
        }
    }
}

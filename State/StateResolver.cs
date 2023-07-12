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

namespace Courel.State
{
    using Loader;
    using Loader.GimmickSpecs;
    using Functions;

    public class StateResolver
    {
        private IF _if;
        private F _f;
        private TS _ts;
        private ITS _its;
        private TD _td;
        private ITD _itd;
        private P _p;
        private Q _q;
        private IQ _iq;
        private E _e;
        private U _u;

        private Combos _combos;

        private List<GimmickPair> _fakes;

        private FStates _state = new FStates();

        public StateResolver(ILoader iLoader)
        {
            SetUpFunctions(iLoader);

            SetUpMembers(iLoader);
        }

        private void SetUpMembers(ILoader iLoader)
        {
            _fakes = iLoader.GetFakes();
        }

        void SetUpFunctions(ILoader iLoader)
        {
            var bpss = ConvertBpmsToBpss(iLoader.GetBpms());

            _combos = new Combos(iLoader.GetCombos());

            _if = new IF(bpss);
            _f = new F(bpss, _if);
            _q = new Q(iLoader.GetWarps(), _if);
            _iq = new IQ(iLoader.GetWarps(), _if);

            _its = new ITS(iLoader.GetStops(), _if, _iq);
            _itd = new ITD(iLoader.GetDelays(), _if, _its, _iq);
            _ts = new TS(iLoader.GetStops(), _if, _iq);

            _td = new TD(iLoader.GetDelays(), _if, _its, _iq);

            _p = new P(iLoader.GetScrolls());

            _e = new E(iLoader.GetSpeeds(), _if, _its, _itd, _iq);

            _u = new U(iLoader.GetTickCounts());
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

        public double GetVFromBeat(double beat)
        {
            return _itd.Eval(_its.Eval(_iq.Eval(_if.Eval(beat))));
        }

        public double GetWFromBeat(double beat)
        {
            return _p.Eval(beat);
        }

        public int GetComboFromBeat(double beat)
        {
            return (int)_combos.Eval(beat);
        }

        public List<double> GetHoldTapBeats(double beginBeat, double endBeat)
        {
            return _u.GetHoldTapBeats(beginBeat, endBeat);
        }

        public bool IsBeatWarpedOver(double beat)
        {
            return _q.IsBeatWarpedOver(beat);
        }

        // TODO: improve performance
        public bool IsBeatFaked(double beat)
        {
            foreach (GimmickPair fake in _fakes)
            {
                if (beat >= fake.Beat && beat < fake.Beat + fake.Value)
                {
                    return true;
                }
            }
            return false;
        }

        public void UpdateStatus(double songTime)
        {
            _state.SongTime = songTime;
            _state.Speed = _e.Eval(songTime);
            _state.DelayedTime = _td.Eval(songTime);
            _state.StoppedTime = _ts.Eval(_state.DelayedTime);
            _state.WarpedTime = _q.Eval(_state.StoppedTime);
            _state.Beat = _f.Eval(_state.WarpedTime);
            _state.Position = _p.Eval(_state.Beat);
            _state.Combos = (int)_combos.Eval(_state.Beat);
        }

        public FStates GetStatus()
        {
            return _state;
        }
    }
}

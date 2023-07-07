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

namespace Courel
{
    using System;
    using Loader.GimmickSpecs;

    public class U
    {
        private List<GimmickPair> _u;
        public double PositiveInfinity = Double.MaxValue / 2;

        public U(List<GimmickPair> tickCounts)
        {
            _u = tickCounts;

            if (tickCounts.Count < 1)
            {
                throw new System.Exception("TickCounts must have at least one signature.");
            }
        }

        public List<double> GetHoldTapBeats(double h1, double h2)
        {
            List<GimmickPair> uprime = GetUPrime(h1, h2);
            List<double> t = GetT(h1, h2, uprime);
            return t;
        }

        private List<double> GetT(double h1, double h2, List<GimmickPair> uprime)
        {
            List<double> t = new List<double>();
            t.Add(h1);

            for (int i = 0; i < uprime.Count - 1; i++)
            {
                var uprimeElement = uprime[i];
                var nextUprimeElement = uprime[i + 1];
                double ai = GetAi(uprimeElement.Beat, uprimeElement.Value);
                int si = GetSi(uprimeElement.Value, ai, nextUprimeElement.Beat);

                for (int k = 0; k < si; k++)
                {
                    double sum = 0;
                    for (int j = 0; j < k; j++)
                    {
                        sum += 1.0d / uprimeElement.Value;
                    }

                    t.Add(ai + sum);
                }
            }
            t.Add(h2);

            return t;
        }

        private double GetAi(double bprimei, double tprimei)
        {
            return bprimei + (bprimei % (1.0d / tprimei));
        }

        private int GetSi(double tprimei, double ai, double bprimeiplus1)
        {
            return (int)Math.Floor(tprimei * (bprimeiplus1 - ai));
        }

        private List<GimmickPair> GetUPrime(double h1, double h2)
        {
            List<GimmickPair> uprime = new List<GimmickPair>();

            uprime.Add(new GimmickPair(h1, G(h1)));
            AddUElementsInInterval(h1, h2, uprime);
            uprime.Add(new GimmickPair(h2, 0));
            return uprime;
        }

        private void AddUElementsInInterval(double h1, double h2, List<GimmickPair> uprime)
        {
            foreach (var uelement in _u)
            {
                if (uelement.Beat > h1 && uelement.Beat < h2)
                {
                    uprime.Add(uelement);
                }
            }
        }

        private double G(double h1)
        {
            var globalMinU = _u[0];
            double globalMin = PositiveInfinity;
            foreach (var uelement in _u)
            {
                if (h1 >= uelement.Beat)
                {
                    var currentMin = h1 - uelement.Beat;
                    if (currentMin <= globalMin)
                    {
                        globalMin = currentMin;
                        globalMinU = uelement;
                    }
                }
                else
                {
                    break;
                }
            }
            return globalMinU.Value;
        }
    }
}

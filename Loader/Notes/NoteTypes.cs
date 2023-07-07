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

namespace Courel.Loader.Notes
{
    public enum NoteType
    {
        // Standard note type
        TapNote,

        // Tap the hold note when it crosses the judgment row, then hold it down until the end crosses the judgment row.
        DdrStyleHold,

        // Tap or hold the hold note when it crosses the judgment row, then hold it down until the end crosses the judgment row.
        // Unlike Ddr-style DdrStyleHolds, Piu-style PiuStyleHolds create multiple invisible InvisibleHoldNotes that contribute to the judgement.
        PiuStyleHold,

        // Tap the roll note when it crosses the judgment row, then hit repeatly it until the end.
        RollHold,

        // hold on the note before it crosses. Lift up when it does cross.
        LiftNote,

        // You can ignore this note: it does nothing for or against you.
        FakeNote,

        // hold the note when it crosses the judgmenet row.
        HoldNote,

        // hold the note when it crosses the judgmenet row, (not visible to the theme). These are result of PiuStyleHolds.
        InvisibleHoldNote,
    }

    public enum Visibility
    {
        // This note won't be shown, but it will be judged normally
        Hidden,

        // You can ignore this note: it does nothing for or against you (it won't be judged)
        Fake,

        // This note will be shown and judged normally
        Normal,
    }
}

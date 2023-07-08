<p align="center">
<picture>
 <source media="(prefers-color-scheme: dark)" srcset="Imgs/Logos/courel-dark.png">
 <img alt="Courel" src="Imgs/Logos/courel-light.png" width=350>
</picture>
</p>

Courel is a stepmania-compatible open-source sequencer for rhythm games.

## Notes

Notes are the core element that players must react and interact to in any rhythm game. They are placed in a Score, which is a particular arrangement of notes through time.

This is a note:

<p align="center">
 <img alt="Courel" src="Imgs/Tutorial/blue-tap-note.png" width=40>
</p>
 but also this is a note:
<p align="center">
 <img alt="Courel" src="Imgs/Tutorial/blue-hold.png" width=40>
</p>

In the most broad sense, a note is a point in time where the player must react to. In Courel, notes are represented by an instance of a class derived from `Note`, which is a generic class that can be specialized into different types of notes.

In Courel, we distinguish two big categories of notes:

1. `SingleNote`s: These are notes that that interact with the player only once, and hence they are judged only once too. For simplicity, we are going to represent them as circles, such as the blue note above.
2. `Hold`s: Unlike `SingleNotes`, these notes span over time, therefore the player must interact with them until they run out of scope. We represent them as two circles connected by a line, e.g. the blue hold above.

The manner in which the player interacts with notes is different for pretty much every rhythm game out there.

### SingleNotes

On the one hand, Courel offers three means of interaction with `SingleNotes` that can be remmaped to any specific interaction that a game needs. Those are:

1. `Tap`: A tap occurs for example when the player presses a button on a controller, or a key on a keyboard, or stomps on a pad in a dancing machine. Arguably, the tap interaction is the most common way of note. Arrows in DDR or PIU are notes that react to Tap events, for example. Courel offers an off-the-shelf `TapNote` class that can be used to represent this kind of notes. Tap events are passed to the sequencer via a method call in the `Sequencer` class. We represent this notes as filled circles:

<p align="center">
 <img alt="Courel" src="Imgs/Tutorial/blue-tap-note.png" width=40>
</p>

2. `Lift`: Lift events are less common than tap events, but they are used in some games. A lift event occurs when the player releases a button on a controller, or a key on a keyboard. In Courel, we have a class `LiftNotes` for this kind of notes. Similarly, to taps, lift events are also passed to the sequencer via a method call in the `Sequencer` class.

3. `Hold` (InputEvent): Holds are actually not events, but rather a state of the input. A hold occurs for example when the player presses and holds a button on a controller, or a key on a keyboard. Courel offers a `HoldNote` class for this kind of notes (not to be confused with `Hold` (Note)). As interactions of this kind are not events, the sequencer queries the state of the input via the interface `IHoldInput` to determine whether a hold is active or note. We represent `HoldNote`s as circumferences.

<p align="center">
 <img alt="Courel" src="Imgs/Tutorial/blue-hold-note.png" width=40>
</p>

### Holds

On the other hand, the interaction and judge with `Hold`s (Notes) is very game dependant. You can think of how different DDR or PIU holds feel like while playing each game. We will cover the judgmets of all notes down below, so it does not matter how it is done right now. What is important, is that Courel implements three different types of holds that are commonly used in rhythm games, which we will represented as two circles connected by a line:

<p align="center">
 <img alt="Courel" src="Imgs/Tutorial/blue-hold.png" width=40>
</p>

1. `DdrStyleHold`s: Courel names this type of hold after DDR, as it is the type of hold used in it. In short, this kind holds need two interactions by the user: a tap when the hold starts, followed by a hold and until it runs out of scope. It generates two judgment events, which we will leave for later.

2. `PiuStyleHold`s: Named after PIU, this hold mimmic the behaviour of the korean arcade. While its implementation is quite tricky, the interaction with the player is simple: hold (or tap from the beginning) until it runs out of scope. What is special about it is that it generates a judgment event which is in sync with the bpm, tickcount and other gimmicks which will be covered later on.

3. `DdrStyleRollHold`: This hold is not unique to DDR, but it follows its behaviour. It is a hold that starts with a tap, and then instead of held, the player must tap repeatedly until it runs out of scope. It generates events for the taps during its lifespan.

Under the hood, Courel maps `DdrStyleHold`s and `PiuStyleHold`s to a number of `SingleNotes` according to the behaviour of each one. Hence, Holds are not judged directly, but rather the `SingleNotes` that they are mapped to. More in detail:

- `PiuStyleHolds` are mapped to a number of `HoldNotes` with `Hidden` visibility (so they are not drawn in the screen), as shown below:

<p align="center">
 <img alt="Courel" src="Imgs/Tutorial/piu-style-holds-conversion.png" width=130>
</p>

- `DdrStyleHolds` and `DdrStyleRollHolds` are mapped to a single head note of type `TapNote`, also with `Hidden` visibility. You can see an example below:

<p align="center">
 <img alt="Courel" src="Imgs/Tutorial/ddr-style-holds-conversion.png" width=130>
</p>

### Visibility

The visibility of a note is a property that determines whether notes are visible or judged. Courel supports natively three types of visibility for any `Note`:

1. `Normal`: The note is visible and judged normally.
2. `Hidden`: The note is not visible, but it is judged normally.
3. `Fake`: The note is visible, but it is not judged.

The notes that must be drawn in the screen can be queried via the `GetVisibleNotes` method in the `Sequencer` class. Beware that it is possible that some hidden notes (e.g. generated for `Hold`s) can be asked to be judged, and not be part of the visible notes returned by the sequencer.

## Score

As stated earlier, notes are arranged in a score. Courel defines scores through the `Score` class, which is an aggregate of `Lane`s and `Row`s. In the picture below you can see part of a score with 3 lanes and 5 rows which have been left empty for clarity (rows are never empty). Lanes are represented as vertical lines, and they are numbered from 0 to 2. You can ask Courel to have as many lanes as you want. Rows are represented as horizontal lines (perpedicular to the lanes), and unlike the number of lanes, it is not a parameter you need to decide in advance -- the amount vary depending on the actual notes you want to place in the score.

<p align="center">
 <img alt="Score" src="Imgs/Tutorial/score-lanes-and-rows.png" width=500>
</p>

### Usage of lanes

The usage of the lanes are very game dependant, but the most common use for them is to separate notes that must be actioned with different buttons/pads. As an example you can think of DDR having four lanes: one for each left, up, down, and right arrows. Tycho for instance has ony one lane, and all kinds of notes are placed in it. Courel does not impose any restriction on the usage of lanes, so you can use them as you wish. What you need to know is the sequeantiality restriction from the notes in the lane. Once a lane is filled up with notes, these are processed from top to bottom as the song progresses. A note in the $n$-th position of a lane won't be asked to be judged until the $n-1$-th note has been judged before. Notes in different lanes are independent from each other, so they can be judged in any order.

### Note positioning within Scores

Notes are placed in this grid by providing two pieces of information: the **beat** at which the note must be actioned, and the **lane** in which the note must be placed.

The `beat` is a number relative to the start of the song that indicates precisely when the note must be actioned. It is task of Courel to figure out when exactly the note must be actioned in after the start of the song ($v$ value, in seconds), and where it should be placed on the scrolling axis of the game ($w$ value). Do not worry on what this values are right now, as they will be somewhat covered later on. It is interesting to note that you are not providing directly the time when the note must be actioned, but rather the beat. This is because the beat is a number that is independent from the bpm of the song, and hence it is easier to work with for stepmakers. The task of retrieving $v$ and $w$, is not trivial at all if you are using the complete gimmick system that Courel implements.

The `lane` is a number that indicates in which lane the note belongs to. As stated earlier, lanes are numbered from 0 to $n-1$, where $n$ is the number of lanes in the score (a number that is defined beforehand). Placing a note in one lane or another is up to the designer of the game, as discussed earlier.

In the image below you can see five `TapNote`s placed in the score. The color of each `TapNote` just indicates the lane in which it is placed.

<p align="center">
 <img alt="Score" src="Imgs/Tutorial/score-with-notes.png" width=450>
</p>

Note that we have notes placed at beat 0, 1, 3, 4. Since we do not have notes on beat 2, a row is not created for it.
Another noteworthy aspect of this note arrangement in lanes and rows is that notes with the same beat and different lanes are placed in the same `Row` object. When interacting with the `Sequencer` via subscriptions (`ISubscriber`), you will be notified in most of scenarios w.r.t. to `Row`s of notes instead of individual notes. This is because most rhythm games judge based on rows of notes instead of individual notes. Courel judges notes individually, but notifies the subscribers with `Row`s to make it more tunable for any game's needs.

### Note positioning

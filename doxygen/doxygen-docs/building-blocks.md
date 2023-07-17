@page notes Notes

[TOC]

Notes are the core element that players must react and interact to in any rhythm game. They are placed in a Score, which is a particular arrangement of notes through time.

This is a note:

 <img alt="Courel" src="https://raw.githubusercontent.com/piured/courel/main/Imgs/Tutorial/blue-tap-note.png" width=40 style="display: block; margin: 0 auto; text-align: center;">

but also this is a note:

 <img alt="Courel" src="https://raw.githubusercontent.com/piured/courel/main/Imgs/Tutorial/blue-hold.png" width=40 style="display: block; margin: 0 auto; text-align: center;">

In the most broad sense, a note is a point in time where the player must react to. In Courel, notes are represented by an instance of a class derived from [Note](@ref Courel.Loader.Notes.Note), which is a generic class that can be specialized into different types of notes.

In Courel, we distinguish two big categories of notes:

1. [SingleNote](@ref Courel.Loader.Notes.SingleNote)s: These are notes that that interact with the player only once, and hence they are judged only once too. For simplicity, we are going to represent them as circles, such as the blue note above.
2. [Hold](@ref Courel.Loader.Notes.Hold)s: Unlike [SingleNote](@ref Courel.Loader.Notes.SingleNote)s, these notes span over time, therefore the player must interact with them until they run out of scope. We represent them as two circles connected by a line, e.g. the blue hold above.

The manner in which the player interacts with notes is different for pretty much every rhythm game out there.

# SingleNotes

On the one hand, Courel offers three means of interaction with [SingleNote](@ref Courel.Loader.Notes.Hold)s that can be remmaped to any specific interaction that a game needs. Those are:

1. Tap: A tap occurs for example when the player presses a button on a controller, or a key on a keyboard, or stomps on a pad in a dancing machine. Arguably, the tap interaction is the most common way of note. Arrows in DDR or PIU are notes that react to Tap events, for example. Courel offers an off-the-shelf [TapNote](@ref Courel.Loader.Notes.TapNote) class that can be used to represent this kind of notes. Tap events are passed to the sequencer via a method call in the [Sequencer](@ref Courel.Sequencer) class. We represent this notes as filled circles:

 <img alt="Courel" src="https://raw.githubusercontent.com/piured/courel/main/Imgs/Tutorial/blue-tap-note.png" width=40 style="display: block; margin: 0 auto; text-align: center;">

2. Lift: Lift events are less common than tap events, but they are used in some games. A lift event occurs when the player releases a button on a controller, or a key on a keyboard. In Courel, we have a class [LiftNote](@ref Courel.Loader.Notes.LiftNote) for this kind of notes. Similarly, to taps, lift events are also passed to the sequencer via a method call in the [Sequencer](@ref Courel.Sequencer) class.

3. Hold (input event, not [Hold](@ref Courel.Loader.Notes.Hold)): Holds are actually not events, but rather a state of the input. A hold occurs for example when the player presses and holds a button on a controller, or a key on a keyboard. Courel offers a [HoldNote](@ref Courel.Loader.Notes.HoldNote) class for this kind of notes (not to be confused with [Hold](@ref Courel.Loader.Notes.Hold) (Note)). As interactions of this kind are not events, the sequencer queries the state of the input via the interface [IHoldInput](@ref Courel.Input.IHoldInput) to determine whether a hold is active or note. We represent [HoldNote](@ref Courel.Loader.Notes.HoldNote)s as circumferences.

 <img alt="Courel" src="https://raw.githubusercontent.com/piured/courel/main/Imgs/Tutorial/blue-hold-note.png" width=40 style="display: block; margin: 0 auto; text-align: center;">

# Holds

On the other hand, the interaction and judge with [Hold](@ref Courel.Loader.Notes.Hold)s (Notes) is very game dependant. You can think of how different DDR or PIU holds feel like while playing each game. We will cover the judgmets of all notes down below, so it does not matter how it is done right now. What is important, is that Courel implements three different types of holds that are commonly used in rhythm games, which we will represented as two circles connected by a line:

 <img alt="Courel" src="https://raw.githubusercontent.com/piured/courel/main/Imgs/Tutorial/blue-hold.png" width=40 style="display: block; margin: 0 auto; text-align: center;">

1. [DdrStyleHold](@ref Courel.Loader.Notes.DdrStyleHold)s: Courel names this type of hold after DDR, as it is the type of hold used in it. In short, this kind holds need two interactions by the user: a tap when the hold starts, followed by a hold and until it runs out of scope. It generates two judgment events, which we will leave for later.

2. [PiuStyleHold](@ref Courel.Loader.Notes.PiuStyleHold)s: Named after PIU, this hold mimmic the behaviour of the korean arcade. While its implementation is quite tricky, the interaction with the player is simple: hold (or tap from the beginning) until it runs out of scope. What is special about it is that it generates a judgment event which is in sync with the bpm, tickcount and other gimmicks which will be covered later on.

3. [DdrStyleRollHold](@ref Courel.Loader.Notes.DdrStyleRollHold): This hold is not unique to DDR, but it follows its behaviour. It is a hold that starts with a tap, and then instead of held, the player must tap repeatedly until it runs out of scope. It generates events for the taps during its lifespan.

Under the hood, Courel maps [DdrStyleHold](@ref Courel.Loader.Notes.DdrStyleHold)s and [PiuStyleHold](@ref Courel.Loader.Notes.PiuStyleHold)s to a number of [SingleNote](@ref Courel.Loader.Notes.Hold)s according to the behaviour of each one. Hence, Holds are not judged directly, but rather the [SingleNote](@ref Courel.Loader.Notes.Hold)s that they are mapped to. More in detail:

- [PiuStyleHold](@ref Courel.Loader.Notes.PiuStyleHold)s are mapped to a number of [HoldNote](@ref Courel.Loader.Notes.HoldNote)s with [Hidden](@ref Courel.Loader.Notes.Visibility.Hidden) visibility (so they are not drawn in the screen), as shown below:

 <img alt="Courel" src="https://raw.githubusercontent.com/piured/courel/main/Imgs/Tutorial/piu-style-holds-conversion.png" width=130 style="display: block; margin: 0 auto; text-align: center;">

- [DdrStyleHold](@ref Courel.Loader.Notes.DdrStyleHold)s and [DdrStyleRollHold](@ref Courel.Loader.Notes.DdrStyleRollHold)s are mapped to a single head note of type [TapNote](@ref Courel.Loader.Notes.TapNote), also with [Hidden](@ref Courel.Loader.Notes.Visibility.Hidden) visibility. You can see an example below:

 <img alt="Courel" src="https://raw.githubusercontent.com/piured/courel/main/Imgs/Tutorial/ddr-style-holds-conversion.png" width=130 style="display: block; margin: 0 auto; text-align: center;">

# Visibility

The visibility of a note is a property that determines whether notes are visible or judged. Courel supports natively three types of visibility for any [Note](@ref Courel.Loader.Notes.Note):

1. [Normal](@ref Courel.Loader.Notes.Visibility.Normal): The note is visible and judged normally.
2. [Hidden](@ref Courel.Loader.Notes.Visibility.Hidden): The note is not visible, but it is judged normally.
3. [Fake](@ref Courel.Loader.Notes.Visibility.Fake): The note is visible, but it is not judged.

The notes that must be drawn in the screen can be queried via the [GetDrawableNotes](@ref Courel.Sequencer.GetDrawableNotes) method in the [Sequencer](@ref Courel.Sequencer) class. Beware that it is possible that some hidden notes (e.g. generated for [Hold](@ref Courel.Loader.Notes.Hold)s) can be asked to be judged, and not be part of the visible notes returned by the sequencer.

@page score Score

[TOC]

As stated earlier, notes are arranged in a score. In Courel, a score is an aggregate of lanes and [Row](@ref Courel.ScoreComposer.Row)s. In the picture below you can see part of a score with 3 lanes and 5 rows which have been left empty for clarity (rows are never empty). Lanes are represented as vertical lines, and they are numbered from 0 to 2. You can ask Courel to have as many lanes as you want. Rows are represented as horizontal lines (perpedicular to the lanes), and unlike the number of lanes, it is not a parameter you need to decide in advance -- the amount vary depending on the actual notes you want to place in the score.

 <img alt="Score" src="https://raw.githubusercontent.com/piured/courel/main/Imgs/Tutorial/score-lanes-and-rows.png" width=500 style="display: block; margin: 0 auto; text-align: center;">

# Usage of lanes

The usage of the lanes are very game dependant, but the most common use for them is to separate notes that must be actioned with different buttons/pads. As an example you can think of DDR having four lanes: one for each left, up, down, and right arrows. Tycho for instance has ony one lane, and all kinds of notes are placed in it. Courel does not impose any restriction on the usage of lanes, so you can use them as you wish. What you need to know is the sequeantiality restriction from the notes in the lane. Once a lane is filled up with notes, these are processed from top to bottom as the song progresses. A note in the \f$n\f$-th position of a lane won't be asked to be judged until the \f$n-1\f$-th note has been judged before. Notes in different lanes are independent from each other, so they can be judged in any order.

# Note positioning within Scores

Notes are placed in this grid by providing two pieces of information: the **beat** at which the note must be actioned, and the **lane** in which the note must be placed.

The `beat` is a number relative to the start of the song that indicates precisely when the note must be actioned. It is task of Courel to figure out when exactly the note must be actioned in after the start of the song (\f$v\f$ value, in seconds), and where it should be placed on the scrolling axis of the game (\f$w\f$ value). Do not worry on what this values are right now, as they will be somewhat covered later on. It is interesting to note that you are not providing directly the time when the note must be actioned, but rather the beat. This is because the beat is a number that is independent from the bpm of the song, and hence it is easier to work with for stepmakers. The task of retrieving \f$v\f$ and \f$w\f$, is not trivial at all if you are using the complete gimmick system that Courel implements.

The `lane` is a number that indicates in which lane the note belongs to. As stated earlier, lanes are numbered from 0 to \f$n-1\f$, where \f$n\f$ is the number of lanes in the score (a number that is defined beforehand). Placing a note in one lane or another is up to the designer of the game, as discussed earlier.

In the image below you can see five [TapNote](@ref Courel.Loader.Notes.TapNote)s placed in the score. The color of each [TapNote](@ref Courel.Loader.Notes.TapNote) just indicates the lane in which it is placed.

 <img alt="Score" src="https://raw.githubusercontent.com/piured/courel/main/Imgs/Tutorial/score-with-notes.png" width=450 style="display: block; margin: 0 auto; text-align: center;">

Note that we have notes placed at beat 0, 1, 3, 4. Since we do not have notes on beat 2, a row is not created for it.
Another noteworthy aspect of this note arrangement in lanes and rows is that notes with the same beat and different lanes are placed in the same [Row](@ref Courel.ScoreComposer.Row) object. When interacting with the [Sequencer](@ref Courel.Sequencer) via subscriptions ([ISubscriber](@ref Courel.Subscription.ISubscriber)), you will be notified in most of scenarios w.r.t. to [Row](@ref Courel.ScoreComposer.Row)s of notes instead of individual notes. This is because most rhythm games judge based on rows of notes instead of individual notes. Courel judges notes individually, but notifies the subscribers with [Row](@ref Courel.ScoreComposer.Row)s to make it more tunable for any game's needs.

When positioning notes of type [Hold](@ref Courel.Loader.Notes.Hold), you need to provide not only one two values of beat, one for the start of the hold, and one for the end of the hold. In the exampe below you can see a [Hold](@ref Courel.Loader.Notes.Hold) placed in the score between beats 1 and 3.

 <img alt="Score" src="https://raw.githubusercontent.com/piured/courel/main/Imgs/Tutorial/score-with-hold.png" width=450 style="display: block; margin: 0 auto; text-align: center;">

@page gimmick-system Gimmmick System

[TOC]

Gimmicks are means to modify the interpretation of the score at runtime. They are a very powerful tool which, when used properly, can be used to create great visual effects in the game without needing to modify the score itself. It also comes in handy for songs with unstable BPMs, or songs with pauses inbetween sections. Courel gimmick specification is inpired by Stepmania 5, so if you are familiar with it, you will feel right at home. Courel asks the gimmicks of a chart through the [IChart](@ref Courel.Loader.IChart) interface that must be implemented by the user.

Each gimmick is retrieved by a method in the interface, and they return a list of [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) objects. Each [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) is associated with a beat (which normally determines where the gimmick starts), and a value (which represents the final state of that gimmick, or a state maintained through time). Each [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) defines the state of a gimmick at a specific point (beat) w.r.t. to the score, and each one works in its own way, so the meaning of the value is different for each gimmick.

Down below we will explain visually what the gimmicks are about, but you can always check out the method documentations to learn more. However, if you really feel like having an in-depth understanding of the gimmick system, you should definitely check out [The Math Behind Gimmicks](@ref the-math-behind-gimmicks). It goes through every gimmick by providing examples, and detailing the math behind them. Indeed, Courel is an open-source implementation of the mathematical expressions found in it.

# Gimmick lifespan types

The span of time or beats each gimmick affects to is differently. Also, some gimmicks are mandatory -- need at minimum one [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) so the score can be generated properly. Think for example of a score without the BPMs gimmick properly set. There is no way of computing when and where the notes must be actioned or placed.

In general, depending on the nature of the gimmick, we separate them into three categories:

1. **Greedy**: Most gimmicks are greedy. Each [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) defining a greedy gimmick try to span as far as possible (in both directions in time) from the beat they are placed at. For example, if a greedy gimmick is defined only with one [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) value, that value will span from the defined beat until the end of the song (actually, \f$\infty\f$), and vice versa, from the beginning of the song (\f$-\infty\f$) until that beat. On the left hand side in the picture below you can see the only [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) of a greedy gimmick, whose beat is 1 (dotted line). Notice that the blue line (which represents the value) spans from \f$-\infty\f$ to \f$\infty\f$.

   <img alt="Greedy gimmicks" src="https://raw.githubusercontent.com/piured/courel/main/Imgs/Tutorial/greedy-gimmicks.png" width=650 style="display: block; margin: 0 auto; text-align: center;">

   When two or more [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair)s are defined, the span to the right of the \f$n\f$-th [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) will be delimited by the beat of the \f$n+1\f$-th [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) (the next gimmick). On the right hand side in the picture above we have a [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) at beat 1, and another at beat 3 (dotted lines), thus the span of the first gimmick is from beat \f$-\infty\f$ to beat 3, and the span of the second gimmick is from beat 3 to beat \f$\infty\f$.

   The following gimmicks are greedy:

   - BPMs
   - Scrolls
   - TickCounts
   - Combos

2. **Transitional Greedy**: Transitional greedy gimmicks behave in the same fashion as greedy gimmicks. Each [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) will try to span as far as possible until another [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) is found. However, they offer a linear transition from the value defined in the \f$n-1\f$-th [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) to the \f$n\f$-th [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair). For example, if we have a [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) at beat 1 with value 1 and transition time 0, and another at beat 2 with value 2 and transition time 1 (in beats), the value of the gimmick at beat 2.5 will be 1.5. Only speed gimmicks are transitional greedy. You can see this in the picture below.

   <img alt="Greedy gimmicks" src="https://raw.githubusercontent.com/piured/courel/main/Imgs/Tutorial/transitional-greedy-gimmicks.png" width=300 style="display: block; margin: 0 auto; text-align: center;">

3. **Range-based**: Range-based gimmicks affect only to a specific range of beats. When defining a [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) the beat value will be the start of the range, and the value will be the span of time (either in seconds or beats, depending on the actual gimmick). For example, if we have a [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) at beat 1 with value 2, the gimmick will affect from beat 1 to beat 3 (beat 1 included, beat 3 excluded, if not stated otherwise). The following gimmicks are range-based:

   - Stops
   - Delays
   - Warps
   - Fakes

# Examples set-up

In order to explain the gimmicks in a more visual way, all the examples below will be based on a score with 5 lanes and a total of 12 [TapNote](@ref Courel.Loader.Notes.TapNote)s, and 1 [PiuStyleHold](@ref Courel.Loader.Notes.PiuStyleHold). Actually, we will use [piured-engine](https://github.com/piured/engine), a Pump It Up simulator that uses Courel as its sequencer to demonstrate how gimmicks work. The score itself is shown below:

```
[
  [
    ["0", "0", "1", "0", "0"],
    ["0", "0", "1", "0", "0"],
    ["0", "0", "1", "0", "0"],
    ["1", "0", "0", "0", "1"]
  ],
  [
    ["0", "0", "1", "0", "0"],
    ["0", "0", "1", "0", "0"],
    ["0", "0", "1", "0", "0"],
    ["1", "0", "0", "0", "1"]
  ],
  [
    ["0", "0", "2", "0", "0"],
    ["0", "0", "0", "0", "0"],
    ["0", "0", "3", "0", "0"],
    ["1", "0", "0", "0", "1"]
  ]
]
```

This score is in JSON format, and it is actually the result of parsing Stepmania's SSC NOTES section with [pegjs-ssc-parser](https://github.com/piulin/pegjs-ssc-parser). In this JSON-like SSC notation, the first level array represents the score. Each second level array represents 4 beats where notes can be placed at. Thus, the first sencond level array consists of notes placed at beats 0, 1, 2, and 3, and similarly the second second level array consists of notes placed at beats 4, 5, 6, and 7. Each position in the third level arrays represent the notes at each lane (we have 5), and the values in them represent the type of note placed at that beat and lane:

- `"0"` stands for no note
- `"1"` stands for a [TapNote](@ref Courel.Loader.Notes.TapNote).
- `"2"` stands for the beginning of a [PiuStyleHold](@ref Courel.Loader.Notes.PiuStyleHold).
- `"3"` stands for the end of a [PiuStyleHold](@ref Courel.Loader.Notes.PiuStyleHold).

The default gimmick configuration, also shown in a similar JSON-like SSC format, is the following:

```
{
  "BPMs": [[0, 60]],
  "stops": [],
  "delays": [],
  "warps": [],
  "tickCounts": [[0, 1]],
  "combos": [[0, 1]],
  "speeds": [[0, 1, 0, 0]],
  "scrolls": [[0, 1]],
  "fakes": [],
}
```

They keys in this dictionary indicate the target gimmick, whereas the values correspond to the list of [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair)s associated with them. For example, the BPMs gimmick is defined with one [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) whose beat is defined in the first position of the second level array (`0`), and value is defined in the second position (`60`). This applies to all gimmicks, except for speeds, which are defined with four values: beat, value, transition time, and transition type. It is not necessary to understand what they do right away, as we will go through each one just below.

This chart (score+gimmicks) results in the following interpretation of the score:

<img alt="BPMs gimmick" src="https://raw.githubusercontent.com/piured/courel/main/Imgs/Tutorial/score.gif" width=400 style="display: block; margin: 0 auto; text-align: center;">

The gimmick system in Courel is Stepmania 5-compatible, including the following gimmicks:

# BPMs

BPM (or Beats Per Minute) is a measure of the tempo of any song. In short, is the amount of beats that occur in a minute. This value is key to keep your notes in sync with the music! A badly set BPM value is going to ruin the whole playing experience in any rhythm game. In Courel, the definition of the BPM is a gimmick itself because it is allowed to set multiple BPMs for a song (so-called BPM changes). This is useful for songs with multiple sections with different tempos, or to create some visual effects.

Note that the BPMs gimmick is a greedy gimmick.
If your song does not contain BPM changes, you need to specify anyways one "global" BPM value. This is done by returning a list with one [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) item in the [GetBpms](@ref Courel.Loader.IChart.GetBpms) method of the [IChart](@ref Courel.Loader.IChart) interface. The value of the [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) returned is the BPM value, and the beat is traditionally set to 0, although any other value will work just fine. You can define as many BPM changes as wished by adding [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair)s at the beats where the BPM change occurs, and setting the value to the new BPM value.

In the example below, we just modified the BPMs gimmick definition by adding a BPMs change at beat 4 with value 120:

```
"BPMs": [[0, 60], [4, 120]],
```

As seen below, when the song time reaches beat 4, the pace at which the notes scroll increases from 60 BPM to 120 BPM.

<img alt="BPMs gimmick" src="https://raw.githubusercontent.com/piured/courel/main/Imgs/Tutorial/example-bpms-gimmick.gif" width=400 style="display: block; margin: 0 auto; text-align: center;">

# Scrolls

Scrolls are a way to modify the _relative position_ at which the notes are placed in the scrolling axes (\f$w\f$ value) as well as the scrolling pace.

The value of each [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) determines the rate of scrolling w.r.t. to the current BPM value, and its relative position is adjusted by that rate so the notes appear in the right position w.r.t. the judging row. Therefore a value of 1 will not have any visible effect because we would be scrolling exactly at the current BPM value, and the position will not be adjusted. Values closer to 0 will make notes scroll slower and appear closer together, whilst values greater than 1 will make notes scroll faster and appear further apart. Negative values will make notes scroll backwards.

A similar effect can actually be done by creating artificial BPM changes, but in most cases this is pretty inconvenient. Remember that the BPM is a measure of the tempo of the song, and it must stay always sync with the music. When adding BPM changes, you need to rewrite the all the notes after the BPM change so they can be interacted with in the same beat, and in some extreme cases, adding too many BPM changes (specially with weird values) makes the score harder to read and maintain. There is also some some scenarios where you cannot achieve even the same results by adding BPM changes than with scrolls.

Scrolls are greedy gimmicks. If you don't want anything to do with them, just return in the [GetScrolls](@ref Courel.Loader.IChart.GetScrolls) method of the [IChart](@ref Courel.Loader.IChart) interface a list with one [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) with the beat set 0 zero and the value set to 1.

In the example below we modified the Scrolls gimmick definition by adding a scroll change at beat 4 with a value of 0, and at beat 7 with a value of 0.5:

```
"scrolls": [
  [0, 1],
  [4, 0],
  [7, 0.5]
]
```

The resulting effect is shown below. Note that from beat 4 to 7 all the notes are crunched into the same position in the scrolling axis due to the scroll value of 0. However, notes are still judged normally at beats 4, 5, 6, and 7, respectively. From beat 7 on, the scrolling and notes' relative position are reduced by a factor of 0.5.

<img alt="Scrolls gimmick" src="https://raw.githubusercontent.com/piured/courel/main/Imgs/Tutorial/example-scroll-gimmick.gif" width=400 style="display: block; margin: 0 auto; text-align: center;">

# TickCounts

TickCounts is a greedy gimmick affecting only [PiuStyleHold](@ref Courel.Loader.Notes.PiuStyleHold)s. As explained before, [PiuStyleHold](@ref Courel.Loader.Notes.PiuStyleHold)s generate judgment events at a certain rate in order to mimmick the behaviour of holds in the Pump It Up original arcade. Under the hood, this is done by generating many [HoldNote](@ref Courel.Loader.Notes.HoldNote)s with [Hidden](@ref Courel.Loader.Notes.Visibility.Hidden) visibility. The rate at which these [HoldNote](@ref Courel.Loader.Notes.HoldNote)s are generated is determined by the `TickCount` gimmick. The value of each [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) determines the rate of [HoldNote](@ref Courel.Loader.Notes.HoldNote)s generated per beat. A value of 1 will generate one [HoldNote](@ref Courel.Loader.Notes.HoldNote) per beat, a value of 2 will generate two [HoldNote](@ref Courel.Loader.Notes.HoldNote)s per beat, and so on. Negative values are not allowed.

If your game uses [PiuStyleHold](@ref Courel.Loader.Notes.PiuStyleHold)s you must return an non-empty list in the [GetTickCounts](@ref Courel.Loader.IChart.GetTickCounts) method of the [IChart](@ref Courel.Loader.IChart) interface.

In the example below, we modified the TickCounts gimmick definition by adding a tick count change at beat 9 with a value of 16:

```
"tickCounts": [
  [0, 1],
  [9, 16]
]
```

Note how at the middle of the hold (beat 9), the amount of judgments generated increases from 1 to 16 per beat.

<img alt="TickCounts gimmick" src="https://raw.githubusercontent.com/piured/courel/main/Imgs/Tutorial/example-tickcounts-gimmick.gif" width=400 style="display: block; margin: 0 auto; text-align: center;">

# Combos

Combos is a greedy gimmick that affects the combo contribution property of [SingleNote](@ref Courel.Loader.Notes.Hold)s which can be accesed through the method [GetCombo](@ref Courel.Loader.Notes.SingleNote.GetCombo). It is important that with independence of the combos value, notes are always judged once. The value of each [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) determines the amount of combo contribution that each [SingleNote](@ref Courel.Loader.Notes.SingleNote) will have. A value of 1 will make each [SingleNote](@ref Courel.Loader.Notes.SingleNote) contribute 1 to the combo, a value of 2 will make each [SingleNote](@ref Courel.Loader.Notes.SingleNote) contribute 2 to the combo, and so on.

You must always return a non-empty list in the [GetCombos](@ref Courel.Loader.IChart.GetCombos) method of the [IChart](@ref Courel.Loader.IChart) interface, even if you are not using this property.

In the example below, we modified the Combos gimmick definition by adding a combo change at beat 4 with a value of 2:

```
"combos": [
  [0, 1],
  [4, 2]
]
```

Note how the combo contribution of each note is doubled from beat 4 on.

<img alt="Combos gimmick" src="https://raw.githubusercontent.com/piured/courel/main/Imgs/Tutorial/example-combos-gimmick.gif" width=400 style="display: block; margin: 0 auto; text-align: center;">

# Speeds

Speeds is a transitional greedy gimmick that affects the drawing positions of notes as well as the scrolling pace at runtime. Unlike Scrolls, it modifies the _absolute position_ of the notes in the scrolling axis globally (all notes). The value of each [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) determines the rate of scrolling w.r.t. to an unitary value (which will be discussed later on) as well as the spacing of the notes. Augmenting the speed value will cause the scroll rate to speed up, and so the spacing between notes.

Since Speeds gimmicks are transitional greedy, they are able to transition from one value to another smoothly (linearly). The transition time can be specified in terms of beats or seconds, although the most common way is to use beats. You are allowed to set the transition time to 0, which will cause the transition to be instantaneous. Negative speed values will cause the scrolling axis to reverse.

You must always return a non-empty list in the [GetSpeeds](@ref Courel.Loader.IChart.GetSpeeds) method of the [IChart](@ref Courel.Loader.IChart) interface. If you are not using this gimmick, just return a list with one [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) with the beat set to 0, the value set to your desired global speed, the transition time set to 0, and the transition type set to 0.

In the example below, we modified the speeds in the following fashion:

```
"speeds": [
  [0, 1, 0, 0],
  [1, 0.25, 1, 0],
  [3, 0, 1, 0],
  [5, -0.5, 0.5, 0],
  [7, 4, 0.5, 0],
  [9, 1, 1, 0]
]
```

where the first element of each [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) is the beat, the second element is the value, the third element is the transition time, and the fourth element is the transition type (`0` stands for beat). The resulting effect is shown below.

<img alt="Speeds gimmick" src="https://raw.githubusercontent.com/piured/courel/main/Imgs/Tutorial/example-speeds-gimmick.gif" width=400 style="display: block; margin: 0 auto; text-align: center;">

# Stops

Stops is a range-based gimmick which artificially stops the song time for a certain amount of seconds. Any note placed at the stopped beat will be judged normally. Notes coming after the stopped beat will be judged normally after the stop has ended. The best use case for Stops (and also Delays) is to sync the BPM with a song, when the music has changed its tempo, or when the music has stopped at some point (e.g. a pause).

The value of each [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) determines the amount of seconds the song time will be stopped. A value of 1 will stop the song time for 1 second, a value of 2 will stop the song time for 2 seconds, and so on. Negative values are not allowed.

You must always return a non-empty list in the [GetStops](@ref Courel.Loader.IChart.GetStops) method of the [IChart](@ref Courel.Loader.IChart) interface. If you are not using this gimmick, just return an empty list.

In the example below (left hand side, in Delays section), we modified the Stops gimmick definition by adding a stop at beat 3 with a value of 1, and at beat 9 with a value of 0.5:

```
"stops": [
  [2, 1]
  [9, 0.5]
]
```

Notice how the stopped song time stops for 1 second at beat 3. Also, note that the note placed at beat 3 is judged before the stops takes place.

# Delays

Delays is a range-based gimmick which operates exactly the same as Stops. The only difference is that as its name might suggest, the song time is delayed instead of stopped. This causes that a note placed at the beat where a delay occurs, will be judged right after the delay has ended. Similarly to notes, judged coming after the delayed beat will be judged normally after the delay has ended. The use case for Delays is the same as for Stops.

The value of each [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) determines the amount of seconds the song time will be delayed w.r.t. to the beat it is placed at. A value of 1 will delay the song time for 1 second, a value of 2 will delay the song time for 2 seconds, and so on. Negative values are not allowed.

You must always return a non-empty list in the [GetDelays](@ref Courel.Loader.IChart.GetDelays) method of the [IChart](@ref Courel.Loader.IChart) interface. Charts not using this gimmick shall return an empty list.

To show the difference w.r.t. to the Stops gimmick above, we modified the Delays gimmick by adding exactly the same delays as in the Stops gimmick:

```
"delays": [
  [2, 1]
  [9, 0.5]
]
```

On the right hand side you can see that the note placed at beat 3 is judged after the delay has ended. Since we are using a BPM of 60, the delay of 1 second, the note will be judged as if it was placed at beat 4.

<img alt="Stops gimmick" src="https://raw.githubusercontent.com/piured/courel/main/Imgs/Tutorial/example-stops-gimmick.gif" width=370>
<img alt="Delays gimmick" src="https://raw.githubusercontent.com/piured/courel/main/Imgs/Tutorial/example-delays-gimmick.gif" width=370>

# Warps

Warps is a range-based gimmick that allows to skip a certain amount of beats in the score. Notes placed inside the range defined by the [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) become [Fake](@ref Courel.Loader.Notes.Visibility.Fake) notes, therefore they will not be judged (obviously, because they cannot be actioned in time). Notes coming after the warped beat will be judged normally. Warps are mostly used to create visual effects.

The value of each [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) determines the amount of beats that will be warped over. A value of 1 will skip 1 beat, a value of 2 will skip 2 beats, and so on. Negative values are not allowed.

You must always return a non-empty list in the [GetWarps](@ref Courel.Loader.IChart.GetWarps) method of the [IChart](@ref Courel.Loader.IChart) interface. Charts not using this gimmick shall return an empty list.

In the example below, we modified the Warps gimmick definition by adding a warp at beat 3 with a value of 4, so will it skill skip 4 beats.

```
"warps": [[3, 4]]
```

Notice how notes placed at beat 3, 4, 5, and 6 appear disappear and the hold appears in less than the flick of a finger, creating the ilusion of note replacement.

<img alt="Warp gimmick" src="https://raw.githubusercontent.com/piured/courel/main/Imgs/Tutorial/example-warp-gimmick.gif" width=400 style="display: block; margin: 0 auto; text-align: center;">

# Fakes

The last gimmick is Fakes, which is a range-based gimmick that allows to assign [Fake](@ref Courel.Loader.Notes.Visibility.Fake) visibility to notes by range. Notes placed inside the range defined by each [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) become [Fake](@ref Courel.Loader.Notes.Visibility.Fake) notes, therefore they will not be judged. Notes coming after the fake beat will be judged normally. Similarly to Warps, Fakes are mostly used to create visual effects.

The value of each [GimmickPair](@ref Courel.Loader.GimmickSpecs.GimmickPair) determines the amount of beats that will be faked over w.r.t. the beat it is placed at. A value of 1 will fake 1 beat, a value of 2 will fake 2 beats, and so on. Negative values are not allowed.

You must always return a non-empty list in the [GetFakes](@ref Courel.Loader.IChart.GetFakes) method of the [IChart](@ref Courel.Loader.IChart) interface. Charts not using this gimmick shall return an empty list.

To show the difference w.r.t. to the Warps gimmick above, we added a Fake gimmick at beat 3 with a value of 4, so notes placed at beats 3, 4, 5, and 6 become [Fake](@ref Courel.Loader.Notes.Visibility.Fake) notes (but they will not be warped over).

```
"fakes": [[3, 4]]
```

<img alt="Warp gimmick" src="https://raw.githubusercontent.com/piured/courel/main/Imgs/Tutorial/example-fakes-gimmick.gif" width=400 style="display: block; margin: 0 auto; text-align: center;">

# Gimmick combination

Gimmicks can be combined in any way you want to create wonderful visual effects. Down below there are some examples of the things that some stepmakers can achieve.

<img alt="Fav gimmicks" src="https://raw.githubusercontent.com/piured/courel/main/Imgs/Tutorial/fav-gimmicks.gif" width=400 style="display: block; margin: 0 auto; text-align: center;">

@page drawing-and-scrolling-notes Drawing and Scrolling Notes

[TOC]

So far we have already covered the basics of the notes, score, and gimmicks. Both setting the notes in the score (by assigning to each one of them a beat and lane) and setting up all the gimmicks is just enough to determine where each note should be placed on the scrolling axis at any song time \f$t\f$, as well as when they should be actioned by the user. Calculating these values goes out of the scope of this manual, but as said earlier, if you wanna have a look at the math behind it, you can learn more [here](@ref positioning-and-scrolling-notes)

Courel is just a sequencer, so it does not provide any means to draw the notes on the screen, or to scroll them. It is up to the end user to implement this. What Courel will give you though is all the values you need to do it.

# Reference value: the unitary value

Courel assumes the following spatial properties for the game objects of the rhythm game:

<img alt="Fav gimmicks" src="https://raw.githubusercontent.com/piured/courel/main/Imgs/Tutorial/unitary-value-scrolling-axis.png" width=200 style="display: block; margin: 0 auto; text-align: center;">

The scrolling axis (shown as a dotted vertical line) is a virtual line which represents the path taken by the notes as they scroll w.r.t. to the song time towards the receptor (more on this later on). This axis can be placed anywhere in the screen, and it can be oriented in any direction. That is up to the designer of the game. You can think of DDR having the scrolling axis from bottom to top, so the notes scroll upwards when the song time increases, or Tycho having it from left to right.

- What is important is that the size of the note aligned with the scrolling axis is assumed to be one unit. This is what we refer to as the unitary value. In the example above you can see that the height of gray bounding box around the note is of size one.

- Another assumption made by Courel is that notes that are one beat apart from each other, will be positioned one unit apart in the scrolling axis, as seen below.

<img alt="Fav gimmicks" src="https://raw.githubusercontent.com/piured/courel/main/Imgs/Tutorial/unitary-spacing-scrolling-axis.png" width=200 style="display: block; margin: 0 auto; text-align: center;">

# Relative and absolute position

Once you have loaded a chart through the method [LoadChart](@ref Courel.Sequencer.LoadChart) in the [Sequencer](@ref Courel.Sequencer) class, Courel reads the notes and gimmicks, and updates the internal properties of the notes in the score. The next time you query the notes to be drawn in the screen via the [GetDrawableNotes](@ref Courel.Sequencer.GetDrawableNotes), these properties can be used to determine the position of the notes in the scrolling axis.

The most important property is the \f$w\f$ value, which can be queried via the [WBegin](@ref Courel.Loader.Notes.Note.WBegin) method in any [Note](@ref Courel.Loader.Notes.Note) class (holds have two \f$w\f$ values -- [WBegin](@ref Courel.Loader.Notes.Hold.WBegin) and [WEnd](@ref Courel.Loader.Notes.Hold.WEnd)). We called it \f$w\f$ just to keep it consistent with the equations behind it. \f$w\f$ is referred to as the relative position.

The **relative position** of a note is the position the note must be drawn in the scrolling axis when the song time \f$t=0\f$. Obviously, this value alone is not very useful, because it does not tell us where the note must be drawn at any other song time.

The position of a note in the scrolling axis at any given song time \f$t\f$ is referred to as its **absolute position**. Courel does not calculate this value for every note for you due to performance reasons -- most of the time you will be culling pretty much all of the drawable notes, so you do not need to calculate these values for all them. Instead, it provides you with the values you need to calculate it yourself.

The absolute position of a note can be calculated by quering the current scroll \f$c\f$ and speed \f$s\f$ values (via [GetScroll](@ref Courel.Sequencer.GetScroll) and [GetSpeed](@ref Courel.Sequencer.GetSpeed) methods), and the relative position \f$w\f$ of the note. The equation is the following:

\f[
\text{Absolute position} = (-w + c) \times s\,.
\f]

Since \f$c\f$ and \f$s\f$ are function of \f$t\f$, the absolute position of the note will change as the song time progresses.

# Scrolling notes

The calculus of the absolute position must be done at every frame (in your `Update` method, for example). By doing so and updating your game object position transform in the scrolling axis accordingly, you will be scrolling them at the right pace. Just as simple as that.

@page judging-notes Judging Notes

[TOC]

The process of assigning a judgment to a note based on the user's input is called judging. Courel allows developers to define custom jugdments, so you can implement the one that fits best for your game. However, Courel needs to know when a note has been judged positively, has been missed, or the action is premature in order to keep its internal state consistent.

The judgment of notes is produced when a note approaches the receptor and the user actions it with the right timing and input event. The place where the receptor is placed (at position 0 w.r.t. the scrolling axis) is called the judgment row.

<img alt="Fav gimmicks" src="https://raw.githubusercontent.com/piured/courel/main/Imgs/Tutorial/judging-row.png" width=500 style="display: block; margin: 0 auto; text-align: center;">

At first glance, it might seem reasonable to use the absolute position of a note to determine how good or bad the judgment should be: the further away a note is w.r.t. to the receptor the worse the judgment would be, and the closest the better. This would be kind of acceptable if we did not have any gimmick that could modify the positioning of the notes during runtime (e.g. see Scrolls and Speeds gimmicks). Courel sorts out this problem by assigning to each [SingleNote](@ref Courel.Loader.Notes.SingleNote) a \f$v\f$ value: the action time.

# Action time

The action time \f$v\f$ of a note is the exact time when a note is expected to be actioned by the user w.r.t. the song time, and luckily is invariant to its absolute position. It is calculated w.r.t. the beat a note is placed at and all the gimmicks that affect the various time spaces of the sequencer. The \f$v\f$ value lives in the exact same space as the song time \f$t\f$, so it is measured in seconds and can be compared to it. You can query the \f$v\f$ value of a note via the [VBegin](@ref Courel.Loader.Notes.SingleNote.VBegin) method, although you will not need to access it directly in most situations.

# Delta time

What you most likely need to use is the delta time of a note w.r.t. to an input event. The delta time of a note is the difference between the action time \f$v\f$ of the note and the song time \f$t\f$ when an input event is produced. The delta time has the following properties:

- It is negative when a note is actioned **before** its action time,
- positive when it is actioned after its action time,
- and zero when the note is actioned just at its action time.

This means that the lower the delta time, the closer is the user's action to the right timing. In all rhythm games that I know of, this translates into better judgments.

# Judgment System

The native judgment system that Courel provides allows informing the sequencer when a note has been hit (or customly judged), has been missed, or its premature to produce a judgment. Any custom judgment system must derivate from [Judgment](@ref Courel.Judge.Judgment) to provide at least the same functionality. This is enforced by the use of the [IJudge](@ref Courel.Judge.IJudge) interface.

Courel is only needs to be aware of three judgment outcomes to operate properly. For explaination purposes only, in the following examples we will use the distance of notes w.r.t. to the receptor in the scrolling axis as mean to determine the judgment, but remember that this is not the way Courel does it.

<img alt="Fav gimmicks" src="https://raw.githubusercontent.com/piured/courel/main/Imgs/Tutorial/judge-areas.png" width=400 style="display: block; margin: 0 auto; text-align: center;">

# Premature judgment

A premature judgment is produced when a note is actioned way before its action time. So before, that we cannot even tell if the user is trying to action the note or not. You can see the blue tap note in the picure above as an example. In such case, the [Premature](@ref Courel.Judge.Judgment.Premature) property must be set to true. When a premature judgment is produced, Courel will not notify subscribers of the event, and the note can be asked to be judged again normally later on. In short, a premature judgment does not alter the state of the sequencer.

# Miss judgment

When a note goes past the judgment row without being actioned, it is considered a miss in most rhythm games (such as the red tap note in the picture above). To inform the sequencer that a note has been missed, [Miss](@ref Courel.Judge.Judgment.Miss) property must be set to true. In such situation, Courel will notify subscribers of the event, and the note won't be asked to be judged again.

# Hit

In most rhythm games, a note is hit when it is actioned at the right timing. Most of the times, we define an area around the receptor where notes can be hit (but at the end of the day this is up to the designer of the game). The closer the note to the recepor, the better the timing was, and therefore the better the judgment (in whatever scale you use). Recall that determining the judgment is made through the delta time, not the actual distance between the note and the receptor. You can see the hit area in the picture above, where a yellow tap note can be seen.

To inform the sequncer that a note has been hit, [Premature](@ref Courel.Judge.Judgment.Premature) and [Miss](@ref Courel.Judge.Judgment.Miss) properties must be set to false. In such situation, Courel will notify subscribers of the event, and the note won't be asked to be judged again.

# Custom judgments

The judgment system in Courel is extensible to custom judgments. Custom judgments will most likely target the hit area. Declare your judgment class by derivate from [Judgment](@ref Courel.Judge.Judgment) and implement the [IJudge](@ref Courel.Judge.IJudge) interface accordingly. As we well see later on, notes can be asked their judgment (therefore your custom judgments) when notified to subscribers. Beware, the native judgment system must be preserved, so you must always set the [Premature](@ref Courel.Judge.Judgment.Premature) and [Miss](@ref Courel.Judge.Judgment.Miss) properties accordingly.

# Judging holds

Holds are not judged per se. As reviewed before, [Hold](@ref Courel.Loader.Notes.Hold)s are always mapped to [SingleNote](@ref Courel.Loader.Notes.SingleNote)s, and that is what you actually judge as far as notes are concerned.

However, the tail of a hold is asked to be judged w.r.t. the end action time \f$v\f$. The end action time of a hold is the action time of the hold at the end of its life.

# Activeness of holds

There is another aspect of holds that is somewhat judged: its activeness. The activeness of a hold is a property that determines if a hold is active or not. An active hold can be reacted to it, and an inactive hold cannot. You can also think of an inactive hold as a hold that has been missed. For example, holds in DDR are missed when the user releases the hold before the end of its life, or in Courel terminology, become inactive.

To determine the activennes of holds, specially for [DdrStyleHold](@ref Courel.Loader.Notes.DdrStyleHold) and [DdrStyleRollHold](@ref Courel.Loader.Notes.DdrStyleRollHold), it is useful to check out the methods [GetElapsedtimeInactive](@ref Courel.Loader.Notes.DdrStyleHold.GetElapsedTimeInactive) and [GetElapsedTimeActive](@ref Courel.Loader.Notes.DdrStyleRollHold.GetElapsedTimeActive). They return the elapsed time in seconds since the hold was last held or released, respectively. You can use a threshold to establish a criterion do determine if a hold is active or not. [PiuStyleHold](@ref Courel.Loader.Notes.PiuStyleHold)s, at least in the original Pump It Up arcade, are always active (you can hit them anytime).

# Asking for judgments

Courel asks for judgments through an instance of a class derivate from [IJudge](@ref Courel.Judge.IJudge) when appropiate. This class must be implemented appropiately by the user of Courel, and it is passed to and existing [Sequencer](@ref Courel.Sequencer) instance via the [SetIJudge](@ref Courel.Sequencer.SetIJudge) method.

@page input-events Input Events

[TOC]

Courel does not provide any means to capture input events. It is up to the user to implement this.
Input events must be passed to Courel via [Tap](@ref Courel.Sequencer.Tap) and [Lift](@ref Courel.Sequencer.Lift) method calls for tap and lift events, respectively. Hold states are retrieved by setting an instance of [IHoldInput](@ref Courel.Input.IHoldInput) via the [SetIHoldInput](@ref Courel.Sequencer.SetIHoldInput) method. This class must be implemented appropiately by the user of Courel.

@page sequencer-events Sequencer Events

[TOC]

Events are the way Courel communicates with the outer world to notify something relevant has happened during runtime, allowing you to react to it accordingly. You subscribe to sequencer events by implementing the interface [ISubscriber](@ref Courel.Subscription.ISubscriber) and passing an instance of it to the [AddSubscriber](@ref Courel.Sequencer.AddSubscriber) method. You can pass as many subscribers as you want, and all of them will be notified when an event occurs.

In the following sections we will review the events that Courel can notify to subscribers.

# Missed notes on Row

[OnMissedSingleNotesOnRow](@ref Courel.Subscription.ISubscriber.OnMissedSingleNotesOnRow) event is triggered as soon as any [SingleNote](@ref Courel.Loader.Notes.SingleNote) in a row has been judged as miss. Remember that missed notes are dependant on your implementation of [IJudge](@ref Courel.Judge.IJudge). The event is triggered only once per each row. If a row is rolled back, the same row can trigger the event again.

Use this event for example to break the combo, trigger a miss tween, or play a sound effect.

# SingleNotes passing the judgment row

[OnHoveringReceptorSingleNotes](@ref Courel.Subscription.ISubscriber.OnHoveringReceptorSingleNotes) is triggered as soon as the action time \f$v\f$ of notes in a row is equal to the song time \f$t\f$ (by definition, all notes in the same row have the same action time \f$v\f$). This means that this method is called when the notes are expected to be actioned for the user. The event is triggered only once per each row. If a row is rolled back, the same row can trigger the event again.

Use this event for example to assist the player with timing by playing a clap sound effect.

# Active holds in action range

[OnActiveHold](@ref Courel.Subscription.ISubscriber.OnActiveHold) is triggered when a hold is in action range and active. Recall that a hold is active when it can be reacted to, and that it is part of the judging system to assess the activeness of a hold.

The **action range** of a hold though, is defined as the difference between the action time \f$v\f$ of the hold and the end action time \f$v\f$ of the hold. We say that a hold is in action range when the song time \f$t\f$ is greater or equal to the action time and less or equal to the end action time. In other words, this method is called when the hold is expected to be held by the user, as long as it is active.

This event is triggered multiple times per hold (per each Update call at the sequencer), as long as the hold is in action range and active. If a hold is rolled back, the same hold can trigger the event again.

Use this event for example to reposition the head of the note in the screen. When the hold is being held, the head of the hold is usually placed so it hovers the receptor in order to indicate the user that the hold is being held.

# Hold reaches end of lifetime

[OnHoldEnded](@ref Courel.Subscription.ISubscriber.OnHoldEnded) is called when a hold is active and has reached its end action time w.r.t. the song time \f$t\f$. This means that the hold is expected to be released by the user. This method is called only once per hold. If a hold is rolled back, the same hold can trigger the event again. Beware that this method can be called even though the hold is not being held.

If you need to asses the end of a hold, use (@ref Courel.Subscription.ISubscriber.OnHoldEndJudged) instead.

This event can be used for instance to create a visual effect when the hold is completed (like in DDR, the "O.K." toast).

# SingleNote is judged

[OnJudgedSingleNoteOnRow](@ref Courel.Subscription.ISubscriber.OnJudgedSingleNoteOnRow) is arguably the most important event to attend to is this one. This method is called when a note has been judged as a hit (i.e. judged as not premature and not missed). This method is called only once per note, but multple times per row. If a row is rolled back, the same note can trigger the event again.

This event is mostly used to update increment the combo count, show a judgment animation, remove notes from the scene, etc. You can access the judgment of the note via the [Judgment](@ref Courel.Loader.Notes.Note.Judgment) method. As most rthythm games evaluate w.r.t. all the notes in a row being judged, this method also provides the row where the note is placed. Check the methods of [Row](@ref Courel.ScoreComposer.Row), some of them might be useful to you.

# Hold becomes inactive

[OnHoldInactive](@ref Courel.Subscription.ISubscriber.OnHoldInactive) is called when an active hold and has become inactive. This means that the hold has been missed, or in Courel terminology, has been released before the end of its life. This method is called only once per hold. If a hold is rolled back (or partially rolled back), the same hold can trigger the event again.

This event is mostly used to visually indicate the user that the hold has been missed and cannot be actioned anymore.

# Rolling back notes

Before diving into the roll back events, it is worth clarifying when notes are rolled back. When using the sequencer in a regular set-up scenario, the song time \f$t\f$ increases over time. As the song progresses, you draw the notes in the screen accordingly to their absolute position, and you attend to the events that Courel notifies to you to keep your game's visuals consistent.

Courel also allows the song time \f$t\f$ to decrease over time at any given song time. When this happens, all previous notes that have been judged until the new song time need to be reset and sorted out in the score so they can be judged again. Putting notes back into the score is what we call rolling back notes.

Courel notifies you when notes are rolled back so you can react to it accordingly. Most rhythm games remove notes from the scene (or cull them) as soon as they are judged, or go out of the screen. When notes are rolled back, it is likely that you will need to put them back into the scene so the user can interact with them again.

Courel's rolling back capabilities allow to create e.g. training sessions on certain parts of a chart that are difficult for the player without needing to restart the song from the beginning. (Although this is something you need to implement yourself).

# Rolling back SingleNotes

[OnRolledBackSingleNotesOnRow](@ref Courel.Subscription.ISubscriber.OnRolledBackSingleNotesOnRow) is called when all notes in a row are rolled back. It is called only once per each row when rolling back.

This method is mostly used to redraw the notes on the screen.

# Partially rolling back Holds

[OnHoldIsPartiallyRolledBack](@ref Courel.Subscription.ISubscriber.OnHoldIsPartiallyRolledBack) is called at each `Update` call until a hold is completely rolled back. A hold is partially rolled back when the new song time \f$t\f$ is in the hold's action range.

This method is mostly used to redraw the hold in the screen, and position is head accordingly.

# Hold is rolled back completely

Unlike the event [OnHoldIsPartiallyRolledBack](@ref Courel.Subscription.ISubscriber.OnHoldIsPartiallyRolledBack), [OnHoldIsRolledBack](@ref Courel.Subscription.ISubscriber.OnHoldIsRolledBack) is called only once per hold when the hold is completely rolled back, i.e. when the new song time \f$t\f$ is before the action time of the hold.

This method is also mostly used to redraw the hold in the screen, and position is head accordingly.

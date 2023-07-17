<img alt="Courel" src="../../Imgs/Logos/courel-dark.png" width=350 style="display: block; margin: auto; text-align: center;">
<br>
<br>

# Introduction

## What is Courel

Courel is a stepmania-compatible open-source sequencer for rhythm games.
When developing a scroll-based rhythm game, Courel will help you to:

1. Determine where the notes should be placed on the screen at any given time.
2. Determine the timing of the notes.
3. Judge the player's input.
4. React to game events.

Courel implements the [gimmick-system](https://github.com/piured/sequencer-guide) of Stepmania 5 in C# and it is designed to be used in Unity.

## What Courel is not

Courel **is not** a game. It is a tool that will help you to develop your own rhythm game. It does not include any graphics, sound effects or music.
It does not try to draw anything on the screen and it does not interact with the player in any way.

## Games using Courel

- [Piured](https://github.com/piured/engine): A _Pump It Up_ simulator.

# Getting Started

## Installation

We are trying to make Courel available through the Unity Package Manager as soon as possible.
Until then, the easist way to set up Courel in your unity project is to add it as a git submodule.

Steps (for Linux):

1.  Create or open your Unity project.
2.  Set up a git repository for it if you haven't already.
3.  Open a terminal and navigate to the root folder of your project.
4.  Then, create a folder for your submodules and navigate to it:

        $ mkdir Submodules
        $ cd Submodules

5.  After that, add Courel as a submodule:

        $ git submodule add https://github.com/piured/courel.git

6.  Once the submodule is cloned, navigate to you `Assets` folder and create a folder for Courel inside the plugins folder.

        $ mkdir -p Plugins/Courel

7.  Finally, create a symbolic link to the Courel folder in your submodule:

        $ ln -s ../../../Submodules/courel/Assets Assets

## Read the docs!

It is highly recommended to read the pages on the sidebar to familiarize yourself with the concepts and terminology used in Courel.
These are key:

- [Notes](@ref notes)
- [Score](@ref score)
- [Gimmick System](@ref gimmick-system)
- [Drawing and Scrolling Notes](@ref drawing-and-scrolling-notes)
- [Judging Notes](@ref judging-notes)
- [Input Events](@ref input-events)
- [Sequencer Events](@ref sequencer-events)

If you want to go deep into the details, have a look at [The Math Behind Gimmicks](@ref the-math-behind-gimmicks).

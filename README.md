<p align="center">
<picture>
 <source media="(prefers-color-scheme: dark)" srcset="Imgs/Logos/courel-dark.png">
 <img alt="Courel" src="Imgs/Logos/courel-light.png" width=350>
</picture>
</p>

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

## Made with Courel

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

It is highly recommended to read the [documentation](https://piured.github.io/courel/index.html) in order to familiarize yourself with the concepts and terminology used in Courel. There you will find a guide on how to use Courel in your project and a reference for all the classes and methods available.

# Contributing

We welcome contributions from anyone and everyone.

If you'd like to contribute, please follow these steps:

1. Fork the repository and clone it to your local machine.
2. Make your changes and test them thoroughly.
3. Commit your changes with a descriptive commit message.
4. Push your changes to your fork.
5. Open a pull request to the original repository.

# Contribution Guidelines

To ensure that your contributions are accepted, we suggest you to:

- Adhere to the coding style and conventions used in the project.
- Write clear and concise commit messages that describe your changes.
- Be respectful and constructive in your comments and criticisms.
- If you're proposing a new feature or improvement, please explain why it's necessary and how it would benefit the project.
- If you're reporting a bug, please include as much information as possible to help us reproduce the issue.

# License

This code is licesend under the terms of the AGPL-3.0 license.

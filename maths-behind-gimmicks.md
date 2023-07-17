@page the-math-behind-gimmicks The Math Behind Gimmicks

[TOC]

In this guide we will delve into gimmicks, and more specifically into the math behind them.
We will examine the StepMania approach to defining the
settings required to set up all different gimmicks that courel implements. Each definition will be
analyzed in its own separate section, where we will provide examples and
the mathematical formalization of how each one of them is implemented.

The picture below is kind of a roadmap of all the functions and spaces that we will be working with. Might be useful
to have it in mind while reading the guide.

<img src="../../Imgs/Understanding-Gimmicks/figure-10.png" width=800 style="display: block; margin: 0 auto; text-align: center;" ref="fig:wwarps">

# From song time to sequencer time

## Introduction

The `SSC` format provides three gimmicks to artificially skip or pause
the note scrolling when the song is playing. Those gimmicks are
`#WARPS`, `#STOPS`, and `#DELAYS`. WARP gimmicks are concerned with
skipping part of the song along with its notes (rendering them as fake
notes). STOPS and DELAYS are gimmicks that allow to stop the scrolling
of the song for a certain amount of time. Although there is a subtle
difference between them, they are essentially identical w.r.t. the
effect that produces. Let us review them one by one to see what each one
of them is doing.

One STOPS definition might look as follows:

        #STOPS: 4=5,6=2;

Let us convert the definition into a JSON-like structure:

      {
        [
          beat: 4,
          stop: 5
        ],
        [
          beat: 6,
          stop: 2
        ]
      }

This `#STOPS` gimmick is telling us a couple of things.

1.  At beat 4, stop the scrolling for 5 seconds. Then resume.

2.  At beat 6, stop the scrolling for 2 seconds. Then resume.

Similarly, one DELAYS defition could look like this:

        #DELAYS: 4=5,6=2;

And after converting it into the friendly structure:

      {
        [
          beat: 4,
          delay: 5
        ],
        [
          beat: 6,
          delay: 2
        ]
      }

The `#DELAYS` definition above is telling us essentially the same story
as the STOPS definition shown before, i.e.:

1.  At beat 4, stop the scrolling for 5 seconds. Then resume.

2.  At beat 6, stop the scrolling for 2 seconds. Then resume.

The difference between these two is that notes that lie exactly at the
stop and delay beat, will need to be tapped before and after the waiting
time for the stops and delays, respectively.

Finally, suppose we have the following WARPS definition:

        #WARPS:2=1,3=2;

It is equivalent to this one:

      {
        [
          beat: 2,
          warp: 1
        ],
        [
          beat: 3,
          warp: 2
        ]
      }

As it turns out, this gimmick is telling us the following information:

1.  At beat 2, warp over the next 1 beat, i.e. skip one beat. Notes with
    beats in the range \f$[2, 3+1) = [2,3)\f$ will become fake notes.

2.  At beat 3, warp over the next 2 following beats, i.e. skip two
    beats. Notes with beats in the range \f$[3, 3+2) = [3,5)\f$ will become
    fake notes.

Note that with this example definition, we could write an equivalente
WARP definition that would produce the same result:

        #WARPS:2=3;

## Challenge

First, we would like to have a function
\f$q: \mathbb{G}\rightarrow \mathbb{S^*}\f$ that maps the stopped time
\f$\mathbb{G}\f$ into the sequencer time \f$\mathbb{S^*}\f$ (i.e. song time with
skips), as well as its inverse
\f$q^{-1}:\mathbb{S^*}\rightarrow \mathbb{G}\f$ that maps from the sequencer
time into the delayed/stopped time. Additionally, we would like to have
a pair of functions \f$t_{(s)}: \mathbb{D} \rightarrow \mathbb{G}\f$, and
\f$t_{(d)}: \mathbb{S} \rightarrow \mathbb{D}\f$ such that
\f$t_{(d)}\circ t_{(s)}: \mathbb{S}\rightarrow \mathbb{G}\f$ would retrieve
the stopped time after stops and delays from the song time \f$\mathbb{S}\f$.
Additionally, we would like to have two inverse functions of
\f$t_{(s)}, t_{(d)}\f$, \f$t^{-1}_{(s)}\f$ for stops and \f$t^{-1}_{(d)}\f$ for
delays so
\f$t_{(s)}^{-1}\circ t_{(d)}^{-1}: \mathbb{G}\rightarrow \mathbb{S}\f$ is
able to map from the delayed/stopped time into the song time.

## Solution

To do so, let \f$f^{-1} : \mathbb{B}\rightarrow \mathbb{S^*}\f$ be a
function calculates the sequencer time given a beat. To simplify things
further, imagine that this song is has constant BPM of 60, so each
second is worth 1 beat. Then \f$f^{-1}(b) = b\f$, being \f$b\f$ a beat.

Alright, under this assumption, we can start working out a function for
the WARPS gimmick. One problem that we might have is while WARPS are
defined in the beat space, as we saw earlier, they act in the sequencer
time space. This is very counterintuitive – we are not actually skipping
beats, but we are actually skipping seconds. That is why we would need a
function (in the real life) \f$f^{-1}\f$ to convert from one space to the
other. Actually, all the functions that we are going to define in the
“Formalization” sections throughout this document are just mappings from
one space to the other. This function \f$f^{-1}\f$ that I am referring to
will be described later on.

Anyways, given that our BPM is 60, beats are equivalent to seconds –
this is just to keep it simple enough. Knowing this, we can write the
function

\f[
q(x) = \begin{cases}
x\,, & \text{if $ x \leq 2 $}\,;\\
x+1\,, & \text{if $2-0 < x \leq 3-1 $}\,;\\
x+1+2\,, & \text{if $ x > 3-1 $}\,;\\
\end{cases}
\f]

that will skip from beat 2 to beat 3, and from
beat 3 to beat 5. You can see the plot of \f$q\f$ below

<img src="../../Imgs/Understanding-Gimmicks/figure-1.png" width=400 style="display: block; margin: 0 auto; text-align: center;" ref="fig:wwarps">

Great! The inverse function

\f[
q^{-1}(x) = \begin{cases}
x\,, & \text{if $ x \leq 2 $}\,;\\
2\,, & \text{if $2 < x \leq 2+1 $}\,;\\
2\,, & \text{if $3 < x \leq 3+2 $}\,;\\
x - 2 - 1\,, & \text{if $x > 3+2 $}\,;\\
\end{cases}
\f]

will just do the opposite, i.e. unskipping the
skipped beats. Next, let us deal with STOPS and DELAYS. Keeping the
assumption that BPM is 60, then we can write the piecewise functions

\f[
t_{(s)}(x) = \begin{cases}
x\,, & \text{if $ x \leq 4 $}\,; \\
4\,, & \text{if $4 < x \leq 4+5 $}\,; \\
x-5\,, & \text{if $4 + 5 < x \leq 6 + 5 $}\,; \\
6+5\,, & \text{if $6 + 5 < x \leq 6 + 5+ 2 $}\,; \\
x-2-5\,, & \text{if $ x > 6+5+2 $}\,; \\
\end{cases}
\label{l}
\f]

and

\f[
t_{(d)}(x) = \begin{cases}
x\,, & \text{if $ x \leq 4 $}\,; \\
4\,, & \text{if $4 < x \leq 4+5 $}\,; \\
x-5\,, & \text{if $4 + 5 < x \leq 6 + 5 $}\,; \\
6+5\,, & \text{if $6 + 5 < x \leq 6 + 5+ 2 $}\,; \\
x-2-5\,, & \text{if $ x > 6+5+2 $}\,; \\
\end{cases}
\f]

that will map from the warped song time to sequencer
time for the STOPS and DELAYS, respectively. A plot of this function can
be seen below.

<img src="../../Imgs/Understanding-Gimmicks/figure-2.png" width=400 style="display: block; margin: 0 auto; text-align: center;" ref="fig:wwarps">

We can also easily calculate the two different inverse functions to
model when notes at stops or delays should be tapped w.r.t. the song
time. On the one hand side, we write the function

\f[
t^{-1}_{(s)}(x) =\begin{cases}
x\,, &\text{if $ x \leq 4 $}\,;\\
x+5\,,& \text{if $ 4 < x \leq 6 $}\,;\\
x+5+2\,,& \text{if $ x > 6 $}\,;\\
\end{cases}
\label{eq:tostops}
\f]
to map from the stopped time into the
delayed time for STOPS. Similarly, the function

\f[
t^{-1}_{(d)}(x) =\begin{cases}
x\,, &\text{if $ x < 4 $}\,;\\
x+5\,,& \text{if $ 4 \leq x < 6 $}\,;\\
x+5+2\,,& \text{if $ x \geq 6 $}\,;\\
\end{cases}
\label{eq:todelays}
\f]
will map from the delayed time into the
song time for DELAYS. Note that the signs at the conditions are slightly
different.

## Function ordering

Calculations shown above only work in the special scenario where only
one of the three gimmicks is defined at a time. This is because their
inputs and outputs are defined in a equivalent space. When two or more
gimmicks of different type (e.g. WARPS + STOPS, or DELAYS + STOPS, etc)
are defined we must take into account in what spaces each of the
functions are working and apply their inverses to \f$f^{-1}\f$. For example,
say that we have defined some WARPS but also some STOPS. We know (by
definition) that \f$q: \mathbb{G}\rightarrow \mathbb{S^*}\f$ and
\f$q^{-1}: \mathbb{S^*}\rightarrow \mathbb{G}\f$, so we are just fine using
\f$f^{-1}\f$ to transform from the beats space \f$\mathbb{B}\f$ into the
sequencer time \f$\mathbb{S^*}\f$. On the other hand, we know that
\f$t_{(s)}: \mathbb{D}\rightarrow \mathbb{G}\f$, therefore we must transform
the beats into the stopped time through \f$f^{-1}\circ q^{-1}\f$.

Actually we can convert from the song time space into the sequencer time
space by applying
\f[
\begin{aligned}
t_{(d)}\circ t_{(s)}\circ q \circ f\,.\end{aligned}
\f]
where
\f$f=\left(  f^{-1} \right)^{-1}\f$.

## Formalization

### Warps

Let
\f$\mathcal{W} =  \left\{\left( b_i^{(w)}, w_i \right)\right\}_{i=1}^{n}\f$
be a sequence of WARPS, where \f$w_i\f$ is the warp (measured in beats) at
beat \f$b_i^{(w)}\f$ and let \f$f^{-1}: \mathbb{B}\rightarrow \mathbb{S^{*}}\f$
be a function that maps from the beat space into the sequencer time
space. We define a new set

\f[
\mathcal{W'} = \left\{\left( b_i', w_i' \right)\right\} =\bigcup_{i=2}^{n} \left\{\left( f^{-1} \left( b_i^{(w)} \right),z \left( w_i, b_i^{(w)} \right) \right)\right\}
\f]

where
\f[
z(w,b) = f^{-1}(b+w) - f^{-1}(b) \,.
\f]

We define the function \f$q: \mathbb{G}\rightarrow \mathbb{S^*}\f$

\f[
q(x) = \begin{cases}
x\,, & \text{if $ x \leq b_1' $}\,;\\
x+ \sum_{j=1}^{i}w_j'\,, & \text{if $ b_i' - \sum_{j=1}^{i-1}w_j' < x \leq b_{i+1}' - \sum_{j=1}^{i}w_j'\,,\quad \forall i=1,\dots,n$}\,;\\
\end{cases}
\label{eq:q-1}
\f]
where \f$b_{n+1}' := \infty\f$ which maps from stopped
time space into the sequencer time space, if
\f$\mathcal{W} \neq \emptyset\f$.

We define the function \f$q^{-1}: \mathbb{S^*}\rightarrow \mathbb{G}\f$

\f[
q^{-1}(x) = \begin{cases}
x\,, & \text{if $ x \leq b_1' $}\,;\\
b_i'- \sum_{j=1}^{i-1}w_j'\,, & \text{if $ b_i' < x \leq b_i'+ w_i'\,,\quad \forall i=1,\dots,n$}\,;\\
x - \sum_{j=1}^{i}w_j'\,, & \text{if $ b_i' + w_i' < x \leq b_{i+1}'\,,\quad \forall i=1,\dots,n$}\,;\\
\end{cases}
\label{eq:q}
\f]
when \f$\mathcal{W} \neq \emptyset\f$, where
\f$b^{(w)}_0 := -\infty\f$, that maps from the sequencer time space into the
stopped time space. If \f$\mathcal{W} = \emptyset\f$, then
\f$q(x) = q^{-1}(x) = x\f$.

### Stops

Let
\f$\mathcal{T}_{(s)} =  \left\{\left( b_i^{(t)}, r_i \right)\right\}_{i=1}^{n}\f$
be a sequence of STOPS, where \f$r_i\f$ is the stop (measured in seconds) at
beat \f$b_i^{(t)}\f$.

We define a new set

\f[
\mathcal{T}_{(s)}' = \left\{\left( c_i^{(s)}, r_i \right)\right\}_{i=1}^{n} = \left\{\left( f^{-1}\circ q^{-1}\left( b_i^{(t)}\right), r_i \right)\right\}_{i=1}^{n}
\label{eq:tprimeset}
\f]
where \f$c_i^{(s)}\f$ is the second from the
start of the song (after delays) of beat \f$b_i^{(t)}\f$. We define the
function \f$t_{(s)}: \mathbb{D}\rightarrow \mathbb{G}\f$

\f[
t_{(s)}(x) = \begin{cases}
x - \sum_{j=1}^{i-1}r_j\,, & \text{if $ c_{i-1}^{(s)} + \sum_{j=1}^{i-1}r_j < x \leq c_i^{(s)} + \sum_{j=1}^{i-1}r_j\,, \quad \forall i=1,\dots,n$}\,;\\
c_i^{(s)}\,, & \text{if $ c_i^{(s)} + \sum_{j=1}^{i-1}r_j < x \leq c_i^{(s)} + \sum_{j=1}^i r_j\,,\quad \forall i=1,\dots,n $}\,;\\
x - \sum_{j=1}^n r_j & \text{if $ x > c_n^{(s)} + \sum_{j=1}^n r_j $}\,;
\end{cases}
\label{eq:t}
\f]
that maps from the delayed song time into the stopped
time space, where \f$c_0^{(s)} := -\infty\f$ when
\f$\mathcal{T}_{(s)}' \neq \emptyset\f$.

The inverse of \f$t_{(s)}\f$,
\f$t_{(s)}^{-1}: \mathbb{G}\rightarrow \mathbb{D}\f$ is defined as

\f[
t_{(s)}^{-1}(x) = \begin{cases}
x\,, & \text{if $ x \leq c_1^{(s)} $}\,;\\
x+ \sum_{j=1}^{i}r_i\,, & \text{if $ c_i^{(s)} < x \leq c_{i+1}^{(s)}\,,\quad \forall i=1,\dots,n$}\,;\\
\end{cases}
\label{eq:t-1s}
\f]
with \f$c_{n+1}^{(s)} := \infty\f$, which maps from
the stopped time space into the delayed time space, if
\f$\mathcal{T'} \neq \emptyset\f$. If \f$\mathcal{T}_{(s)}' = \emptyset\f$, then
\f$t_{(s)}(x) = t_{(s)}^{-1}(x) = x\f$.

### Delays

Delays are defined in a very similar fashion to stops. Let
\f$\mathcal{T}_{(d)} =  \left\{\left( b_i^{(d)}, r_i \right)\right\}_{i=1}^{n}\f$
be a sequence of DELAYS, where \f$r_i\f$ is the delay (measured in seconds)
at beat \f$b_i^{(d)}\f$.

We define a new set

\f[
\mathcal{T}_{(d)}' = \left\{\left( c_i^{(d)}, r_i \right)\right\}_{i=1}^{n} = \left\{\left( f^{-1}\circ q^{-1}\circ t_{(s)}^{-1}\left( b_i^{(t)}\right), r_i \right)\right\}_{i=1}^{n}
\f]
where \f$c_i^{(d)}\f$ is the second from the
start of the song of beat \f$b_i^{(t)}\f$. We define the function
\f$t_{(d)}: \mathbb{S}\rightarrow \mathbb{D}\f$

\f[
t_{(d)}(x) = \begin{cases}
x - \sum_{j=1}^{i-1}r_j\,, & \text{if $ c_{i-1}^{(d)} + \sum_{j=1}^{i-1}r_j < x \leq c_i^{(d)} + \sum_{j=1}^{i-1}r_j\,, \quad \forall i=1,\dots,n$}\,;\\
c_i^{(d)}\,, & \text{if $ c_i^{(d)} + \sum_{j=1}^{i-1}r_j < x \leq c_i^{(d)} + \sum_{j=1}^i r_j\,,\quad \forall i=1,\dots,n $}\,;\\
x - \sum_{j=1}^n r_j & \text{if $ x > c_n^{(d)} + \sum_{j=1}^n r_j $}\,;
\end{cases}
\f]
that maps from the song time space into the delayed
time space, where \f$c_0^{(d)} := -\infty\f$ when
\f$\mathcal{T}_{(d)}' \neq \emptyset\f$.

The inverse of \f$t_{(d)}\f$,
\f$t_{(d)}^{-1}: \mathbb{D}\rightarrow \mathbb{S}\f$ is defined as

\f[
t_{(d)}^{-1}(x) = \begin{cases}
x\,, & \text{if $ x < c_1^{(d)} $}\,;\\
x+ \sum_{j=1}^{i}r_i\,, & \text{if $ c_i^{(d)} \leq x < c_{i+1}^{(d)}\,,\quad \forall i=1,\dots,n$}\,;\\
\end{cases}
\f]
with \f$c_{n+1}^{(d)} := \infty\f$, which maps from
the delayed time space into the song time space, if
\f$\mathcal{T'} \neq \emptyset\f$. If \f$\mathcal{T}_{(d)}' = \emptyset\f$, then
\f$t_{(d)}(x) = t_{(d)}^{-1}(x) = x\f$.

# From sequencer time to beat

## Introduction

A `SSC` file gives a list of pairs which defines the bpms. The first
item in the pair is the target beat, and the second item is the desired
BPM from that beat on. Let us imagine we have a `SSC` file with the
following definition:

        #BPMS:0=120,8=70,13=200;

Let us convert this cumbersome definition into a friendly structure:

      {
        [
          beat: 0,
          bpm: 120
        ],
        [
          beat: 8,
          bpm: 180
        ],
        [
          beat: 13,
          bpm: 60
        ]
      }

This `#BPMS` definition is telling us three things:

1.  From beat \f$- \infty\f$ to beat 8, the BPM is 120.

2.  From beat 8 to beat 13, the BPM is 180.

3.  From beat 13 to beat \f$+\infty\f$, the BPM is 60.

## Challenge

We want to find a function \f$f : \mathbb{R} \rightarrow \mathbb{R}\f$ that
retrieves the current beat given the a second in the sequencer time
space. This function is useful when a song is playing and we want to
know at what beat we are at if we know how much time has passed since
the start of the song. Also, as we will see later on, notes scroll at a
BPM rate, so if we can have the inverse function of \f$f\f$, \f$f^{-1}\f$, we
can sort of know when the steps should be tapped as well.

## Solution

First, let us convert BPMS to BPSS (Beats Per Second), since we are
going to provide the input in seconds instead of minutes. We can do so
by dividing the BPMS by 60, i.e.

\f[
\text{BPS}(x) = x \times \frac{\text{Beats}}{\text{Minute}} = x \times \frac{1 \times \text{Minute}}{60 \times \text{Seconds}} \frac{\text{Beats}}{\text{Minute}} = \frac{x}{60} \times \frac{\text{Beats}}{\text{Second}}\,.
\label{eq:bpm2bps}
\f]

Next, let us define a piecewise function
\f$f': \mathbb{R} \rightarrow \mathbb{R}\f$ that gives the current BPS given
the current Beat. Taking the `#BPMS` toy example from the previous
section, we get that
\f[
f'(x) = \begin{cases}
2\,, & \text{if $x \leq 8\,;$}\\
3\,, & \text{if $8 < x \leq 13\,;$}\\
1\,, & \text{if $x > 13\,.$}\\
\end{cases}
\label{eq:beat2bps}
\f]

Below you can see the plot of \f$f'\f$ we just defined.

<img src="../../Imgs/Understanding-Gimmicks/figure-4.png" width=400 style="display: block; margin: 0 auto; text-align: center;" ref="fig:wwarps">

Note that by using \f$f'\f$, we can get the BPS at any beat of the song.
This is great, but it does not quite solve our problem.

Next, we can calculate the SPB (Seconds Per Beat) by just inversing the
BPS, i.e.
\f[
\text{SPB} = \frac{1}{\text{BPS}}\,,
\label{eq:bps2spb}
\f]
and therefore we can define a function
\f$t: \mathbb{R} \rightarrow \mathbb{R}\f$

\f[
t(x) = x\times \text{SPB}
\label{eq:beat2seconds}
\f]
that given a beat \f$x\f$ retrieves the
current second.

Let

\f[
f^{-1}(x) = \begin{cases}
\frac{x}{2}\,, & \text{if $x \leq 8\,;$}\\[1em]
\frac{8}{2}+\frac{x-8}{3}\,, & \text{if $8 < x \leq 13\,;$}\\[1em]  
 \frac{8}{2}+\frac{5}{3}+x- 13\,, & \text{if $x > 13\,;$}\\
\end{cases}
\label{eq:beat2second}
\f]
be the function that given a beat \f$x\f$
retrieves the current second. This function
can be rewritten recursively as

\f[
f^{-1}(x) = \begin{cases}
\frac{x}{2}\,, & \text{if $x \leq 8\,;$}\\[1em]
f^{-1}(8)+\frac{x-8}{3}\,, & \text{if $8 < x \leq 13\,;$}\\[1em]  
 f^{-1}(13) + x- 13\,, & \text{if $x > 13\,.$}\\
\end{cases}
\f]

The figure down below depicts the function \f$f^{-1}\f$.

<img src="../../Imgs/Understanding-Gimmicks/figure-5.png" width=400 style="display: block; margin: 0 auto; text-align: center;" ref="fig:wwarps">

As it turns out, the function \f$f\f$ that we are looking for is just the
inverse function of \f$f^{-1}\f$, thus

\f[
f(x) = \begin{cases}
2x\,, & \text{if $x \leq 4\,;$}\\[1em]
(x-4)\times 3 + 8\,, & \text{if $4 < x \leq 5.6\,;$}\\[1em]  
 x-5.6 + 13\,, & \text{if $x > 5.6\,.$}\\
\end{cases}
\f]

A plot of \f$f\f$ can be seen below.

<img src="../../Imgs/Understanding-Gimmicks/figure-6.png" width=400 style="display: block; margin: 0 auto; text-align: center;" ref="fig:wwarps">

## Formalization

Let \f$\{\left( b_i, v_i \right)\}_{i=1}^{n}\f$ be a sequence of \f$n\f$ beat
signatures, where \f$v_i\f$ is the BPS value at beat \f$b_i\f$. Let
\f$f^{-1}: \mathbb{B} \rightarrow \mathbb{S^{*}}\f$ be a function that
provided a beat, returns the seconds in the sequencer time space. We
define this function as a \f$n\f$-step piecewise function

\f[
f^{-1}(x) = \begin{cases}
\frac{x}{v_1}\,, & \text{if $x \leq b_{2} $ }\,;\\
f^{-1}(b_{i}) + \frac{x-b_{i}}{v_i}\,, & \text{if $b_{i} < x \leq b_{i+1}\,; \quad \forall i=2,\ldots,n $}\,; \\
\end{cases}
\f]
where \f$b_1 := 0\f$, and
\f$b_{n+1} := \infty\f$.

Analogously, let \f$f: \mathbb{S^{*}} \rightarrow \mathbb{B}\f$ be a
function that provided a second in the sequencer time space, returns the
beat from the zero second. We define this function as a \f$n\f$-step
piecewise function
\f[
f(x) = \begin{cases}
v*1x\,, & \text{if $x \leq f^{-1}(b*{2}) $ }\,;\\
\left[x-f^{-1}(b_{i})\right]\times v*i + b*{i}\,, & \text{if $f^{-1}(b_{i}) < x \leq f^{-1}(b_{i+1})\,;\quad \forall i=2,\ldots,n$}\,. \\
\end{cases}
\f]

# From beat to note position

## Introduction

There are a pair of stepmania definitions that influence the position
where notes should be placed on the screen. One of them is `#BPMS`,
which arguably is the rate at what notes travel upwards towards the
receptor w.r.t. to the music rhythm. We have already dealt with it in
the previous section.

However, there is another gimmick that plays a role in the note
positioning: `#SCROLLS`. Let’s have a look at an example:

        #SCROLLS:0=1,4=0,10=2;

Again, let us convert this cumbersome definition into a friendly
structure:

      {
        [
          beat: 0,
          scroll: 1
        ],
        [
          beat: 4,
          scroll: 0
        ],
        [
          beat: 10,
          scroll: 2
        ]
      }

The scroll gimmick changes the effective BPM at a given beat by a rate
defined by the scroll value. Thus, in this example, the SCROLLS gimmick
is changing the BPMs as follows:

1.  From beat 0 to beat 4, the BPM is its value times 1.

2.  From beat 4 to beat 10, the BPM is its value times 0. (all the steps
    in between this beats, will have the same position)

3.  From beat 10 on, the BPM is its value times 2.

## Challenge

We would like to have a function \f$p: \mathbb{B} \rightarrow \mathbb{P}\f$
that given a beat, it retrieves the position w.r.t. the origin (or where
the receptor is) where a note at that beat should be drawn.

## Solution

Let us define a function \f$p\f$ that given a beat, retrieves the effective
beat (i.e., beat with applied scrolls). For that matter, we just need to
check out in what beats the scroll is taking place, and change the beat
accordingly to the scroll rate. For our toy example the resultant \f$p\f$
function looks like this

\f[
p(x) = \begin{cases}
x\,, & \text{if $x \leq 4\,;$}\\
(x-4)\times 0 + p(4)\,, & \text{if $4 < x \leq 10\,;$}\\
(x-10)\times 2+ p(10)\,, & \text{if $ x > 10\,.$}\\
\end{cases}
\label{eq:beat2effective-bps}
\f]

The function \f$p\f$ is depicted below.

<img src="../../Imgs/Understanding-Gimmicks/figure-7.png" width=400 style="display: block; margin: 0 auto; text-align: center;" ref="fig:wwarps">

Now, this is great! By asking \f$p\f$, now we have the effective beat, and
therefore the position. Note that de drawing position IS NOT the beat
that we are going to use to calculate when the note is needed to be
tapped!

## Formalization

Let

\f[
\left\{ \left( b_{i}^{(s)},s_{i} \right) \right\} _{i=1}^{n} = \mathcal{S} = B^{(s)} \times S
\label{eq:S}
\f]
be a sequence of \f$m\f$ scroll signatures, where
\f$s_i \in S = \{s_j\}_{j=1}^{n}\f$ is the scroll value at beat
\f$b_i^{(s)} \in B^{(s)}= \{b_j^{(s)}\}_{j=1}^{n}\f$.

We define the function \f$p: \mathbb{B}\rightarrow \mathbb{P}\f$

\f[
p(x) = \begin{cases}
s_1x\,, & \text{if $ x \leq b^{(s)}_2 $}\,;\\
p \left( b^{(s)}_i \right) + \left( x-b^{(s)}_i \right) \times s_i\,, & \text{if $ b^{(s)}_{i} < x \leq b^{(s)}_{i+1}\,; \quad \forall i=2,\dots,n$}\,,
\end{cases}
\label{eq:position-final}
\f]
as the function that retrieves the
position given a beat, where \f$b^{(s)}_{i+1} := \infty\f$.

# From beat to scroll

## Introduction

Another issue that we might have is to know how far upwards we should
scroll the notes from its original position given the current beat. If
we did not have any other gimmicks, this would be very easy to compute.
Let us suppose that the notes redendered in the screen are squares of
one unity of length and height, and that one beat is worth one distance
of separation, as shown:

<img src="../../Imgs/Understanding-Gimmicks/figure-8.png" width=400 style="display: block; margin: 0 auto; text-align: center;" ref="fig:wwarps">

In this set up, the amount of scroll that we have to apply for a given
beat \f$x\f$ is just \f$-x\f$ (if we were scrolling upwards, on the \f$y\f$ axis).

However there are two gimmicks that modify this scroll function w.r.t.
the beat and song time space in a number of different ways: `#SPEEDS`
(song time space), and `#SCROLLS` (beat space). Let us see and example
to know what these modifiers do.

On the one hand, suppose we have the following `#SPEEDS` definition:

        #SPEEDS:4=2=1=0,6=0.5=1=1;

which is equivalent to this one:

      {
        [
          beat: 0,
          speed: 1,
          span: 0,
          type: 0
        ],
        [
          beat: 6,
          speed: 7 ,
          span: 1,
          type: 1
        ]
      }

Well, the information provided with this gimmick is the following:

1.  From the song time second corresponding to beat 0 onwards, the
    separation between notes increases by a factor of 1, the scrolling
    speed also increases by a factor of 1, and because the `type` is
    `0`, this transition is smoothly applied in the span of 0 **beats**.

2.  From the song time second corresponding to beat 6 onwards, the
    separation between notes increases by a factor of 7, the scrolling
    speed also increases by a factor of 7, and because the `type` is
    `1`, this transition is smoothly applied in the span of 1
    **second**.

On the other hand, we have already talked about how the `#SCROLLS`
definition changes where the notes should be placed on the screen. It
turns out that also has an impact on the scrolling function. Taking the
same example from the previous section
we can extract the following information:

1.  From beat 0 to beat 4, the scrolling 1 times as fast.

2.  From beat 4 to beat 10, the scrolling is 0 times as fast.

3.  From beat 10 on, the the scrolling is 2 times as fast.

## Challenge

We would like to have a function \f$e: \mathbb{S}\rightarrow \mathbb{E}\f$
that calculates the speed factor at a given beat, and a function
\f$g: \mathbb{B}\rightarrow \mathbb{T}\f$ that calculates the scroll value.

## Solution

#### Speeds

The function that we need to come up for the speeds is simple as well.
However, it gets a bit complicated when dealing with speeds of `type` 0:
Since speeds are defined at the beat space, but work from the song time
space, we need somehow to convert a span of beats into a span of seconds
so we can perform operations in the same units. We do so by mapping
units defined in beats into the song time space. To keep things simple,
let us imagine that we are running a 60 BPM song with no delays or
stops. In this particular case, we can write
\f[
e(x) = \begin{cases}
1\,, & \text{if $ x \leq 6 $}\,;\\
\frac{7-1}{1}(x-6)+1\,, & \text{if $ 6 < x \leq 6+1 $}\,;\\
7\,, & \text{if $ x > 6+1 $}\,.
\end{cases}
\label{eq:example-speeds}
\f]

<img src="../../Imgs/Understanding-Gimmicks/figure-9.png" width=400 style="display: block; margin: 0 auto; text-align: center;" ref="fig:wwarps">

#### Scrolls

There is not much to say here. The function \f$g\f$ that we are looking for
is just identical to the function \f$p\f$,
\f[
g(x) = p(x)\,.
\label{eq:ddd}
\f]

## Formalization

#### Speeds

Let
\f$\mathcal{E} =  \left\{\left( b_i^{(e)}, s_i, p_i, t_i \right)\right\}_{i=1}^{n}\f$
be a sequence of SPEEDS, where \f$s_i\f$ is the speed change at beat
\f$b_i^{(e)}\f$ with a span transition of \f$p_i\f$ beats if \f$t_i = 0\f$, or \f$p_i\f$
seconds if \f$t_i=1\f$.

We define a new sequence
\f$\mathcal{E'} =  \left\{\left( t_i, s_i, p'_i \right)\right\}_{i=1}^{n}\f$
from \f$\mathcal{E}\f$ as
\f[
\mathcal{E'} = \bigcup_{i=1}^{n} \begin{cases}
\left\{\left( h \left( b_i^{(e)} \right), s_i,g \left( p_i, b_i^{(e)} \right) \right)\right\}\,, & \text{if $ t_i =0 $}\,;\\
\left\{\left( h \left( b_i^{(e)} \right), s_i, p_i \right)\right\}\,, & \text{otherwise}\,,\\
\end{cases}
\label{eq:eprime}
\f]
where
\f[
\begin{aligned}
h(b) = \left( f^{-1}\circ q^{-1}\circ t_{(s)}^{-1} \circ t_{(d)}^{-1} \right)(b)\end{aligned}
\f]

and
\f[
g(p,b) = h(p+b) - h(b)\,.
\label{eq:iureio}
\f]

We define the function \f$e:\mathbb{S}\rightarrow \mathbb{E}\f$

\f[
e(x) = \begin{cases}
s_1\,, & \text{if $ x\leq t_1 $}\,;\\
\frac{s_i-s_{i-1}}{p'_i} \left( x-t_{i}\right)+s_{i-1} \,, & \text{if $ t_{i} < x \leq t_{i}+p'_i \land p'_i \neq 0\,,\quad \forall i=1,\dots,n$}\,;\\
s_i\,, &\text{if $ t_{i}+p'_i < x \leq t_{i+1}\,,\quad \forall i=1,\dots,n$}\,,\\
\end{cases}
\label{eq:e}
\f]
where \f$t_{n+1} := \infty\f$, and \f$s_0 := s_1\f$.

#### Scrolls

We define \f$g: \mathbb{B}\rightarrow \mathbb{T}\f$
\f[
g(x) = p(x)\,.
\label{eq:finalGx}
\f]

# Positioning and scrolling notes

Alright! Now we have formally defined everything we need to build a
sequencer using stepmania’s notation. This section will show how to use
all functions defined formally in the sections above to determine where
to draw the notes in the screen at any given song time.

The table below gathers all the functions that we are going to need as well as a brief
descripcion for each one of them. Also, the figure shown in the first lines of this page
will come in handy to visually see the mapping between the different spaces.

| Symbol               | Description                                                                                                                          |
| :------------------- | :----------------------------------------------------------------------------------------------------------------------------------- |
| \f$\mathbb{S}\f$     | Song time space (s) \f$\mathbb{S} = \mathbb{R}\f$.                                                                                   |
| \f$\mathbb{D}\f$     | Delayed time space (s) \f$\mathbb{D} = \mathbb{R}\f$.                                                                                |
| \f$\mathbb{G}\f$     | Stopped song time space (s) \f$\mathbb{G} = \mathbb{R}\f$.                                                                           |
| \f$\mathbb{S^{*}}\f$ | Sequencer time space (s) \f$\mathbb{S^{*}} = \mathbb{R}\f$.                                                                          |
| \f$\mathbb{B}\f$     | Beat space (beats) \f$\mathbb{B} = \mathbb{R}\f$.                                                                                    |
| \f$\mathbb{P}\f$     | Position space (units) \f$\mathbb{P} = \mathbb{R}\f$.                                                                                |
| \f$\mathbb{T}\f$     | Scroll space (units) \f$\mathbb{T} = \mathbb{R}\f$.                                                                                  |
| \f$\mathbb{E}\f$     | Speed space (speed factor units) \f$\mathbb{E} = \mathbb{R}\f$.                                                                      |
| \f$t_{(d)}\f$        | Function \f$t_{(d)}: \mathbb{S}\rightarrow \mathbb{D}\f$ that maps from the song time space into the delayed time space.             |
| \f$t_{(d)}^{-1}\f$   | Function \f$t_{(d)}^{-1}: \mathbb{D}\rightarrow \mathbb{S}\f$ that maps from the the delayed time space into the song time space.    |
| \f$t_{(s)}\f$        | Function \f$t_{(s)}: \mathbb{D}\rightarrow \mathbb{G}\f$ that maps from the delayed time space into the stopped time space.          |
| \f$t_{(s)}^{-1}\f$   | Function \f$t_{(s)}^{-1}: \mathbb{G}\rightarrow \mathbb{D}\f$ that maps from the the stopped time space into the delayed time space. |
| \f$q\f$              | Function \f$q: \mathbb{G}\rightarrow \mathbb{S^*}\f$ that maps from the stopped space into the sequencer time space.                 |
| \f$q^{-1}\f$         | Function \f$q^{-1}: \mathbb{S^*}\rightarrow \mathbb{G}\f$ that maps from the sequencer time space into the stopped time space.       |
| \f$f\f$              | Function \f$f: \mathbb{S^{*}}\rightarrow \mathbb{B}\f$ that maps from the sequencer time space into the beat space.                  |
| \f$f^{-1}\f$         | Function \f$f^{-1}: \mathbb{B}\rightarrow \mathbb{S^{*}}\f$ that maps from the beat space into the sequencer time space.             |
| \f$p\f$              | Function \f$p: \mathbb{B}\rightarrow \mathbb{P}\f$ that maps from the beat space into the position space.                            |
| \f$e\f$              | Function \f$e: \mathbb{S}\rightarrow \mathbb{E}\f$ that maps from the song space into the speed space.                               |
| \f$g\f$              | Function \f$g: \mathbb{B}\rightarrow \mathbb{T}\f$ that maps from the beat space into the scroll space.                              |

We will take the following assumptions:

1.  We are modeling a rhythm game whose notes scroll upwards.

2.  The receptor is placed at the origin of the scrolling axis.

3.  The beat 0 has a position 0 in the scrolling axis.

Let \f$\mathcal{N} =  \left\{ \left( u_{i} \right) \right\}_{i=1}^{n}\f$ be
a sequence of notes where \f$u_{i}\f$ is the beat when the \f$i\f$-th note
should be tapped. We define a new set

\f[
\mathcal{N'} = \left\{ \left( u_{i}, v_i, w_i \right) \right\} = \bigcup_{\{u\}\in \mathcal{N}} \left\{ \left(u, z(u), p(u) \right) \right\}
\label{eq:set}
\f]
where \f$z: \mathbb{B}\rightarrow \mathbb{S}\f$

\f[
z(x) = \left(f^{-1}\circ q^{-1}\circ t_{(s)}^{-1}\circ t_{(d)}^{-1}\right)(x)\,,
\label{eq:final-g}
\f]
and thus \f$v_i\f$ is the exact time where the
\f$i\f$-th note should be tapped, and \f$w_i\f$ is its relative position at beat
0 in the scrolling axis. At a given moment in time \f$t\f$ w.r.t. to the
begginig of the song, the \f$i\f$-th note should be positioned at

\f[
\left[- w_i + \left( t_{(d)}\circ t_{(s)}\circ q \circ f \circ g \right)(t) \right] \times e(t)\,.
\label{eq:final-position}
\f]

# Tickcounts and Piu-style holds

Let
\f[
\mathcal{U} = \{(b_{i},t_{i})\}_{i=1}^{n}
\f]
be a sequence of
TICKCOUNTS, where \f$t_{i}\f$ is the tick count at beat \f$b_{i}\f$.

For each piu-style hold in the span of beats \f$h_1, h_2\f$, where \f$h_1\f$ is
the first beat of the hold and \f$h_2\f$ is the last beat, we construct a
new set
\f[
\begin{aligned}
\mathcal{U'} =\{(b_{i}',t_{i}')\}_{i=1}^{n'} &= \{ (h_1, g(h_1)) \} \\
&\cup \bigcup_{(b,t)\in \mathcal{U}} \begin{cases}
(b, t)\,, & \text{if $ b > h1 \land b < h_2 $}\,;\\
\emptyset\,, & \text{otherwise}\,;
\end{cases} \\
& \cup\{ (h_2, 0) \}\,, \end{aligned}
\f]
where

\f[
g(x) = \min_{\substack{(b,t)\in \mathcal{U};\\ x \geq b}} x-b\,.
\f]
We
define the sequence of beats where hidden hold notes should be place as

\f[
\begin{aligned}
\mathcal{T} &= \{h_1\}\\
& \cup \bigcup_{i=1}^{n'-1} \bigcup_{k=1}^{s_i} \left\{ a_i + \sum_{j=1}^{k-1}\frac{1}{t_{i}'} \right\}\\
& \cup \{h_2\}\,,\end{aligned}
\f]
where

\f[
a_i = b_i' + b_i' \mod \frac{1}{t_{i}'}
\f]
and

\f[
s_i = \lfloor t_{i}' (b_{i+1}'-a_i)\rfloor\,.
\f]

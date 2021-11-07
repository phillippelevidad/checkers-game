# Checkers Game with Autoplay

This is a simple checkers game implementation that I made just for fun.

![gif showing running app](https://media.giphy.com/media/Yulm7DHmHgkmDXEdKm/giphy.gif)

### Motivation

Working primarily as an Architect and Back-end Developer, the idea of developing a Checkers game sounded challeging. And so I began, right away!

Soon I realized this game had just enough complex "business" rules to justify a DDD (Domain Driven Design) approach, which I'm a fan of.

The idea was for players to interact via a Console Application, making moves by typing the desired from/to positions, like "3A to 4B".

But since I had to build an engine to calculate all possible moves for a player and make sure that the requested moves were valid, the engine presented itself as the perfect starting-point for a very simple AI.

And there you have it: a simple Checkers game auto-played by a Kamikaze AI :smile:

### Running the game

Open a terminal from the `ConsoleApp` folder and run with:

```
dotnet run
```

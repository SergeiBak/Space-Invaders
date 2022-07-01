# Space Invaders
<img width="276.48" height="155.52" src="https://github.com/SergeiBak/PersonalWebsite/blob/master/images/SpaceInvaders.png?raw=true">

## Table of Contents
* [Overview](#Overview)
* [Test The Project!](#test-the-project)
* [Code](#Code)
* [Technologies](#Technologies)
* [Resources](#Resources)
* [Donations](#Donations)

## Overview
This project is a recreation of Space Invaders, a 1978 shoot 'em up style arcade game. A fun fact is that Space Invaders is actually the first fixed shooter game, and set the template for all of the shoot 'em up games that followed. This solo project was developed in Unity using C# as part of my minigames series where I utilize various resources to remake simple games in order to further my learning as well as to have fun!   

In Space Invaders, the player controls a laser cannon that can move left and right, as well as fire at any approaching Invaders. The Invaders make their way towards the player by moving side to side, and dropping down a level every time they reach the edge of the screen. The goal of the game is to blast away as many waves of Invaders as possible! But beware, the less Invaders left, the faster they speed up!    

## Test The Project!
In order to play this version of Space Invaders, follow the [link](https://sergeibak.github.io/PersonalWebsite/SpaceInvaders.html) to a in-browser WebGL build (No download required!).

## Code
A brief description of all of the classes is as follows:
- The `Bunker` class represents the four bunkers protecting the player, they get gradually destroyed by projectiles.
- The `GameManager` class handles the state of the game, as well as tracking score & lives, and updating UI.
- The `Invader` class represents each shootable enemy in the game.
- The `Invaders` class represents the entire field of invaders as a whole, and moves them together.
- The `MysteryShip` class handles the mystery ship that occasionally appears across the top of the screen for bonus points if shot.
- The `Player` class handles player movement & shooting input.
- The `Projectile` class handles the lasers & missiles fired by the player and enemies.

## Technologies
- Unity
- Visual Studio
- GitHub
- GitHub Desktop

## Resources
- Credit goes to [Zigurous](https://www.youtube.com/channel/UCyaKsKqYTghxgAqywfefAzg) for the helpful base game tutorial!
- Game Guide [Reference](https://www.digitpress.com/library/books/book_how_to_win_at_video_games_complete_guide.pdf).
- Gameplay [Reference](https://www.youtube.com/watch?v=MU4psw3ccUI).

## Donations
This game, like many of the others I have worked on, is completely free and made for fun and learning! If you would like to support what I do, you can donate at my metamask wallet address: ```0x32d04487a141277Bb100F4b6AdAfbFED38810F40```. Thank you very much!

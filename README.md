<!--
*** Based on the Best-README-Template: https://github.com/othneildrew/Best-README-Template
***
*** To avoid retyping too much info. Do a search and replace for the following:
*** github_username, repo_name, email, project_title, project_description
-->



<!-- PROJECT SHIELDS -->
[![Release][release-shield]][release-url]
[![Last Commit][last-commit-shield]][last-commit-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]



<!-- PROJECT LOGO -->
<br />
<p align="center">
  <a href="https://github.com/Tim-W-James/Time-Travel-Roguelite-Game/images/logo.png">
    <img src="images/logo.png" alt="Logo" width="120" height="120">
  </a>

  <h2 align="center">Time Travel Roguelite Game</h2>

  <p align="center">
    A prototype game with procedurally generated levels and time manipulation mechanics.
    <br />
<!--     <a href="https://github.com/Tim-W-James/Time-Travel-Roguelite-Game"><strong>Explore the docs »</strong></a>
    <br /> 
    <br /> -->
    <a href="https://github.com/Tim-W-James/Time-Travel-Roguelite-Game/tree/main/_Demo">View Demo</a>
<!--     ·
    <a href="https://github.com/Tim-W-James/Time-Travel-Roguelite-Game/issues">Report Bug</a> -->
<!--     ·
    <a href="https://github.com/Tim-W-James/Time-Travel-Roguelite-Game/issues">Request Feature</a> -->
  </p>
</p>



<!-- TABLE OF CONTENTS -->
<details open="open">
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#features">Features</a></li>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#usage">Usage</a>
      <ul>
<!--         <li><a href="#prerequisites">Prerequisites</a></li> -->
        <li><a href="#running-the-demo">Running the Demo</a></li>
        <li><a href="#controls">Controls</a></li>
        <li><a href="#development">Development</a></li>
<!--         <li><a href="#example-usecases">Example Usecases</a></li> -->
      </ul>
    </li>
<!--     <li><a href="#roadmap">Roadmap</a></li> -->
<!--     <li><a href="#contributing">Contributing</a></li> -->
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgements">Acknowledgements</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

<!-- [![Product Name Screen Shot][product-screenshot]](https://example.com) -->

Explore unique procedurally generated levels and search for the exit, while fighting enemies and dodging traps as you go. 
There is also a treasure room hidden somewhere in the level, and a challenge room before the exit.
Each hit you take will reduce your HP, but you can find pickups in the level that will increase your HP.
Use melee attacks, fireballs and your dash abilities to defeat enemies. 
<br><br>
You can also slow down time temporarily, during which you will be able to move at full speed but enemies and projectiles will be slowed.
While time is slowed your abilities are modified. 
Your melee attack becomes a shield, your fireball becomes a barrier that reflects enemy projectiles, and your dash becomes a blink that makes you invulnerable.

### Features
* Grid based procedurally generated levels (rooms will be arranged differently each time)
* Players can slow down time and unlock new abilities
* Top-down bullet-hell gameplay
* 2D lighting
* Pixel perfect rendering
* Gamepad support with rumble
* Contextual audio

### Built With

* [Unity3D 2019.3.0b9](https://unity.com/)
* C#
* Visual Studio Code



<!-- USAGE -->
## Usage

### Running the Demo

For a `.exe` application built for `Windows x86-64`, find the latest version in the [Demo](https://github.com/Tim-W-James/Time-Travel-Roguelite-Game/tree/main/_Demo) folder. 
Download the entire folder and run the `Project Velocity PGP Prototype.exe`. 
The application supports standard Windows GamePads (e.g., XBox 360 controllers) and they will automatically be detected.

### Controls

##### Keyboard Controls:
- **WASD/Arrow Keys**: Move
- **Mouse**: Aim
- **Space**: Temporarily slows down time (gives new abilities)
- **Left Click**: Shoot (while time is slowed becomes *reflect*)
- **Right Click**: Melee (while time is slowed becomes *shield*)
- **Shift**: Dash (while time is slowed becomes *blink*)

##### Debug Controls:
- **L**: Reload Level (may cause player to get stuck in terrain)

##### Gamepad Controls (should be automatically detected):
- **Left Joystick**: Move
- **Right Joystick**: Aim
- **Right Bumper**: Temporarily slows down time (gives new abilities)
- **Right Trigger**: Shoot (while time is slowed becomes *reflect*)
- **X**: Melee (while time is slowed becomes *shield*)
- **Left Trigger**: Dash (while time is slowed becomes *blink*)

### Development

To work on the project, use Unity3D version 2019.3.0b9.



<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE` for more information.



<!-- CONTACT -->
## Contact

Email: [tim.jameswork9800@gmail.com](mailto:tim.jameswork9800@gmail.com "tim.jameswork9800@gmail.com")

Project Link: [https://github.com/Tim-W-James/Time-Travel-Roguelite-Game](https://github.com/Tim-W-James/Time-Travel-Roguelite-Game)



<!-- ACKNOWLEDGEMENTS -->
## Acknowledgements

* Placeholder sprites were derived from [Hyper Light Drifter](https://store.steampowered.com/app/257850/Hyper_Light_Drifter/) by Heart Machine.





<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[release-shield]: https://img.shields.io/github/v/release/Tim-W-James/Time-Travel-Roguelite-Game.svg?include_prereleases&style=for-the-badge
[release-url]: https://github.com/Tim-W-James/Time-Travel-Roguelite-Game/v/release/
[last-commit-shield]: https://img.shields.io/github/last-commit/Tim-W-James/Time-Travel-Roguelite-Game.svg?style=for-the-badge
[last-commit-url]: https://github.com/Tim-W-James/Time-Travel-Roguelite-Game/last-commit
[contributors-shield]: https://img.shields.io/github/contributors/Tim-W-James/Time-Travel-Roguelite-Game.svg?style=for-the-badge
[contributors-url]: https://github.com/Tim-W-James/Time-Travel-Roguelite-Game/graphs/contributors
[contributors-shield]: https://img.shields.io/github/contributors/Tim-W-James/Time-Travel-Roguelite-Game.svg?style=for-the-badge
[contributors-url]: https://github.com/Tim-W-James/Time-Travel-Roguelite-Game/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/Tim-W-James/Time-Travel-Roguelite-Game.svg?style=for-the-badge
[forks-url]: https://github.com/Tim-W-James/Time-Travel-Roguelite-Game/network/members
[stars-shield]: https://img.shields.io/github/stars/Tim-W-James/Time-Travel-Roguelite-Game.svg?style=for-the-badge
[stars-url]: https://github.com/Tim-W-James/Time-Travel-Roguelite-Game/stargazers
[issues-shield]: https://img.shields.io/github/issues/Tim-W-James/Time-Travel-Roguelite-Game.svg?style=for-the-badge
[issues-url]: https://github.com/Tim-W-James/Time-Travel-Roguelite-Game/issues
[license-shield]: https://img.shields.io/github/license/Tim-W-James/Time-Travel-Roguelite-Game.svg?style=for-the-badge
[license-url]: https://github.com/Tim-W-James/Time-Travel-Roguelite-Game/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/github_username
[product-screenshot]: images/screenshot.png

<!-- USEFUL LINKS FOR MARKDOWN
* https://www.markdownguide.org/basic-syntax
* https://www.webpagefx.com/tools/emoji-cheat-sheet
* https://shields.io
* https://choosealicense.com
* https://pages.github.com
* https://daneden.github.io/animate.css
* https://connoratherton.com/loaders
* https://kenwheeler.github.io/slick
* https://github.com/cferdinandi/smooth-scroll
* http://leafo.net/sticky-kit
* http://jvectormap.com
* https://fontawesome.com -->

<a id="readme-top"></a>

<br />
<div align="center">
  <h3 align="center">Kimble Console App</h3>
  <h4 align="center">Vesa Runtti / 26.08.2024</h4>
</div>



<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
      </ul>
    </li>
    <li><a href="#running">Running</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

A simple console app which replicates AI players behavior and logic until game is finished. Game is considered finished when one of four AI players has gathered all the markers to goal slots, A.K.A traditional Kimble rules. Currently AI's have three different behaviors and one property which determine their personality/characteristic.

<p align="left"><a href="https://www.tactic.net/site/rules/UK/01533.pdf">Kimble Rules</a></p>

<p align="right">(<a href="#readme-top">back to top</a>)</p>


### Built With

* C#
* Visual Studio
* LINQ

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- GETTING STARTED -->
## Getting Started

### Prerequisites

Project requires to have a GameSetting.json file inside Settings folder but this file has been committed to repository so it's not necessary to create it before running.
* GameSetting.json
  ```sh
  {
    "Iterations": 20,
    "LapSize": 28,
    "GoalSlots": 4,
    "TeamsCount": 4,
    "TeamMarkers": 4,
    "TeamRiskTaking": [
        {"Red": 0.5},
        {"Green": 0.5},
        {"Blue": 0.5},
        {"Yellow": 0.5}
      ],
    "TeamSkills": [
        {"Red": [ 0, 1, 2 ]},
        {"Green": [ 1 ]},
        {"Blue": [ 1, 2 ]},
        {"Yellow": [ 0 ]}
      ]
  }
  ```

#### Iterations
##### Accepted values | not tested...
- How many iterations of game will be run.
#### Lapsize
##### Accepted values | not tested...
- Kimble board ring size, usually 28 slots.
#### GoalSlots
##### Accepted values | not tested...
- Goal slots foreach team, usually 4 slots.
#### TeamsCount
##### Accepted values | not tested...
- Teams count, usually 4 teams.
#### TeamMarkers
##### Accepted values | not tested...
- Team markers/pieces count, usually 4 markers.

#### TeamRiskTaking
##### Accepted values | 0f - 1f
0. Min risk taking.<br/>
Always avoid spawning new markers on board if other team marker in range.

1. Max risk taking<br/>
Always spawn no matter how close other team marker is.

### TeamSkills
##### Accepted values | [ ] -0 - 1 - 2
0. "MarkerEater"<br/>
Tries to eat other team markers when possible.

1. "AmbushStart"<br/>
Tries to eat other team marker is current team spawn slot.

2. "SafeStart"<br/>
Avoid spawning marker when chance to get eaten is higher than risk taking float.


## Running

### Solution

Kimble_Solution.sln

- Found inside "Kimble" folder at repository root. In order to run solution you will have to have Visual Studio installed. To run solution inside Visual Studio double click solution to open it inside Visual Studio, press F5 or use Start button inside Visual Studio to run solution.

### Build
Kimble.exe 

- Found inside "Build" folder at repository root.


## Logs

### Statistics

After running build or solution, application should create a "Logs" folder which has game statistics as in JSON format for later viewing. JSON is regenerated after each run so it wont save data from multiple runs.

* Statistics.json
  ```sh
  {
    "gameCount": 20,
    "AverageTurnCount": 256,
    "AverageEatCount": 10,
    "AverageSpawnCount": 23,
    "AverageMoveCount": 133,
    "turnCount": 5136,
    "eatCount": 207,
    "spawnCount": 474,
    "moveCount": 2671,
    "wins": {
      "Red": 11,
      "Green": 4,
      "Blue": 2,
      "Yellow": 4
    }
  }
  ```


<p align="right">(<a href="#readme-top">back to top</a>)</p>

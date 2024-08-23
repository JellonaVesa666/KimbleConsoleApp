<a id="readme-top"></a>

<br />
<div align="center">
  <h3 align="center">Kimble Console App</h3>
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
    <li><a href="#usage">Usage</a></li>
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
  }```

<!-- USAGE EXAMPLES -->
## Usage

#### Lapsize
#### Accepted values, not tested...
- Kimble board ring size, usually 28 slots.
#### GoalSlots
#### Accepted values, not tested...
- Goal slots foreach team, usually 4 slots.
#### TeamsCount
#### Accepted values, not tested...
- Teams count, usually 4 teams.
#### TeamMarkers
#### Accepted values, not tested...
- Team markers/pieces count, usually 4 markers.

#### TeamRiskTaking
#### Accepted value range 0 - 1
0. Min risk taking.<br/>
Always avoid spawning new markers on board if other team marker in range.

1. Max risk taking<br/>
Always spawn no matter how close other team marker is.

### TeamSkills
#### Accepted single values 0 - 1 - 2
0. "MarkerEater"<br/>
Tries to eat other team markers when possible.

1. "AmbushStart"<br/>
Tries to eat other team marker is current team spawn slot.

2. "SafeStart"<br/>
Avoid spawning marker when chance to get eaten is higher than risk taking float.



<!-- USAGE EXAMPLES -->
## Running

In order to run solution you will have to have Visual Studio installed. Launching solution is straight forward navigate to repository root folder and open "Kimble_Solution.sln". Opening solution should launch Visual Studio as solution opened. After Visual Studio is open hit F5 or use Start, button to launch solution inside Visual Studio.


Kimble.exe can be also launched from project root by navigating inside "Build" folder and launching "Kimble.exe"


<p align="right">(<a href="#readme-top">back to top</a>)</p>
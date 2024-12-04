# AI-Sim
This project aim to exercise with the use of State Pattern to create a complex AI that simulates the behaviour of a lumberjack during a day-night cycle.

The main purpose of the lumberjack is to cut trees; at the start of each day it will look for a tree to cut. 
Cutting trees consumes stats.

Each Lumberjack has different stats: Hunger, Thirst and Stamina.
When one the stats reaches a critical state the lumberjack will behave diffently:
- Critical Hunger
  - Find food source
  - Move towards food source
  - Eat
- Critical On Thirst:
  - Find water source
  - Move towards water source
  - Drink
- Critical on Stamina
  - Move towards bed
  - Sleep

  The state machine uses push down automata pattern.

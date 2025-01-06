
# Blade Wanderer

## Overview
Welcome to my Unity **Blade Wanderer** project! This game serves as a portfolio piece showcasing my skills in game development, programming patterns, and Unity-specific workflows. The project is currently a work in progress and demonstrates my ability to implement and refactor complex systems with a focus on good programming practices.

## Features

### Player Mechanics
- **Weapon System**: The player can pick up weapons, equip or unequip them.
- **Dodging**: A dodge mechanic allows the player to evade enemy attacks.
- **Stamina Management**:
    - Stamina regenerates over time when the player is idle.
    - Stamina consumption during actions like dodging or attacking.
    - Smooth updates to the stamina bar using DOTween.
- **Soul Collection**: After defeating enemies, souls are visually represented with VFX and displayed in the UI.

### Enemy AI
- **Behavior States**: Enemies can patrol, chase the player, or engage in combat.
- **Last Seen Location**: If the player escapes, the enemy moves to the last known location.
- **Patrolling**: Enemies return to their patrol route if the player is not found.

### Combat System
- **Hit Detection**: Tracks the angle of incoming attacks.
- **Blood Effects**: Visual feedback for both player and enemies when hit.

### Health Management
- **Health Bars**: Smoothly animated health bars for both player and enemies using DOTween.
- **Damage Feedback**: Immediate UI updates and visual effects to reflect damage taken.

### Visuals
- **Lightweight VFX**: For events like soul collection and blood effects.
- **UI Integration**: Real-time health and stamina updates.
- **DOTween Integration**: Smooth animations for health and stamina bars.

### Audio Implementation
- Added sound effects for core gameplay elements such as walking, combat hits, and environmental sounds.
- Future plans for adding additional sounds, including ambient sounds and more action-related audio, to further enrich the gaming experience.

### Technical Features
- **Programming Patterns**: StateMachine, Factory, and Singleton patterns are extensively used.
- **Save/Load System**: Game state is saved to and loaded from files.

## Upcoming Features
- Currently focusing on creating and refining scenes and the world-building aspect of the game.
- More sound effects planned, with a focus on enhancing immersion and player experience across all game events.

## Installation
1. Clone this repository:
   ```bash
   git clone https://github.com/Rehvid/BladeWanderer.git
   ```
2. Open the project in Unity (version **6000.0.28f1**).
3. Play the game from the Unity Editor.

## About Me
I am an aspiring Junior Unity Developer passionate about creating engaging and polished gameplay experiences. This project highlights my understanding of Unity, programming patterns, and gameplay design. If you’re interested in discussing my work or have an opportunity, feel free to reach out!

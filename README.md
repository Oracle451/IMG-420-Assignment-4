# IMG-420 Assignment 4

## Structure

```
res://
├── assets/           # Visuals of the project including sprite sheets and tile sets
├── scenes/           # Scene files (.tscn) – you will create these in Godot
├── scripts/          # C# scripts used by the scenes
└── README.md         # This file
```

## Features
- A playable character that can be moved with the arrow keys.
- An enemy that follows you around the map and deals damage on contact.
- Heath pickups that allow the player to restore health.

---

## Resources
- All assets for this project are sourced from: https://pixel-boy.itch.io/ninja-adventure-asset-pack

---

## Controls

| Action                | Keyboard / Mouse     | 
|-----------------------|----------------------|
| Movement              | `Arrow Keys`         |

---

# Assignment Requirements

## Requirement: Tile-based world
Build your level using a TileMap node and one or more TileSet resources. Each tile will represent part of the world (floor, walls, obstacles, etc.). Remember: a tilemap is a grid of tiles used to lay out the game, and you need to create a TileSet before you can paint tiles.

## Solution:
The map of this game is using a grass and dirt tileset where the grass is mapped as a walkable navigation layer with no collision and the dirt is not navigatable and has collision.

---

## Requirement: Player character
Use a CharacterBody2D node as the root of your player. Add a Sprite2D (or AnimatedSprite2D) and a CollisionShape2D as children. Move the character in _PhysicsProcess() using a velocity vector and call move_and_slide() or move_and_collide() to handle collisions. For top-down movement you can take the input vector with Input.get_vector() and multiply by a speed, then pass it to move_and_slide(). For a platformer, apply gravity each frame (velocity.y += gravity * delta) and only allow jumping when is_on_floor() returns true.

## Solution:
The player character is a sprite 2d with a sprite sheet as its texture. By using the arrow keys the player can be moved: up, down, left, and right.

---

## Requirement: Sprite animation
Create animations for your player and at least one enemy. Use the AnimatedSprite2D node or a Sprite2D combined with an AnimationPlayer. According to the Godot class reference, AnimatedSprite2D is like Sprite2D but carries multiple textures as animation frames; the frames are stored in a SpriteFrames resource and configured via the editor. At minimum, implement idle and movement animations. You should be able to change animations in code (e.g., play a “walk” animation when moving).

## Solution:
The player character has a sprite sheet as its texture and it uses an animation player to move through the sprite sheet when walking.

---

## Requirement: Enemies with pathfinding
Your level must include at least one enemy that can chase or patrol. Implement enemy movement using 2D navigation:  
- Add navigation layers to your TileSet by expanding its Navigation Layers section and adding a layer. Paint navigation onto walkable tiles via the TileSet editor.  
- Attach a NavigationAgent2D node to your enemy. In C#, call set_movement_target() on the agent to follow the player’s position. Use the agent’s velocity_computed signal (or poll its next_path_position) to move the enemy toward the next waypoint.  
Enemies should avoid walls and obstacles defined in the TileMap.

## Solution:
There is one skeleton enemy which spawns on the opposite side of the map as the player and will always be navigating to the players current position. As soon as the enemy comes in contact with the player, it deals 10 damage and respawns in its start location.

---

## Requirement: Particle effects
Create at least two particle systems using Particles2D. For example, use a particle effect when the player picks up an item, when an enemy is defeated, or for environmental effects such as torches or smoke. Remember that particle systems simulate complex effects like sparks, fire or mist. Add a ParticlesMaterial to each Particles2D node; adjust parameters such as lifetime, speed and randomness.

## Solution:
Health packs give off a red particle effect all around them and the enemy gives off a white particle effect that floats off of them and fades away in a way that resembles smoke.

---

## Requirement: Interactions and simple physics
Implement interactions between the player and the environment:  
- **Collisions** – The player cannot walk through walls; collisions should be handled by the physics engine. Walls should be defined in the TileSet via the collision layer (as shown in the platformer recipe) or by adding CollisionShape2D nodes to static obstacles.  
- **Collectibles or projectiles** – Add an item (e.g., coin, key) that the player can pick up. When collected, play a particle effect and increment a score or change a game state.  

## Solution:
- **Collisions** – The player will collide with dirt on the tilemap as well as the enemy sprite
- **Collectibles or projectiles** – Health Packs are scattered around the map which the player can pick up to gain 20 health. After 5 seconds the health pack will respawn.


---

## Requirement: UI and feedback
Display minimal UI (score, health or timers). Use Godot’s UI nodes (such as Label and TextureProgressBar) to show game state.

## Solution:
The players health is displayed in the top left corner of the screen and updates as they take damage or heal.
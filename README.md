# ProjectOldBay
Began in july 2023 and ongoing.

A 2D side-scroll combat game. In this repository are all of my scripts to run in **Unity**. 
This is a collabrative project that I have been working on with friends. I am the sole programmer on the team so this work is my own. 

This is my first medium sized game outside small tutorial series I have followed on youtube before. The code takes heavy advantage of Unity's MonoBehavior System. Many classes are very small. 

I will describe each file in order of its dependency on others (low to high).

## Weapon 
Combat in the works in two forms. Melee or Ranged. These modes can be switched between by unholstering/holstering your gun.

* **Aiming.cs**: Aims of the player gun in accordance of mouse position on screen as a rotation around the Z axis.
* **AmmoPickup.cs**: Applies to objects that when collected increase the players ammunition. (AmmoCrates/Guns of the same type.)
* **Gun.cs**: Manages gun state(holstered/not), ammunition, time between shots, shooting logic(raycast method), and does the approprate damage to enemies.
* **WeaponSwap.cs**: Simply toggles between the hostered & melee, or unholstered & ranged attack statues. Depends on if you have picked up a gun yet or not. 

## Player
* **MeleeAttack.cs**: Applies damage to enemies within a specified hitbox with variable damage. Same class is applied to enemies to attack the player.
* **PlayerController.cs**: Manages melee control and animation of player. Will be refactored to handle other high level tasks(Possibly absorb PlayerHealth.cs).
* **PlayerHealth.cs**: Manges health state of player as well as associated helath bar. Triggers death animation when player goes to zero health.
* **PlayerMovement2D.cs**: Handles logic surrounding player runnong, jumping, crouching, and correctly animation handle animation states based on behavior. Also makes sure gun faces correct direction based on how player faces. 

## Enemy

* **EnemyController.cs**: Controls Enemy life and death states. Destroys object on death(after associated animation).
* **EnemyGroundPatrol.cs**: Mangaes the Investigative state of Enemies. A Finite State Machine controls whether an enemy patrols a path delineated by a set of waypoints, is chasing player upon discovery, or is in range and ready to attack.(These ranges are different for enemies who attack with melee or ranged weapons.)
* **Projectile.cs**:

## Game State/Extras
* **Exit.cs**: Simply Ends the Program when ExitGame() is Called, This is called from the pause menu and main menu.
* **Pause.cs**: When manages the activation/deactivation of a pause to all gameplay. This is used when acessing menu's to suspend gameplay. When the menu closes we resume.
* **SmoothCameraFollow.cs**: Controls the motion as it follows the player. The damping factors in the horizontal and vertical directions can be changed at will. This allows for a smooth camera follow.
* **SpawnOnEnemyDeath.cs**: This class allows me to spawn enemies to appear when another enemy(or interactible object) is destroyed. This is helful when to I want to create a game flow based on the players actions which allows me to be time independent.

This is an ongoing project of mine that started from a tutorial on Unity Learn. It is my first game project and the reason I am making it is to get a good foundation on the basics before
moving onto another project.

The tutorial was called "sprite flight 2D" (hence the name), and
it was about learning how to create a basic 2D game. You can find the original tutorial in the link below:

https://learn.unity.com/course/2d-beginner-game-sprite-flight

After I finished the tutorial, I decided to take it a step further and add more content, and evolve it into a full small game to add it to my portfolio.

The first idea was change the game mechanics and make it a bit more interested, but giving the player the ability to shoot. This was a fairly easy implementation
although, I tried to spice it up a bit with an overheat mechanic, where the player's gun would stop working if he/she continuously firing at the asteroids on screen.

After I have added that feature, I wanted the game to be able to be played by a gamepad controller, and have Android controller capabilities. I used the new Input System of
unity to implement a more responsive movement (the original was click to move to the location, kind of like the retro game "Asteroids") and implement an aiming system. I found it
tricky to implement the aiming system, as the new input system was not working well with the mouse input (or at least I could not find a solution that would make use of the new Input System), 
so I created 2 different types of input, one for Gamepad/Android and one for Keyboard and mouse. After some refining on the overall control, I have managed to create a good fluid contol scheme,
which worked well for both inputs, and with the ability to change the sensitivity for the aiming.

When the player control was finished, I moved onto the enemy system, which I am currently working on. I created an endless spawening system, which allowed a certain amount
of enemies on the screen, so the player is not overwhelmed but gives him/her a challenge and a pseudo scoring system based on the enemy's size. I cannot say that it is ideal, but it works
well for the prototype of the game. Regarding this, I have some plans to implement a basic enemy AI, and maybe change the enemy types to something else that can actually chase an shoot at the player,
chase the player and create a bit of a challenge. Another idea is some boss fights maybe to slice down the game into different levels.

After I implemented all the above and had a decent game loop, where enemies where spaning, the player was avoiding them and was shooting at them and on each kill the playe was awardered some points,
until the player touches any of the enemies and dies, I thought about UI. I created a basic HUD for the maing game scene, and a pause menu with a model for confirmation if the player wants to exit
the game. The pause functionality has not yet been implemented but it is on its way.

Roadmap ahead:
- Android input implementation
- Enemy AI, new enemy types, boss enemies
- Player power ups
- Main menu UI
- Pause game capabilities
- Different level creation
- General polishing (new sprites, more particles, etc)
- Performance Optimization

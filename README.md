**The University of Melbourne**
# COMP30019 – Graphics and Interaction

## Teamwork plan/summary

<!-- [[StartTeamworkPlan]] PLEASE LEAVE THIS LINE UNTOUCHED -->

<!-- Fill this section by Milestone 1 (see specification for details) -->

#### Team Roles:
* Alex: Procedural generation of game objects, player shielding
* Ben: Game logic, including player and obstacle implementation; Game Backgrounds using particle systems; Custom shaders, post processing effects
* Yu-Tien: Design and Modeling, including 3-D modeling for various game objects
* Simai: Menus and Design, including assisting with 3-D modeling and menu design

Roles are not final, and team members are expected to provide help where needed and possible.
  
Team members will work in separate files where possible, to prevent loss of data in overlapping push/pull requests.

When necessary, team members should notify others of changes made in shared files, and watch for push/pull requests.

Primary Communication will be performed over Instagram Chat, with weekly status meetings held over Zoom (https://unimelb.zoom.us/j/8138112491?pwd=ckhKOWFaendSWm1vWGZTT3N5NmZlUT09).
  
<!-- [[EndTeamworkPlan]] PLEASE LEAVE THIS LINE UNTOUCHED -->

## Final report

### Table of contents
* [Game Summary](#game-summary)
* [Instructions](#Instructions)
* [Design](#Design)
* [Graphics](#Graphics)
* [Procedural Generation](#Procedural-Generation)
* [Particle Systems](#Particle-Systems)
* [Querying Methods](#Querying-Methods)
* [Observational Methods](#Observational-Methods)
* [References](#References)
* [Technologies](#technologies)

### Game Summary
_Blockade Runners_ is a 2.5D infinite scroller that has you flying your spaceship through an asteroid field fighting off enemies. Asteroids and enemies are spawned in randomly, with bosses spawned at regular intervals. As the game goes on, obstacles are spawned more frequently, and enemy strength increases. The goal of the game is to survive as long as possible, while obtaining the highest score you can by destroying asteroids and enemies.

### Instructions
#### Gameplay
Use WASD or the arrow keys to control your ship to avoid obstacles.
Use the mouse to aim and fire your ship's cannons to destroy obstacles.

Fly through the blue rings to activate shields to protect yourself (shields are depleted after 3 hits).
Fly through the green rings to regain some lost health (up to the starting value).

Press 'M' to switch between simple movement and inertia movement.
While in inertia movement, press 'Shift' or 'B' to brake.

#### Customization
In the customization menu, select which weapons (controlling fire rate and damage) and engines (controlling speed and health) you want to use in game, and then press confirm to set your choices.

### Design
2.5D gameplay was chosen to simplify the design process and allow for easier control by the player. Controls are mapped to both WASD and the arrow keys to be more accessible and intuitive. The game was designed to become more challenging as time goes on, by varying how the randomized obstacles are spawned (location, speed, damage, health), allowing for a gameplay experience that gets steadily more challenging as time goes on. Progressively more challenging bosses spawn in regularly to help mark how far the player has come, while also acting as an added challenge to overcome.
Simple movement and controls were chosen over more complicated inertia movement systems to improve player comfort.
Players are also allowed to customize their ship, to allow for different play styles (fast and light versus slow and heavy).

All Game objects were modelled using Blender, including a spaceship, asteroids, enemies, a boss, bonus rings, weapons, and engines. The design and the colours were inspired by https://www.youtube.com/watch?v=kIswhf-DZnc. The textures are created by adjusting the parameters of material properties in Blender, such as base colour, metallic, specular, and roughness. 

The User Interface is designed using Adobe Illustrator, and most of the 2D sprites on the game menu are designed using Procreate.

### Graphics
We used 2.5D gameplay and graphics, using custom 3D modeled objects.
Most of the game objects used standard rendering, while custom shaders were used for added effects on specific game objects.

The first shader, located at _Assets/Shaders/HealthRingFancyShader_ was written for the in-game health ring that the player can fly through to gain back some health after taking damage. The base of this shader uses a Phong illumination model, with a hardcoded base colour (green), which has been modified to add in various effects. In the vertex shader stage, the vertices are transformed as needed, and the hardcoded colour is set. Additionally, the variable maintaining the world coordinates is modified using a sin function to modify how lighting is calculated in the fragment shader. The fragment shader uses the Phong Illumination model, with ambient light modified using a sin function, and the dot product for RGB reflections mapped to a new input, to adjust the appearance of the ring.

The second shader, located at _Assets/Shaders/ShieldUnlitShader_ was written for the shield that the player can get. This shader uses the shield capacity to adjust the appearance of the shield as it takes damage, including increasing the total transparency, and increasing the rate at which the transparency pulses. An additional sin function was used to adjust the colours in the shield, to give a ripple effect. A pseudo-Fresnel effect is implemented (with the dot product function outputs remapped) to give the shield a rounded appearance, by using the dot product to transform the transparency value. These functions were written in the fragment shader to give a smoother appearance.

### Procedural Generation
The planets that pass the player in the background as they are progressing are procedurally generated and completely created during runtime.
This is done by creating a new 2D texture from scratch using an algorithm that creates pseudo random patterns that can be applied to our blank texture.
Perlin noise is commonly used to add the appearance of realism in computer graphics, so it seemed to be an appropriate choice for our game.
The algorithm generates float values between 0 and 1 which are then used to determine the colours of each pixel of the texture.
This texture is then applied to object as it is instantiated into the game.
In order to avoid repeating patterns in the algorithm's output, a random seed is generated on each creation of a new planet that ensures that each planet has a unique pattern different from all others.
The texture generator then draws from a pre-made colour palette that contains a wide array of colours based off real planets (Jupiter being the main provider) as well as some non-real planets that are just cool to look at.
It is almost impossible that while playing the game you will ever see two identical planets.
Additionally, planets also vary in more simple aspects such as scale, and spawn location, both determined randomly at runtime.

### Particle Systems
A custom particle system was used to generate the stars seen in the background of the game.
Attributes varied included the effect shape and emission behavior, to effectively create a system where particles travel across the screen and give the impression of horizontal movement.

A script was written which will increase the speed multiplier of the system as time went on, so that as the game continues the stars crossing the screen move faster, giving the impression of steady acceleration (as the stars speed up, the trails also get longer, making the background more dynamic and giving the player additional feedback of how far they've gotten). Star speed is set using the square root of the game time, allowing the acceleration to keep track with that of other game objects.

Randomness was used to vary the speed of the stars as they are spawned in, providing the illusion of depth thanks to the pseudo multi-planer effect.
Randomness was also used when setting the color of the stars, to make the background appear more dynamic.

The impact and destruction special effects were also created using simple particle systems.

### Querying Methods
Semi-structured interviews were used for the playtest process to discover current design flaws. It is a commonly used exploratory investigation method that follows a pre-defined interview structure but allows flexible adjustments to the questions.

Before conducting our interviews, sample interview questions are prepared as a guideline: The order of questions can be changed, and the content can be modified as the interview proceeds. Possible probing questions are also listed below each question.

#### Methodology:
Five participants were recruited after being asked for their consent. Before the evaluation started, participants were informed that the entire process would be recorded. In the beginning, the interviewer briefly introduced the game to ensure participants had a basic understanding of the game genre and the gameplay. Then participants were asked to explore and play the game on the laptop freely. Finally,semi-structured interviews were conducted to get valuable feedback from these game testers. The whole process lasted around 40 minutes. All collected data were organized in the "Feedback from Interviews" section based on the audio recordings and interview notes written by the note-taker.

#### Participants:
| Participants | Name (Pronouns) | Age  | Degree            | Game Experience                                           |
| :----------- | :-------------- | :--- | :---------------- | :-------------------------------------------------------- |
| P1           | DYJ (he/him)    | 24   | art               | experienced video game player                             |
| P2           | YS (she/her)    | 21   | engineering and it | experienced console game player                           |
| P3           | MG (he/him)     | 23   | engineering and it | experienced video game player                             |
| P4           | JZS (he/him)    | 31   | engineering and it | novice game player, but has experience in shooting games  |
| P5           | TS (she/her)    | 34   | engineering and it | novice game player, but has experience in using a gamepad |

#### Sample Interview Questions:
* Q1: Have you played this kind of video game before?
  * Probing 1: If yes, does it affect your expectations of our game before playing?
  * Probing 2: If yes, in which ways would you like to compare the two games?
* Q2: What do you think of the game balance? 1 for easy & 5 for difficult, how would you rate it?
  * Probing: How to balance if you are a game designer?
* Q3: What do you think of the gameplay design?
  * Probing 1: Are the keyboard controls easy to use & remember?
  * Probing 2: Are the game objects clear enough for you to know what they represent for? ( E.g. When the bonus ring appears on the screen, does it give a clue that you need to collect it to get a shield?)
  * Probing 3: How to improve if you are a game designer?
* Q4: What do you think of the overall design of menus? (main/pause/game over/hud)
  * Probing: How to improve if you are a game designer?
* Q5: Did the instructions demonstrate clearly how to play this game?
* Q6: What do you expect if we add a customization function?
  * Probing 1: What else components or attributes would you like to customize except from engines and weapons?
  * Probing 2: Would you be satisfied with three types of engines/weapons? Want more or less or okay?
  * Probing 3: What do you expect to see during the play after customizing your spaceship? (HUD presentation? Effect? Animation?)
* Q7: Do you have other suggestions for future improvements?

#### Interviews Notes:
* 1. Ratings on Easy / Difficult
  * [P1] Score: 700+. Rating “1”. 
  * [P2] Score: 200+. Rating “3-4”. 
  * [P3] Score: 800+. Rating “3”.
  * [P4] Score: 100+. Rating “2”.
  * [P5] Score: 200+. Rating “3”.

* 2. Possible Ways of Balancing the Game
  * [P1,P2,P3] Add various types of enemies with different modes of attacking
  * [P1,P4,P5] Set levels in the game
  * [P1] Players can choose to upgrade their weapon on each level
  * [P2] The second boss should be stronger than the first boss, and should have more diverse ways of attacking
  * [P3] Enhance the power of bullets
  * [P3] Set an upper limit on the game difficulty: the speed of all asteroids/enemies will not increase anymore once reach this limit
  * [P3] Change the background for an infinite game periodically so that players will not get bored
  * [P5] Switch different maps/scenes so that players will not get bored

* 3. Feedback for Key Control
  * [P1,P2,P5] Hard to aim
    * [P1] increase the range of attack
    * [P2] increase the fire speed
  * [P3] Add trump card on the right click
  * [P5] Failed to realize the mouse could be used to control the spaceship direction or aim the target because she thought that normally it would focus on one direction. It would be better if explain it in the instructions.

* 4. Feedback for Game objects
  * [P1,P5] Visualize the shield status to inform players when the shield is going to disappear (If the shield will disappear after several times of crashes, display the number of shields left; If the shield will disappear after a period of time, display the time left)
  * [P1] Except from the shield bonus rings:
      * gain a certain health value
      * increase the bullet attack speed
      * add more bullets 
  * [P1] Adjust the ring size to 60% of the spaceship
  * [P2] The models on the screen should be smaller to allow more space to move
  * [P2] The initial position of the spaceship should be on the left side
  * [P2,P3,P5] The red bonus ring is a bit confusing (They tried to avoid colliding because they thought red objects are dangerous. P2 suggested using blue for shield and green for health, and P3 suggested adding different icons on models, but P5 thought the improved version after changing the colour is still not clear)
  * [P3,P4] At predefined intervals, a bonus ring that allows the player to enter the invincibility state for a few seconds would appear (or another spaceship will come for help by your side for a few seconds)
  * [P4,P5] The planets that appeared in the background are confusing

* 5. Feedback for Menus
  * Main Menu
    * [P1] Make the button colour consistent with the overall style
    * [P2] No need to add flash animations on the buttons
    * [P4] Unclear word using: The word “customization” has two meanings: one for the spaceship, another for difficulty adjustment
  * Instruction Page
    * [P1] Allow players to select if they want to use WASD or ARROWS
    * [P2] Too much space on the right side
    * [P2,P5] Instruct the new player on how to play when they start a new game (display the keyboard control instruction in the game scene)
  * Customisation Page
    * [P1] Add the customisation of trump card(either protection or attack), which players could only use once for each level
    * [P1,P2,P5] For the customisation of weapons and engines, add some demos to explain the description on this page. For example, it could be shown by Health and Speed. E.g., the heavier engine would have a longer Health Bar, the lighter engine would fly faster.
    * [P3] Unlock new cannons/engines based on the best score
    * [P4] Allow players to customize the colour of the spaceship
    * [P5] Allow players to choose special skills when they reach certain scores, e.g. increase the accuracy for aiming the target enemy
  * Pause Menu
    * [P1] The button is not obvious at all, should set the opacity to 100% directly
    * [P1] When the pop-up is displayed, and the player is going to select the buttons, the direction of the spaceship would also turn toward the mouse
    * [P4] The text colour is not clear enough
  * Game Over Menu
    * [P1] Screenshot function for them to show off (?)
    * [P3,P4] Display “my best score” 
    * [P5] Make the “Score” stand out more
  * HUD
    * [P1] The Health Bar could have a scale so they would have the idea of the damage caused by asteroids/enemies/boss 
    * [P2,P4,P5] Add corresponding icons/ text to indicate previous customizations

#### Changes Made Based on Interview Feedback Included:
* Added another bonus ring for healing
* Changed the colour of bonus rings to avoid misunderstandings
* Changed the initial position of the spaceship from the middle to the left side
* Changed the appearance of the health bar
* Made the pause button clearer 
* Adjusted the animation for buttons
* Changed the wording from "Customization" to "Spaceship Customization" in the main menu
* On the customisation page, added models and health indicators to explain the description on this page
* Displayed the type of engine and cannon that the player customized on the HUD
* Shrunk game objects to give player more space to move

### Observational Methods
Cooperative Evaluation was used to get feedback from participants in a casual manner. Participants played the game, and were allowed to speak with the observer while playing, including questions both from and to the players. Written notes of participants' impressions were made, including both specific feedback from individual participants, as well as general feedback that all participants agreed upon.

#### Participants:
| Participants | Name (Pronouns) | Age  | Degree  | Game Experience     |
| :----------- |:--------------- | :--- | :------ |:--------------------|
| P6           | CB (they/them)  | 20   | theatre | novice player       |
| P7           | LC (she/they)   | 18   | arts    | novice player       |
| P8           | ES (he/him)     | 19   | science | experienced player  |
| P9           | BB (he/him)     | 18   | design  | experienced player  |
| P10          | MH (he/him)     | 20   | music   | experienced player  |
| P11          | CL (he/him)     | 20   | arts    | novice player       |
| P12          | WI (he/him)     | 18   | arts    | experienced player  |

#### Feedback from Observations:
* General Feedback
  * Player shield was overpowered
  * Expressed desire for player health indicator
* CB
  * Had difficulty shooting at small enemies
  * Difficulty fighting off boss
  * Found asteroids too difficult to destroy
* LC
  * Found independent movement and aim directions awkward to play with
  * Killed quickly by first boss (even after health had been lowered based on observations of CB)
* ES
  * Noticed framerate issues, esp. during asteroid explosions
  * Indicated desire for some indicator of player shooting rate
  * Found boss overly aggressive (but did not die) - Suggested that boss should keep greater distance if shooting at player
  * Expressed desire for an indicator of enemy health levels
  * Found that collisions dealt insignificant levels of damage to the player
  * Could not tell that particles in asteroid explosions were not simple spheres
  * When using inertia movement, found that deceleration from brake was too fast
  * Had difficulty distinguishing backdrop set pieces from game objects to be avoided
* BB
  * On menu, recommended that flashing buttons should not disappear fully (participant is studying design)
  * Found that player shield was overpowered, expressed desire for it to be weaker
* MH
  * Obtained highscore of all players observed!
  * Found inertia movement too slippery to play with
  * Suggested alternate method for shielding using set duration instead of health
  * Suggested improvements for distance gauge - remove decimals, add in fun space-units (like parsecs)
  * Expressed additional desire for weapon cooldown time indicator - suggested tying indicator to the mouse (like Minecraft does)
  * Expressed desire for weapon powerups in game - increased damage, increased firerate 
* CL
  * Echoed previous feedback
* WI
  * Echoed previous feedback

#### Changes Made from Gameplay Observations Included:
* Added player health indicator
* Framerate fixes
  * Asteroid explosion particles changed to spheres
  * Player movement simplified to remove extra calculations
* Enemy rebalancing
  * Asteroid health set to a fixed value and reduced
  * Small enemy hitbox size increased
  * Boss made less aggressive - keeps greater distance from player, lowered health
* Shield rebalancing
  * Shield capacity lowered from 5 hits to 3
* Units added to distance meter, which is rounded to two decimal places

### References
* https://github.com/COMP30019
  * https://github.com/COMP30019/Workshop-8
  * https://github.com/COMP30019/Workshop-9
  * https://github.com/COMP30019/Workshop-10
  * https://github.com/COMP30019/Workshop-11

* https://docs.unity3d.com/
  * https://docs.unity3d.com/520/Documentation/Manual/RenderingPaths.html
  * https://docs.unity3d.com/Manual/CreatingPrefabs.html
  * https://docs.unity3d.com/560/Documentation/Manual/Prefabs.html
  * https://docs.unity3d.com/ScriptReference/Object.Instantiate.html
  * https://docs.unity3d.com/ScriptReference/ParticleSystem.html
  * https://docs.unity3d.com/520/Documentation/Manual/SL-UnityShaderVariables.html
  * https://docs.unity3d.com/Manual/shader-writing-vertex-fragment.html
  * https://docs.unity3d.com/Manual/SL-VertexFragmentShaderExamples.html
  * https://docs.unity3d.com/ScriptReference/Texture2D-ctor.html
  * https://docs.unity3d.com/ScriptReference/Mathf.PerlinNoise.html
  * https://docs.unity3d.com/ScriptReference/Object.DontDestroyOnLoad.html

* https://answers.unity.com/questions/
  * https://answers.unity.com/questions/1105218/how-to-make-an-object-shoot-a-projectile.html
  * https://answers.unity.com/questions/1655687/how-can-i-change-the-speed-of-my-particles-during.html
  
* https://forum.unity.com
  * https://forum.unity.com/threads/normal-information-for-the-unlit-shader.559288/
  * https://forum.unity.com/threads/angle-between-object-and-camera.214382/

* https://cla.purdue.edu/academic/rueffschool/ad/etb/resources/ad41700_unity3d_workshop03_f13.pdf
* https://catlikecoding.com/unity/tutorials/scriptable-render-pipeline/custom-shaders/
* https://www.dorian-iten.com/fresnel/
* https://www.cs.utexas.edu/~bajaj/graphics2012/cs354/lectures/lect14.pdf
* https://pastebin.com/iGuv7AvX
* https://www.youtube.com/watch?v=kIswhf-DZnc
* https://www.youtube.com/watch?v=zc8ac_qUXQY&t=1s
* https://www.youtube.com/watch?v=BLfNP4Sc_iA


### Technologies
Project is created with:
* Unity 2022.1.9f1 
* Ipsum version: 2.33
* Ament library version: 999

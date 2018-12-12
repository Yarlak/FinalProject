*********************
WIZARD SOCCER README
*********************
	TEAM MEMBERS - list of the students who contributed to this project
	RELEVANT FILES - overview of the contents of the submission
	LAUNCHING THE GAME - information pertaining to running the game on PC
	PLAYING THE GAME - brief tutorial on basics of gameplay and menu navigation

*************************************************************************************************************
IMPORTANT:
**********
The majority of cosmetic assets (3D models, textures, special effects) were not produced by our team. Many were downloaded from the 
Unity Asset Store. Please see a list of credits by pressing the "Credits" button after launching the game.

Additionally we used online coding resources for reference like StackOverflow, Unity Answers, and Unity Forum

*************************************************************************************************************
--------------------
TEAM MEMBERS
--------------------
	Matthew Russiello
	Albert James
	Tong Xu
	Thomas Blessing
	Jay Trask

--------------------
RELEVANT FILES
--------------------
	/TestPlan.PDF - contains step by step instructions on how to test certain features manually
	/CommitHistory.PDF - an ordered list of all commits done by the team. Each team member is labeled as follows:
		MR - Matt Russiello
		TX - Tong Xu
		T - Thomas Blessing
		JT - Jay Trask
		AJ - Albert James
	
	/builds
		/WizardSoccer.exe - player used to launch the game
		/WizardSoccer_Data - folder which houses all the resources needed to play the game
		/UnityPlayer.dll - needed to launch the game
	
	/Assets/Scripts
		This folder contains all the scripts written for the project
	

--------------------
LAUNCHING THE GAME
--------------------
Perform the following steps twice as two players are needed to play the game.
In the end you should have 2 instances of the game running on your PC.
You could also run one instance on two separate computers with internet access.
	- Use a Windows OS
	- Open "builds" folder
	- Open the file "WizardSoccer.exe"
	- Select resolution of 1280 x 720
	- Click "Play"

-----------------
PLAYING THE GAME
-----------------
For this section I will be using Player1 and Player2 to refer to the two separate instances of the game being run.
	
	Joining a Match:
	----------------
		Player1
			- From the main menu select "Multiplayer"
			- Select "Online"
			- Type in the name of your game (this can be any string and will only be needed for Player2 to find your match)
			- Click "Host Online Match"
			
			NOTE -- If you are unable to type in a name for the game just click "Host Online Match" it will make a game called "default"
		
		Player2
			- From the main menu select "Multiplayer"
			- Select "Online"
			- Type in the name of the match Player1 made
			- Click "Join Online Match"
			- Another white button will appear to the left with the name of the game - click on it to join the match
	
	Playing a Match:
	----------------
		- Scoreboard displays current score in top left corner of screen
		- Click "Stop Match" to exit the current match - if host (player who created match) leaves then the client (other player) will also be booted
		- There are six buttons on the bottom of the screen that each represent a different spell the player can use to try and score a goal
		- From left to right these spells are:
			FIREBALL - shoot a projectile that can hit the ball with enough force to propel it forward
			WALL - create a temporary barrier that can block the ball and projectile spells from your opponent
			WIND - create a temporary line of tornadoes that apply a force to the ball when the ball comes near them
			Hex - turn the opponent into a chicken disabling them from using spells for some turns
			Curse - temporarily lower the accuracy of your opponent
			Monster - summon a minion that can shoot the ball at your opponent's goal
		- Select a spell to cast which is not currently recharging (recharging spells have a number displayed over them detailing how many turns until they are again available)
		- A series of runes (diamonds) will appear on the screen
		- Starting with the brightest colored rune trace the shape. They must be colored in order so if you miss one you must go back and retrace it before moving on to trace others
			FOR EXAMPLE:
				
				A - B - C - D
				
				Consider the above list ABCD as a series of 4 runes. You need to trace C before you can trace D. B must be traced before C and A must be traced before B.
		- Quickly trace the runes (you only have a few seconds). Each rune will turn pink after being traced. 
		
		- MAKE SURE TO KEEP YOUR MOUSE PRESSED DOWN AFTER YOUR LAST RUNE UNTIL AFTER YOU HAVE AIMED YOUR SHOT
		
		- The percentage of runes you have traced will appear in the top left corner of the screen. The higher the percentage the more accurate your shot will be
		- Keeping your mouse pressed. The Runes will disappear and show present you a red cube that shows the place on which you are currently aiming your spell
		- Release the mouse button to lock in your coordinates for the spell
		- Once both players have locked their coordinates for their spells the game will execute both spells
		- First to 3 goals wins!
		- On "Game Over" screen click the "Stop Match" button to exit to main menu





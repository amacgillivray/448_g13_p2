Battleship:

We created battleship in Visual Studio using Windows Forms.  The language used is C#.

We followed the following design process:
Main Menu Form - links to game type and credits

Local Game:
Player Creator Form ( Creates to player objects and uses them to create a Game Object) - This Game object gets passed to every form until the game ends - Links to Game Setup

Game Setup (lets the user choose how many ships they want) - links to Game choice

Game Choice (lets players place their ships) - Links to GameForm

GameForm - Playing Window, lets the user target a coordinate and fire - Links to landing page or if the game is finished the win page

Landing Page - lets the users switch spots and links back to GameForm

Win Page - Plays when a player wins

Credits:
Credit Page: Credits the contributors of the project.

Classes:
Program: Executive that only opens the first form

Game: Class that handles all game interactions. Contains two Player objects, and variables for the number of ships each player has in the game. Has methods for handling shots, hits, misses, and sinking ships.

Player: Class that contains data necessary for each player. Contains a list of Hits, Misses, and Ships, the players name, and the number of ships sunk.
	Hits and Misses are stored as 2 element integer arrays following the format: hit[col, row]
	Ships are represented by two dimensional integer arrays. One dimension of the array is the squares the ship occupies, inside each square is the second dimesion, the column and row of that square, and if the square has been hit or not (1 for hit, 0 for miss)
		Access them accordingly
			ship[square][col, row, hit] (if the square is hit ship[x][2] == 1)






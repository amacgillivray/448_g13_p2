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



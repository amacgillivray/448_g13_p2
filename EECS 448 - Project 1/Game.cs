using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EECS_448___Project_1
{
    public class Game {
        #region variables 
        private int playerTurn;
        private Player playerOne;
        private Player playerTwo;
        public int ai_level;
        private bool ai_havehit = false;
        //private int[2] ai_hitbase;
        private bool ai_trynorth = false;
        private bool ai_trysouth = false;
        private bool ai_tryeast = false;
        private bool ai_trywest = false;
        private Random rand = new Random();
        #endregion

        #region getters & setters
        public int getPlayerTurn() { return playerTurn; }

        public void setPlayerTurn(int player)
        {
            if (player > 2 || player < 1) throw new Exception("Invalid player number!");
            else playerTurn = player;
        }

        public ref Player getCurrentPlayer() {
            if (playerTurn == 1) return ref playerOne;
            else return ref playerTwo;
        }

        public ref Player getCurrentOpponent() {
            if (playerTurn == 2) return ref playerOne;
            else return ref playerTwo;
        }

        public Player getPlayerOne() { return playerOne; }
        public Player getPlayerTwo() { return playerTwo; }
        

        //Sets the current player value to the passed value
        public void setPlayerOne(Player one) { playerOne = one; }
        public void setPlayerTwo(Player two) { playerTwo = two; }

		#endregion

		#region Constructors
		//constructors
		public Game() {
            playerOne = new Player();
            playerTwo = new Player();
            playerTurn = 1;
            ai_level = 0;
        }

        public Game(Player one, Player two) {
            playerOne = one;
            playerTwo = two;
            playerTurn = 1;
            ai_level = 0;
        }

        public Game(Player one, Player two, int ai_difficulty) {
            playerOne = one;
            playerTwo = two;
            playerTurn = 1;
            ai_level = ai_difficulty;
        }

        public Game(int ai_difficulty)
        {
            playerOne = new Player();
            playerTwo = new Player();
            playerTurn = 1;
            ai_level = ai_difficulty;
        }
        #endregion

        #region methods

        //exchange whose turn it is
        public void swapCurrentPlayer() {
            if (getCurrentPlayer() == playerOne) {
                setPlayerTurn(2);
            } else {
                setPlayerTurn(1);
            }
        }


        //check sunk
        private bool isSunk(int[] shot, int[][] ship) {
            //check if any square is not hit (index 2 in each square) ship[sqaure][index 0: col, index 1: row, index 2: hit? (0  = no)
            for (int i = 0; i < ship.Length; i++) {
                if (ship[i][2] == 0) return false; //if the current square is not hit, return false
            }

            //if no square is un-hit, return true (the ship is sunk)
            return true;
        }


        //check hit
        private int[][] shipHit(int[] shot) {
            //loop through each of the current players unsunk ships
            for (int i = 0; i < getCurrentOpponent().getShips().Count; i++) {

                //create copy of indexed ship 
                int[][] ship = getCurrentOpponent().getShips()[i];

                //loop through each square in ship and see if the matches any of those spots
                for (int j = 0; j < ship.Length; j++) {
                    //get ship square coordinates
                    int[] shipSquare = { ship[j][0], ship[j][1] };

                    //compare coordinates
                    if (shot.SequenceEqual(shipSquare)) {
                        //set this ship's square to "hit"
                        getCurrentOpponent().getShips()[i][j][2] = 1;
                        return ship; //return the hit ship's array
                    }
                }
            }
            //miss
            return null;
        }

        //fire
        public void fire(int[] shot) {
            //create ship object of the ship that is hit by the guessed shot (returns null if miss)
            int[][] hitShip = shipHit(shot);

            //create copy
            int[] shotCopy = new int[2];
            shotCopy[0] = shot[0];
            shotCopy[1] = shot[1];

            //check if hit
            if (hitShip != null) {    
                //add hit
                getCurrentPlayer().addHit(shotCopy);

                //check if sunk
                if (isSunk(shot, hitShip)) {
                    //show message box of what you or the AI sank
                    //if (ai_level == 0 || playerTurn == 1 )
                        MessageBox.Show(whichShip(hitShip.Length));
                }

            } else {
                getCurrentPlayer().addMiss(shotCopy);
            }
        }

        private string whichShip(int squares) {
            string ship = "";
            switch(squares) {
                case 1:
                    ship = "Patrol Boat";
                    break;
                case 2:
                    ship = "Destroyer";
                    break;
                case 3:
                    ship = "Cruiser";
                    break;
                case 4:
                    ship = "Battleship";
                    break;
                case 5:
                    ship = "Aircraft Carrier";
                break;
			}

            if (ai_level == 0 || playerTurn == 1) 
                return "You sank " + getCurrentOpponent().getName() + "'s " + ship + "!";
            else
                return "Your " + ship + " was destroyed!";
        }



        private int rng()
        {
            return rand.Next(10);
        }

        public void hitgen_easy()
        {
            playerTurn = 2;
            int row = rng();
            int col = rng();
            int[] targetSquare = new int[2];
            targetSquare[0] = col;
            targetSquare[1] = row;

            bool targeted = false;

            while (!targeted)
            {
                targeted = true;
                row = rng();
                col = rng();
                targetSquare[0] = col;
                targetSquare[1] = row;
                //check if target is legal (has it already been guessed?)
                for (int i = 0; i < getCurrentPlayer().getHits().Count; i++)
                {     //check if targeted square is on a hit
                    if (targetSquare.SequenceEqual(getCurrentPlayer().getHits()[i])) targeted = false; //no longer target confirmed
                }
                for (int i = 0; i < getCurrentPlayer().getMisses().Count; i++)
                {
                    if (targetSquare.SequenceEqual(getCurrentPlayer().getMisses()[i])) targeted = false; //no longer target confirmed
                }
            }

            fire(targetSquare);
            playerTurn = 1;
        }

        public void hitgen_medium()
        {
            playerTurn = 2;
            if (ai_havehit)
            {
                
            }
            else
            {

            }
            playerTurn = 1;
        }

        public void hitgen_hard()
        {
            playerTurn = 2;
            playerTurn = 1;
        }

        #endregion
    }
}

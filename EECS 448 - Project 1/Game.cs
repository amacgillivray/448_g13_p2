using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECS_448___Project_1
{
    public class Game {
        #region varibales 
        private int playerTurn;
        private Player currentPlayer;
        private Player currentOpponent;
        private Player playerOne;
        private Player playerTwo;
        #endregion

        #region getters & setters
        public int getPlayerTurn() { return playerTurn; }
        public Player getCurrentPlayer() { return currentPlayer; }
        public Player getCurrentOpponent() { return currentOpponent; }
        public Player getPlayerOne() { return playerOne; }
        public Player getPlayerTwo() { return playerTwo; }
        public void setPlayerTurn(int turn) { playerTurn = turn; }
        
        //set 
        public void setCurrentPlayer(int player) {
            if (player == 1) currentPlayer = playerOne;
            else if (player == 2) currentPlayer = playerTwo;
            else throw new Exception("Invalid player number!");
            setCurrentOpponent();
        }
        public void setCurrentOpponent() {
            if (currentPlayer == playerOne) currentOpponent = playerTwo;
            else currentOpponent = playerOne;
        }

        public void setPlayerOne(Player one) { playerOne = one; }
        public void setPlayerTwo(Player two) { playerTwo = two; }

        #endregion

        //constructor
        public Game() {
            playerOne = new Player();
            playerTwo = new Player();
            playerTurn = 0;
        }

        public Game(Player one, Player two) {
            playerOne = one;
            playerTwo = two;
        }

        #region previous Version
        /*public int playerTurn = 1;
        public string playerOneName;
        public string playerTwoName;

        public List<int[][]> playerOneShips;
        public List<int[][]> playerTwoShips;

        public List<int[]> playerOneHits;
        public List<int[]> playerTwoHits;

        public List<int[]> playerOneMisses;
        public List<int[]> playerTwoMisses;

        public Game(string playerOneName, string playerTwoName)
        {
            this.playerOneName = playerOneName;
            this.playerTwoName = playerTwoName;

        }*/


        /*public bool isHit(int[] guess, int turn)
        {
            switch (turn)
            {
                case 1:
                    foreach (int[][] i in playerTwoShips)
                    {
                        foreach(int[] space in i)

                            if (space.SequenceEqual(guess))
                                return true;
                    }
                    break;
                case 2:
                    foreach (int[][] i in playerOneShips)
                    {
                        foreach (int[] space in i)
                            if (space.SequenceEqual(guess))
                                return true;
                    }
                    break;
                    return false;
            }

            return true;
            
        }*/
        #endregion

        public bool isSunk(int[] guess) { 
            return false;
        }


        #region methods

        //start game
        /*  1. Get player data (name, ships) from game setup form
         *  2. Determine starting player
         *  3. Game loop
         */
        public void startGame(Player one, Player two) {
            //Get player data
            playerOne = one;
            playerTwo = two;

            //determine starting player
            Random rand = new Random();
            setCurrentPlayer(rand.Next(1, 2));       
        }

        //game turn
        /*  1. Current player guesses (on game form)   
         *  2. Determine hit or miss
         *      - if hit, check if sunk
         *          -if sunk, check if the game needs to end
         *  3. Switch current players
         *  4. Call again
         */

        //fire
        public void fire(int[] shot) {
            //create ship object of the ship that is hit by the guessed shot (returns null if miss)
            int[][] hitShip = shipHit(shot);

            //check if hit
            if(shipHit(shot) != null) {
                Console.WriteLine("Hit");

                //add hit to list
                currentPlayer.addHit(shot);

                //check if sunk
                if (isSunk(shot, hitShip)) {
                    //get which ship (carrier, battleship, etc)
                    Console.WriteLine("You sunk a ship");

                    //remove ship from opponents list of ships. 
                    currentOpponent.removeShip(hitShip.Length - 1);
                }

            } else {
                currentPlayer.addMiss(shot);
                Console.WriteLine("miss");
            }

        }

        //check hit
        private int[][] shipHit(int[] shot) {
            //loop through each of the current players unsunk ships
            for (int i = 0; i < currentOpponent.getShips().Count; i++) {

                //create copy of indexed ship 
                int[][] ship = currentOpponent.getShips()[i];

                //loop through each square in ship and see if the matches any of those spots
                for (int j = 0; j < ship.Length; j++) {
                    //get ship square coordinates
                    int[] shipSquare = { ship[j][0], ship[j][1] }; 

                    //compare coordinates
                    if (shot.SequenceEqual(shipSquare)) {
                        return ship;
                    }
                }
            }
            //miss
            return null;
        }
        
        //check sunk
        private bool isSunk(int[] shot, int[][] ship) {
            //check if any square is not hit (index 2 in each square) ship[sqaure][index 0: col, index 1: row, index 2: hit? (0  = no)
            for(int i = 0; i < ship.Length; i++) {
                if (ship[i][2] == 0) return false;
            }

            //if no square is un-hit, return true (the ship is sunk)
            return true;
        }

        #endregion


    }
}

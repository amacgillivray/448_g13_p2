using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECS_448___Project_1
{
    public class Game {
        #region variables 
        private int playerTurn;
        private Player playerOne;
        private Player playerTwo;
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
            setPlayerTurn(rand.Next(1, 2));       
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
                getCurrentPlayer().addHit(shot);

                //check if sunk
                if (isSunk(shot, hitShip)) {
                    //get which ship (carrier, battleship, etc)
                    Console.WriteLine("You sunk a ship");

                    //remove ship from opponents list of ships. 
                    getCurrentOpponent().removeShip(hitShip.Length - 1);
                }

            } else {
                getCurrentPlayer().addMiss(shot);
                Console.WriteLine("Miss");
            }

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
                        return ship;
                    }
                }
            }
            //miss
            return null;
        }

        //Returns the index of the ship that is hit; -1 if no ship is hit
        private int shipHit2(int[] shot)
        {
            //Loops through each of the opponents ships
            for (int i = 0; i < getCurrentOpponent().getShips().Count; i++) {

                //Loops through each position on the ship
                for (int j = 0; j < getCurrentOpponent().getShips()[i].Length; j++) {

                    //Checks if the ship position matches the shot position; returns the index of the ship if it does
                    if (shot.SequenceEqual(getCurrentOpponent().getShips()[i][j])) return i;
                }
            }

            //Returns -1 if the shot does not match any ship positions
            return -1;
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

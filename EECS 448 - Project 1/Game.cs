using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECS_448___Project_1
{
    public class Game {
        //variables
        private int playerTurn;
        private Player currentPlayer;
        private Player currentOpponent;
        private Player playerOne;
        private Player playerTwo;

        //getters/setters
        public int getPlayerTurn() { return playerTurn; }
        public Player getCurrentPlayer() { return currentPlayer; }
        public Player getCurrentOpponent() { return currentOpponent; }
        public Player getPlayerOne() { return playerOne; }
        public Player getPlayerTwo() { return playerTwo; }
        public void setPlayerTurn(int turn) { playerTurn = turn; }
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
            switch (playerTurn)
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
            }

            return false;
            
        }

            return true;
            
        }*/
        #endregion

        public bool isSunk(int[] guess) { 
            return false;
        }
    }
}

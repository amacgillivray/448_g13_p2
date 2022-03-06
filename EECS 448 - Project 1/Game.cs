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

        enum cell_status
        {
            notChecked,
            outOfBounds,
            calledMiss,
            calledHit,
            callable
        };
        struct ai_direction {
            public ai_direction(
                int argx, 
                int argy)
            {
                x = argx;
                y = argy;
            }
            public int x;
            public int y;
        };
        private int ai_tracking_dir = -1;
        private Stack<ai_direction> ai_hits = new Stack<ai_direction>();
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

            ai_direction copy;

            //check if hit
            if (hitShip != null)
            {
                //add hit
                getCurrentPlayer().addHit(shotCopy);

                if (getCurrentPlayer() == playerTwo && ai_level > 0)
                {
                    int x = shotCopy[0];
                    int y = shotCopy[1];

                    if (ai_hits.Count() > 0)
                    {
                        copy = ai_hits.Peek();
                        if (copy.x == x)
                        {
                            if (copy.y == (y-1))
                            {
                                if (ai_tracking_dir == -1)
                                    ai_tracking_dir = 0;
                            } else if (copy.y == (y+1))
                            {
                                if (ai_tracking_dir == -1)
                                    ai_tracking_dir = 1;
                            }
                        } else if (copy.y == y)
                        {
                            if (copy.x == (x - 1))
                            {
                                if (ai_tracking_dir == -1)
                                    ai_tracking_dir = 2;
                            }
                            else if (copy.x == (x + 1))
                            {
                                if (ai_tracking_dir == -1)
                                    ai_tracking_dir = 3;
                            }
                        } 
                        if (ai_tracking_dir == -1)
                        {
                            Console.WriteLine("Did not set tracking direction");
                        }
                    }

                    ai_hits.Push( 
                        new ai_direction(
                            shotCopy[0],
                            shotCopy[1]
                        )
                    );   
                    
                    // todo - update previous top of stack, if there is one.
                }

                //check if sunk
                if (isSunk(shot, hitShip))
                {
                    //show message box of what you or the AI sank
                    //if (ai_level == 0 || playerTurn == 1 )
                    MessageBox.Show(whichShip(hitShip.Length));

                    // crawl hitShip array, check for matches in stack and pop them
                    ai_direction[] cache = new ai_direction[ai_hits.Count()];
                    bool[] cache_keep = new bool[ai_hits.Count()];
                    int[] targetSquare = new int[2];
                    int i = ai_hits.Count() - 1;
                    //for (int i = 0; i < ai_hits.Count(); i++)
                    while (ai_hits.Count() > 0)
                    {
                        cache[i] = ai_hits.Pop();
                        // may be backwards
                        targetSquare[0] = cache[i].x;
                        targetSquare[1] = cache[i].y;
                        if (shipHit(targetSquare) == shipHit(shot))
                            cache_keep[i] = false;
                        else
                            cache_keep[i] = true;
                        i--;
                    }

                    //for (i = cache.Length-1; i >= 0; i--)
                    for (i = 0; i < cache.Length; i++)
                    {
                        if (cache_keep[i])
                            ai_hits.Push(cache[i]);
                    }
                    //ai_tracking_dir = -1;
                }
            }
            else
            {
                if (ai_hits.Count() > 0)
                {
                    int row = shotCopy[1];
                    int col = shotCopy[0];
                      

                    // need to do: 
                    // if (tracking_dir != -1)
                    // pop entries that are in this direction
                    // get back to origin
                    // add the first hit since we started tracking back to the top of the stack
                    // reverse tracking direction

                    copy = ai_hits.Peek();

                    switch (ai_tracking_dir)
                    {
                        case 0:
                            ai_tracking_dir = 1;
                            break;
                        case 1:
                            ai_tracking_dir = 0;
                            break;
                        case 2:
                            ai_tracking_dir = 3;
                            break;
                        case 3:
                            ai_tracking_dir = 2;
                            break;
                    }

                    if (copy.x == row)
                    {
                        if (copy.y == (col - 1))
                        {
                            //ai_tracking_dir = -1;
                        }
                        else if (copy.y == (col + 1))
                        {
                            //ai_tracking_dir = -1;
                        }
                    }
                    else if (copy.y == col)
                    {
                        if (copy.x == (row - 1))
                        {
                            //ai_tracking_dir = -1;
                        }
                        else if (copy.x == (row + 1))
                        {
                            //ai_tracking_dir = -1;
                        }
                    }
                }
                getCurrentPlayer().addMiss(shotCopy);
            }
        }

        private cell_status check_cell(int x, int y)
        {
            int[] targetSquare = new int[2];
            // may be reversed
            targetSquare[0] = x;
            targetSquare[1] = y;

            if (x > 9 || x < 0 || y > 9 || y < 0)
            {
                return cell_status.outOfBounds;
            }
            else
            {
                for (int i = 0; i < getCurrentPlayer().getHits().Count; i++)
                {     //check if targeted square is on a hit
                    if (targetSquare.SequenceEqual(getCurrentPlayer().getHits()[i]))
                    {
                        return cell_status.calledHit;
                    }
                }
                for (int i = 0; i < getCurrentPlayer().getMisses().Count; i++)
                {
                    if (targetSquare.SequenceEqual(getCurrentPlayer().getMisses()[i]))
                    {
                        return cell_status.calledMiss;
                    }
                }

                return cell_status.callable;
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
            int[] targetSquare = new int[2];

            bool targeted = false;

            while (!targeted)
            {
                targeted = true;
                targetSquare[0] = rng();
                targetSquare[1] = rng();

                if (check_cell(targetSquare[0], targetSquare[1]) != cell_status.callable)
                    targeted = false;
            }
            Console.WriteLine("Easy-AI Firing at X: " + targetSquare[0] + "; Y: " + targetSquare[1]);
            fire(targetSquare);
            playerTurn = 1;
        }

        public void hitgen_medium()
        {
            playerTurn = 2;
            if (ai_hits.Count() > 0)
            {
                ai_direction last = ai_hits.Peek();
                int[] targetSquare = new int[2];
                targetSquare[0] = last.x;
                targetSquare[1] = last.y;

                if (ai_tracking_dir != -1)
                {
                    switch (ai_tracking_dir)
                    {
                        // north
                        case 0:
                            targetSquare[1]--;
                            Console.WriteLine("Medium-AI: Tracked north");
                            break;
                        // south
                        case 1:
                            targetSquare[1]++;
                            Console.WriteLine("Medium-AI: Tracked south");
                            break;
                        // east
                        case 2:
                            targetSquare[0]++;
                            Console.WriteLine("Medium-AI: Tracked east");
                            break;
                        // west
                        case 3:
                            targetSquare[0]--;
                            Console.WriteLine("Medium-AI: Tracked west");
                            break;
                    }

                    if (targetSquare[0] < 0 ||
                        targetSquare[0] > 9 ||
                        targetSquare[1] < 0 ||
                        targetSquare[1] > 9)
                    {
                        switch (ai_tracking_dir)
                        {
                            case 0:
                                ai_tracking_dir = 1;
                                break;
                            case 1:
                                ai_tracking_dir = 0;
                                break;
                            case 2:
                                ai_tracking_dir = 3;
                                break;
                            case 3:
                                ai_tracking_dir = 2;
                                break;
                        }

                        Console.WriteLine("Move " + targetSquare[0] + "," + targetSquare[1] + " is out of bounds; skipping.");
                        ai_hits.Pop();
                        hitgen_medium();
                        return;
                    }
                }
                else
                {
                    int dir = rng() % 4;
                    int i = 0;
                    bool chosedir = false;

                    while (i < 4)
                    {
                        if (dir == 0)
                        {
                            if (check_cell(last.x, last.y - 1) == cell_status.callable)
                            {
                                Console.WriteLine("Medium-AI: Chose north.");
                                targetSquare[1]--;
                                chosedir = true;
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Medium-AI: Skipped north.");
                                dir++;
                                i++;
                                continue;
                            }
                        }
                        else if (dir == 1)
                        {
                            if (check_cell(last.x, last.y + 1) == cell_status.callable)
                            {
                                Console.WriteLine("Medium-AI: Chose south.");
                                targetSquare[1]++;
                                chosedir = true;
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Medium-AI: Skipped south.");
                                dir++;
                                i++;
                                continue;
                            }
                        }
                        else if (dir == 2)
                        {
                            if (check_cell(last.x + 1, last.y) == cell_status.callable)
                            {
                                Console.WriteLine("Medium-AI: Chose east.");
                                targetSquare[0]++;
                                chosedir = true;
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Medium-AI: Skipped east.");
                                dir++;
                                i++;
                                continue;
                            }
                        }
                        else if (dir == 3)
                        {
                            if (check_cell(last.x - 1, last.y) == cell_status.callable)
                            {
                                Console.WriteLine("Medium-AI: Chose west.");
                                targetSquare[0]--;
                                chosedir = true;
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Medium-AI: Skipped west.");
                                dir = 0;
                                i++;
                                continue;
                            }
                        }

                        if (chosedir)
                        {
                            if (check_cell(targetSquare[0], targetSquare[1]) != cell_status.callable)
                            {
                                i = 0;
                                chosedir = false;
                            }
                        }
                    }

                    if (!chosedir)
                    {
                        Console.WriteLine("Failed to choose direction in hitgen medium");
                        ai_hits.Pop();
                        hitgen_medium();
                        return;
                    }
                }

                // forcefully prevent from targeting already targeted square
                for (int i = 0; i < getCurrentPlayer().getHits().Count; i++)
                {     //check if targeted square is on a hit
                    if (targetSquare.SequenceEqual(getCurrentPlayer().getHits()[i]))
                    {
                        Console.WriteLine("Move " + targetSquare[0] + "," + targetSquare[1] + "is already called (hit), skipping.");
                        if (ai_tracking_dir != -1)
                        {
                            switch (ai_tracking_dir)
                            {
                                // north
                                case 0:
                                    targetSquare[1]--;
                                    Console.WriteLine("Medium-AI: Tracked north");
                                    break;
                                // south
                                case 1:
                                    targetSquare[1]++;
                                    Console.WriteLine("Medium-AI: Tracked south");
                                    break;
                                // east
                                case 2:
                                    targetSquare[0]++;
                                    Console.WriteLine("Medium-AI: Tracked east");
                                    break;
                                // west
                                case 3:
                                    targetSquare[0]--;
                                    Console.WriteLine("Medium-AI: Tracked west");
                                    break;
                            }
                        }
                        //ai_hits.Pop();
                        //hitgen_medium();
                        //return;
                    }
                }
                for (int i = 0; i < getCurrentPlayer().getMisses().Count; i++)
                {
                    if (targetSquare.SequenceEqual(getCurrentPlayer().getMisses()[i]))
                    {
                        Console.WriteLine("Move " + targetSquare[0] + "," + targetSquare[1] + "is already called (miss), skipping.");
                        ai_hits.Pop();
                        hitgen_medium();
                        return;
                    }
                }

                fire(targetSquare);
                playerTurn = 1;
                Console.WriteLine("Medium-AI firing at X: " + targetSquare[0] + "; Y: " + targetSquare[1]);
            }
            else
            {
                Console.WriteLine("Medium-AI: ai_hits has " + ai_hits.Count() + " entries; reverting to Easy-AI.");
                hitgen_easy();
            }
        }

        public void hitgen_hard()
        {
            playerTurn = 2;
            for (int i = 0; i < getCurrentOpponent().getShips().Count; i++)
            {

                //create copy of indexed ship 
                int[][] ship = getCurrentOpponent().getShips()[i];

                //loop through each square in ship and see if the matches any of those spots
                for (int j = 0; j < ship.Length; j++)
                {
                    //get ship square coordinates
                    int[] shipSquare = { ship[j][0], ship[j][1] };

                    //compare coordinates
                    if (getCurrentOpponent().getShips()[i][j][2] == 0)
                    {
                        int[] shot = new int[2];
                        shot[0] = getCurrentOpponent().getShips()[i][j][0];
                        shot[1] = getCurrentOpponent().getShips()[i][j][1];
                        //set this ship's square to "hit"
                        fire(shot);
                        // return
                        playerTurn = 1;
                        return;
                    }
                }
            }
            playerTurn = 1;
        }



        #endregion
    }
}

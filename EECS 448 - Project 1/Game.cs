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

        enum fire_direction
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
                int argy, 
                fire_direction argn, 
                fire_direction args, 
                fire_direction arge, 
                fire_direction argw)
            {
                x = argx;
                y = argy;
                north = argn;
                south = args;
                east = arge;
                west = argw;
            }
            public int x;
            public int y;
            public fire_direction north;
            public fire_direction south;
            public fire_direction east;
            public fire_direction west;
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
                    int row = shotCopy[0];
                    int col = shotCopy[1];

                    if (ai_hits.Count() > 0)
                    {
                        copy = ai_hits.Pop();
                        if (copy.x == row)
                        {
                            if (copy.y == (col-1))
                            {
                                copy.south = fire_direction.calledHit;
                                if (ai_tracking_dir == -1)
                                    ai_tracking_dir = 0;
                            } else if (copy.y == (col+1))
                            {
                                copy.north = fire_direction.calledHit;
                                if (ai_tracking_dir == -1)
                                    ai_tracking_dir = 1;
                            }
                        } else if (copy.y == col)
                        {
                            if (copy.x == (row - 1))
                            {
                                copy.east = fire_direction.calledHit;
                                if (ai_tracking_dir == -1)
                                    ai_tracking_dir = 2;
                            }
                            else if (copy.x == (row + 1))
                            {
                                copy.west = fire_direction.calledHit;
                                if (ai_tracking_dir == -1)
                                    ai_tracking_dir = 3;
                            }
                        } 
                        if (ai_tracking_dir == -1)
                        {
                            Console.WriteLine("Did not set tracking direction");
                        }
                        ai_hits.Push(copy);
                    }

                    ai_hits.Push( 
                        new ai_direction(
                            shotCopy[0],
                            shotCopy[1],
                            check_direction(row, col - 1),
                            check_direction(row, col + 1),
                            check_direction(row - 1, col),
                            check_direction(row + 1, col)
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

                    copy = ai_hits.Pop();

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
                            copy.south = fire_direction.calledMiss;
                            //ai_tracking_dir = -1;
                        }
                        else if (copy.y == (col + 1))
                        {
                            copy.north = fire_direction.calledMiss;
                            //ai_tracking_dir = -1;
                        }
                    }
                    else if (copy.y == col)
                    {
                        if (copy.x == (row - 1))
                        {
                            copy.east = fire_direction.calledMiss;
                            //ai_tracking_dir = -1;
                        }
                        else if (copy.x == (row + 1))
                        {
                            copy.west = fire_direction.calledMiss;
                            //ai_tracking_dir = -1;
                        }
                    }

                    ai_hits.Push(copy);
                }
                getCurrentPlayer().addMiss(shotCopy);
            }
        }

        private fire_direction check_direction(int x, int y)
        {
            int[] targetSquare = new int[2];
            // may be reversed
            targetSquare[0] = y;
            targetSquare[1] = x;

            if (x > 9 || x < 0 || y > 9 || y < 0)
            {
                return fire_direction.outOfBounds;
            }
            else
            {
                for (int i = 0; i < getCurrentPlayer().getHits().Count; i++)
                {     //check if targeted square is on a hit
                    if (targetSquare.SequenceEqual(getCurrentPlayer().getHits()[i]))
                    {
                        return fire_direction.calledHit;
                    }
                }
                for (int i = 0; i < getCurrentPlayer().getMisses().Count; i++)
                {
                    if (targetSquare.SequenceEqual(getCurrentPlayer().getMisses()[i]))
                    {
                        return fire_direction.calledMiss;
                    }
                }

                return fire_direction.callable;
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
            Console.WriteLine("Easy-AI Firing at X: " + col + "; Y: " + row);
            fire(targetSquare);
            playerTurn = 1;
        }

        public void hitgen_medium()
        {
            playerTurn = 2;
            if (ai_hits.Count() > 0)
            {
                int[] targetSquare = new int[2];

                int x;
                int y;
                ai_direction last = ai_hits.Peek();
                x = last.x;
                y = last.y;

                if (ai_tracking_dir != -1)
                {
                    switch (ai_tracking_dir)
                    {
                        // north
                        case 0:
                            y--;
                            Console.WriteLine("Medium-AI: Tracked north");
                            break;
                        // south
                        case 1:
                            y++;
                            Console.WriteLine("Medium-AI: Tracked south");
                            break;
                        // east
                        case 2:
                            x++;
                            Console.WriteLine("Medium-AI: Tracked east");
                            break;
                        // west
                        case 3:
                            x--;
                            Console.WriteLine("Medium-AI: Tracked west");
                            break;
                    }

                    if (x < 0 || x > 9 || y < 0 || y > 9)
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

                        Console.WriteLine("Move " + x + "," + y + " is out of bounds; skipping.");
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
                            if (check_direction(last.x, last.y-1)==fire_direction.callable)
                            {
                                Console.WriteLine("Medium-AI: Chose north.");
                                y--;
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
                            if (check_direction(last.x, last.y+1)==fire_direction.callable)
                            {
                                Console.WriteLine("Medium-AI: Chose south.");
                                y++;
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
                            if (check_direction(last.x+1, last.y)==fire_direction.callable)
                            {
                                Console.WriteLine("Medium-AI: Chose east.");
                                x++;
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
                            if (check_direction(last.x-1, last.y)==fire_direction.callable)
                            {
                                Console.WriteLine("Medium-AI: Chose west.");
                                x--;
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
                    }

                    if (!chosedir)
                    {
                        Console.WriteLine("Failed to choose direction in hitgen medium");
                        ai_hits.Pop();
                        hitgen_medium();
                        return;
                    }
                }

                targetSquare[0] = x;
                targetSquare[1] = y;

                // forcefully prevent from targeting already targeted square
                for(int i = 0; i < getCurrentPlayer().getHits().Count; i++)
                {     //check if targeted square is on a hit
                    if (targetSquare.SequenceEqual(getCurrentPlayer().getHits()[i]))
                    {
                        Console.WriteLine("Move " + x + "," + y + "is already called (hit), skipping.");
                        ai_hits.Pop();
                        hitgen_medium();
                        return;
                    }
                }
                for (int i = 0; i < getCurrentPlayer().getMisses().Count; i++)
                {
                    if (targetSquare.SequenceEqual(getCurrentPlayer().getMisses()[i]))
                    {
                        Console.WriteLine("Move " + x + "," + y + "is already called (miss), skipping.");
                        ai_hits.Pop();
                        hitgen_medium();
                        return;
                    }
                }

                fire(targetSquare);
                playerTurn = 1;
                Console.WriteLine("Medium-AI firing at X: " + x + "; Y: " + y);
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

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
        struct ai_hit {
            public ai_hit(
                int argx, 
                int argy)
            {
                x = argx;
                y = argy;
            }
            public ai_hit(
                ai_hit arg
            )
            {
                x = arg.x;
                y = arg.y;
            }
            public int x;
            public int y;
        };
        private int ai_tracking_dir = -1;
        private int ai_tracked_dist = 0;
        private int ai_reverse_ct = 0;
        private int dbg_ct = 1;
        private Stack<ai_hit> ai_hits = new Stack<ai_hit>();
        private Stack<ai_hit> ai_dead_hits = new Stack<ai_hit>();
        private bool tried_find_dir_twice = false;
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
            if (hitShip != null || check_cell(shotCopy[0], shotCopy[1]) == cell_status.calledHit)
            {

                //add hit
                getCurrentPlayer().addHit(shotCopy);

                if (getCurrentPlayer() == playerTwo && ai_level > 0)
                {
                    if (ai_tracking_dir != -1)
                    {
                        if (ai_tracked_dist == 0)
                            ai_tracked_dist = 2;
                        else
                            ai_tracked_dist++;
                    }

                    if (ai_hits.Count()+1 > 1 && ai_tracking_dir == -1)
                        set_ai_tracking_dir(shotCopy[0], shotCopy[1]);

                    ai_hits.Push(
                        new ai_hit(
                            shotCopy[0],
                            shotCopy[1]
                        )
                    );

                    Console.WriteLine("Size of AI_HITS: " + ai_hits.Count());
                }

                //check if sunk
                if (isSunk(shot, hitShip))
                {
                    //show message box of what you or the AI sank
                    MessageBox.Show(whichShip(hitShip.Length));

                    // increment towards win
                    getCurrentPlayer().addSunk();
                    
                    // console log
                    Console.WriteLine("Sank ship of size " + hitShip.Length);
                    
                    // update ai hits for medium ai.
                    ai_sank_ship(hitShip, shot);
                }

                return;
            }
            else
            {
                getCurrentPlayer().addMiss(shotCopy);
                if (ai_hits.Count() > 0 && check_cell(shotCopy[0], shotCopy[1]) == cell_status.calledMiss && playerTurn == 2)
                {
                    Console.WriteLine("Current Player: " + playerTurn);
                    // need to do
                    // if (tracking_dir != -1)
                    // pop entries that are in this direction
                    // get back to origin
                    // add the first hit since we started tracking back to the top of the stack
                    // reverse tracking direction

                    if (ai_tracking_dir != -1)
                    {
                        Console.WriteLine("AI was tracking in Dir " + ai_tracking_dir + " and encountered Miss.");
                        Console.WriteLine("ai_tracked_dist: " + ai_tracked_dist);
                        Console.WriteLine("ai_hits count: " + ai_hits.Count());
                        reset_tracked_ai_hits();
                        reverse_ai_tracking_dir();
                        Console.WriteLine("ai_hits count after removing hits from stack: " + ai_hits.Count());
                        Console.WriteLine("AI is now tracking in Dir " + ai_tracking_dir);
                    }
                }
            }
        }

        private cell_status check_cell(int x, int y)
        {
            int[] targetSquare = new int[2];
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
            }
            return cell_status.callable;
        }

        private void ai_sank_ship( int[][] hitShip, int[] shot )
        {
            // crawl hitShip array, check for matches in stack and pop them
            ai_hit[] cache = new ai_hit[ai_hits.Count()];
            bool[] cache_keep = new bool[ai_hits.Count()];
            int[] targetSquare = new int[2];
            int i = ai_hits.Count() - 1;
            //for (int i = 0; i < ai_hits.Count(); i++)

            Console.WriteLine("ai_hits count before popping stack: " + ai_hits.Count());
            while (ai_hits.Count() > 0)
            {
                cache[i] = ai_hits.Pop();
                targetSquare[0] = cache[i].x;
                targetSquare[1] = cache[i].y;
                if (shipHit(targetSquare) == shipHit(shot))
                    cache_keep[i] = false;
                else
                    cache_keep[i] = true;
                i--;
            }
            Console.WriteLine("ai_hits count after popping stack: " + ai_hits.Count());

            //for (i = cache.Length-1; i >= 0; i--)
            for (i = 0; i < cache.Length; i++)
            {
                if (cache_keep[i])
                    ai_hits.Push(cache[i]);
            }
            Console.WriteLine("ai_hits count after restoring stack: " + ai_hits.Count());
            ai_tracking_dir = -1;
        }

        // Determines whether or not the hit coordinate can still yield any valid orthogonal moves
        private bool hit_has_valid_moves(ai_hit hit)
        {
            // check north
            if (check_cell(hit.x, hit.y - 1) == cell_status.callable)
                return true;

            // check south
            if (check_cell(hit.x, hit.y + 1) == cell_status.callable)
                return true;

            // check east
            if (check_cell(hit.x + 1, hit.y) == cell_status.callable)
                return true;

            // check west
            if (check_cell(hit.x - 1, hit.y) == cell_status.callable)
                return true;

            return false;
        }

        private void set_ai_tracking_dir( int x, int y )
        {   

            int i = 0;
            int sz = ai_hits.Count() + ai_dead_hits.Count();
            int restore = ai_hits.Count();

            if (sz == 0)
            {
                ai_tracking_dir = -1;
                return;
            }
            
            ai_hit copy;
            ai_hit[] hits = new ai_hit[sz];

            // Get all hit cells both dead and live into hits array
            //while (i < sz)
            //{
                while (ai_hits.Count() > 0)
                {
                    hits[i] = ai_hits.Pop();
                    i++;
                }
                while (ai_dead_hits.Count() > 0)
                {
                    hits[i] = ai_dead_hits.Pop();
                    i++;
                }
            //}

            // Check hits for anything adjacent.
            // the first (most recent) hit in an adjacent square will be used 
            // to determine the direction.
            for (i = 0; i < sz; i++)
            {
                copy = hits[i];

                if (i < restore)
                    ai_hits.Push(hits[i]);
                else
                    ai_dead_hits.Push(hits[i]);
                
                if (copy.x == x)
                {
                    if (copy.y == (y - 1))
                    {
                        Console.WriteLine("AI will now track South (1).");
                        ai_tracking_dir = 1;
                        break;
                    }
                    else if (copy.y == (y + 1))
                    {
                        Console.WriteLine("AI will now track North (0).");
                        ai_tracking_dir = 0;
                        break;
                    }
                }
                else if (copy.y == y)
                {
                    if (copy.x == (x - 1))
                    {
                        Console.WriteLine("AI will now track East (2).");
                        ai_tracking_dir = 2;
                        break;
                    }
                    else if (copy.x == (x + 1))
                    {
                        Console.WriteLine("AI will now track West (3).");
                        ai_tracking_dir = 3;
                        break;
                    }
                }
            }

            if (i < sz)
            {
                for (; i < sz; i++)
                {
                    if (i < restore)
                        ai_hits.Push(hits[i]);
                    else
                        ai_dead_hits.Push(hits[i]);
                }
            }
            if (ai_tracking_dir == -1)
                Console.WriteLine("Could not find tracking dir.");
        }

        private void reverse_ai_tracking_dir()
        {
            if (ai_reverse_ct == 0 && ai_tracking_dir != -1)
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
                ai_reverse_ct++;
            }
            else
            {
                ai_reverse_ct = 0;
                ai_tracking_dir = -1;
                ai_tracked_dist = 0;
            }
        }

        private void reset_tracked_ai_hits()
        {
            //Console.WriteLine("Resetting tracked AI hits (" + ai_tracked_dist + ")");

            //// Revert to origin
            //int restore_len = ai_tracked_dist;
            //int i = 0;
            //ai_hit origin;
            //ai_hit[] cache = new ai_hit[ai_hits.Count()];
            //while (ai_tracked_dist > 0 && ai_hits.Count() > 1)
            //{
            //    //if (hit_has_valid_moves(ai_hits.Peek()))
            //    cache[i] = ai_hits.Pop();
            //    //else
            //    //    ai_dead_hits.Push(ai_hits.Pop());
            //    ai_tracked_dist--;
            //    i++;
            //}
            ////origin = ai_hits.Pop();
            //Console.WriteLine("After popping tracked AI hits: Count = " + ai_hits.Count());

            //// i = how many elements we added to the cache
            //// restore_len = how many elements we expected
            //while (i >= 0)
            //{
            //    ai_hits.Push(cache[i]);
            //    //restore_len--;
            //    i--;
            //}
            ////ai_hits.Push(origin);
            ///

            ai_tracking_dir = -1;

            // Starting with origin, remove hits until there is a valid move
            while (!hit_has_valid_moves(ai_hits.Peek()))
            {
                ai_dead_hits.Push(ai_hits.Pop());
            }
        }

        private void prune_ai_hits()
        {
            Console.WriteLine("Pruning AI hits (" + ai_hits.Count() + ")");
            ai_hit[] cache = new ai_hit[ai_hits.Count()];
            
            int i = 0;
            while (ai_hits.Count() > 0)
            {
                if (hit_has_valid_moves(ai_hits.Peek()))
                {
                    cache[i] = ai_hits.Pop();
                    i++;
                }
                else
                    ai_dead_hits.Push(ai_hits.Pop());
            }

            while (i >= 0)
            {
                ai_hits.Push(cache[i]);
                i--;
            }
        }


        private void medium_trackedshot()
        {
            Console.WriteLine("TRACKED SHOT");
            ai_hit last = ai_hits.Peek();
            int[] targetSquare = new int[2];
            targetSquare[0] = last.x;
            targetSquare[1] = last.y;

            bool can_fire = false;

            while (!can_fire)
            {
                switch (ai_tracking_dir)
                {
                    // north: y-1
                    case 0:
                        targetSquare[1]--;
                        Console.WriteLine("Medium-AI: Tracked north");
                        break;
                    // south: y+1 
                    case 1:
                        targetSquare[1]++;
                        Console.WriteLine("Medium-AI: Tracked south");
                        break;
                    // east: x+1
                    case 2:
                        targetSquare[0]++;
                        Console.WriteLine("Medium-AI: Tracked east");
                        break;
                    // west: x-1
                    case 3:
                        targetSquare[0]--;
                        Console.WriteLine("Medium-AI: Tracked west");
                        break;
                }

                if (check_cell(targetSquare[0], targetSquare[1]) != cell_status.callable)
                {
                    Console.WriteLine("Cell " + targetSquare[0] + "," + targetSquare[1] + " is not callable. Reversing tracking direction.");

                    reset_tracked_ai_hits();
                    reverse_ai_tracking_dir();
                    hitgen_medium();

                    return;
                }
                else
                {
                    can_fire = true;
                }
            }

            Console.WriteLine("Medium-AI firing at X: " + targetSquare[0] + "; Y: " + targetSquare[1]);
            fire(targetSquare);
            return;
        }

        private void medium_untrackedshot()
        {
            Console.WriteLine("ORTHOGONAL SHOT");
            ai_hit last = ai_hits.Peek();
            int[] targetSquare = new int[2];
            targetSquare[0] = last.x;
            targetSquare[1] = last.y;

            int dir = 1; //rng() % 4;
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
                prune_ai_hits();
                hitgen_medium();
                return;
            }

            Console.WriteLine("Medium-AI firing at X: " + targetSquare[0] + "; Y: " + targetSquare[1]);
            fire(targetSquare);
            return;
        }

        

        public void hitgen_medium()
        {
            
            Console.WriteLine();
            Console.WriteLine("AI TURN");

            playerTurn = 2;
            if (ai_hits.Count() > 0)
            {
                ai_hit last = ai_hits.Peek();
                Console.WriteLine("Medium-AI: Working with hit " + last.x + ", " + last.y + " and " + ai_hits.Count() + " live hits.");
                Console.WriteLine("Medium-AI: Tracking Dir  " + ai_tracking_dir + " with dist " + ai_tracked_dist);

                if (ai_tracking_dir != -1)
                    medium_trackedshot();
                else
                    medium_untrackedshot();

                playerTurn = 1;
                return;
            }
            else
            {
                Console.WriteLine("Medium-AI: ai_hits has " + ai_hits.Count() + " entries; reverting to Easy-AI.");
                
                if (dbg_ct == -1)
                {
                    hitgen_easy();
                } else //if (dbg_ct == 0)
                {
                    //int[] shot = new int[2];
                    //shot[0] = 0;
                    //shot[1] = 4;
                    ////ai_tracked_dist++;
                    //fire(shot);
                    hitgen_hard();
                    dbg_ct--;
                }
            }
            playerTurn = 1;
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

        private string whichShip(int squares)
        {
            string ship = "";
            switch (squares)
            {
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



        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECS_448___Project_1 {
    public class Player {
		#region variables
		private string name;
        private List<int[]> hits; //list of 2 element int arrays, representing coordinates of hits this player guessed
        private List<int[]> misses; //list of 2 element in arrays, representing coordinates of misses this player guessed
        private List<int[][]> ships; //list of ships placed by the player. each ship is an array of 3-element int arrays with the form { horizontal pos, vertical pos, is hit (0 false, 1 if true) }
        int sunkships;
        #endregion

        //Constructors
        public Player() {
            name = "";
            hits = new List<int[]>();
            misses = new List<int[]>();
            ships = new List<int[][]>();
            sunkships = 0;
        }

        public Player(string _name) {
            name = _name;
            hits = new List<int[]>();
            misses = new List<int[]>();
            ships = new List<int[][]>();
            sunkships = 0;
        }

        #region name methods
        //setname
        public void setName(string _name) {
            name = _name;
        }
        //getname 
        public string getName() {
            return name;
        }
        #endregion



        #region ship methods

        //add ship
        public void addShip(int[][] ship) {
            ships.Add(ship);
        }

        //remove ships
        public void removeShip(int index) {
            ships.RemoveAt(index);
        }

        //get all ships
        public List<int[][]> getShips() {
            return ships;
        }

        //Increment number of ships sunk by this player
        public void addSunk()
        {
            sunkships++;
        }

        //get number of ships sunk by this player
        public int getSunk()
        {
            return sunkships;
        }
        #endregion

        #region hit & miss methods
        //addHit
        public void addHit(int[] hit) {
            if (hit.Length == 2) hits.Add(hit);
            else throw new Exception("Passed in hit has invalid coordinates!");
        }

        //get hits
        public List<int[]> getHits() {
            return hits;
        }

        //add miss
        public void addMiss(int[] miss) {
            if (miss.Length == 2) misses.Add(miss);
            else throw new Exception("Passed in miss has invalid coordinates");
        }

        //get misses
        public List<int[]> getMisses() {
            return misses;
        }
        #endregion
    }
}

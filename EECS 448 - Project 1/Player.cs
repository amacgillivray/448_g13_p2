using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECS_448___Project_1 {
    public class Player {
        //variables
        private string name;
        private List<int[]> hits; //list of 2 element int arrays, representing coordinates of hits this player guess
        private List<int[]> misses; //list of 2 element in arrays, representing coordinates of misses this player guessed
        private List<int[][]> ships; //list of players ship coordinates. ships are listed as 2 dimensional array of 2 element arrays representing coordinates [list index][ship square][square coordinates]

        //Constructors
        public Player() {
            name = "";
            hits = new List<int[]>();
            misses = new List<int[]>();
            ships = new List<int[][]>();
        }

        public Player(string _name) {
            name = _name;
            hits = new List<int[]>();
            misses = new List<int[]>();
            ships = new List<int[][]>();
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
            try {
                ships.Add(ship);
            } 
            catch {
                throw new Exception("Ship has invalid dimensions!");
            }
        }

        //remove ships
        public void removeShip(int index) {

        }

        //get all ships
        public List<int[][]> getShips() {
            return ships;
        }

        #endregion



        #region hit & miss methods

        //addHit
        public void addHit(int[] hit) {
            if (hit.Length == 2) {
                hits.Add(hit);
            } else {
                throw new Exception("Passed in hit has invalid coordinates!");
            }
        }

        //get hits
        public List<int[]> getHits() {
            return hits;
        }

        //add miss
        public void addMiss(int[] miss) {
            if (miss.Length == 2) {
                misses.Add(miss);
            } else {
                throw new Exception("Passed in miss has invalid coordinates");
            }
        }

        //get misses
        public List<int[]> getMisses() {
            return misses;
        }
        #endregion
    }
}

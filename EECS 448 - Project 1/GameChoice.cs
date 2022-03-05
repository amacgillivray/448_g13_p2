using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EECS_448___Project_1 { 


    public partial class GameChoice : Form
    {
		#region variables
		public bool player_1 = true;
        public List<int[][]> player_1_location = new List<int[][]>();
        public List<int[][]> player_2_location = new List<int[][]>();
        Point lastLegalPosition = new Point();
        Game game = new Game();

        List<Ship> ships = new List<Ship>();

        bool isMouseDown = false; //is the mouse button down on the picturebox
        bool isKeyDown = false;
		#endregion

        //constructor
		public GameChoice(Game game,int shipNum) {
            this.KeyPreview = true;
            InitializeComponent();
            this.game = game;
            //update label
            boardLabel.Text = game.getPlayerOne().getName();

            // Creates the corresonding number of ships based on the button clicked on the previous form
            for(int i = 1; i <= shipNum; i++) 
            {
                addShips(i);
            }

            //snap ships
            foreach(Ship ship in ships) ship.snap();
        }

		#region ship class
		private class Ship
        {
            public Rectangle rectangle; //rectangle used in drawing, will be passed in methods to picturebox
            public bool selected;       //is the ship selected
            public bool isRotated;      //true when ship is verticle
            public int  numSquares;     //number of squares this ship occupies

            public Ship(int length)
            {
                // rectangle = new Rectangle(x + Formatting.offset, y - Formatting.offset, length * Formatting.squareSize - 2 * Formatting.offset, Formatting.squareSize - 2 * Formatting.offset);
                numSquares = length;
                rectangle = new Rectangle();
                rectangle.X = Formatting.offset;
                rectangle.Y = Formatting.squareSize * length + Formatting.offset;
                rectangle.Width = Formatting.squareSize * length - 2 * Formatting.offset;
                rectangle.Height = Formatting.shipWidth;
                selected = false;
                isRotated = false;
            }

            public void rotate()
            {
                int oldWidth = rectangle.Width;
                rectangle.Width = rectangle.Height;
                rectangle.Height = oldWidth;
                if(isRotated) isRotated = false;
                else isRotated = true;
            }

            //snap
            public void snap() {
                //get approximate nearest row and col
                double approximateCol = (double)rectangle.Location.X / (double)Formatting.squareSize;
                double approximateRow = (double)rectangle.Location.Y / (double)Formatting.squareSize;

                //set x to nearest col
                double distToColRoundDown = Math.Abs((int)Math.Floor(approximateCol) * Formatting.squareSize - rectangle.X);
                double distToColRoundUp = Math.Abs((int)Math.Ceiling(approximateCol) * Formatting.squareSize - rectangle.X);
                if(distToColRoundUp <= distToColRoundDown) rectangle.X = (int)Math.Ceiling(approximateCol) * Formatting.squareSize + Formatting.offset + 1;
                else rectangle.X = (int)Math.Floor(approximateCol) * Formatting.squareSize + Formatting.offset + 1;

                //set y to nearest row
                double distToRowRoundDown = Math.Abs((int)Math.Floor(approximateRow) * Formatting.squareSize - rectangle.Y);
                double distToRowRoundUp = Math.Abs((int)Math.Ceiling(approximateRow) * Formatting.squareSize - rectangle.Y);
                if (distToRowRoundUp <= distToRowRoundDown) rectangle.Y = (int)Math.Ceiling(approximateRow) * Formatting.squareSize + Formatting.offset + 1;
                else rectangle.Y = (int)Math.Floor(approximateRow) * Formatting.squareSize + Formatting.offset + 1;
            }

            //check overlap
            public bool checkOverlap(Ship ship) {
                //is compared ship this?
                if(ship != this) {
                    bool xOverlap = false;
                    bool yOverlap = false;

                    //check if overlap on x region
                    if(rectangle.X < ship.rectangle.X + ship.rectangle.Width && rectangle.X + rectangle.Width > ship.rectangle.X) xOverlap = true;
				
                    //check overlap on y region
                    if(rectangle.Y < ship.rectangle.Y + ship.rectangle.Height && rectangle.Y + rectangle.Height> ship.rectangle.Y) yOverlap = true;

                    //if there is x and y overlap, then the two ships overlap
                    if(xOverlap && yOverlap) return true;
                }

                return false;
			}

            //check in bounds
            public bool checkInBounds() {
                bool inXBounds = false;
                bool inYBounds = false;

                //check x bounds
                if(rectangle.X >= 0 && rectangle.X + rectangle.Width < Formatting.squareSize * 10) inXBounds = true;

                //check y bounds
                if(rectangle.Y >= 0 && rectangle.Y + rectangle.Height < Formatting.squareSize * 10) inYBounds = true;

                //check in bounds
                if(inXBounds && inYBounds) return true;
                else return false;
			}

            //get coordinates from ship
            public int[][] getGameCoordinates() {
                int[][] coords = new int[numSquares][];
                int col = 0;
                int row = 0;



                //go through each square and record its coordinates
                for(int i = 0; i < numSquares; i++) {
                    //is this ship rotated
                    if(isRotated) {
                        col = (rectangle.X) / Formatting.squareSize;
                        row = (rectangle.Location.Y + Formatting.squareSize * i) / Formatting.squareSize;
                    } else {
                        col = (rectangle.X + Formatting.squareSize * i) / Formatting.squareSize;
                        row = (rectangle.Y) / Formatting.squareSize;
                    }

                    coords[i] = new int[] { col, row, 0 };
				}


                return coords;
            }

        }

        #endregion
        
        //save coords
        private int[][] saveGameCoords(int[][] coords) {
            int[][] copyCoords = new int[coords.Length][];
            Array.Copy(coords, copyCoords, coords.Length);

            return copyCoords;
        }
		

        //addRects
        private void addShips(int num)
        {
            Ship ship = new Ship(num);
            ships.Add(ship);
        }

        //get selected ship
        private Ship getSelectedShip()
        {
            foreach (Ship ship in ships)
            {
                if (ship.selected) return ship;
            }

            return null;
        }



        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            //pen
            Pen pen = new Pen(Color.White, 1);
            Point start = new Point();
            Point end = new Point();

            //draw horizontal lines
            for (int j = 0; j < 11; j++)
            {
                start.X = 0;
                start.Y = j * pictureBox.Height / 10;
                end.X = pictureBox.Width;
                end.Y = start.Y;
                e.Graphics.DrawLine(pen, start, end);
            }
            //draw bottom border
            start.Y = pictureBox.Height - (int)Math.Ceiling(pen.Width);
            end.Y = start.Y;
            e.Graphics.DrawLine(pen, start, end);

            //draw verticle lines
            for (int i = 0; i < 11; i++)
            {
                //draw verticle lines
                start.X = i * pictureBox.Width / 10;
                start.Y = 0;
                end.X = start.X;
                end.Y = pictureBox.Height;
                e.Graphics.DrawLine(pen, start, end);

            }
            //draw right side border
            start.X = pictureBox.Width - (int)Math.Ceiling(pen.Width);
            end.X = start.X;
            e.Graphics.DrawLine(pen, start, end);

            //brush
            SolidBrush brush = new SolidBrush(Color.LightGray);
            //draw ships
            foreach (Ship ship in ships)
            {
                //if ship is selected highlight
                if (ship.selected)
                {
                    brush.Color = Color.Yellow;
                }
                else
                {
                    brush.Color = Color.LightGray;
                }


                e.Graphics.FillRectangle(brush, ship.rectangle);


            }


        }
        
        //reset button click
        private void button1_Click(object sender, EventArgs e)
        {
            //reset ship positions
            foreach (Ship ship in ships)
            {
                ship.rectangle.X = Formatting.offset;
                ship.rectangle.Y = Formatting.squareSize * ship.numSquares + Formatting.offset;
                ship.rectangle.Width = Formatting.squareSize * ship.numSquares - 2 * Formatting.offset;
                ship.rectangle.Height = Formatting.shipWidth;
            }
            pictureBox.Refresh();
        }

        //next player / play game button press
        private void button2_Click(object sender, EventArgs e)  
        {
            if (player_1) {
                //save to player one
                foreach(Ship ship in ships) {
                    game.getPlayerOne().addShip(saveGameCoords(ship.getGameCoordinates()));
                }

                //it is now player twos turn to place ships,
                // or time to start the game if playing vs ai
                if (game.ai_level == 0)
                {
                    player_1 = false;
                    //update button text
                    button2.Text = "Play Game";
                    //reset ships
                    button1_Click(sender, e);

                    //update label
                    boardLabel.Text = game.getPlayerTwo().getName();
                } else
                {
                    button1_Click(sender, e);
                    //AI places ships
                    // Random rnd = new Random();
                    bool overlapping = true;
                    bool outofbounds = false;

                    foreach (Ship ship2 in ships)
                    {
                        Random rnd = new Random();
                        overlapping = true;
                        outofbounds = true;
                        //ship2.selected = true;
                        do
                        {
                            overlapping = false;
                            outofbounds = false;
                            //generate random coordinates
                            ship2.rectangle.X = rnd.Next(300);
                            ship2.rectangle.Y = rnd.Next(300);
                            ship2.snap();

                            foreach (Ship ship in ships)
                                if (ship2.checkOverlap(ship)) overlapping = true;

                            //check in bounds
                            outofbounds = !ship2.checkInBounds() 
                                || (ship2.rectangle.Location.X + ship2.rectangle.Width)>300 
                                || (ship2.rectangle.Location.X + ship2.rectangle.Width)<0
                                || (ship2.rectangle.Location.Y + ship2.rectangle.Height)>300
                                || (ship2.rectangle.Location.Y + ship2.rectangle.Height)<0;

                        } while (outofbounds || overlapping);
                        //ship2.snap();
                        //game.getPlayerTwo().addShip(saveGameCoords(ship2.getGameCoordinates()));
                        //ship2.selected = false;
                    }

                    foreach (Ship ship in ships)
                    {
                        game.getPlayerTwo().addShip(saveGameCoords(ship.getGameCoordinates()));
                    }

                    GameForm gameForm = new GameForm(ref game);
                    gameForm.Show(); //show the game form
                    this.Close();    //close this form
                }
            } else {
                //if player 2 is setting up their board
                foreach(Ship ship in ships) {
                    game.getPlayerTwo().addShip(saveGameCoords(ship.getGameCoordinates()));
                }
                GameForm gameForm = new GameForm(ref game);
                gameForm.Show(); //show the game form
                this.Close();

            }
        }

        //key down event
        private void GameChoice_KeyDown(object sender, KeyEventArgs e)
        {
            //check if key is down
            if (!isKeyDown)
            {
                //set key to down
                isKeyDown = true;

                //rotate
                if (e.KeyCode == Keys.R)
                {
                    //rotate
                    Ship selectedShip = getSelectedShip();

                    if (selectedShip != null) selectedShip.rotate();

                    //refresh
                    pictureBox.Refresh();
                }


            }
            else
            {
                //set key up
                isKeyDown = false;
            }
        }

        //key up event
        private void GameChoice_KeyUp(object sender, KeyEventArgs e)
        {
            isKeyDown = false;
            pictureBox.Refresh();
        }

        //mouse down event
        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            Point mouse = new Point(e.Location.X, e.Location.Y);

            //check if mouse is already down
            if (!isMouseDown)
            {
                //set mouse down tracker
                isMouseDown = true;

                //check if any ship is selected
                foreach (Ship ship in ships)
                {
                    Console.WriteLine(" mouse: " + mouse.X + " " + mouse.Y + " || ship " + ship.rectangle.Location.X + " " + ship.rectangle.Location.Y);
                    Console.WriteLine("Range X: " + ship.rectangle.Location.X + " - " + (ship.rectangle.Location.X + ship.rectangle.Width));
                    Console.WriteLine("Range Y: " + ship.rectangle.Location.Y + " - " + (ship.rectangle.Location.Y + ship.rectangle.Height));


                    //see if mouse is in ships rectangle
                    if (mouse.X <= ship.rectangle.Location.X + ship.rectangle.Width && mouse.X >= ship.rectangle.Location.X &&
                       mouse.Y <= ship.rectangle.Location.Y + ship.rectangle.Height && mouse.Y >= ship.rectangle.Location.Y)
                    {
                        ship.selected = true;
                        lastLegalPosition = ship.rectangle.Location; //store location
                    }
                    else
                    {
                        ship.selected = false;
                    }
                }
            }

            pictureBox.Refresh();
        }

        //mouse up event
        private void pictureBox_MouseUp(object sender, MouseEventArgs e) {
            bool overlap = false;
            bool outOfBounds = true;

            //if a ship is selected do this
            if(getSelectedShip() != null) {
                //snap
                getSelectedShip().snap();

                //check overlap
                foreach(Ship ship in ships) {
                    if(getSelectedShip().checkOverlap(ship)) overlap = true;
                }

                //check in bounds
                outOfBounds = !getSelectedShip().checkInBounds();

                //if overlapped or out of bounds return to legal space
                if(overlap || outOfBounds) getSelectedShip().rectangle.Location = lastLegalPosition;

                //deselect all ships
                foreach(Ship ship in ships) ship.selected = false;
            }
            
            //set mouse down tracker
            isMouseDown = false;

            //refresh the picturebox
            pictureBox.Refresh();
        }

        //mouse move event
        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            //check if mouse is down
            if (isMouseDown)
            {
                foreach (Ship ship in ships)
                {
                    if (ship.selected)
                    {
                        ship.rectangle.X = e.Location.X - ship.rectangle.Width / 2; //move ship
                        ship.rectangle.Y = e.Location.Y - ship.rectangle.Height / 2;
                    }
                }

                //refresh picturebox
                pictureBox.Refresh();
            }
        }

        //back button
		private void backButton_Click(object sender, EventArgs e) {
            //create new game
            Game backGame = new Game();                     //creates new game object
            //set names
            backGame.getPlayerOne().setName(game.getPlayerOne().getName());
            backGame.getPlayerTwo().setName(game.getPlayerTwo().getName());

            //set old game to null
            game = null;

            //new setup form
            SetupPage setup = new SetupPage(backGame);      // passes the game to the next form
            setup.Show();                               // shows the next form
            this.Close();                               // closes this form
        }
	}

	//formating class
	public static class Formatting {
        public const int squareSize = 30;
        public const int offset = (int)(squareSize * 0.1);
        public const int shipWidth = (int)(squareSize * 0.8);
    }
}

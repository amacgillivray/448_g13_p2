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
        public bool player_1 = true;
        public List<int[][]> player_1_location = new List<int[][]>();
        public List<int[][]> player_2_location = new List<int[][]>();
        Point lastLegalPosition = new Point();

        public GameChoice() {
            this.KeyPreview = true;
            InitializeComponent();
            //addShips(1);
            addShips(2);
            addShips(3);
            //addShips(4);
            //addShips(5);

            //snap ships
            foreach(Ship ship in ships) ship.snap();
        }

        private class Ship
        {
            public Rectangle rectangle; //rectangle used in drawing, will be passed in methods to picturebox
            public bool selected;       //is the ship selected
            public bool isRotated;      //true when ship is verticle

            public Ship(int length)
            {
                // rectangle = new Rectangle(x + Formatting.offset, y - Formatting.offset, length * Formatting.squareSize - 2 * Formatting.offset, Formatting.squareSize - 2 * Formatting.offset);
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
                Console.WriteLine("rotate called!");
                int oldWidth = rectangle.Width;
                rectangle.Width = rectangle.Height;
                rectangle.Height = oldWidth;
                Console.WriteLine("Width: " + rectangle.Width + "Height: " + rectangle.Height);
            }

            //snap
            public void snap() {
                //get approximate nearest row and col
                double approximateCol = (double)rectangle.Location.X / (double)Formatting.squareSize;
                double approximateRow = rectangle.Location.Y / Formatting.squareSize;

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

                return false;
			}
        }

        List<Ship> ships = new List<Ship>();

        bool isMouseDown = false; //is the mouse button down on the picturebox
        bool isKeyDown = false;

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
        

        private void button1_Click(object sender, EventArgs e)
        {
            //reset
            foreach (Ship ship in ships)
            {
                
                    ship.rectangle.X = 0; 
                    ship.rectangle.Y = ship.rectangle.Width;
              
            }
            pictureBox.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)  
        {
            //next player
            player_1 = false;
            button2.Hide();
            button3.Show();
            button1.Show();
            playGame.Show();
            foreach (Ship ship in ships)
            {

                ship.rectangle.X = 0;
                ship.rectangle.Y = ship.rectangle.Width;

            }
            pictureBox.Refresh();

        }

      




        private void GameChoice_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine("KeyPressed" + e.KeyCode);

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

        private void GameChoice_KeyUp(object sender, KeyEventArgs e)
        {
            isKeyDown = false;
            pictureBox.Refresh();
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine(isMouseDown);

           // Console.WriteLine(e.Location.X + " " + e.Location.Y);

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

                    Console.WriteLine("Selected: " + ship.selected);
                }
            }

            pictureBox.Refresh();
        }

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

        private void button3_Click(object sender, EventArgs e)
        {
            // save the location of ship
            List<int> eachLocation= new List<int>();
            
            foreach (Ship ship in ships)
            {
                eachLocation.Add(ship.rectangle.X);
                eachLocation.Add(ship.rectangle.Width);
                eachLocation.Add(ship.rectangle.Y);
                eachLocation.Add(ship.rectangle.Height);
                if (player_1)
                {
                   // player_1_location.Add(eachLocation);
                }
                else
                {
                   // player_2_location.Add(eachLocation);
                }
                     
                
            }
            button3.Hide();
            button1.Hide();

        }

        private void playGame_Click(object sender, EventArgs e)
        {
            //play a game
            this.Hide();
        }
    }

    public static class Formatting {
        public const int squareSize = 30;
        public const int offset = (int)(squareSize * 0.1);
        public const int shipWidth = (int)(squareSize * 0.8);
    }
}

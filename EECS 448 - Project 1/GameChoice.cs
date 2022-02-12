using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EECS_448___Project_1
{
    public partial class GameChoice : Form
    {
        public bool player_1 = true;
        public List<int[][]> player_1_location = new List<int[][]>();
        public List<int[][]> player_2_location = new List<int[][]>();

        public GameChoice()
        {
            this.KeyPreview = true;
            InitializeComponent();
            addShips(1);
            addShips(2);
            addShips(3);
            addShips(4);
            addShips(5);
        }

        private class Ship
        {
            public Rectangle rectangle; //rectangle used in drawing, will be passed in methods to picturebox
            public bool selected;       //is the ship selected
            public bool isRotated;      //true when ship is verticle

            public Ship(int length, int x, int y)
            {
                rectangle = new Rectangle(x, y, length * 30, 30);
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
        }

        List<Ship> ships = new List<Ship>();

        bool isMouseDown = false; //is the mouse button down on the picturebox
        bool isKeyDown = false;

        //addRects
        private void addShips(int num)
        {
            Ship ship = new Ship(num, 0, 30*num);
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



        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            //pen
            Pen pen = new Pen(Color.White, 1);
            Point start = new Point();
            Point end = new Point();

            //draw horizontal lines
            for (int j = 0; j < 11; j++)
            {
                start.X = 0;
                start.Y = j * pictureBox2.Height / 10;
                end.X = pictureBox2.Width;
                end.Y = start.Y;
                e.Graphics.DrawLine(pen, start, end);
            }
            //draw bottom border
            start.Y = pictureBox2.Height - (int)Math.Ceiling(pen.Width);
            end.Y = start.Y;
            e.Graphics.DrawLine(pen, start, end);

            //draw verticle lines
            for (int i = 0; i < 11; i++)
            {
                //draw verticle lines
                start.X = i * pictureBox2.Width / 10;
                start.Y = 0;
                end.X = start.X;
                end.Y = pictureBox2.Height;
                e.Graphics.DrawLine(pen, start, end);

            }
            //draw right side border
            start.X = pictureBox2.Width - (int)Math.Ceiling(pen.Width);
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
            pictureBox2.Refresh();
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
            pictureBox2.Refresh();

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
                    pictureBox2.Refresh();
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
            pictureBox2.Refresh();
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
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
                    }
                    else
                    {
                        ship.selected = false;
                    }

                    Console.WriteLine("Selected: " + ship.selected);
                }
            }

            pictureBox2.Refresh();
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            //deselect all ships
            foreach (Ship ship in ships) ship.selected = false;

            //set mouse down tracker
            isMouseDown = false;

            //refresh the picturebox
            pictureBox2.Refresh();
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
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
                pictureBox2.Refresh();
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
                    player_1_location.Add(eachLocation);
                }
                else
                {
                    player_2_location.Add(eachLocation);
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
}

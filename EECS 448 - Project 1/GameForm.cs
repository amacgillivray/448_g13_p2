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
    public partial class GameForm : Form {

        //test
        Player one = new Player("John");
        Player two = new Player("Jane");
        Game game = new Game();

        //trackers
        bool targeted = false;
        int[] targetSquare = new int[2];
        bool mouseDown = false;


        public GameForm() {
            InitializeComponent();
            InitializePictureBoxes();

            game.setPlayerOne(one);
            game.setPlayerTwo(two);
            game.setCurrentPlayer(1);
                       
        }

        private void InitializePictureBoxes() {
            myBoardPictureBox.Refresh();
            oppBoardPictureBox.Refresh();
        }

        #region drawing methods

        private void drawGridLines(object sender, PaintEventArgs e, PictureBox pictureBox) {
            Console.WriteLine("Drawing grid lines");
            //pen
            Pen pen = new Pen(Color.White, 1);
            Point start = new Point();
            Point end = new Point();

            //draw horizontal lines
            for (int j = 0; j < 11; j++) {
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
            for (int i = 0; i < 11; i ++) {
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
        }



        private void drawHits(object sender, PaintEventArgs e, List<int[]>hits, PictureBox pictureBox) { //takes list of coordinates of hits and the picture box to draw them on
            Console.WriteLine("Draw hits");
            int penThickness = 3;
            int offset = 3;
            Pen pen = new Pen(Color.Red, penThickness);

            foreach (int[] hit in hits) { //hit[col,row]
                //coordinates
                int col = hit[0];
                int row = hit[1];

                //draw line from top left to bottom right of square
                Point downStart = new Point(col * pictureBox.Width / 10 + offset, (row + 1) * pictureBox.Height / 10 - offset);
                Point downEnd = new Point((col + 1) * pictureBox.Width / 10 - offset, row * pictureBox.Height / 10 + offset);
                e.Graphics.DrawLine(pen, downStart, downEnd);

                //draw line from bottom left to top right of square
                Point upStart = new Point(col * pictureBox.Width / 10 + offset, row * pictureBox.Height / 10 + offset);
                Point upEnd = new Point((col + 1) * pictureBox.Width / 10 - offset, (row + 1) * pictureBox.Height / 10 - offset);
                e.Graphics.DrawLine(pen, upStart, upEnd);

                //add exception handling
               
                //dispose
            }
        }

        private void drawMisses(object sender, PaintEventArgs e, List<int[]>misses, PictureBox pictureBox) {
            Console.WriteLine("Drawing misses");
            SolidBrush brush = new SolidBrush(Color.WhiteSmoke);
            int offset = 3;
            
            foreach(int[] miss in misses) {
                //coordinates of miss
                int col = miss[0];
                int row = miss[1];

                //rectangpe
                Rectangle circle = new Rectangle(col * pictureBox.Width / 10 + offset, row * pictureBox.Height / 10 + offset, 24, 24);

                //draw ellipse
                e.Graphics.FillEllipse(brush, circle);
            }
        }

        private void drawShips(object sender, PaintEventArgs e, List<int[,]>ships, PictureBox pictureBox) {
            Console.WriteLine("Drawing ships");
            SolidBrush brush = new SolidBrush(Color.Gray);
            int offset = 3;

            foreach (int[,] ship in ships) {
                //get starting coordinates
                int startingCol = ship[0, 0];
                int startingRow = ship[0, 1];

                //get ending coordinates
                int endingCol = ship[ship.Length / 2 - 1, 0];
                int endingRow = ship[ship.Length / 2 - 1, 1];

                //ship dimensions in squares
                int squareWidth = Math.Abs(endingCol - startingCol + 1);
                int squareHeight = Math.Abs(endingRow - startingRow + 1);
                if (squareWidth < 1) squareWidth = 1;
                if (squareHeight < 1) squareHeight = 1;

                Console.WriteLine(squareWidth + " " + squareHeight);

                //ship dimensions in pixels
                int shipWidth = squareWidth * pictureBox.Width / 10 - 2 * offset;
                int shipHeight = squareHeight * pictureBox.Height / 10 - 2 * offset;

                //rectangle
                Rectangle rect = new Rectangle(startingCol * pictureBox.Width / 10 + offset, (startingRow + 1) * pictureBox.Height / 10 + offset, shipWidth, shipHeight);

                Console.WriteLine(rect.X + " " + rect.Y + " " + shipHeight + " " + shipWidth);

                //draw rectangle
                e.Graphics.FillRectangle(brush, rect);

                Console.Write("Ship drawn");
            }
        }


        private void drawTargetSquare(object sender, PaintEventArgs e, int[] target) {
            Console.WriteLine("Drawing target square");
            if (targeted) {
                //Pen
                Pen pen = new Pen(Color.Orange, 3);

                //starting 
                Point start = new Point();
                Point end = new Point();

                //draw top line
                e.Graphics.DrawLine(pen, target[0] * oppBoardPictureBox.Width / 10, target[1] * oppBoardPictureBox.Height / 10,
                                         (target[0] + 1) * oppBoardPictureBox.Width / 10, target[1] * oppBoardPictureBox.Height / 10);
                //draw bottom line
                e.Graphics.DrawLine(pen, target[0] * oppBoardPictureBox.Width / 10, (target[1] + 1) * oppBoardPictureBox.Height / 10,
                                         (target[0] + 1) * oppBoardPictureBox.Width / 10, (target[1] + 1) * oppBoardPictureBox.Height / 10);
                //draw left line
                e.Graphics.DrawLine(pen, target[0] * oppBoardPictureBox.Width / 10, target[1] * oppBoardPictureBox.Height / 10,
                                         target[0] * oppBoardPictureBox.Width / 10, (target[1] + 1) * oppBoardPictureBox.Height / 10);
                //draw right line
                e.Graphics.DrawLine(pen, (target[0] + 1) * oppBoardPictureBox.Width / 10, target[1] * oppBoardPictureBox.Height / 10,
                                         (target[0] + 1) * oppBoardPictureBox.Width / 10, (target[1] + 1) * oppBoardPictureBox.Height / 10);

            }
        }

        #endregion



        private void myBoardPictureBox_Paint(object sender, PaintEventArgs e) {
            drawGridLines(sender, e,this.myBoardPictureBox);

            Player player; //current player

            /*List<int[]> hits = new List<int[]>();
            for(int i = 0; i < 10; i ++) {
                int[] hit = { 3, i };
                hits.Add(hit);
            }

            List<int[]> misses = new List<int[]>();
            int[] miss = { 7, 5 };
            misses.Add(miss);

            List<int[,]> ships = new List<int[,]>();
            int[,] ship = { { 1, 2 }, { 1, 3 }, { 1, 4 }, { 1, 5 } };
            ships.Add(ship);*/

            //determine whos turn it is
            if(game.getPlayerTurn() == 1) {
                player = game.getPlayerOne();
            } else {
                player = game.getPlayerOne();
            }

            //draw ships
            drawShips(sender, e, player.getShips(), myBoardPictureBox);

            //draw hits
            drawHits(sender, e, player.getHits(), myBoardPictureBox);

            //draw missses
            drawMisses(sender, e, player.getMisses(), myBoardPictureBox);
        }


        #region oppBoard Methods
        
        //mouse move
        private void oppBoardPictureBox_MouseMove(object sender, MouseEventArgs e) {
            if(!targeted) {
                int row = e.Location.Y / (oppBoardPictureBox.Height / 10);
                int col = e.Location.X / (oppBoardPictureBox.Width / 10);
                char colAlpha = (char)(col + 65);

                targetingLabel.Text = "Targeting: " + colAlpha + (row + 1);
            }
        }
        
        //mouse down
        private void oppBoardPictureBox_MouseDown(object sender, MouseEventArgs e) {
            //check if mouse is already down
            if (!mouseDown) {
                Console.WriteLine("Mouse Down");
                mouseDown = true;
                targeted = true;

                //set targeted square
                int col = e.Location.X / (oppBoardPictureBox.Height / 10);
                int row = e.Location.Y / (oppBoardPictureBox.Height / 10);
                targetSquare[0] = col;
                targetSquare[1] = row;

                //check if target is legal
                for(int i = 0; i < game.getCurrentOpponent().getHits().Count; i++) {  //check 
                    if (targetSquare.SequenceEqual(game.getCurrentOpponent().getHits()[i])) targeted = false;
                }
                for(int i = 0; i < game.getCurrentOpponent().getMisses().Count; i++) {
                    Console.WriteLine("miss " + game.getCurrentOpponent().getMisses()[i][0] + " " + game.getCurrentOpponent().getMisses()[i][1] + " target: " + targetSquare[0] + " " + targetSquare[1]);
                    if (targetSquare.SequenceEqual(game.getCurrentOpponent().getMisses()[i])) targeted = false;
                }

                //update label
                char colAlpha = (char)(col + 65);
                targetingLabel.Text = "Targeting: " + colAlpha + (row + 1);

                //paint
                targetingLabel.Text = targeted.ToString();
                this.Refresh();
            }
        }

        //mouse up
        private void oppBoardPictureBox_MouseUp(object sender, MouseEventArgs e) {
            mouseDown = false;
        }


        //paint
        private void oppBoardPictureBox_Paint(object sender, PaintEventArgs e) {
            Console.WriteLine("Called");
            drawGridLines(sender, e, oppBoardPictureBox);

            List<int[]> hits = new List<int[]>();
            for (int i = 0; i < 10; i++) {
                int[] hit = { 3, i };
                game.getPlayerTwo().addHit(hit);
            }

            List<int[]> misses = new List<int[]>();
            int[] miss = { 7, 5 };
            game.getPlayerTwo().addMiss(miss);

            List<int[,]> ships = new List<int[,]>();
            int[,] ship = { { 1, 2 }, { 1, 3 }, { 1, 4 }, { 1, 5 } };
            game.getPlayerTwo().addShip(ship);


            //draw ships
            drawShips(sender, e, game.getPlayerTwo().getShips(), oppBoardPictureBox);

            //draw hits
            drawHits(sender, e, game.getPlayerTwo().getHits(), oppBoardPictureBox);

            //draw missses
            drawMisses(sender, e, game.getPlayerTwo().getMisses(), oppBoardPictureBox);

            //draw target outline
            Console.WriteLine("Targeted: " + targeted);
            if (targeted) drawTargetSquare(sender, e, targetSquare);
        }



        #endregion

        private void GameForm_Load(object sender, EventArgs e)
        {

        }
    }
}

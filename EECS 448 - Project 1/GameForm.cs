using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace EECS_448___Project_1 {
    public partial class GameForm : Form {

        //vars
        Game game = new Game();
        bool targeted = false;
        int[] targetSquare = new int[2];
        bool mouseDown = false;
        bool isspecialshot = false;


        public GameForm(ref Game game) {
            InitializeComponent();
            this.game = game;
            InitializePictureBoxes();

        }

        //draws pictureboxes for the first time
        private void InitializePictureBoxes() {
            //game.swapCurrentPlayer();
            updateBoardLabels();
            myBoardPictureBox.Refresh();
            oppBoardPictureBox.Refresh();
        }

        //update board labels
        private void updateBoardLabels() {
            oppBoardLabel.Text = game.getCurrentOpponent().getName();
            myBoardLabel.Text = game.getCurrentPlayer().getName();
		}

        #region drawing methods
        private void drawGridLines(object sender, PaintEventArgs e, PictureBox pictureBox) {
            //pen
            Pen pen = new Pen(Color.White, 1);
            Point start = new Point();
            Point end = new Point();

            //draw horizontal lines
            for(int j = 0; j < 11; j++) {
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
            for(int i = 0; i < 11; i++) {
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


        //Draw hits
        private void drawHits(object sender, PaintEventArgs e, List<int[]> hits, PictureBox pictureBox) { //takes list of coordinates of hits and the picture box to draw them on
            int penThickness = 3;
            int offset = 3;
            Pen pen = new Pen(Color.Red, penThickness);

            foreach(int[] hit in hits) { //hit[col,row]
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
            }
        }

        //draw misses
        private void drawMisses(object sender, PaintEventArgs e, List<int[]> misses, PictureBox pictureBox) {
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

        //draw ships
        private void drawShips(object sender, PaintEventArgs e, List<int[][]> ships, PictureBox pictureBox) {
            SolidBrush brush = new SolidBrush(Color.Gray);
            int offset = 3;

            foreach(int[][] ship in ships) {
                //get starting coordinates
                int startingCol = ship[0][0];
                int startingRow = ship[0][1];

                //get ending coordinates
                int endingCol = ship[ship.Length - 1][0];
                int endingRow = ship[ship.Length - 1][1];

                //ship dimensions in squares
                int squareWidth = Math.Abs(endingCol - startingCol + 1);
                int squareHeight = Math.Abs(endingRow - startingRow + 1);
                if(squareWidth < 1) squareWidth = 1;
                if(squareHeight < 1) squareHeight = 1;

                //ship dimensions in pixels
                int shipWidth = squareWidth * pictureBox.Width / 10 - 2 * offset;
                int shipHeight = squareHeight * pictureBox.Height / 10 - 2 * offset;

                //rectangle
                Rectangle rect = new Rectangle(startingCol * pictureBox.Width / 10 + offset, startingRow * pictureBox.Height / 10 + offset, shipWidth, shipHeight);

                //draw rectangle
                e.Graphics.FillRectangle(brush, rect);
            }
        }

        //draw targetted square
        private void drawTargetSquare(object sender, PaintEventArgs e, int[] target) {
            if(targeted) {
                //Pen
                Pen pen = new Pen(Color.Orange, 3);

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

        //Paint function for current player's picture box, this gets called on Refresh()
        private void myBoardPictureBox_Paint(object sender, PaintEventArgs e) {
            //draw grid first
            drawGridLines(sender, e, this.myBoardPictureBox);

            //draw ships
            drawShips(sender, e, game.getCurrentPlayer().getShips(), myBoardPictureBox);

            //draw hits
            drawHits(sender, e, game.getCurrentOpponent().getHits(), myBoardPictureBox);

            //draw missses
            drawMisses(sender, e, game.getCurrentOpponent().getMisses(), myBoardPictureBox);
        }


        #region oppBoard Methods
        //mouse move event
        private void oppBoardPictureBox_MouseMove(object sender, MouseEventArgs e) {
            if(!targeted) {
                int row = e.Location.Y / (oppBoardPictureBox.Height / 10);
                int col = e.Location.X / (oppBoardPictureBox.Width / 10);
                char colAlpha = (char)(col + 65);

                targetingLabel.Text = "Targeting: " + colAlpha + (row + 1);
            }
        }

        //mouse down event
        private void oppBoardPictureBox_MouseDown(object sender, MouseEventArgs e) {
            //check if mouse is already down
            if(!mouseDown) {
                mouseDown = true;
            }
        }

        //mouse up event
        private void oppBoardPictureBox_MouseUp(object sender, MouseEventArgs e) {
            //check if the mouse was down
            if(mouseDown) {
                targeted = true; //player has now selected a target

                //set targeted square
                int col = e.Location.X / (oppBoardPictureBox.Height / 10);
                int row = e.Location.Y / (oppBoardPictureBox.Height / 10);
                targetSquare[0] = col;
                targetSquare[1] = row;

                //check if target is legal (has it already been guessed?)
                for(int i = 0; i < game.getCurrentPlayer().getHits().Count; i++) {     //check if targeted square is on a hit
                    if(targetSquare.SequenceEqual(game.getCurrentPlayer().getHits()[i])) targeted = false; //no longer target confirmed
                }
                for(int i = 0; i < game.getCurrentPlayer().getMisses().Count; i++) {   //check if targeted square is on a miss
                    if(targetSquare.SequenceEqual(game.getCurrentPlayer().getMisses()[i])) targeted = false; //no longer target confirmed
                }

                //update label
                char colAlpha = (char)(col + 65);
                targetingLabel.Text = "Targeting: " + colAlpha + (row + 1);

                //paint
                this.Refresh();
                mouseDown = false;
            }
        }


        AIwin win1;
        YouWin win2;

        void AIwin_FormClosed(object sender, FormClosedEventArgs e)
        {
            win1 = null;  //If form is closed make sure reference is set to null
            Application.Exit(); //close application when form is closed
        }

        void YouWin_FormClosed(object sender, FormClosedEventArgs e)
        {
            win2 = null;  //If form is closed make sure reference is set to null
            Application.Exit(); //close application when form is closed
        }

        //painth function for opponents picturebox
        private void oppBoardPictureBox_Paint(object sender, PaintEventArgs e) {
            if(game.getCurrentOpponent().getSunk() >= game.getCurrentOpponent().getShips().Count())
            {
                win1 = new AIwin();
                win1.Show();                 //show win screen
                win1.FormClosed += AIwin_FormClosed; //add an event handler for when form is closed
                this.Hide();
            }
            //draw grid lines
            drawGridLines(sender, e, oppBoardPictureBox);
            
            //remove this at the end so enemy ships will not show
            // drawShips(sender, e, game.getCurrentOpponent().getShips(), myBoardPictureBox);
            
            //draw hits
            drawHits(sender, e, game.getCurrentPlayer().getHits(), oppBoardPictureBox);

            //draw missses
            drawMisses(sender, e, game.getCurrentPlayer().getMisses(), oppBoardPictureBox);

            //draw target outline
            if(targeted) drawTargetSquare(sender, e, targetSquare);
        }
		#endregion

		#region fire methods
		//Fire button clicked
		private void fireButton_Click(object sender, EventArgs e) {
            fire(); //call the fire button
        }

        //fire method
        private void fire() {
            if(targeted)                       //if there is a valid target
            {
                int delay = 1000;               //sets duration of delay for 1 second
                if (game.ai_level > 0)
                    delay = 266;

                if (!isspecialshot)             //normal shot
                    game.fire(targetSquare);        //call game fire method, targetting targetSquare (see Game.cs)
                else                            //special shot
                {
                    int[] sixsquares = new int[2];
                    for (int i = targetSquare[0] - 1; i <= targetSquare[0] + 1; i++)
                    {
                        for (int j = targetSquare[1] - 1; j <= targetSquare[1] + 1; j++)
                        {
                            if (i >= 0 && i <= 10 && j >= 0 && j <= 10)
                            {
                                sixsquares[0] = i;
                                sixsquares[1] = j;
                                game.fire(sixsquares);
                            }
                        }
                    }
                    isspecialshot = false;
                }//special shot


                targeted = false;               //no longer targetting
                oppBoardPictureBox.Refresh();   //refreshes images on board
                Thread.Sleep(delay);            //pauses for 1 second (1000 milliseconds)

                //check win conditions
                if(game.getCurrentPlayer().getSunk() >= game.getCurrentOpponent().getShips().Count()) {
                    win2 = new YouWin();
                    win2.Show();                 //show win screen
                    win2.FormClosed += YouWin_FormClosed; //add an event handler for when form is closed
                    this.Hide();
                } else {
                    if (game.ai_level == 0) // for vs player
                    { 
                        Form2 landing = new Form2(ref game);
                        landing.Show();                         //Swaps to the landing form
                        game.swapCurrentPlayer();               //Changes the current player (flips the boards)
                        InitializePictureBoxes();
                        this.Close();
                    } else // for vs ai
                    {
                        switch (game.ai_level)
                        {
                            case 1:
                                game.hitgen_easy();
                                break;
                            case 2:
                                game.hitgen_medium();
                                break;
                            case 3:
                                game.hitgen_hard();
                                break;
                        }
                        myBoardPictureBox.Refresh();
                        oppBoardPictureBox.Refresh();
                    }
                }
            }
        }
        #endregion

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label40_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            isspecialshot = true;
            this.button1.Enabled = false;
        }
    }
}

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
    public partial class PlayerCreator : Form
    {
        private int ai_level;

        public PlayerCreator()
        {
            ai_level = 0;
            InitializeComponent();
        }

        public PlayerCreator(int ai_difficulty)
        {
            ai_level = ai_difficulty;
            //textBox2.Text = "Computer";
            InitializeComponent();
            textBox2.Text = "Computer";
            textBox2.Enabled = false;
        }

        //next button
        private void button1_Click(object sender, EventArgs e)
        {
            Player player1 = new Player(textBox1.Text);  //creates player 1 from textbox 1
            Player player2;
            if (ai_level == 0)
                player2 = new Player(textBox2.Text);  //creates player 2 from textbox 2
            else
                player2 = new Player("Computer");  //creates player 2 from textbox 2
            Game game = new Game(player1, player2, ai_level);     //creates a game(object) using the players created
            SetupPage setup = new SetupPage(game);      // passes the game to the next form
            setup.Show();                               // shows the next form
            this.Hide();                               // Hides this form
        }
    }
}

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
        public PlayerCreator()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Player player1 = new Player(textBox1.Text);  //creates player 1 from textbox 1
            Player player2 = new Player(textBox2.Text);  //creates player 2 from textbox 2
            Game game = new Game(player1, player2);     //creates a game(object) using the players created
            SetupPage setup = new SetupPage(game);      // passes the game to the next form
            setup.Show();                               // shows the next form
            this.Close();                               // closes this form
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

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
            Player player1 = new Player(textBox1.Text);
            Player player2 = new Player(textBox2.Text);
            Game game = new Game(player1, player2);
            SetupPage setup = new SetupPage(game);
            setup.Show();
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

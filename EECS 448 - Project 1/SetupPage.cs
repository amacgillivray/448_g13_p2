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
    public partial class SetupPage : Form {

        Game game = new Game();

        public SetupPage(Game game) {
            InitializeComponent();
            this.game = game;
        }

        //one ship game
        private void button1_Click(object sender, EventArgs e)
        {
            GameChoice choice = new GameChoice(game, 1);
            choice.Show();
            this.Close();
        }

        //two ship game
        private void button2_Click(object sender, EventArgs e)
        {
            GameChoice choice = new GameChoice(game, 2);
            choice.Show();
            this.Close();
        }

        //three ship game
        private void button3_Click(object sender, EventArgs e)
        {
            GameChoice choice = new GameChoice(game, 3);
            choice.Show();
            this.Close();
        }

        //four ship game
        private void button4_Click(object sender, EventArgs e)
        {
            GameChoice choice = new GameChoice(game, 4);
            choice.Show();
            this.Close();
        }

        //five ship game
        private void button5_Click(object sender, EventArgs e)
        {
            GameChoice choice = new GameChoice(game, 5);
            choice.Show();
            this.Close();
        }
    }
}

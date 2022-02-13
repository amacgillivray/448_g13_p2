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
        public SetupPage() {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            GameChoice choice = new GameChoice();
            choice.Show();

            //Game game = new Game();
            //GameForm gameForm = new GameForm();
            //gameForm.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Game game = new Game();
            GameForm gameForm = new GameForm();
            gameForm.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Game game = new Game();
            GameForm gameForm = new GameForm();
            gameForm.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Game game = new Game();
            GameForm gameForm = new GameForm();
            gameForm.Show();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Game game = new Game();
            GameForm gameForm = new GameForm();
            gameForm.Show();

            this.Close();
        }
    }
}

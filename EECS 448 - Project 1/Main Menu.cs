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
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void LocalGameButton_Click(object sender, EventArgs e) {
            GameChoice game = new GameChoice();
            game.Show();
            //SetupPage setup = new SetupPage();
            //setup.Show();
            this.Hide();
        }
    }
}

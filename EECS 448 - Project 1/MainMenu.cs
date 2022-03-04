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

            PlayerCreator players = new PlayerCreator(); //Creates the playercreator form
            players.Show();                             //shows next form
            this.Hide();                                //Hides this form
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreditsPage credits = new CreditsPage(); //Creates a Credits form
            credits.Show();                          //Shows the credits               
            this.Hide();                               // Hides this form
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}

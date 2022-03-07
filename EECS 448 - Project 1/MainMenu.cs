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

        PlayerCreator players;
                  
        private void LocalGameButton_Click(object sender, EventArgs e) {
            if (players == null)
            {
               players = new PlayerCreator();   //Create form if not created
               players.FormClosed += players_FormClosed;  //Add eventhandler to cleanup after form closes
            }

            players.Show(this);  //Show Form assigning this form as the forms owner
            this.Hide();
        }

        void players_FormClosed(object sender, FormClosedEventArgs e)
        {
            players = null;  //If form is closed make sure reference is set to null
            //Show();
            Application.Exit();
        }

        CreditsPage credits;

        private void button1_Click(object sender, EventArgs e)
        {
            if (credits == null)
            {
               credits = new CreditsPage();   //Create form if not created
               credits.FormClosed += credits_FormClosed;  //Add eventhandler to cleanup after form closes
            }

            credits.Show(this);  //Show Form assigning this form as the forms owner
            this.Hide();
        }

        void credits_FormClosed(object sender, FormClosedEventArgs e)
        {
            credits = null;  //If form is closed make sure reference is set to null
            Application.Exit();
        }

        chooselevelAI choose;

        private void button3_Click(object sender, EventArgs e)
        {
            if (choose == null)
            {
               choose = new chooselevelAI();   //Create form if not created
               choose.FormClosed += choose_FormClosed;  //Add eventhandler to cleanup after form closes
            }

            choose.Show(this);  //Show Form assigning this form as the forms owner
            this.Hide();
        }

        void choose_FormClosed(object sender, FormClosedEventArgs e)
        {
            choose = null;  //If form is closed make sure reference is set to null
            //Show();
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

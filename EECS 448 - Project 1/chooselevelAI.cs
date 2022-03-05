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

namespace EECS_448___Project_1
{
    public partial class chooselevelAI : Form
    {
        PlayerCreator players;
        private int selection = 0;

        public chooselevelAI()
        {
            InitializeComponent();
        }

        public int get_difficulty()
        {
            return selection;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (players == null)
            {
                players = new PlayerCreator(1);   //Create form if not created
                players.FormClosed += players_FormClosed;  //Add eventhandler to cleanup after form closes
            }

            players.Show(this);  //Show Form assigning this form as the forms owner
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (players == null)
            {
                players = new PlayerCreator(2);   //Create form if not created
                players.FormClosed += players_FormClosed;  //Add eventhandler to cleanup after form closes
            }

            players.Show(this);  //Show Form assigning this form as the forms owner
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (players == null)
            {
                players = new PlayerCreator(3);   //Create form if not created
                players.FormClosed += players_FormClosed;  //Add eventhandler to cleanup after form closes
            }

            players.Show(this);  //Show Form assigning this form as the forms owner
            this.Hide();
        }

        void players_FormClosed(object sender, FormClosedEventArgs e)
        {
            players = null;  //If form is closed make sure reference is set to null
            // Show();
        }
    }
}

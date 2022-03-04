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
        public chooselevelAI()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //vars
            Game game = new Game(1);
           
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Game game = new Game(2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Game game = new Game(3);
        }
    }
}

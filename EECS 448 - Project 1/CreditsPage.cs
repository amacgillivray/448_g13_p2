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
    public partial class CreditsPage : Form
    {
        public CreditsPage()
        {
            InitializeComponent();
        }

        private void label_Click(object sender, EventArgs e)
        {
            Form1 mainmenu = new Form1();
            mainmenu.Show();
            this.Close();
        }

        private void CreditsPage_Load(object sender, EventArgs e)
        {

        }

        private void CreditsPage_Click(object sender, EventArgs e)
        {
            Form1 mainmenu = new Form1();
            mainmenu.Show();
            this.Close();
        }
    }
}

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
    public partial class ConstructionPage : Form
    {
        public ConstructionPage()
        {
            InitializeComponent();
        }

        private void ConstructionPage_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 mainmenu = new Form1();
            mainmenu.Show();
            this.Close();
        }
    }
}

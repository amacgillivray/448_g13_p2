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
    public partial class Form2 : Form {

        //var (passed around
        Game game = new Game();

        //constructor with param
        public Form2(ref Game game)
        {
            InitializeComponent();
            this.game = game;
        }

        //okay button
        private void button1_Click(object sender, EventArgs e)
        {
            GameForm gameForm = new GameForm( ref game);
            gameForm.Show();
            this.Close();
           
        }
    }
}

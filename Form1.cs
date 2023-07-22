using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Doctors_Office
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void patinetsBtn_Click(object sender, EventArgs e)
        {
            patientsForm ptn = new patientsForm();
            ptn.Show();
        }

        private void button_WOC1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}

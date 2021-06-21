using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FruitFlyWingStatWithGraphInCSharpdotNet
{
    public partial class frmSplashScreen : Form
    {
        public frmSplashScreen()
        {
            InitializeComponent();
        }

        int pbvalue = 0; //Progress Bar value
        private void frmSplashScreen_Load(object sender, EventArgs e)
        {
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Step = 20;
            progressBar1.Value = 0;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.PerformStep();
            pbvalue += 20;
            if (pbvalue > 100)
            {
                timer1.Stop();
                this.Hide();
                frmMain form2 = new frmMain();
                form2.Closed += (s, args) => this.Close();
                form2.Show();
            }
        }
    }
}

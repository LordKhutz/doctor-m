using System;
using System.Windows.Forms;

namespace doctor_m
{
    public partial class frmIndex : Form
    {
        public frmIndex()
        {
            InitializeComponent();
        }
        frmLogin Login = new frmLogin();
        private void frmIndex_Load(object sender, EventArgs e)
        {
            //focus on the login screen
            Login.MdiParent = this;
            Login.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //change the text on the title bar of the main window.
            if (ActiveMdiChild.Text == "Login")
            {
                Login.username = "";
            }
            this.Text = "Dr. M. | " + ActiveMdiChild.Text + " | logged in as : " + Login.username;
        }
    }
}

using System;
using System.Windows.Forms;
using System.Data;

namespace doctor_m
{

    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }
        DataTable appointments = new DataTable();

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                MdiParent.Close();
            }
            else
            {
                txtName.Clear();
                txtPass.Clear();
                txtName.Focus();
            }
        }

        public string username = "";
        private void btnOK_Click(object sender, EventArgs e)
        {
            frmPatientManagement frm = new frmPatientManagement();
            bool fail = false;
            frm.MdiParent = MdiParent;
            load_table();
            if (appointments.Rows.Count == 1)
            {
                if (appointments.Rows[0].ItemArray[1].ToString() == txtPass.Text)
                {
                    frm.Show();
                    username = txtName.Text;
                    txtName.Clear();
                    txtPass.Clear();
                }
                else
                    fail = true;
            }
            else
                fail = true;
            if (fail)
            {
                MessageBox.Show("Incorrect credentials submitted", "Login failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPass.Clear();
                txtPass.Focus();
            }

        }
        private void load_table()
        {
            clsDB_conn myDb_Con = new clsDB_conn();
            appointments = myDb_Con.db_query("SELECT * FROM users  WHERE  userName = '" +txtName.Text + "'", "|DataDirectory|/doctor_m.mdb");
        }
        private void frmLogin_Load(object sender, EventArgs e)
        {
            txtName.Focus();
        }
    }
}

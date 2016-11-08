using System;
using System.Data;
using System.Windows.Forms;

namespace doctor_m
{
    public partial class frmNewUser : Form
    {
        public frmNewUser()
        {
            InitializeComponent();
        }

        private void frmNewUser_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'doctor_mDataSet.users' table. You can move, or remove it, as needed.
            usersTableAdapter.Fill(doctor_mDataSet.users);
            // TODO: This line of code loads data into the 'doctor_mDataSet.users' table. You can move, or remove it, as needed.
            usersTableAdapter.Fill(doctor_mDataSet.users);
            bindingNavigatorAddNewItem.PerformClick();
            usersBindingNavigator.Visible = false;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Validate();
                usersBindingSource.EndEdit();
                tableAdapterManager.UpdateAll(doctor_mDataSet);
                MessageBox.Show("Save Successful.", "Add User", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            //catch any error related to the data entered.
            catch (DataException er)
            {
                if (MessageBox.Show(er.Message + "\nTry Again?", "Add User", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
                {
                    Close();
                }
                else
                {
                    userNameTextBox.Text = "";
                    userNameTextBox.Focus();
                }
            }
        }
    }
}

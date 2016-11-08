using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Data;

namespace doctor_m
{
    public partial class frmPatientManagement : Form
    {
        public frmPatientManagement()
        {
            InitializeComponent();
        }
        DataTable appointments = new DataTable();
        StreamReader fileToPrint;
        System.Drawing.Font printFont;
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //save new patient details
            clsDB_conn myDb_Con = new clsDB_conn();
            DataTable temp  = myDb_Con.db_query("SELECT * FROM patients WHERE patient_name = '"+ txtName.Text +"' AND patient_surname = '"+ txtSurname.Text +"'", "|DataDirectory|/doctor_m.mdb");
            if (!(temp.Rows.Count > 0))
            {
                temp = myDb_Con.db_query("SELECT * FROM patients", "doctor_m.mdb");
                myDb_Con.db_query("INSERT INTO `patients` (`patient_name`,`patient_surname`,`patient_telephone`,`patient_address`) VALUES ('" + txtName.Text + "', '" + txtSurname.Text + "', '" + txtTelephone.Text + "', '" + txtAddress.Text + "')", "doctor_m.mdb");
                DataTable temp2 = myDb_Con.db_query("SELECT * FROM patients", "doctor_m.mdb");       
                MessageBox.Show("Save Successful", "Add patient", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Data failed to write to table,\nPatient already in Database.", "Add patient", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void searchPatientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //display the result of a search
            //prevent index out of range related crashes 
            try
            {
                string input = Interaction.InputBox("Please enter a patiants name or telephone number", "search user");
                clsDB_conn myDb_Con = new clsDB_conn();
                appointments = myDb_Con.db_query("SELECT * FROM patients  WHERE  patient_name = '" + input + "' OR patient_telephone = '" + input + "'", "doctor_m.mdb");
                frmPatientManagement frm = new frmPatientManagement();
                if (appointments.Rows.Count > 0)
                {
                    for (int i = 0; i < appointments.Rows.Count; i++)
                    {
                        listBox1.Items.Add("Patient Details:");
                        for (int j = 1; j < 5; j++)
                            listBox1.Items.Add("\t" + appointments.Rows[i].ItemArray[j]);
                    }
                }
                else
                    MessageBox.Show("Patient not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IndexOutOfRangeException m)
            {
                MessageBox.Show(m.Message+"\nPlease contact the administrator.","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        //allows you to view all patients in the form of a report.
        private void allPatientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //save the report to a file.
            clsDB_conn myDb_Con = new clsDB_conn();
            DataTable temp = myDb_Con.db_query("SELECT * FROM patients", "doctor_m.mdb");
            string path;
            SaveFileDialog sf = new SaveFileDialog();
            sf.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            sf.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (sf.ShowDialog(this) == DialogResult.OK)
            {
                path = sf.FileName;
                frmPatientManagement frm = new frmPatientManagement();
                ///prevent io releted crashes
                try
                {
                    if (temp.Rows.Count > 0)
                    {
                        if (File.Exists(path))
                            File.Delete(path);
                        using (StreamWriter wr = File.AppendText(path))
                        {
                            for (int j = 1; j < temp.Rows.Count; j++)
                            {

                                wr.WriteLine("Patient : " + temp.Rows[j].ItemArray[1] + " " + temp.Rows[j].ItemArray[2]);
                                for (int i = 3; i < 5; i++)
                                    wr.WriteLine("\t" + temp.Rows[j].ItemArray[i]);
                            }
                        }
                        MessageBox.Show("Save Successful", "write to file", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                        MessageBox.Show("No Patients found", "write to file", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (IOException a)
                {
                    MessageBox.Show(a.Message + "\nFailed to open file,\nPlease try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        //prints the patients report.
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //print the report.
            string path;
            OpenFileDialog of = new OpenFileDialog();
            of.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            of.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (of.ShowDialog(this) == DialogResult.OK)
            {
                path = of.FileName;
                fileToPrint = new StreamReader(path);
                printFont = new System.Drawing.Font("Arial", 10);
                printDocument1.Print();
                fileToPrint.Close();

            }
        }
        

        //allows you to change your password.
        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string input = Interaction.InputBox("Please enter your username", "change password");
            clsDB_conn myDb_Con = new clsDB_conn();
            load_table(input);
            if (appointments.Rows.Count > 0)
            {
                myDb_Con.db_query("UPDATE users  SET  passKey = '" + Interaction.InputBox("Please enter your new Password", "change password") + "' WHERE userName = '" + input + "'", "doctor_m.mdb");
            }
            else
                MessageBox.Show("User does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  
        }
        //loads the contents of the datatable in the database to the application.
        private void load_table(string val)
        {
            clsDB_conn myDb_Con = new clsDB_conn();
            appointments = myDb_Con.db_query("SELECT * FROM users  WHERE  userName = '" + val + "'", "doctor_m.mdb");
        }
        private void appointmentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddApointment newformApointment = new frmAddApointment();
            newformApointment.MdiParent = MdiParent;
            newformApointment.Show();
        }

        private void scheduleNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }
        static int CountNonSpaceChars(string value)
        {
            int result = 0;
            foreach (char c in value)
            {
                if (!char.IsWhiteSpace(c))
                {
                    result++;
                }
            }
            return result;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            //dynamically check if there are empty fields, for enabling the save button.
            if (
            txtName.Text != "" &&
            txtSurname.Text != "" &&
            CountNonSpaceChars(txtTelephone.Text) == 10 &&
            txtAddress.Text != ""
                )
            {
                btnSave.Enabled = true;
            }
            else
                btnSave.Enabled = false;
        }

        private void newUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmNewUser newUser = new frmNewUser();
            newUser.MdiParent = MdiParent;
            newUser.Show();
        }
    }
}

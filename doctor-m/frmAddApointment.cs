using System;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using Microsoft.VisualBasic;
namespace doctor_m
{
    public partial class frmAddApointment : Form
    {
        
        DataTable appointments = new DataTable();
        string patient_name ="";
        public frmAddApointment()
        {
            InitializeComponent();

        }

        //search for appointments by date, just select a date and data will show.
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (btnDelete.Enabled == false)
                load_table();
        }

        private void frmAddApointment_Load(object sender, EventArgs e)
        {
           load_table();
            comboBox1.SelectedIndex = 0;
            ToolTip delettip = new ToolTip();
            delettip.SetToolTip(btnDelete, "to delete a record, search for a patient, if the patient has an appointment, you can select the date and the time of the appointment and delete the record.");
            ToolTip savetip = new ToolTip();
            savetip.SetToolTip(btnSave, "only appointments after the current time can be saved");
            ToolTip canceltip = new ToolTip();
            canceltip.SetToolTip(btnCancel, "returns to the previous window with modifying the data.");
        }
        //loads the table for displaying.
        private void load_table()
        {
            clsDB_conn myDb_Con = new clsDB_conn();
            appointments.Reset();
            appointments = myDb_Con.db_query("SELECT patients.patient_name AS Patient, patients.patient_telephone AS `Phone Number`,appointments.comment AS Comment , appointments.time AS `Appointment Time` FROM appointments, patients  WHERE date_id = '" + 
                dateTimePicker1.Value.ToShortDateString() +
                "' AND appointments.patient_id = patients.patient_id ORDER BY appointments.time", "|DataDirectory|/doctor_m.mdb");
            dataGridView1.DataSource = appointments;
            lblviewTitle.Text = "Appointments for : " + dateTimePicker1.Value.ToShortDateString();
        }

        //dynamically search for the patient when the text typed into the textbox.
        //only when the text in the textbox is green, can you create a new appointment or delete an appointment.
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            clsDB_conn myDb_Con = new clsDB_conn();
            appointments = myDb_Con.db_query("SELECT * FROM patients WHERE patient_name = '"+ textBox2.Text+"'", "doctor_m.mdb");
            if (appointments.Rows.Count == 1)
            {
                textBox2.ForeColor = Color.Green;
                patient_name = appointments.Rows[0].ItemArray[0].ToString();
                btnSave.Enabled = true;
            }
            else
            {
                textBox2.ForeColor = Color.Red;
                btnSave.Enabled = false;
            }
        }
        //saves the appointment on the databse.
        private void button1_Click(object sender, EventArgs e)
        {
            clsDB_conn myDb_Con = new clsDB_conn();
            //checks if the chosen appointment date and time is available.
            if (DateTime.Compare(DateAndTime.Now, Convert.ToDateTime(dateTimePicker1.Text +" "+ comboBox1.SelectedItem)) < 0)
            {
                appointments = myDb_Con.db_query("SELECT * FROM appointments WHERE time = '"+ comboBox1.SelectedItem +"'", "doctor_m.mdb");
                if (appointments.Rows.Count < 1)
                {
                    myDb_Con.db_query("INSERT INTO `appointments` (date_id, patient_id, `time`, comment) VALUES ('" +
                        dateTimePicker1.Value.ToShortDateString() + "', '" + patient_name + "', '" + comboBox1.SelectedItem + "', '" +
                        Interaction.InputBox("Comment for appointment", "Appointment", "no comment") + "');", "doctor_m.mdb");
                    load_table();
                    textBox2.Text = null;
                    comboBox1.SelectedIndex = 0;
                    textBox2.Focus();
                }
                else
                    MessageBox.Show("please Select another time, " + Convert.ToDateTime(dateTimePicker1.Text + " " + comboBox1.SelectedItem).ToString() + "\nis not Available.", "Appointment");
            }
            else
                MessageBox.Show("please Select another time, " + Convert.ToDateTime(dateTimePicker1.Text + " " + comboBox1.SelectedItem).ToString() + "\nis not Available", "Appointment");
                comboBox1.Focus();
        }
        //searches for all appointments by patient, also enables you to delete an appointment.
        private void searchForAppointmentByPatientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string patient_id;
            string patient = Interaction.InputBox("Search for patient", "Search", "user");
            clsDB_conn myDb_Con = new clsDB_conn();
            DataTable tamp = myDb_Con.db_query("SELECT * FROM patients WHERE patient_name = '" + patient + "'", "doctor_m.mdb");
            if (tamp.Rows.Count == 1)
            {
                patient_id = tamp.Rows[0].ItemArray[0].ToString();
                appointments = myDb_Con.db_query("SELECT appointments.date_id AS `Date`, appointments.time AS `Appointment Time` FROM appointments, patients  WHERE appointments.patient_id = " + patient_id + "", "doctor_m.mdb");
                if (appointments.Rows.Count < 1)
                    MessageBox.Show("Patient has No appointments", "Appointments", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                    dataGridView1.DataSource = appointments;
                    lblviewTitle.Text = "Appointments for : " + patient;
                    textBox2.Text = patient;
                }
            }
            else
                MessageBox.Show("Patient Not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        //allows use to verify if the selected patient has any appointment and enables the delete appointment button.
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (textBox2.ForeColor == Color.Green)
            {
                clsDB_conn myDb_Con = new clsDB_conn();
                appointments = myDb_Con.db_query("SELECT * FROM appointments WHERE  patient_id = " + patient_name + " AND date_id = '" + Convert.ToDateTime(dateTimePicker1.Text).ToShortDateString() + "' AND `time` = '" + comboBox1.Text + "'", "doctor_m.mdb");
                if (appointments.Rows.Count > 0)
                {
                    btnDelete.Enabled = true;
                }
                else
                    btnDelete.Enabled = false;
            }
            
        }

        //to delete a record, search for a patient, if the patient has an appointment, you can select the date and the time of the appointment and delete the record
        //use the datetime picker to select the date and the combobox to select the time that corresponds to the appointment you want to delete.
        //button will only enable if the patient exists and has an apponitment at the date and time selected.
        private void button2_Click(object sender, EventArgs e)
        {
            clsDB_conn myDb_Con = new clsDB_conn();
            myDb_Con.db_query("DELETE * FROM appointments WHERE  patient_id = " + patient_name + " AND date_id = '" + Convert.ToDateTime(dateTimePicker1.Text).ToShortDateString() +"' AND `time` = '" + comboBox1.Text +"'", "doctor_m.mdb");
            load_table();
            textBox2.Clear();
            comboBox1.SelectedIndex = 0;
        }
        private void mouseOver(object sender, EventArgs e)
        {
        }
        private void mous_hover(object sender, EventArgs e)
        {

        }
    }
}

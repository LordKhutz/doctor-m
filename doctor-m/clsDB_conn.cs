using System;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;

namespace doctor_m
{
    public class clsDB_conn
    {
        public clsDB_conn()
        {

        }
        private OleDbConnection db_conn = new OleDbConnection();
        private string db_con(string db_loc)
        {
            string conn_str = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+db_loc + ";";
            //prevent crash when connecting to the database
            try
            {
                db_conn.ConnectionString = conn_str;
                db_conn.Open();
            }
            catch(OleDbException e)
            {
                return (e.Message + "\nFailed to connect to the database\nPlease Contact Administrator.");
            }
            return (null);
        }
        public DataTable db_query(string query, string db_loc)
        {
            DataTable myTable = new DataTable();
            //prevent crash if requested table does not exist or illigal changes are requested
            try
            {
                OleDbCommand comm = new OleDbCommand();
                comm.CommandText = query;
                comm.Connection = db_conn;
                if (db_con(db_loc) == null)
                {
                    myTable.Load(comm.ExecuteReader());
                    db_conn.Close();
                }
                else
                    MessageBox.Show(db_con(db_loc),"Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //this can throw many different exceptions, thats why its generic.
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\nProgram failed to qury the database, operations may not function as intended.\nPlease Contact Administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return (myTable);
        }
    }
}

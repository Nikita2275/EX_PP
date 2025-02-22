using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace _8_800_555_35_35
{
    public partial class Panel_Admin : Form
    {
        DataBase dataBase = new DataBase();
        public Panel_Admin()
        {
            InitializeComponent();
        }

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("id_user", "ID");
            dataGridView1.Columns.Add("login_user", "Логин");
            dataGridView1.Columns.Add("password_user", "Пароль");
            var checkColumn = new DataGridViewCheckBoxColumn();
            checkColumn.HeaderText = "IsAdmin";
            dataGridView1.Columns.Add(checkColumn);
        }

        private void ReadSingRow(IDataRecord record)
        {
            dataGridView1.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetBoolean(3));
        }

        private void RefreshDataGrid()
        {
            dataGridView1.Rows.Clear();

            string queryString = $"select * from Register";

            SqlCommand command = new SqlCommand(queryString, dataBase.GetConnection());

            dataBase.OpenConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingRow(reader);
            }
            reader.Close();

            dataBase.CloseConnection();
        }

        private void Panel_Admin_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid();
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            dataBase.OpenConnection();

            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var id = dataGridView1.Rows[index].Cells[0].Value.ToString();
                var isadmin = dataGridView1.Rows[index].Cells[3].Value.ToString();

                var ChangeQuery = $"update Register set is_admin = '{isadmin}' where id_user = '{id}'";

                SqlCommand command = new SqlCommand(ChangeQuery, dataBase.GetConnection());
                command.ExecuteNonQuery();
            }

            dataBase.CloseConnection();

            RefreshDataGrid();
        }

        private void materialRaisedButton2_Click(object sender, EventArgs e)
        {
            dataBase.OpenConnection();

            var SelectedRowIndex = dataGridView1.CurrentCell.RowIndex;

            var id = Convert.ToInt32(dataGridView1.Rows[SelectedRowIndex].Cells[0].Value);

            var deleteQuery = $"delete from Register where id_user = '{id}'";

            SqlCommand command = new SqlCommand(deleteQuery, dataBase.GetConnection());
            command.ExecuteNonQuery();

            dataBase.CloseConnection();

            RefreshDataGrid();
        }
    }
}

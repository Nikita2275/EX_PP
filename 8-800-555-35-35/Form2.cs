using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using System.Data.SqlClient;
using System.Windows;

namespace _8_800_555_35_35
{
    enum RowState
    {
        Existed,
        New,
        Modified,
        ModifiedNew,
        Deleted
    }

    public partial class Form2 : MaterialForm
    {
        private readonly CheckUser _user;

        DataBase dataBase = new DataBase();

        int selectedRow;
        public Form2(CheckUser user)
        {
            _user = user;
            InitializeComponent();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        private void IsAdmin()
        {
            управлениеToolStripMenuItem.Enabled = _user.IsAdmin;
            panel2.Enabled = _user.IsAdmin;
            panel3.Enabled = _user.IsAdmin;
            panel6.Enabled = _user.IsAdmin;
        }

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("id_Exhibit", "ID");
            dataGridView1.Columns.Add("name", "Название");
            dataGridView1.Columns.Add("opiss", "Описание");
            dataGridView1.Columns.Add("price", "Цена");
            dataGridView1.Columns.Add("IsNew", String.Empty);


            dataGridView2.Columns.Add("id_Exhibition", "ID");
            dataGridView2.Columns.Add("name", "Название выставки");
            dataGridView2.Columns.Add("name", "Название выставки");
            dataGridView2.Columns.Add("data_n", "Дата начала");
            dataGridView2.Columns.Add("data_o", "Дата окончания");
            dataGridView2.Columns.Add("IsNew", String.Empty);

        }

        private void ClearFields()
        {
            textBox_id.Text = "";
            textBox_name.Text = "";
            textBox_price.Text = "";
            textBox_opiss.Text = "";
        }

        private void ReadSingRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetInt32(3), RowState.ModifiedNew);
        }

        private void ReadSingRow2(DataGridView dgw2, IDataRecord record2)
        {
            dgw2.Rows.Add(record2.GetInt32(0), record2.GetString(1), record2.GetString(2), record2.GetDateTime(3), record2.GetDateTime(4), RowState.ModifiedNew);
        }

        private void RefreshDataGrid(DataGridView dgw, DataGridView dgw2)
        {
            dgw.Rows.Clear();
            dgw2.Rows.Clear();

            string queryString = $"select * from Exhibits";
            string queryString2 = $"select Exhibitions.id_Exhibition, Exhibitions.name, Exhibits.name, Exhibitions.data_n, Exhibitions.data_o from Exhibits \r\nJOIN Exhibitions \r\nON Exhibits.id_Exhibit = Exhibitions.Exhibit_id;";

            SqlCommand command = new SqlCommand(queryString, dataBase.GetConnection());
            SqlCommand command2 = new SqlCommand(queryString2, dataBase.GetConnection());

            dataBase.OpenConnection();

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ReadSingRow(dgw, reader);
            }
            reader.Close();

            SqlDataReader reader2 = command2.ExecuteReader();
            while (reader2.Read())
            {
                ReadSingRow2(dgw2, reader2);
            }
            reader2.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            toolStripLabel1.Text = $"{_user.Login}: {_user.Status}";
            IsAdmin();
            CreateColumns();
            RefreshDataGrid(dataGridView1, dataGridView2);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];

                textBox_id.Text = row.Cells[0].Value.ToString();
                textBox_name.Text = row.Cells[1].Value.ToString();
                textBox_opiss.Text = row.Cells[2].Value.ToString();
                textBox_price.Text = row.Cells[3].Value.ToString();
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[selectedRow];

                textBoxID.Text = row.Cells[0].Value.ToString();
                textBox4Name.Text = row.Cells[1].Value.ToString();
                textBox_name_ex.Text = row.Cells[2].Value.ToString();
                textBox_date_n.Text = row.Cells[3].Value.ToString();
                textBox_date_o.Text = row.Cells[4].Value.ToString();
            }
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
        }

        private void Search(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string searchString = $"select * from Exhibits where concat (id_Exhibit, name, price, opiss) like '%" + textBox_Search.Text + "%'";

            SqlCommand com = new SqlCommand(searchString, dataBase.GetConnection());

            dataBase.OpenConnection();
            
            SqlDataReader read = com.ExecuteReader();
            
            while (read.Read())
            {
                ReadSingRow(dgw, read);
            }
            read.Close();
        }

        private void deleteRow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            int index2 = dataGridView2.CurrentCell.RowIndex;

            dataGridView1.Rows[index].Visible = false;
            dataGridView2.Rows[index2].Visible = false;

            if (dataGridView1.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView1.Rows[index].Cells[4].Value = RowState.Deleted;
                return;
            }
            dataGridView1.Rows[index].Cells[4].Value = RowState.Deleted;

            if (dataGridView2.Rows[index2].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView2.Rows[index2].Cells[4].Value = RowState.Deleted;
                return;
            }
            dataGridView2.Rows[index2].Cells[4].Value = RowState.Deleted;
        }

        private void update()
        {
            dataBase.OpenConnection();

            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView1.Rows[index].Cells[4].Value;

                if (rowState == RowState.Existed)
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                    var deleteQuery = $"delete from Exhibits where id_Exhibit = {id}";

                    SqlCommand command = new SqlCommand(deleteQuery, dataBase.GetConnection());
                    command.ExecuteNonQuery();
                }

                if (rowState == RowState.Modified)
                {
                    var id = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    var name = dataGridView1.Rows[index].Cells[1].Value.ToString();
                    var opissanie = dataGridView1.Rows[index].Cells[2].Value.ToString();
                    var prise = dataGridView1.Rows[index].Cells[3].Value.ToString();

                    var ChangeQuery = $"update Exhibits set name = '{name}', price = '{prise}', opiss = '{opissanie}' where id_Exhibit = '{id}'";

                    SqlCommand command = new SqlCommand(ChangeQuery, dataBase.GetConnection());
                    command.ExecuteNonQuery();
                }
            }

            for (int index = 0; index < dataGridView2.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView2.Rows[index].Cells[5].Value;

                if (rowState == RowState.Existed)
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var id = Convert.ToInt32(dataGridView2.Rows[index].Cells[0].Value);
                    var deleteQuery = $"delete from Exhibitions where id_Exhibition = {id}";

                    SqlCommand command = new SqlCommand(deleteQuery, dataBase.GetConnection());
                    command.ExecuteNonQuery();
                }

                if (rowState == RowState.Modified)
                {
                    var id = dataGridView2.Rows[index].Cells[0].Value.ToString();
                    var name = dataGridView2.Rows[index].Cells[1].Value.ToString();
                    var data_n = dataGridView2.Rows[index].Cells[3].Value.ToString();
                    var data_o = dataGridView2.Rows[index].Cells[4].Value.ToString();

                    var ChangeQuery = $"update Exhibitions set name = '{name}', data_n = '{data_n}', data_o = '{data_o}' where id_Exhibition = '{id}'";

                    SqlCommand command = new SqlCommand(ChangeQuery, dataBase.GetConnection());
                    command.ExecuteNonQuery();
                }
            }

            dataBase.CloseConnection();
        }

        private void Change()
        {
            var SelectedRowIndex = dataGridView1.CurrentCell.RowIndex;

            var id = textBox_id.Text;
            var name = textBox_name.Text;
            int price;
            var opiss = textBox_opiss.Text;

            if (dataGridView1.Rows[SelectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                if(int.TryParse(textBox_price.Text, out price))
                {
                    dataGridView1.Rows[SelectedRowIndex].SetValues(id, name, opiss, price);
                    dataGridView1.Rows[SelectedRowIndex].Cells[4].Value = RowState.Modified;
                }
                else System.Windows.MessageBox.Show("Цена должна иметь цифровой формат!", "Ошибка!", MessageBoxButton.OK, (MessageBoxImage)MessageBoxIcon.Information);
            }
        }

        private void Change_2()
        {
            var SelectedRowIndex = dataGridView2.CurrentCell.RowIndex;

            var id = textBoxID.Text;
            var name = textBox4Name.Text;
            var name_ex = textBox_name_ex.Text;
            var data_n = textBox_date_n.Text;
            var data_o = textBox_date_o.Text;

            if (dataGridView2.Rows[SelectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridView2.Rows[SelectedRowIndex].SetValues(id, name, name_ex, data_n, data_o);
                dataGridView2.Rows[SelectedRowIndex].Cells[5].Value = RowState.Modified;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Search(dataGridView1);
        }

        private void materialRaisedButton2_Click(object sender, EventArgs e)
        {
            deleteRow();
            ClearFields();
        }

        private void Deleted_Click(object sender, EventArgs e)
        {
            deleteRow();
            ClearFields();
        }

        private void materialRaisedButton4_Click(object sender, EventArgs e)
        {
            update();
        }

        private void Sav_Click(object sender, EventArgs e)
        {
            update();
        }

        private void Chang_Click(object sender, EventArgs e)
        {
            Change_2();
            ClearFields();
        }

        private void materialRaisedButton3_Click(object sender, EventArgs e)
        {
            Change();
            ClearFields();
        }

        private void управлениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Panel_Admin panel = new Panel_Admin();
            panel.Show();
        }

        private void мнформацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void документацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();
            form5.Show();
        }

        private void орлрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1, dataGridView2);
        }
    }
}
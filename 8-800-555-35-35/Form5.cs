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

namespace _8_800_555_35_35
{
    public partial class Form5: MaterialForm
    {
        DataBase dataBase = new DataBase();
        public Form5()
        {
            InitializeComponent();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        private void CreateColumns()
        {
            dataGrid_doc.Columns.Add("id_Document", "ID");
            dataGrid_doc.Columns.Add("name", "Название");
            dataGrid_doc.Columns.Add("place_time", "Место и время изготовления");
            dataGrid_doc.Columns.Add("quantity", "Количество");
            dataGrid_doc.Columns.Add("material", "Материал");
            dataGrid_doc.Columns.Add("size", "Размер");
            dataGrid_doc.Columns.Add("safety", "Сохранность");
            dataGrid_doc.Columns.Add("opiss", "Описание");
            dataGrid_doc.Columns.Add("source_of_receipt", "Источник поступления");
            dataGrid_doc.Columns.Add("originator", "Составитель");
            dataGrid_doc.Columns.Add("data", "Дата");
        }

        private void ReadSingRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetInt32(3), record.GetString(4), record.GetString(5), record.GetString(6), record.GetString(7), record.GetString(8), record.GetString(9), record.GetDateTime(10));
        }

        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string queryString = $"select Documentation.id_Document, Exhibits.name, Documentation.place_time, Documentation.quantity, Documentation.material, Documentation.size, Documentation.safety, Exhibits.opiss, Documentation.source_of_receipt, Documentation.originator, Documentation.data from Exhibits \r\nJOIN Documentation \r\nON Exhibits.id_Exhibit = Documentation.Exhibit_id;";

            SqlCommand command = new SqlCommand(queryString, dataBase.GetConnection());

            dataBase.OpenConnection();

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ReadSingRow(dgw, reader);
            }
            reader.Close();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGrid_doc);
        }
    }
}

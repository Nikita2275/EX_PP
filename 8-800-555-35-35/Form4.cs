using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin.Controls;
using System.Data.SqlClient;
using MaterialSkin;
using System.Windows;

namespace _8_800_555_35_35
{
    public partial class Form4 : MaterialForm
    {
        DataBase dataBase = new DataBase();
        public Form4()
        {
            InitializeComponent();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            dataBase.OpenConnection();

            var name = textBox_name_ex.Text;
            int price;
            var opiss = textBox_opiss_ex.Text;

            if (int.TryParse(textBox_price_ex.Text, out price))
            {
                var addQuery = $"insert into Exhibits (name, price, opiss) values ('{name}', '{price}', '{opiss}')";

                var command = new SqlCommand(addQuery, dataBase.GetConnection());
                command.ExecuteNonQuery();

                System.Windows.MessageBox.Show("Запись успешно создана!", "Успешно!", MessageBoxButton.OK, (MessageBoxImage)MessageBoxIcon.Information);
            }
            else System.Windows.MessageBox.Show("Цена должна иметь цифровой формат!", "Ошибка!", MessageBoxButton.OK, (MessageBoxImage)MessageBoxIcon.Information);

            dataBase.CloseConnection();
        }
    }
}
